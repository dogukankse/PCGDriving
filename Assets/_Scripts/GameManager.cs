using System;
using UnityEngine;

namespace _Scripts
{
    public class GameManager:MonoBehaviour
    {
        private MapGenerator _mapGenerator;
        private SunController _sunController;
        
        private void Awake()
        {
            _mapGenerator = FindObjectOfType<MapGenerator>();
            _sunController = FindObjectOfType<SunController>();
        }

        private void Start()
        {
            _mapGenerator.AdjustRoadLamps = _sunController.FindRoadLamps;
        }
    }
}