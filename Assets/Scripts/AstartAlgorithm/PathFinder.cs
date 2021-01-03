using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : TSingleton<PathFinder>
{
    //private Map _map;
    //private TileSort _compare = new TileSort();

    //public override void Init()
    //{
    //    //_map = FindObjectOfType<Map>();
    //}

    //public void SetMap(Map map)
    //{
    //    _map = map;
    //}

    //private int Distance(Tile a, Tile b)
    //{
    //    int y = Mathf.Abs(a.Y - b.Y);
    //    int x = Mathf.Abs(a.X - b.X);

    //    return Mathf.Min(x, y) * 14 + Mathf.Abs(x - y) * 10;
    //}

    //public List<Tile> PathFinding(Tile start, Tile target, string str = "")
    //{
    //    //if (target.TILETYPE == TileType.Wall)
    //    //return null;

    //    if (start == null || target == null)
    //        return null;

    //    //if (CheckTileSurrond(target.Position))
    //        //return null;

    //    List<Tile> path = new List<Tile>();
    //    List<Tile> openList = new List<Tile>();
    //    List<Tile> closedList = new List<Tile>();

    //    Tile curr = start;
    //    curr.GCOST = 0;
    //    openList.Add(curr);

    //    int count = 0;

    //    while (curr != target)
    //    {
    //        if (count >= _map._width * _map._height)
    //            break;
    //        count++;

    //        Tile[] neighbor = Neighbor(curr);

    //        for (int i = 0; i < neighbor.Length; i++)
    //        {
    //            if (closedList.Contains(neighbor[i]))
    //                continue;


    //            TileType type = neighbor[i].TILETYPE;

    //            if (str == "Attack")
    //            {
    //                if (type == TileType.Wall || type == TileType.Hole)
    //                    continue;
    //            }
    //            //else if (str == "Move")
    //            //{
    //            //    if (type == TileType.Wall || type == TileType.Hole || type == TileType.Monster)
    //            //        continue;
    //            //}
    //            else
    //            {
    //                if (type == TileType.Wall || type == TileType.Hole || type == TileType.Monster)
    //                    continue;
    //            }

    //            int gCost = curr.GCOST + Distance(curr, neighbor[i]);

    //            if (!openList.Contains(neighbor[i]) || gCost < neighbor[i].GCOST)
    //            {
    //                neighbor[i].GCOST = gCost;
    //                neighbor[i].HCOST = Distance(neighbor[i], target);
    //                neighbor[i].PARENT = curr;
    //                openList.Add(neighbor[i]);
    //            }
    //        }

    //        if (!closedList.Contains(curr))
    //            closedList.Add(curr);

    //        if (openList.Contains(curr))
    //            openList.Remove(curr);

    //        if (openList.Count > 0)
    //        {
    //            openList.Sort(_compare);
    //            curr = openList[0];
    //        }
    //    }

    //    if (curr == target)
    //        path = PathTrace(start, target);

    //    return path;
    //}

    //private List<Tile> PathTrace(Tile start, Tile target)
    //{
    //    List<Tile> path = new List<Tile>();
    //    Tile curr = target;

    //    while (curr != start)
    //    {
    //        if (curr.PARENT == null)
    //            break;

    //        path.Add(curr);
    //        curr = curr.PARENT;
    //    }

    //    path.Reverse();

    //    Queue<Tile> qTile = new Queue<Tile>();

    //    return path;
    //}

    //private Tile[] Neighbor(Tile tile)
    //{
    //    List<Tile> neighbor = new List<Tile>();

    //    for (int x = -1; x < 2; x++)
    //    {
    //        for (int y = -1; y < 2; y++)
    //        {
    //            if (x == 0 && y == 0)
    //                continue;
    //            Tile neighborTile = _map._tiles[tile.X + x, tile.Y + y];

    //            if (neighborTile != null && neighborTile.TILETYPE != TileType.Wall)
    //                neighbor.Add(_map._tiles[tile.X + x, tile.Y + y]);
    //        }
    //    }
    //    return neighbor.ToArray();
    //}


    //private bool CheckTileSurrond(Vector2Int target)
    //{
    //    for (int x = -1; x < 2; x++)
    //        for (int y = -1; y < 2; y++)
    //        {
    //            if (x == 0 && y == 0)
    //                continue;

    //            Tile tile = _map._tiles[target.x + x, target.y + y];
    //            if (tile != null && tile.TILETYPE == TileType.Floor)
    //                return false;
    //        }
    //    return true;
    //}
}
