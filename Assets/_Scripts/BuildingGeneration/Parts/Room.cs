using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.BuildingGeneration.Parts
{
    public static class Room
    {
        public static GameObject[] roomPrefabs;


        public static void Render(GameObject parent, Vector3 pos)
        {
            GameObject go;
            Vector3 goPosition = pos;
            if (pos.y != 0)
            {
                GameObject pre = parent.transform.Find("" + new Vector3(pos.x, pos.y - 1, pos.z)).gameObject;
                Transform origin = pre.transform;
                Mesh mesh = pre.GetComponentInChildren<MeshFilter>().mesh;
                Vector3 localScale = origin.localScale;
                goPosition = new Vector3(goPosition.x, origin.position.y + (mesh.bounds.size.y * localScale.y),
                    goPosition.z);
            }

            go = Object.Instantiate(roomPrefabs.GetRandomFrom(), goPosition, Quaternion.identity);
            go.transform.SetParent(parent.transform);
            go.name = "" + pos;
        }

        private static GameObject InstantiateNextTo(GameObject prefab, GameObject preObject, Quaternion quaternion)
        {
            Transform origin = preObject.transform;
            Mesh mesh = preObject.GetComponentInChildren<MeshFilter>().mesh;
            var localScale = origin.localScale;
            Vector3 pos = new Vector3(
                mesh.bounds.size.x * localScale.x,
                mesh.bounds.size.y * localScale.y,
                mesh.bounds.size.z * localScale.z);
            return Object.Instantiate(prefab, origin.position + pos, quaternion);
        }
    }

    public enum RoomType
    {
        SIMPLE,
        DOOR,
        WINDOW
    }
}