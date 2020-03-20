using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.NewBuildingGeneration
{
    public class Room
    {
        private readonly GameObject[] _walls;
        private readonly GameObject[] _roofs;

        public Room(GameObject[] walls, GameObject[] roofs)
        {
            _walls = walls;
            _roofs = roofs;
        }

        public Boo.Lang.List<GameObject> CreateWalls(Vector3 pos, bool front, bool right, bool back, bool left)
        {
            Boo.Lang.List<GameObject> walls = new Boo.Lang.List<GameObject>();
            //left
            if (left)
                walls.Add(Object.Instantiate(_walls.GetRandomFrom(),
                    new Vector3(-.5f, 0, -.5f) + pos,
                    Quaternion.Euler(0, 0, 0)));
            //front
            if (front)
                walls.Add(Object.Instantiate(_walls.GetRandomFrom(),
                    Vector3.zero + pos,
                    Quaternion.Euler(0, 270, 0)));
            //right
            if (right)
                walls.Add(
                    Object.Instantiate(_walls.GetRandomFrom(),
                        new Vector3(-.5f, 0, .5f) + pos,
                        Quaternion.Euler(0, 180, 0)));
            //back
            if (back)
                walls.Add(Object.Instantiate(_walls.GetRandomFrom(),
                    new Vector3(-1f, 0, 0) + pos,
                    Quaternion.Euler(0, 90, 0)));

            return walls;
        }

        public GameObject CreateRoof(Vector3 pos)
        {
            return Object.Instantiate(_roofs.GetRandomFrom(),
                new Vector3(-.5f, .5f, 0) + pos,
                Quaternion.Euler(90, 0, 0));
        }
    }
}