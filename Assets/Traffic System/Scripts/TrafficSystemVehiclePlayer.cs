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
using UnityEngine.Events;

public class TrafficSystemVehiclePlayer : TrafficSystemVehicle
{
    public delegate void HasEnteredTrafficLightTrigger(TrafficSystemTrafficLight a_trafficLight);

    public HasEnteredTrafficLightTrigger hasEnteredTrafficLightTrigger;

    Dictionary<Transform, float> whellCheck = new Dictionary<Transform, float>();
    private float nextWheelCheckTime = 0.0f;
    public float wheelCheckPeriod = 0.1f;
    
    Dictionary<String,float> PenaltyTimes = new Dictionary<string, float>();
    public int currentPoint = 100;
    public UnityAction<int,int,string> pointUpdate;
    
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

    private static string CRASH = "crash";
    private static string CAR_CRASH = "car_crash";
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider)
        {
            if (collision.collider.GetComponent<TrafficSystemVehicle>())
            {
                Debug.Log("Car crash penalty");
                DecreasePoint(CAR_CRASH,1);
            }
            else
            {
                Debug.Log("Crash non car");
                DecreasePoint(CRASH,1);
            }

          
        }

    }

    private static string LANE_SWITCH = "lane_switch";
    private void CheckLaneColliders()
    {


        Vector3 centerOfFront = Vector3.MoveTowards(m_wheelsFront[0].position, m_wheelsFront[1].position,
            Vector3.Distance(m_wheelsFront[0].position, m_wheelsFront[1].position) / 2);
        
        RaycastHit[] hits_left = Physics.RaycastAll(new Ray(centerOfFront, Vector3.left),12f);
        RaycastHit[] hits_right = Physics.RaycastAll(new Ray(centerOfFront, Vector3.right),12f);
        
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
    
    private void DecreasePoint(String type, int point)
    {
        if ( !PenaltyTimes.ContainsKey(type) || (PenaltyTimes.ContainsKey(type) && Mathf.Abs(Time.deltaTime - PenaltyTimes[type]) > 0 ))
        {
            currentPoint -= point;
            pointUpdate(currentPoint,point,type);
        }
        PenaltyTimes[type] = Time.deltaTime;
     
    }
    
    private void ChangeLane(TrafficSystem.DriveSide side)
    {
        if (side != currentDriverSide)
        {
            Debug.Log("Penalty lane change");
            DecreasePoint(LANE_SWITCH, 1);
        }

        currentDriverSide = side;
    }
    
    private static string SIDEWALK = "side_walk";
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
                    DecreasePoint(SIDEWALK, 1);
                }
            }

            foreach (var wheel in m_wheelsRear)
            {
                if (wheelIsOnSideWalk(wheel))
                {
                    Debug.Log("Sidewalk penalty rear");
                    DecreasePoint(SIDEWALK, 1);
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

    private static string RED_LIGHT = "red_light";
    private void OnTriggerExit(Collider other)
    {
        var light = other.GetComponent<TrafficSystemTrafficLight>();
        if (light && light.m_status == TrafficSystemTrafficLight.Status.RED)
        {
            //RED LIGHT PENALY
            Debug.Log("Red light penalty");
            DecreasePoint(RED_LIGHT, 1);
        }
    }
}