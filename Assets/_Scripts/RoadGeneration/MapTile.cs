using UnityEngine;

public class MapTile
{
    public Tile tile;
    public GameObject road;

    public MapTile()
    {
    }

    public MapTile(Tile tile,GameObject road)
    {
        this.tile = tile;
        this.road = road;
    }
    
    

    public override string ToString()
    {
        if (tile == null)
        {
            return "-1";
        }

        return tile.id + "";
    }
}