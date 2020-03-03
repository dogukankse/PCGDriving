using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

enum TileType
{
    EMPTY,
    INTERSECTION_T,
    INTERSECTION_X,
    TURNING,
    STRAIGHT
}

public class Map
{
    public MapTile[,] map;
    private int size;
    private Dictionary<TileType, int> typeLimits = new Dictionary<TileType, int>();
    private Dictionary<TileType, int> typeCount = new Dictionary<TileType, int>();
    private int intersectionMinDistance = 1;
    private int turningMinDistance = 1;
    TileType[] intersectionTypes = {TileType.INTERSECTION_T, TileType.INTERSECTION_X};

    public Map(int size)
    {
        this.size = size;
        SetTypeLimits(size);
        map = new MapTile[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                map[x, y] = new MapTile();
            }
        }
    }

    private int InAnyRadius(int x, int y, int radius, TileType[] types)
    {
        int startX = inLimits(x - radius);
        int startY = inLimits(y - radius);

        int endX = inLimits(x + radius);
        int endY = inLimits(y + radius);

        int count = 0;
        for (int i = startX; i < endX; i++)
        {
            if (map[i, y].tile != null && types.Contains(getTypeOf(map[i, y].tile.id)))
            {
                count++;
            }
        }

        for (int j = startY; j < endY; j++)
        {
            if (map[x, j].tile != null && types.Contains(getTypeOf(map[x, j].tile.id)))
            {
                count++;
            }
        }

        return count;
    }

    private int inLimits(int x)
    {
        if (x < 0) return 0;
        if (x >= size) return size - 1;
        return x;
    }

    private void SetTypeLimits(int size)
    {
        typeLimits[TileType.EMPTY] = -1;
        typeLimits[TileType.INTERSECTION_T] = -1;
        typeLimits[TileType.INTERSECTION_X] = -1;
        typeLimits[TileType.TURNING] = -1;
        typeLimits[TileType.STRAIGHT] = -1;

        typeCount[TileType.EMPTY] = 0;
        typeCount[TileType.INTERSECTION_T] = 0;
        typeCount[TileType.INTERSECTION_X] = 0;
        typeCount[TileType.TURNING] = 0;
        typeCount[TileType.STRAIGHT] = 0;
    }

    private int RandomId(List<int> tls, int x, int y)
    {
        List<int> tiles = new List<int>(tls); //copy of tls

        int index = RandomSelection(tiles);
        TileType type = getTypeOf(tiles[index]);
        if (intersectionTypes.Contains(type))
        {
            //check minimum distance rule
            if (InAnyRadius(x, y, intersectionMinDistance, intersectionTypes) > 0)
            {
                tiles.RemoveAll(p => getTypeOf(p) == type); //remove the type
            }

            index = RandomSelection(tiles); //reselect
            type = getTypeOf(tiles[index]);
        }

        IncreaseType(type);
        return tiles[index];
    }

    private int RandomSelection(List<int> tiles)
    {
        float[] weights = tiles.Select(p => getWeightOf(p)).ToArray();
        float total = weights.Sum();
        float randomWeight = Random.Range(0f, total);
        int index = 0;
        while (randomWeight > 0 && index < tiles.Count)
        {
            randomWeight = randomWeight - weights[index];
            index++;
        }

        if (index > 0) 
            return index - 1;
        else 
            return 0;
    }

    private void IncreaseType(TileType type)
    {
        typeCount[type]++;
    }

    private TileType getTypeOf(int id)
    {
        if (id == 1 || id == 2) return TileType.STRAIGHT;
        if (id == 3) return TileType.INTERSECTION_X;
        if (id == 4 || id == 5 || id == 6 || id == 7) return TileType.TURNING;
        if (id == 8 || id == 9 || id == 10 || id == 11) return TileType.INTERSECTION_T;
        return TileType.EMPTY;
    }

    private float getWeightOf(int id)
    {
        return getWeightOf(getTypeOf(id));
    }

    private float getWeightOf(TileType type)
    {
        if (typeLimits[type] != -1 && typeCount[type] >= typeLimits[type])
        {
            return 0f;
        }

        switch (type)
        {
            case TileType.EMPTY:
                return 1f;
            case TileType.TURNING:
                return 2f;
            case TileType.STRAIGHT:
                return 8f;
            case TileType.INTERSECTION_T:
                return 2f;
            case TileType.INTERSECTION_X:
                return 1f;
        }

        return 0f;
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