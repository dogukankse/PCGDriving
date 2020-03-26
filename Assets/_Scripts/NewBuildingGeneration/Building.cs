using UnityEngine;

namespace _Scripts.NewBuildingGeneration
{
    public class Building
    {
        private readonly GameObject[] _walls;
        private readonly GameObject[] _roofs;
        private readonly bool _indoors;
        private Room room;
        private bool _hasDoor = false;

        public Building(GameObject[] walls, GameObject[] roofs, bool indoors)
        {
            _walls = walls;
            _roofs = roofs;
            _indoors = indoors;
            room = new Room(_walls, _roofs,_hasDoor);
        }


        public void CreateBuilding(int width, int height, int depth)
        {
            GameObject building = new GameObject("Building");
            for (int y = 0; y < height; y++)
            {
                for (int z = -depth / 2; z < depth / 2; z++)
                {
                    for (int x = -width / 2; x < width / 2; x++)
                    {
                        Vector3 pos = new Vector3(x, y, z);
                        GameObject go = new GameObject($"Room {pos}");

                        if (!_indoors)
                        {
                            if (x == width - 1)
                                foreach (GameObject roomObject in room.CreateWalls(pos, true, true, false, false))
                                    roomObject.transform.SetParent(go.transform);
                            else if (x == 0)
                                foreach (GameObject roomObject in room.CreateWalls(pos, false, false, true, true))
                                    roomObject.transform.SetParent(go.transform);
                            else
                                foreach (GameObject roomObject in room.CreateWalls(pos, true, false, true, true))
                                    roomObject.transform.SetParent(go.transform);
                        }
                        else
                        {
                            foreach (GameObject roomObject in room.CreateWalls(pos, true, true, true, true))
                                roomObject.transform.SetParent(go.transform);
                        }

                        if (y == height - 1)
                            room.CreateRoof(pos).transform.SetParent(go.transform);

                        go.transform.SetParent(building.transform);
                    }
                }
            }
        }
    }
}