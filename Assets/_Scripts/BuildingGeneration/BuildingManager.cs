using System;
using _Scripts.BuildingGeneration.Parts;
using UnityEngine;

namespace _Scripts.BuildingGeneration
{
    public class BuildingManager:MonoBehaviour
    {

       [SerializeField] private GameObject[] rooms;


        private void Start()
        {
            Room.roomPrefabs = rooms;
            Building.Generate(new Vector3(10,5,10),Vector3.zero );
        }
    }
}