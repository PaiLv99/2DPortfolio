using System.Collections.Generic;
using UnityEngine;

// Ray cast Algorithm
public class FoV 
{
    public Map Map { get; private set; }
    public int SightRadius { get; set; }

    private Dictionary<Vector2Int, Tile> _visibleTiles = new Dictionary<Vector2Int, Tile>();

    List<Vector2Int> mList = new List<Vector2Int>();

    public List<Vector2Int> List => mList;

    public FoV(Map map, int radius)
    {
        Map = map;
        SightRadius = radius;
    }

    public void SetMap(Map map)
    {
        Map = map;
    }

    public Dictionary<Vector2Int, Tile> GetVisibleTiles()
    {
        return _visibleTiles;
    }

    public void Compute(Vector2 position)
    {
        _visibleTiles.Clear();

        int dx = (int)position.x;
        int dy = (int)position.y;

        int left    = Mathf.Max(0, dx - SightRadius);
        int top     = Mathf.Max(0, dy - SightRadius);
        int right   = Mathf.Min(Map._width, dx + SightRadius + 1);
        int bottom  = Mathf.Min(Map._height, dy + SightRadius + 1);

        for (int y = top; y < bottom; y++)
            for (int x = left; x < right; x ++)
                RayCastWithBresenham(position, new Vector2Int(x, y), SightRadius);
    }   

    private void RayCastWithBresenham(Vector2 start, Vector2Int target, int radius)
    {
        Vector2 delta = target - start;

        Vector2 primaryStep = new Vector2(Mathf.Sign(delta.x), 0);
        Vector2 secondaryStep = new Vector2(0, Mathf.Sign(delta.y));

        int primary = (int)Mathf.Abs(delta.x);
        int secondary = (int)Mathf.Abs(delta.y);

        if (secondary > primary)
        {
            int temp = primary;
            primary = secondary;
            secondary = temp;

            Vector2 tempVector2 = primaryStep;
            primaryStep = secondaryStep;
            secondaryStep = tempVector2;
        }

        Vector2 curr = start;
        int error = 0;

        while (true)
        {
            if (Helper.DistanceVector2(curr, start) > SightRadius * SightRadius)
                break;

            int x = 0;
            int y = 0;

            if (curr.x <= Map._width || curr.x >= 0 || curr.y <= Map._height || curr.y >= 0)
            {
                x = (int)curr.x;
                y = (int)curr.y;
            }

            if (curr == target)
                break;

            if (Map._tiles[x,y] != null && !Map._tiles[x,y].Transparent )
            {
                break;
            }

            if (Map._tiles[x,y] != null)
            {
                if ( _visibleTiles.ContainsKey(Map._tiles[x, y].Position) == false )
                    _visibleTiles.Add(Map._tiles[x, y].Position, Map._tiles[x, y]);

                if( mList.Contains(Map.Tiles[x,y].Position) == false )
                {
                    mList.Add(Map.Tiles[x, y].Position);
                }
            }

            curr += primaryStep;
            error += secondary;

            if (error * 2 >= primary)
            {
                curr += secondaryStep;
                error -= primary;
            }
        }
    }
}
