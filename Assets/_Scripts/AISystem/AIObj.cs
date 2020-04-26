using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.AISystem
{
    public class AIObj : MonoBehaviour
    {
        private enum AIState
        {
            ROTATE,
            MOVE,
            IDLE
        }


        public AIPoint startPoint;

        private AIPoint _currPoint;
        private AIPoint _nextPoint;
        private AIPoint _lastPoint;
        private Animator _animator;

        [SerializeField] private AIState _currState;


        private float _speed = 2f;
        private int _dir;
        private static readonly int Idle = Animator.StringToHash("idle");
        private static readonly int Speed = Animator.StringToHash("speed");
        private const float _rayDistance = 1f;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _speed = Random.Range(0.5f, 2f);
            transform.position = startPoint.transform.position;
            _currPoint = startPoint;
            _nextPoint = startPoint.connectedPoints[Random.Range(0, startPoint.connectedPoints.Count)];
        }

        private void Update()
        {
            if (_currPoint != startPoint && _currPoint.connectedPoints.Count <= 1)
                _dir = Random.Range(0, _currPoint.connectedPoints.Count);

            var objTransform = transform;
            float singleStep = _speed * Time.deltaTime;
            _currState = AIState.ROTATE;

            if (Physics.Raycast(transform.position + (transform.up / 2), transform.TransformDirection(Vector3.forward),
                out var hit, _rayDistance))
            {
                if (hit.collider.isTrigger)
                {
                    string a = $"{name} trigger: {hit.collider.name}";
                    Debug.DrawRay(objTransform.position + (objTransform.up / 2),
                        transform.TransformDirection(Vector3.forward), Color.blue);


                    if (hit.transform.TryGetComponent<TrafficSystemTrafficLight>(out var trafficLight))
                    {
                        a += $" {trafficLight.m_status}";
                        switch (trafficLight.m_status)
                        {
                            case TrafficSystemTrafficLight.Status.GREEN:
                                IdleAnim(true);
                                break;
                            case TrafficSystemTrafficLight.Status.RED:
                                _currState = AIState.MOVE;
                                break;
                            case TrafficSystemTrafficLight.Status.YELLOW:
                                IdleAnim(true);
                                break;
                        }
                    }

                    print(a);
                }
                //if is collider
                else
                {
                    print($"{name} collider: {hit.collider.name}");
                    if (_currState != AIState.ROTATE)
                    {
                        IdleAnim(true);
                    }
                    else
                        _currState = AIState.MOVE;


                    Debug.DrawRay(transform.position + (objTransform.up / 2),
                        transform.TransformDirection(Vector3.forward), Color.red);
                }
            }
            else
                _currState = AIState.MOVE;

            //move if safe
            if (_currState == AIState.MOVE)
            {
                Debug.DrawRay(transform.position + (objTransform.up / 2),
                    transform.TransformDirection(Vector3.forward), Color.green);

                if (RotateToDestination(singleStep))
                {
                    IdleAnim(false);
                    MoveToDestination(singleStep);
                }
                else
                    IdleAnim(true);


                if (transform.position == _nextPoint.transform.position)
                {
                    try
                    {
                        var a = _currPoint.connectedPoints[_dir];
                    }
                    catch
                    {
                        _dir = Random.Range(0, _currPoint.connectedPoints.Count);
                    }

                    _dir = Random.Range(0, _currPoint.connectedPoints.Count);
                    _lastPoint = _currPoint;
                    _nextPoint = _lastPoint.connectedPoints[_dir];
                    _currPoint = _nextPoint;
                }
            }
        }

        private void IdleAnim(bool isOn)
        {
            if (isOn)
                _currState = AIState.IDLE;
            _animator.SetBool(Idle, isOn);
        }

        private void MoveToDestination(float step)
        {
            _currState = AIState.MOVE;
            WalkAnim();
            transform.position = Vector3.MoveTowards(transform.position, _nextPoint.transform.position, step);
        }

        private void WalkAnim()
        {
            _animator.SetFloat(Speed, _speed);
        }

        private bool RotateToDestination(float step)
        {
            _currState = AIState.ROTATE;
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
}