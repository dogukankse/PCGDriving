using System;
using System.Collections.Generic;
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

        public GameObject trafficCar;
        public int trafficCount;

        private void Awake()
        {
            _mapGenerator = new MapGenerator(seed, size);
            _sunController = FindObjectOfType<SunController>();
        }

        private void Start()
        {
            _mapGenerator.AdjustRoadLamps = _sunController.FindRoadLamps;
            var connectors = _mapGenerator.DrawRoads();
            InitTraffic(connectors);
        }

        public void InitTraffic(List<TrafficSystemNode> nodes)
        {
            for (int i = 0; i < trafficCount; i++)
            {
                int random = Random.Range(0, nodes.Count-1);
                var node = nodes[random];
                var car = Instantiate(trafficCar);
                car.transform.position = node.transform.position;
                car.transform.rotation = node.transform.rotation;
                car.GetComponent<TrafficSystemVehiclePlayerAuto>().m_nextNode = node;
            }
        }
    }
}