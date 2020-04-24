/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using NWH.VehiclePhysics;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {

    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 230;

    private Transform needleTranform;
    private Transform speedLabelTemplateTransform;
    private Transform signals;
    private float speedMax;
    private float speed;
    private Transform longLight;
    private Transform lowLight;
    
    private void Awake() {
        needleTranform = transform.Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        signals = transform.Find("Signal");
        longLight = transform.Find("High Beam");
        lowLight = transform.Find("Low Beam");
        speedLabelTemplateTransform.gameObject.SetActive(false);
        
        speed = 0f;
        speedMax = 200f;

        CreateSpeedLabels();
    }

    public void UpdateSignal(TrafficSystemVehiclePlayer.SignalType type)
    {
        signals.GetComponent<SignalController>().updateType(type);
    }

    private void Update() {
        //speed += 30f * Time.deltaTime;
        //if (speed > speedMax) speed = speedMax;

        needleTranform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    public void setLightType(TrafficSystemVehiclePlayer.LightType lightType)
    {
        if (lightType == TrafficSystemVehiclePlayer.LightType.None)
        {
            longLight.GetComponent<DashLight>().Active = false;
            lowLight.GetComponent<DashLight>().Active = false;
        }else if (lightType == TrafficSystemVehiclePlayer.LightType.Low)
        {
            longLight.GetComponent<DashLight>().Active = false;
            lowLight.GetComponent<DashLight>().Active = true;
        }
        else if (lightType == TrafficSystemVehiclePlayer.LightType.Long)
        {
            longLight.GetComponent<DashLight>().Active = true;
            lowLight.GetComponent<DashLight>().Active = false;
        }
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void CreateSpeedLabels() {
        int labelAmount = 10;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for (int i = 0; i <= labelAmount; i++) {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }
        
        signals.SetAsLastSibling();
        needleTranform.SetAsLastSibling();
    
    }

    private float GetSpeedRotation() {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = speed / speedMax;

        return Mathf.Abs(ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize);
    }
}
