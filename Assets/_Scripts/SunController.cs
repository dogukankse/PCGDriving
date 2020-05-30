using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class SunController : MonoBehaviour
    {
        [Header("Sun")] [SerializeField] private Light _sun;

        [Tooltip("Simulate 24H in seconds")] [SerializeField]
        private float _secondsInDay = 120f;

        [Tooltip("midnight => 0\nsunrise => 0.25\nnoon => 0.5\nsunset => 0.75")] [Range(0f, 1f)] [SerializeField]
        public float currentTime;

        private const float _timeMultiplier = 1f;

        [SerializeField] private float _sunInitialIntensity;
        [SerializeField] private bool _realtime;

        private List<Light> _roadLamps;

        private void Awake()
        {
            _sun = GetComponent<Light>();
        }

        private void Start()
        {
            _sunInitialIntensity = 1f;
        }

        public void FindRoadLamps()
        {
            var lamps = GameObject.FindGameObjectsWithTag("RoadLamp");
            _roadLamps = new List<Light>();

            foreach (var lamp in lamps)
                _roadLamps.Add(lamp.GetComponentInChildren<Light>());
        }

        private void Update()
        {
            UpdateSun();
            if (!_realtime) return;
            currentTime += (Time.deltaTime / _secondsInDay) / _timeMultiplier;

            if (currentTime >= 1) currentTime = 0;
        }

        /// <summary>
        /// rotate the sun 360 degrees around the x-axis according to the current time of day
        /// </summary>
        private void UpdateSun()
        {
            _sun.transform.localRotation = Quaternion.Euler((currentTime * 360f) - 90f, 140f, 0f);

            float intensityMultiplier = 1.5f;
            if (currentTime <= 0.25f || currentTime >= 0.75f)
                intensityMultiplier = 0;
            else if (currentTime >= 0.25f)
                intensityMultiplier = Mathf.Clamp01((currentTime - 0.25f) * 50f);
            else if (currentTime <= 0.75f)
                intensityMultiplier = Mathf.Clamp01((currentTime - 0.75f) * 50f);


            _sun.intensity = _sunInitialIntensity * intensityMultiplier;

            UpdateRoadLamps(1 - intensityMultiplier);
        }

        private void UpdateRoadLamps(float intensityMultiplier)
        {
            intensityMultiplier = intensityMultiplier <= .5f ? 0f : 1f;

            foreach (var lamp in _roadLamps)
                lamp.intensity = 10 * intensityMultiplier;
        }
    }
}