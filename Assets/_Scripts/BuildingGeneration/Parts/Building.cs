using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.BuildingGeneration.Parts
{
    public static class Building
    {
        private static GameObject building;

        public static void Generate(Vector3 bounds, Vector3 center)
        {
            building = new GameObject("Building");

            Render(building, bounds);
            building = null;
        }

        private static void Render(GameObject parent, Vector3 bounds)
        {
            for (int i = 0; i < bounds.y; i++)
            {
                for (int j = 0; j < bounds.z; j++)
                {
                    for (int k = 0; k < bounds.x; k++)
                    {
                        if (j == 0 || j == bounds.z - 1 || k == 0 || k == bounds.x - 1)
                            Room.Render(parent, new Vector3(k, i, j));
                    }
                }
            }
        }
    }
}