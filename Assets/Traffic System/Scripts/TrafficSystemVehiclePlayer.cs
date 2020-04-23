﻿/// <summary>
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
using _Scripts;
using UnityEngine.Events;

public class TrafficSystemVehiclePlayer : TrafficSystemVehicle
{
    
    
    public static string SIDEWALK_PENALTY = "side_walk_penalty";
    public static string CRASH_PENALTY = "crash_penalty";
    public  static string CAR_CRASH_PENALTY = "car_crash_penalty";
    public static string RED_LIGHT_PENALTY = "red_light_penalty";
    public static string LANE_SWITCH_PENALTY = "lane_switch_penalty";
    
    public delegate void HasEnteredTrafficLightTrigger(TrafficSystemTrafficLight a_trafficLight);

    public HasEnteredTrafficLightTrigger hasEnteredTrafficLightTrigger;

    Dictionary<Transform, float> whellCheck = new Dictionary<Transform, float>();
    private float nextWheelCheckTime = 0.0f;
    public float wheelCheckPeriod = 0.1f;

    Dictionary<String, float> PenaltyTimes = new Dictionary<string, float>();
    public int currentPoint = 100;
    public UnityAction<int, int, string> pointUpdate;

    private TrafficSystem.DriveSide currentDriverSide;

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


    public override void Start()
    {
        hasEnteredTrafficLightTrigger += ProcessHasEnteredTrafficLightTrigger;

        // no need to do anyting, we just need to override TrafficSystemVehicle since this is the player
    }

    public override void Update()
    {
        CheckSideWalkPenalty();
        CheckLaneColliders();
    }
    
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider)
        {
            if (collision.collider.GetComponent<TrafficSystemVehicle>())
            {
                Debug.Log("Car crash penalty");
                DecreasePoint(CAR_CRASH_PENALTY);
            }
            else
            {
                Debug.Log("Crash non car");
                DecreasePoint(CRASH_PENALTY);
            }
        }
    }



    private void CheckLaneColliders()
    {
        Vector3 centerOfFront = Vector3.MoveTowards(m_wheelsFront[0].position, m_wheelsFront[1].position,
            Vector3.Distance(m_wheelsFront[0].position, m_wheelsFront[1].position) / 2);

        RaycastHit[] hits_left = Physics.RaycastAll(new Ray(centerOfFront, Vector3.left), 12f);
        RaycastHit[] hits_right = Physics.RaycastAll(new Ray(centerOfFront, Vector3.right), 12f);

        foreach (var hit in hits_left)
        {
            if (hit.collider.CompareTag("LaneSwitch"))
            {
                ChangeLane(TrafficSystem.DriveSide.LEFT);
            }
        }

        foreach (var hit in hits_right)
        {
            if (hit.collider.CompareTag("LaneSwitch"))
            {
                ChangeLane(TrafficSystem.DriveSide.RIGHT);
            }
        }
    }

    private void DecreasePoint(String type)
    {
        if (PenaltyTimes.ContainsKey(type))
        {
            //check last time
            if(Math.Abs(PenaltyTimes[type] - Time.time) > 1){
                //if the time of last penalty is greater
                currentPoint -= PenaltyPoints.get(type);
                pointUpdate(currentPoint, PenaltyPoints.get(type), type);
            }
        }else{
            currentPoint -= PenaltyPoints.get(type);
            pointUpdate(currentPoint, PenaltyPoints.get(type), type);
        }
        
        PenaltyTimes[type] = Time.time;
    }

    private void ChangeLane(TrafficSystem.DriveSide side)
    {
        if (side != currentDriverSide)
        {
            Debug.Log("Penalty lane change");
            DecreasePoint(LANE_SWITCH_PENALTY);
        }

        currentDriverSide = side;
    }

    private void CheckSideWalkPenalty()
    {
        if (Time.time > nextWheelCheckTime)
        {
            nextWheelCheckTime += wheelCheckPeriod;
            foreach (var wheel in m_wheelsFront)
            {
                if (wheelIsOnSideWalk(wheel))
                {
                    Debug.Log("Sidewalk penalty front");
                    DecreasePoint(SIDEWALK_PENALTY);
                }
            }

            foreach (var wheel in m_wheelsRear)
            {
                if (wheelIsOnSideWalk(wheel))
                {
                    Debug.Log("Sidewalk penalty rear");
                    DecreasePoint(SIDEWALK_PENALTY);
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
            DecreasePoint(RED_LIGHT_PENALTY);
        }
    }
}