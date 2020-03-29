using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class OcclusionCulling : MonoBehaviour
{
    [SerializeField] private float _radius;
    private SphereCollider _collider;


    private GameObject[] gameObjects;

    private Collider[] olds;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        _collider.radius = _radius;
    }

    /* private void OnTriggerEnter(Collider other)
     {
         print("trigger: " + other.name);
         if (!other.CompareTag("Road")) return;
         other.GetComponent<Renderer>().enabled = true;
     }*/

    /* private void OnTriggerExit(Collider other)
     {
         print("trigger: " + other.name);
         if (!other.CompareTag("Road")) return;
         other.GetComponent<Renderer>().enabled = false;
     }*/


    private void FixedUpdate()
    {
        var others = Physics.OverlapSphere(transform.position, _radius);

        Renderer[] renderers = { };
        foreach (var other in others)
        {
            renderers = other.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                if (renderer.enabled && !renderer.gameObject.CompareTag("Road")) continue;
                renderer.enabled = true;
            }
        }

        if (olds != null)
        {
            var inOldsNotOthers = olds.Except(others).ToList();
            CloseRenderers(inOldsNotOthers);
        }

        olds = others;
    }

    private void CloseRenderers(List<Collider> inOldsNotOthers)
    {
        foreach (var obj in inOldsNotOthers)
        {
            if (obj != null)
            {
                if (!obj.enabled) continue;
                obj.enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.DrawWireSphere(position, _radius);

        /* Gizmos.DrawWireCube(position + new Vector3(_radius + _radius / 2, 0, 0),
             new Vector3(_radius / 2, _radius * 2, _radius * 2));
         Gizmos.DrawWireCube(position + new Vector3(0, 0, _radius + _radius / 2),
             new Vector3(_radius * 2, _radius * 2, _radius / 2));
 
         Gizmos.DrawWireCube(position - new Vector3(_radius + _radius / 2, 0, 0),
             new Vector3(_radius / 2, _radius * 2, _radius * 2));
         Gizmos.DrawWireCube(position - new Vector3(0, 0, _radius + _radius / 2),
             new Vector3(_radius * 2, _radius * 2, _radius / 2));*/
    }
}