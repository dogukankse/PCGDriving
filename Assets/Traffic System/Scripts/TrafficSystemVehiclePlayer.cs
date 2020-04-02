/// <summary>
/// 
/// Traffic system vehicle player.
/// 
/// All you have to do is drop the "TrafficSystemVehiclePlayer.cs" script onto any part of your player vehicle that has a collider on it,
/// or to be more correct, any part of your vehicle that you "want" to collider with the Traffic System vehicles.
/// 
/// The colliders for the player can be collision or triggers, the script will detect both. The Traffic System vehicles will then detect
/// that a player controlled vehicle has collidered with them and they will brake and use the correct physics for collision.
/// 
/// </summary>

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficSystemVehiclePlayer : TrafficSystemVehicle
{
    public delegate void HasEnteredTrafficLightTrigger(TrafficSystemTrafficLight a_trafficLight);

    public HasEnteredTrafficLightTrigger hasEnteredTrafficLightTrigger;

    Dictionary<Transform, float> whellCheck = new Dictionary<Transform, float>();
    private float nextWheelCheckTime = 0.0f;
    public float wheelCheckPeriod = 0.1f;

    public override void Awake()
    {
        base.Awake();
        initWhellMap();
    }

    private void initWhellMap()
    {
        foreach (var transform in m_wheelsFront)
        {
            whellCheck[transform] = 0f;
        }

        foreach (var transform in m_wheelsRear)
        {
            whellCheck[transform] = 0f;
        }
    }

    /// <summary>
    /// To use the HasEnteredTrafficLightTrigger all you need to do is add this to your script or put code in the function below
    /// 
    /// in void Start -
    ///     [TrafficSystemVehiclePlayer].hasEnteredTrafficLightTrigger += ProcessHasEnteredTrafficLightTrigger;
    /// 
    /// in void Destroy -
    ///     [TrafficSystemVehiclePlayer].hasEnteredTrafficLightTrigger -= ProcessHasEnteredTrafficLightTrigger;
    ///
    /// Then define your own function
    ///    void ProcessHasEnteredTrafficLightTrigger( TrafficSystemTrafficLight a_trafficLight )
    ///    {
    ///   	  do something in here...
    ///    }
    /// 
    /// </summary>
    public override void Start()
    {
        hasEnteredTrafficLightTrigger += ProcessHasEnteredTrafficLightTrigger;
        
        // no need to do anyting, we just need to override TrafficSystemVehicle since this is the player
    }

    public override void Update()
    {
        checkSideWalkPenalty();
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider)
        {
            if (collision.collider.GetComponent<TrafficSystemVehicle>())
            {
                Debug.Log("Car crash penalty");
            }
            else
            {
                Debug.Log("Crash non car");
            }

          
        }

    }
    
    private void checkSideWalkPenalty()
    {
        if (Time.time > nextWheelCheckTime)
        {
            nextWheelCheckTime += wheelCheckPeriod;
            foreach (var wheel in m_wheelsFront)
            {
                if (wheelIsOnSideWalk(wheel))
                {
                    Debug.Log("Sidewalk penalty front");
                }
            }

            foreach (var wheel in m_wheelsRear)
            {
                if (wheelIsOnSideWalk(wheel))
                {
                    Debug.Log("Sidewalk penalty rear");
                }
            }
        }
    }

    private bool wheelIsOnSideWalk(Transform wheels)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(wheels.position, Vector3.down));
        foreach (var hit in hits)
        {
            Collider collider = hit.collider;
            if (collider.GetComponent<TrafficSystem>())
            {
                float distance = hit.distance;
                //Debug.Log("distance diff"+wheels.name+" -- "+(whellCheck[wheels] - distance));
                if (whellCheck[wheels] != 0 && Mathf.Abs(whellCheck[wheels] - distance) > 0.05f)
                {
                    whellCheck[wheels] = hit.distance;
                    return true;
                }

                whellCheck[wheels] = hit.distance;
            }
        }

        return false;
    }

    public void ProcessHasEnteredTrafficLightTrigger(TrafficSystemTrafficLight a_trafficLight)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        var light = other.GetComponent<TrafficSystemTrafficLight>();
        if (light && light.m_status == TrafficSystemTrafficLight.Status.RED)
        {
            //RED LIGHT PENALY
            Debug.Log("Red light penalty");
        }
    }
}