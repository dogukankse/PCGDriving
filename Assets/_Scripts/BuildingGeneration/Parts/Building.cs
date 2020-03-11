using UnityEngine;

namespace _Scripts.BuildingGeneration.Parts
{
    public class Building
    {
        private Vector2Int Size { get; }
        private Wing[] Wings { get; }

        public Building(int sizeX, int sizeY, Wing[] wings)
        {
            Size = new Vector2Int();
            Wings = wings;
        }

        public override string ToString()
        {
            string building = "Building:(" + Size + "; " + Wings.Length + ")\n";
            for (int i = 0; i < Wings.Length; i++)
            {
                building += "\t" + Wings[i] + "\n";
            }
            return building;
        }
    }
}