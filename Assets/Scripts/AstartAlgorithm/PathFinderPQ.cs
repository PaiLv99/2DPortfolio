using System;
using System.Collections.Generic;
using UnityEngine;

public struct PQNode : IComparable<PQNode>
{
    public int F, H, G, X, Y;

    public int CompareTo(PQNode other)
    {
        if (F < other.F) return 1;
        if (F > other.F) return -1;

        if (F == other.F)
        {
            if (H < other.H) return 1;
            if (H > other.H) return -1;
        }
        return 0;
    }
}

public class PathFinderPQ
{
    private Map _map;
    private int[] _deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };
    private int[] _deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
    private int[] _cost   = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };

    private bool[,] _closedList; 
    private int[,] _openList;

    public void SetMap(Map map)
    {
        _map = map;

        _closedList = new bool[_map._width, _map._height];
        _openList = new int[_map._width, _map._height];
    }

    public List<Tile> PathFind(Tile start, Tile target)
    {
        InitNode();

        PriorityQueue<PQNode> pQueue = new PriorityQueue<PQNode>();

        int startH = CalcDistance(target, start);

        pQueue.Push(new PQNode() { F = 0 + startH, H = startH, G = 0, X = start.X, Y = start.Y });
        _openList[start.X, start.Y] = startH;
        start.PARENT = null;

        while (pQueue.Count > 0)
        {
            PQNode node = pQueue.Pop();

            if (_closedList[node.X, node.Y])
                continue;

            _closedList[node.X, node.Y] = true;

            if (node.X == target.X && node.Y == target.Y)
                break;

            for (int i = 0; i < _deltaX.Length; i++)
            {
                int neighborX = node.X + _deltaX[i];
                int neighborY = node.Y + _deltaY[i];

                if (neighborX < 0 || neighborX >= _map._width || neighborY < 0 || neighborY >= _map._height)
                    continue;

                if (CheckType(neighborX, neighborY))
                    continue;

                if (_closedList[neighborX, neighborY])
                    continue;

                int g = node.G + _cost[i];
                int h = CalcDistance(target, _map._tiles[neighborX, neighborY]);

                if (_openList[neighborX, neighborY] < g + h)
                    continue;

                _openList[neighborX, neighborY] = g + h;
                pQueue.Push(new PQNode() { F = g + h, H = h, G = g, X = neighborX, Y = neighborY });
                _map._tiles[neighborX, neighborY].PARENT = _map._tiles[node.X, node.Y];
            }
        }

        return PathTrace(target);
    }

    private int CalcDistance(Tile a, Tile b)
    {
        int y = Mathf.Abs(a.Y - b.Y);
        int x = Mathf.Abs(a.X - b.X);

        return Mathf.Min(x, y) * 14 + Mathf.Abs(x - y) * 10;
    }

    private bool CheckType(int x, int y)
    {
        TileType type = _map._tiles[x, y].TILETYPE;

        switch(type)
        {
            case TileType.Monster: return true;
            case TileType.Hole: return true;
            case TileType.Wall: return true;
        }

        return false;
    }

    private List<Tile> PathTrace(Tile target)
    {
        List<Tile> path = new List<Tile>();
        Tile curr = target;

        while (true)
        {
            if (curr.PARENT == null)
                break;

            path.Add(curr);
            curr = curr.PARENT;
        }

        path.Reverse();
        return path;
    }

    private void InitNode()
    {
        for (int x = 0; x < _map._width; x++)
            for (int y = 0; y < _map._height; y++)
            {
                _openList[x, y] = int.MaxValue;
                _closedList[x, y] = false;
                if (_map._tiles[x, y] != null)
                    _map._tiles[x, y].PARENT = null;
            }
    }
}



