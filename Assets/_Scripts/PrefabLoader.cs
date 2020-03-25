using UnityEngine;

namespace _Scripts
{
    public class PrefabLoader
    {
        private GameObject GetPrefab(string path)
        {
            GameObject loadedObject = Resources.Load(path) as GameObject;
            if (loadedObject == null)
            {
                return Resources.Load("Prefabs/Error") as GameObject;
            }

            return loadedObject;
        }

        public static class Folder
        {
            public static readonly string Roads = "Prefabs/Roads/";
            public static readonly string Cars = "Prefabs/Cars/";
            public static readonly string BuildingRoofs = "Prefabs/Building/Roofs";
            public static readonly string BuildingWalls = "Prefabs/Building/Walls";
        }
    }
}