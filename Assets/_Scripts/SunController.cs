using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class SunController : MonoBehaviour
    {
        [Header("Sun")] [SerializeField] private Light sun;

        [Tooltip("Simulate 24H in seconds")] [SerializeField]
        private float secondsInDay = 120f;

        [Tooltip("midnight => 0\nsunrise => 0.25\nnoon => 0.5\nsunset => 0.75")] [Range(0f, 1f)] [SerializeField]
        private float currentTime;

        private float timeMultiplier = 1f;

        private float sunInitialIntensity;
        [SerializeField] private bool realtime;

        private List<Light> _roadLamps ;

        private void Start()
        {
            sunInitialIntensity = sun.intensity;
        }

        public void FindRoadLamps()
        {
            GameObject[] lamps = GameObject.FindGameObjectsWithTag("RoadLamp");
            _roadLamps = new List<Light>();

            for (int i = 0; i < lamps.Length; i++)
                _roadLamps.Add(lamps[i].GetComponentInChildren<Light>());
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

           UpdateRoadLamps(1 - intensityMultiplier);
        }

        private void UpdateRoadLamps(float intensityMultiplier)
        {
            intensityMultiplier = intensityMultiplier <= .5f ? 0f : 1f;

           /* for (int i = 0; i < _roadLamps.Count; i++)
            {
                _roadLamps[i].intensity = 10 * intensityMultiplier;
            }*/
        }
    }
}