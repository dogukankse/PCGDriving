using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        private MapGenerator _mapGenerator;
        private SunController _sunController;

        [SerializeField] private PedestrianSystem _pedestrianSystem;

        [SerializeField] private Camera _videoCam;

        [SerializeField] private int seed;
        [SerializeField] private int size;

        public GameObject[] trafficCars;
        public int trafficCount;


        public GameObject[] pedestrians;
        public int pedestrianCount;

        private GameObject _car;
        [SerializeField] private GameObject _player;

        private void Awake()
        {
           
            Random.InitState(seed);
            _mapGenerator = new MapGenerator(seed, size);
            _sunController = FindObjectOfType<SunController>();
        }

        private void Start()
        {
            _mapGenerator.AdjustRoadLamps = _sunController.FindRoadLamps;
            //_mapGenerator.CreatePlayer = InitPlayer;
            _mapGenerator.AfterCreation = AfterCreation;
            //node 1: road
            // node 2: pedestrian
            StartCoroutine(_mapGenerator.CreateRoads());
        }

     

        void AfterCreation()
        {
            (List<TrafficSystemNode>, List<PedestrianNode>) nodes = _mapGenerator.GetRoads();
            InitTraffic(nodes.Item1.Where(p =>
                p.m_isPrimary &&
                !p.Parent.name.Contains("Intersection") &&
                p.m_roadType == TrafficSystem.RoadType.LANES_2 &&
                Math.Abs((int) p.transform.rotation.eulerAngles.x) != 90
            ).ToList());
            InitPedestrian(nodes.Item2);
            _player.GetComponentInChildren<IOCcam>().layerMsk = ~0;
            Destroy(_videoCam.gameObject);
        }

        void InitPedestrian(List<PedestrianNode> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                /*GameObject go = nodes[i].gameObject;
                PedestrianObjectSpawner spawner = go.AddComponent<PedestrianObjectSpawner>();
                spawner.m_objectPrefabs.AddRange(_pedestrianSystem.m_objectPrefabs);
                spawner.m_nodeObjectSpawnChance = _pedestrianSystem.m_randomObjectSpawnChancePerNode;
                spawner.m_startNode = nodes[i];*/
            }
        }

        private void InitTraffic(List<TrafficSystemNode> nodes)
        {
            List<TrafficSystemNode> positions = new List<TrafficSystemNode>();

            for (int i = 0; i < trafficCount; i++)
            {
                int random = Random.Range(0, nodes.Count);
                GameObject randomCar = trafficCars.GetRandomFrom();
                var node = nodes[random];
                if (positions.Contains(node)) continue;
                positions.Add(node);
                if (node.m_connectedNodes.Count > 0)
                    positions.Add(node.m_connectedNodes.First());
                if (node.m_connectedLocalNode)
                    positions.Add(node.m_connectedLocalNode);
                var vehicle = randomCar.GetComponent<TrafficSystemVehicle>();
                node.Parent.SpawnRandomVehicle(vehicle, node);
            }
        }
    }
}