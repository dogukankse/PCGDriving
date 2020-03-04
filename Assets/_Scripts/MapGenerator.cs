using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour
{
    private Map _map;
    public int size;
    public TextMeshProUGUI text;
    public Tile[] tiles;
    public int seed = 0;
    public GridLayoutGroup grid;
    public Texture2D[] textures;
    public GameObject[,] roads;
    public GameObject automobile;
    private void Start()
    {
        Random.seed = seed;
        _map = new Map(size);
        foreach (Tile tile in tiles)
        {
            foreach (string s in tile.downS.Split(' ').ToList())
            {
                tile.down.Add(int.Parse(s));
            }

            foreach (string s in tile.leftS.Split(' ').ToList())
            {
                tile.left.Add(int.Parse(s));
            }
        }

        _map.Generate(tiles);
        PrintMap(_map.map);
        DrawMap(_map.map);
        DrawRoads(_map.map);
    }

    private void DrawMap(MapTile[,] map)
    {
        grid.cellSize = new Vector2(120f / size, 120f / size);
        grid.constraintCount = size;

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                Tile tile = map[x, y].tile;
                GameObject go = new GameObject(tile.id.ToString());
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                RectTransform rt = go.AddComponent<RectTransform>();
                rt.pivot = Vector2.zero;
                Texture2D t = textures[tile.id];
                sr.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = grid.cellSize;
                go.transform.SetParent(grid.transform);
            }
        }
    }

    private GameObject GetRoadPrefab(Tile tile)
    {
        var loadedObject = Resources.Load($"Prefabs/{tile.id}");
        if (loadedObject == null)
        {
            return Resources.Load("Prefabs/0") as GameObject;
        }

        return Instantiate(loadedObject as GameObject);
    }

    private int DrawRoads(MapTile[,] map)
    {
        List<TrafficSystemNode> unconnectedNodes = new List<TrafficSystemNode>();
        roads = new GameObject[map.GetLength(0), map.GetLength(1)];
        float size = 0f;
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                Tile tile = map[x, y].tile;

                GameObject road = GetRoadPrefab(tile);

                if (size == 0d)
                {
                    GameObject ground = GameObject.Find("Ground");
                    size = ground.GetComponent<Collider>().bounds.size.x;
                }

                road.transform.position = new Vector3(y * size, 0, x * size);
                road.transform.rotation = Quaternion.Euler(road.transform.rotation.eulerAngles + new Vector3(0, 90, 0));
                road.transform.SetParent(transform);
                roads[x, y] = road;
                var nodes = road.GetComponentsInChildren<TrafficSystemNode>().Where(
                    p => p.m_connectedNodes.Count == 0 &&
                         p.m_roadType == TrafficSystem.RoadType.LANES_2).ToList();
                unconnectedNodes.AddRange(nodes);
            }
        }

        foreach (var node in unconnectedNodes)
        {
            if (node.m_connectedNodes.Count == 0)
            {
                var nearestNode = getNearestNode(node);
                if (nearestNode == null)
                {
                    continue;
                }
                
                node.AddConnectedNode(nearestNode);
                nearestNode.m_driveSide = node.m_driveSide;
                
            }
        }

        automobile.GetComponent<TrafficSystemVehiclePlayerAuto>().m_nextNode = unconnectedNodes[9];
        automobile.transform.position = unconnectedNodes[9].transform.position;
        
        return 0;
    }

    private TrafficSystemNode getNearestNode(TrafficSystemNode n)
    {
        var nodes = GetComponentsInChildren<TrafficSystemNode>().Where(p =>
            p.m_roadType == TrafficSystem.RoadType.LANES_2 &&
            p.transform.position != n.transform.position &&
            p.m_isPrimary &&
            p.transform.parent.parent.parent.gameObject != n.transform.parent.parent.parent.gameObject &&
            !p.m_connectedNodes.Contains(n) &&
            !n.m_connectedNodes.Contains(p) &&
            p.m_connectedNodes.Count == 1
        ).ToList();
        TrafficSystemNode bestNode = nodes[0];
        float bestDistance = 99999;
        foreach (var node in nodes)
        {
            var distance = Vector3.Distance(node.transform.position, n.transform.position);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestNode = node;
            }
        }
        
        return bestNode;
    }

    private void PrintMap(MapTile[,] map)
    {
        for (int y = 0;
            y < map.GetLength(0);
            y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                text.text += map[x, y].tile.id + " ";
            }

            text.text += "\n";
        }
    }
}