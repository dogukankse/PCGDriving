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
    }

    private void DrawMap(MapTile[,] map)
    {
        grid.cellSize = new Vector2(1000f / size, 1000f / size);
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