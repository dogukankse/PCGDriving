using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.AISystem;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        private MapGenerator _mapGenerator;
        private SunController _sunController;

        [SerializeField] private AIManager _aiManager;

        [Header("Video")] [SerializeField] private Camera _videoCam;
        [SerializeField] private Text _videoText;

        [SerializeField] private int seed;
        [SerializeField] private int size;

        public GameObject[] trafficCars;
        public int trafficCount;

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
            _player.GetComponentInChildren<Camera>().targetDisplay = 1;
            _player.GetComponentInChildren<AudioListener>().enabled = false;

            _videoCam.targetDisplay = 0;
            _sunController.enabled = false;
            _mapGenerator.AdjustRoadLamps = _sunController.FindRoadLamps;
            _mapGenerator.AfterCreation = AfterCreation;

            StartCoroutine(_mapGenerator.CreateRoads(_videoText));
        }


        private void AfterCreation()
        {
            var (trafficSystemNodes, pedestrianNodes) = _mapGenerator.GetRoads();
            InitTraffic(trafficSystemNodes.Where(p =>
                p.m_isPrimary &&
                !p.Parent.name.Contains("Intersection") &&
                p.m_roadType == TrafficSystem.RoadType.LANES_2 &&
                Math.Abs((int) p.transform.rotation.eulerAngles.x) != 90
            ).ToList());

            _player.GetComponentInChildren<IOCcam>().layerMsk = ~0;
            _player.GetComponentInChildren<Camera>().targetDisplay = 0;
            _player.GetComponentInChildren<AudioListener>().enabled = true;

            _sunController.enabled = true;
            InitPedestrian();


            Destroy(_videoCam.gameObject);
            Destroy(_videoText.gameObject);
        }

        private void InitPedestrian()
        {
            _aiManager.FindAIPoints();
            _aiManager.isCreationFinish = true;
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