using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.AISystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        private MapGenerator _mapGenerator;
        [SerializeField] private SunController _sunController;

        [SerializeField] private AIManager _aiManager;

        [Header("Video")] [SerializeField] private Camera _videoCam;
        [SerializeField] private Text _videoText;

        [SerializeField] private GridLayoutGroup _grid;

        public GameObject[] trafficCars;
        public int trafficCount;

        private GameObject _car;
        [Header("Player")] [SerializeField] private GameObject _player;

        [SerializeField] private Camera _playerCam;

        private void Awake()
        {
            Random.InitState(GameData.Seed);
            _mapGenerator = new MapGenerator(GameData.Seed, GameData.Size);
            
            // Cursor.visible = false;
            // Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
            _sunController.gameObject.SetActive(true);
            _mapGenerator.AdjustRoadLamps = _sunController.FindRoadLamps;
            _mapGenerator.AfterCreation = AfterCreation;
            StartCoroutine(_mapGenerator.CreateRoads(_videoText));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                GameObject o;
                (o = _videoCam.gameObject).SetActive(!_videoCam.gameObject.activeSelf);
                _playerCam.gameObject.SetActive(!o.activeSelf);
            }
            
            if(Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(0);

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

            _playerCam.GetComponent<AudioListener>().enabled = true;

            _sunController.enabled = true;
            _sunController.currentTime = Random.Range(0f, 1f);
            
            InitPedestrian();
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