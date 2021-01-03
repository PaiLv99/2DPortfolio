using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public enum MazeDir { North, East, South, West, Max }
    Map _map;

    public Maze(Map map)
    {
        _map = map;
    }

    public void Digging(int x, int y)
    {
        if (x < 1 || x >= _map._width - 1 || y < 1 || y >= _map._height - 1)
            return;

        int openDirCount = 0;

        for (int dy = -1; dy <= 1; dy += 2)
        {
            Tile tile = _map.GetTile(x, y + dy);
            if (tile == null || tile.ID == 0)
                openDirCount++;
        }

        for (int dx = -1; dx <= 1; dx += 2)
        {
            Tile tile = _map.GetTile(x + dx, y);
            if (tile == null || tile.ID == 0)
                openDirCount++;
        }

        if (openDirCount < 3)
            return;

        {
            Tile tile = _map.GetTile(x, y);
            tile.ID = _map._roomID;
        }

        List<MazeDir> dirs = new List<MazeDir>();

        if ( y % 2 == 1 )
        {
            dirs.Add(MazeDir.East);
            dirs.Add(MazeDir.West);
        }

        if ( x % 2 == 1)
        {
            dirs.Add(MazeDir.North);
            dirs.Add(MazeDir.South);
        }

        while (dirs.Count > 0)
        {
            MazeDir dir = dirs[Random.Range(0, dirs.Count)];
            switch(dir)
            {
                case MazeDir.North: Digging(x, y + 1); break;
                case MazeDir.East: Digging(x + 1, y); break;
                case MazeDir.South: Digging(x, y - 1); break;
                case MazeDir.West: Digging(x - 1, y); break;
            }
            dirs.Remove(dir);
        }
        return;
    }
}


