using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        private MapGenerator _mapGenerator;
        private SunController _sunController;

        [SerializeField] private int seed;
        [SerializeField] private int size;

        public GameObject[] trafficCars;
        public int trafficCount;

        private void Awake()
        {
            Random.seed = seed;
            _mapGenerator = new MapGenerator(seed, size);
            _sunController = FindObjectOfType<SunController>();
        }

        private void Start()
        {
            _mapGenerator.AdjustRoadLamps = _sunController.FindRoadLamps;
            var connectors = _mapGenerator.DrawRoads();
            InitTraffic(connectors.Where(p =>
                p.m_isPrimary &&
                !p.Parent.name.Contains("Intersection") &&
                Math.Abs((int) p.transform.rotation.eulerAngles.x) != 90
            ).ToList());
        }

        public void InitTraffic(List<TrafficSystemNode> nodes)
        {
            List<TrafficSystemNode> positions = new List<TrafficSystemNode>();

            for (int i = 0; i < trafficCount; i++)
            {
                int random = Random.Range(0, nodes.Count - 1);
                GameObject randomCar = trafficCars[i % trafficCars.Length];
                var node = nodes[random];
                if (positions.Contains(node))
                {
                    continue;
                }

                positions.Add(node);
                node.Parent.SpawnRandomVehicle(randomCar.GetComponent<TrafficSystemVehicle>(), node);
            }
        }
    }
}