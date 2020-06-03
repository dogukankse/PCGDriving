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
using System.Collections.Generic;
using _Scripts;
using NWH.VehiclePhysics;
using UnityEngine;
using UnityEngine.Events;

public class TrafficSystemVehiclePlayer : TrafficSystemVehicle
{
    public enum LightType
    {
        None,
        Low,
        Long
    }

    public enum SignalType
    {
        None,
        Left,
        Right
    }

    public static string SIDEWALK_PENALTY = "side_walk_penalty";
    public static string CRASH_PENALTY = "crash_penalty";
    public static string CAR_CRASH_PENALTY = "car_crash_penalty";
    public static string RED_LIGHT_PENALTY = "red_light_penalty";
    public static string LANE_SWITCH_PENALTY = "lane_switch_penalty";
    public static string SPEED_PENALTY = "speed_penalty";
    public static string CAR_DISTANCE_PENALTY = "car_distance_penalty";

    public delegate void HasEnteredTrafficLightTrigger(TrafficSystemTrafficLight a_trafficLight);

    public HasEnteredTrafficLightTrigger hasEnteredTrafficLightTrigger;

    Dictionary<Transform, float> whellCheck = new Dictionary<Transform, float>();
    private float nextWheelCheckTime = 0.0f;
    public float wheelCheckPeriod = 0.1f;

    private SignalType signalType = SignalType.None;

    Dictionary<String, float> PenaltyTimes = new Dictionary<string, float>();
    public int currentPoint = 100;
    public UnityAction<int, int, string> pointUpdate;
    public GameObject Speedometer;

    public LightType lightType = LightType.None;
    public LightController lightController;

    private Speedometer _speedometer;
    private TrafficSystem.DriveSide currentDriverSide;
    public GameObject speedGauge;
    public GameObject rpmGauge;
    private AnalogGauge _speedGauge;
    private AnalogGauge _rpmGauge;
    private MSVehicleControllerFree vehicle;
    private float clampGear;

    public override void Awake()
    {
        base.Awake();
        initWhellMap();
        _speedometer = Speedometer.GetComponent<Speedometer>();
        lightController = GetComponent<LightController>();
        _speedGauge = speedGauge.GetComponent<AnalogGauge>();
        _rpmGauge = rpmGauge.GetComponent<AnalogGauge>();
        vehicle = GetComponent<MSVehicleControllerFree>();
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
        CheckCarDistance();
        CheckLaneColliders();
        UpdateSignal();
        UpdateLight();
        clampGear = Mathf.Clamp(vehicle.currentGear, -1, 1);
        if (clampGear == 0)
        {
            clampGear = 1;
        }

        float speed = vehicle.KMh * clampGear;
        _speedGauge.Value = vehicle.KMh * clampGear;
        _rpmGauge.Value = vehicle.sumRPM;
        if (speed > 50)
        {
            DecreasePoint(SPEED_PENALTY);
        }
        
        
    }

    private void UpdateLight()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (lightType == LightType.None)
            {
                lightType = LightType.Low;
            }
            else if (lightType == LightType.Low)
            {
                lightType = LightType.Long;
            }
            else if (lightType == LightType.Long)
            {
                lightType = LightType.None;
            }

            lightController.SetType(lightType);
            _speedometer.setLightType(lightType);
        }
    }

    private void UpdateSignal()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (signalType == SignalType.Left)
            {
                signalType = SignalType.None;
                _speedometer.UpdateSignal(signalType);
                Debug.Log("left none");
            }
            else
            {
                signalType = SignalType.Left;
                _speedometer.UpdateSignal(signalType);
                Debug.Log("left z");
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (signalType == SignalType.Right)
            {
                signalType = SignalType.None;
                _speedometer.UpdateSignal(signalType);
                Debug.Log("right none");
            }
            else
            {
                signalType = SignalType.Right;
                _speedometer.UpdateSignal(signalType);
                Debug.Log("right c");
            }
        }
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


    private void CheckCarDistance()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(
            transform.position + m_vehicleCheckOffset + -transform.right * m_vehicleCheckDistLeftRayOffset,
            transform.forward, out hitInfo, m_vehicleCheckDistRay))
        {
            TrafficSystemVehicle hitVehicle = hitInfo.transform.GetComponent<TrafficSystemVehicle>();
            if (hitVehicle && hitVehicle != this && hitVehicle.m_velocity > 0)
            {
                DecreasePoint(CAR_DISTANCE_PENALTY);
            }
        }
    }

    private void DecreasePoint(String type)
    {
        if (PenaltyTimes.ContainsKey(type))
        {
            //check last time
            if (Math.Abs(PenaltyTimes[type] - Time.time) > 1)
            {
                //if the time of last penalty is greater
                currentPoint -= PenaltyPoints.get(type);
                pointUpdate(currentPoint, PenaltyPoints.get(type), type);
            }
        }
        else
        {
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