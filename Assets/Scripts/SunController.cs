using System;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [Header("Sun")] [SerializeField] private Light sun;

    /// <summary>
    /// simulate 24H in seconds
    /// </summary>
    [SerializeField] private float secondsInDay = 120f;

    /// <summary>
    /// midnight => 0
    /// sunrise => 0.25
    /// noon => 0.5
    /// sunset => 0.75
    /// </summary>
    [Range(0f, 1f)] [SerializeField] private float currentTime = 0f;

    private float timeMultiplier = 1f;

    private float sunInitialIntensity;
    [SerializeField] private bool realtime = false;


    private void Start()
    {
        sunInitialIntensity = sun.intensity;
    }

    private void Update()
    {
        UpdateSun();
        if (!realtime) return;
        currentTime += (Time.deltaTime / secondsInDay) / timeMultiplier;

        if (currentTime >= 1) currentTime = 0;
    }

    /// <summary>
    /// rotate the sun 360 degrees around the x-axis according to the current time of day
    /// </summary>
    private void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTime * 360f) - 90f, 170f, 0f);

        float intensityMultiplier = 1f;
        if (currentTime <= 0.23f || currentTime >= 0.75f)
            intensityMultiplier = 0;
        else if (currentTime <= 0.25f)
            intensityMultiplier = Mathf.Clamp01((currentTime - 0.23f) * 50f);
        else if (currentTime >= 0.73f)
            intensityMultiplier = Mathf.Clamp01((currentTime - 0.73f) * 50f);


        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}