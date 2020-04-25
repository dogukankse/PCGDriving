using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIObj : MonoBehaviour
{
    public AIPoint startPoint;

    private AIPoint _currPoint;
    private AIPoint _nextPoint;
    private AIPoint _lastPoint;
    private Animator _animator;


    private float _speed = 2f;
    private int _dir = 0;
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Speed = Animator.StringToHash("speed");
    private const float _rayDistance = 1f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _speed = Random.Range(0.5f, 2f);
        transform.position = startPoint.transform.position;
        _currPoint = startPoint;
        _nextPoint = startPoint.connectedPoints.ToArray().GetRandomFrom();
    }

    private void Update()
    {
        float singleStep = _speed * Time.deltaTime;
        
        if (_currPoint != startPoint && _currPoint.connectedPoints.Count <= 1)
            Destroy(gameObject);

        if (Physics.Raycast(transform.position + (transform.up / 2), transform.TransformDirection(Vector3.forward),
            out var hit, _rayDistance))
        {
            if (!hit.transform.gameObject.TryGetComponent<AIObj>(out var isObj))
            {
                Debug.DrawRay(transform.position + (transform.up / 2), transform.TransformDirection(Vector3.forward),
                    Color.red);
                Debug.Log($"{hit.collider.name}");
                IdleAnim(true);
            }
        }
        else
        {
            Debug.DrawRay(transform.position + (transform.up / 2), transform.TransformDirection(Vector3.forward),
                Color.white);
            if (RotateToDestination(singleStep))
            {
                IdleAnim(false);
                MoveToDestination(singleStep);
            }
            else
                IdleAnim(true);
        }

        if (transform.position == _nextPoint.transform.position)
        {
            if (_currPoint.connectedPoints[_dir] == null)
                _dir = Random.Range(0, _currPoint.connectedPoints.Count);

            _lastPoint = _currPoint;
            _nextPoint = _lastPoint.connectedPoints[_dir];
            _currPoint = _nextPoint;
        }
    }

    private void IdleAnim(bool isOn)
    {
        _animator.SetBool(Idle, isOn);
    }

    private void MoveToDestination(float step)
    {
        
        WalkAnim();
        transform.position = Vector3.MoveTowards(transform.position, _nextPoint.transform.position, step);
    }

    private void WalkAnim()
    {
        _animator.SetFloat(Speed, _speed);
    }

    private bool RotateToDestination(float step)
    {
        var objTransform = transform;
        Vector3 targetDirection = _nextPoint.transform.position - objTransform.position;
        Vector3 newDirection = Vector3.RotateTowards(objTransform.forward, targetDirection, step, 0.0f);

        Quaternion lookTo = Quaternion.LookRotation(newDirection);
        if (lookTo != transform.rotation)
        {
            transform.rotation = lookTo;
            return false;
        }

        return true;
    }
}