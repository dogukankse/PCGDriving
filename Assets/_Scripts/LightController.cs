using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private TrafficSystemVehiclePlayer.LightType type;
    public GameObject[] rearLights;
    public GameObject[] longLights;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetType(TrafficSystemVehiclePlayer.LightType type)
    {
        this.type = type;
        Debug.Log("Light type changed " + type);
        refresh();
    }

    private void refresh()
    {
        if (type == TrafficSystemVehiclePlayer.LightType.None)
        {
            foreach (var rearLight in rearLights)
            {
                rearLight.GetComponent<Light>().enabled = false;
            }

            foreach (var light in longLights)
            {
                light.GetComponent<Light>().enabled = false;
            }
        }
        else if (type == TrafficSystemVehiclePlayer.LightType.Low)
        {
            foreach (var rearLight in rearLights)
            {
                rearLight.GetComponent<Light>().enabled = true;
            }

            foreach (var light in longLights)
            {
                light.GetComponent<Light>().enabled = false;
            }
        }
        else if (type == TrafficSystemVehiclePlayer.LightType.Long)
        {
            foreach (var rearLight in rearLights)
            {
                rearLight.GetComponent<Light>().enabled = false;
            }

            foreach (var light in longLights)
            {
                light.GetComponent<Light>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}