using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map
{
    public MapTile[,] map;
    private int size;


    public Map(int size)
    {
        this.size = size;
        map = new MapTile[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                map[x, y] = new MapTile();
            }
        }
    }

    private int intersection = 0;

    private int RandomId(List<int> tiles, int x, int y)
    {
        List<int> cpy = new List<int>(tiles);
        /*
        if (y == 0)
        {
            cpy.RemoveAll(p => new[] {2, 3, 6, 7, 8, 9, 10}.Contains(p));
        }

        if (y == size - 1)
        {
            cpy.RemoveAll(p => new[] {2, 3, 4, 5, 8, 10, 11}.Contains(p));
        }

        if (x == 0)
        {
            cpy.RemoveAll(p => new[] {1, 3, 5, 6, 8, 9, 11}.Contains(p));
        }

        if (x == size - 1)
        {
            cpy.RemoveAll(p => new[] {1, 3, 4, 7, 9, 10, 11}.Contains(p));
        }*/
        if (intersection >= size / 10)
        {
            cpy.RemoveAll(p => new int[] {3}.Contains(p));
        }

        int random = cpy[Random.Range(0, cpy.Count)];
        if (new int[] {3}.Contains(random))
        {
            intersection++;
        }

        return random;
    }


    public void Generate(Tile[] tiles)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x == y && x == 0) map[x, y].tile = tiles[Random.Range(0, 12)];
                else if (y == 0 && x > 0)
                {
                    var preTile = map[x - 1, y].tile;
                    int index = RandomId(preTile.left, x, y);
                    map[x, y].tile = tiles[index];
                }
                else if (x == 0 && y > 0)
                {
                    var preTile = map[x, y - 1].tile;
                    int index = RandomId(preTile.down, x, y);
                    map[x, y].tile = tiles[index];
                }
                else
                {
                    var preTileL = map[x - 1, y].tile;
                    var preTileD = map[x, y - 1].tile;
                    var intersect = preTileL.left.Intersect(preTileD.down).ToList();
                    int index = RandomId(intersect, x, y);
                    map[x, y].tile = tiles[index];
                }
            }
        }
    }
}