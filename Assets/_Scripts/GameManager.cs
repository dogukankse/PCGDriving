using System;
using UnityEngine;

namespace _Scripts
{
    public class GameManager:MonoBehaviour
    {
        private MapGenerator _mapGenerator;
        private SunController _sunController;

        [SerializeField] private int seed;
        [SerializeField] private int size;
        
        private void Awake()
        {
            _mapGenerator = new MapGenerator(seed,size);
            _sunController = FindObjectOfType<SunController>();
        }

        private void Start()
        {
            _mapGenerator.AdjustRoadLamps = _sunController.FindRoadLamps;
            _mapGenerator.DrawRoads();

        }
    }
}