using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MapGenerator : MonoBehaviour
{
    private Map _map;
    public int size;
    public TextMeshProUGUI text;
    public Tile[] tiles;

    public GridLayoutGroup grid;
    public Texture2D[] textures;

    private void Start()
    {
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
            return Resources.Load($"Prefabs/{0}") as GameObject;
        }

        return Instantiate(loadedObject as GameObject);
    }

    private void DrawRoads(MapTile[,] map)
    {
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
                road.transform.rotation = Quaternion.Euler(road.transform.rotation.eulerAngles+new Vector3(0,90,0)); 

            }
        }
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