using UnityEngine;

namespace _Scripts.NewBuildingGeneration
{
    public class BuildingGenerator : MonoBehaviour
    {
        [Header("Prefabs")] [SerializeField] private GameObject[] _walls;
        [SerializeField] private GameObject[] _roofs;

        [Header("Settings")] [SerializeField] private int _width;
        [SerializeField] private int _depth;
        [SerializeField] private int _height;
        [SerializeField] private bool _indoors = false;

        private void Start()
        {
            Building building = new Building(_walls,_roofs,_indoors);
            building.CreateBuilding(_width,_height,_depth);
            
            
        }
        


    }
}