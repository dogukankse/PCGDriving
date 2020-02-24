using System.Linq;
using UnityEngine;

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

    public void Generate(Tile[] tiles)
    {
        for (int y = 0;y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x == y && x == 0) map[x, y].tile = tiles[Random.Range(0, 12)];
                else if (y == 0 && x > 0)
                {
                    var preTile = map[x - 1, y].tile;
                    int index = preTile.left[Random.Range(0, preTile.left.Count)];
                    map[x, y].tile = tiles[index];
                }
                else if (x == 0 && y > 0)
                {
                    var preTile = map[x, y - 1].tile;
                    int index = preTile.down[Random.Range(0, preTile.down.Count)];
                    map[x, y].tile = tiles[index];
                }
                else
                {
                    var preTileL = map[x - 1, y].tile;
                    var preTileD = map[x, y - 1].tile;
                    var intersect = preTileL.left.Intersect(preTileD.down).ToList();
                    int index = intersect[Random.Range(0, intersect.Count)];
                    map[x, y].tile = tiles[index];
                }
            }
        }
    }
}