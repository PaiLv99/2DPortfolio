using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Decoration
{
    private Map _map;
    private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    //private TileSort _compare = new TileSort();

    public Decoration(Map map)
    {
        _map = map;
        _sprites = GameMng.Map._sprites;
    }

    public void BuildDecoration(Room room)
    {
        int width = room._width;
        int height = room._height;

        if (width <= 5 || height <= 5)
            return;

        if (width >= 7 && height >= 7)
        {
            List<Tile> doors = FindDoor(room);

            if (doors.Count > 2)
            {
                //int rand = Random.Range(0, 2);
                //if (rand >= 1)
                //    Maze(room, doors);
                //else
                    //Hole(room);
            }
            else
            {
                int rand = Random.Range(0, 4);
                switch (rand)
                {
                    case 0: Hole(room); break;
                    case 1: Column(room); break;
                    case 2: Wall(room); break;
                    case 3: break;
                }
            }
        }
    }

    private void Hole(Room room)
    {
        room._isDeco = true;
        int interval = 3;

        for (int x = room._x + interval; x < room._xMax - interval; x++)
        {
            for (int y = room._y + interval; y < room._yMax - interval; y++)
            {
                _map._tiles[x, y].SetSprite(_sprites["HoleCenter"]);
                _map._tiles[x, y].TILETYPE = TileType.Hole;
                if (x == room._x + interval)
                    _map._tiles[x, y].SetSprite(_sprites["HoleWest"]);
                if (x == room._xMax - interval - 1)
                    _map._tiles[x, y].SetSprite(_sprites["HoleEast"]);
                if (y == room._y + interval)
                    _map._tiles[x, y].SetSprite(_sprites["HoleSouth"]);
                if (y == room._yMax - interval - 1)
                    _map._tiles[x, y].SetSprite(_sprites["HoleNorth"]);

                if (x == room._x + interval && y == room._y + interval)
                    _map._tiles[x, y].SetSprite(_sprites["HoleSW"]);
                if (x == room._xMax - interval - 1&& y == room._y + interval)
                    _map._tiles[x, y].SetSprite(_sprites["HoleSE"]);

                if (x == room._x + interval && y == room._yMax - interval - 1)
                    _map._tiles[x, y].SetSprite(_sprites["HoleNW"]);
                if (x == room._xMax - interval - 1&& y == room._yMax - interval - 1) 
                    _map._tiles[x, y].SetSprite(_sprites["HoleNE"]);
            }
        }
    }

    // Tile change without path tile
    //private void Maze(Room room, List<Tile> doors)
    //{
    //    room._isDeco = true;

    //    Dictionary<Vector2, Tile> randomTile = new Dictionary<Vector2, Tile>();

    //    for (int x = room._x; x < room._xMax; x++)
    //        for (int y = room._y; y < room._yMax; y++)
    //            ChangeTile(x, y, "HoleCenter", TileType.Hole);

    //    // Esential Path
    //    for (int i = 0; i < doors.Count; i++)
    //    {
    //        if (i == doors.Count)
    //            break;

    //        List<Tile> path = new List<Tile>();
    //        Tile start = doors[i];
    //        Tile target = doors[i + 1];

    //        if (start != null && target != null)
    //            path = TilePathFinder.Instance.PathFinding(start, target, false);

    //        if (path != null)
    //            for (int j = 0; j < path.Count; j++)
    //                if (path[j].TILETYPE == TileType.Hole)
    //                    ChangeTile(path[j].X, path[j].Y, "Tile", TileType.Floor);

    //        // start tile select 
    //        int rand = Random.Range(0, path.Count);
    //        Tile tile = path[rand];

    //        if (!randomTile.ContainsKey(new Vector2(tile.X, tile.Y)))
    //            randomTile.Add(new Vector2(tile.X, tile.Y), tile);

    //        // tile adjacent 
    //        List<Tile> adjacent = GetAdjacentTile(tile.X, tile.Y, randomTile);
    //        rand = Random.Range(0, adjacent.Count);
    //        Tile next = new Tile();
    //        // next tile while roop 
    //        if (adjacent.Count <= 0)
    //            return;
             
    //        next = adjacent[rand];
    //        while (true)
    //        {
    //            adjacent = GetAdjacentTile(next.X, next.Y, randomTile);

    //            if (adjacent.Count <= 0)
    //                break;

    //            // next tile select;
    //            rand = Random.Range(0, adjacent.Count);
    //            next = adjacent[rand];
    //            randomTile.Add(new Vector2(next.X, next.Y), next);
    //        }

    //        foreach (var iter in randomTile)
    //        {
    //            Tile value = iter.Value;
    //            ChangeTile(value.X, value.Y, "Tile", TileType.Floor);
    //        }
    //    }
    //    // Random Path
    //}

    private List<Tile> GetAdjacentTile(int x, int y, Dictionary<Vector2, Tile> prevTile)
    {
        List<Tile> adjacent = new List<Tile>();

        if (!prevTile.ContainsKey(new Vector2(x + 1, y)) && _map._tiles[x + 1, y].TILETYPE == TileType.Hole)
            adjacent.Add(_map._tiles[x + 1, y]);
        if (!prevTile.ContainsKey(new Vector2(x - 1, y)) && _map._tiles[x - 1, y].TILETYPE == TileType.Hole)
            adjacent.Add(_map._tiles[x -1, y]);
        if (!prevTile.ContainsKey(new Vector2(x, y + 1)) && _map._tiles[x, y + 1].TILETYPE == TileType.Hole)
            adjacent.Add(_map._tiles[x, y + 1]);
        if (!prevTile.ContainsKey(new Vector2(x, y - 1)) && _map._tiles[x, y - 1].TILETYPE == TileType.Hole)
            adjacent.Add(_map._tiles[x, y - 1]);

        return adjacent;
    }

    private void Column(Room room)
    {
        room._isDeco = true;

        if (5 > Random.Range(0, 10))
        {
            for (int x = room._x + 1; x < room._xMax; x++)
            {
                for (int y = room._y + 1; y < room._yMax; y++)
                {
                    if (x == room._x + 1 && y == room._y + 1)
                    {
                        ChangeTile(x, y, "ColumnFront", TileType.Wall);
                        ChangeTile(x, y + 1, "ColumnTop", TileType.Wall);
                    }

                    if (x == room._xMax - 2 && y == room._yMax - 2)
                    {
                        ChangeTile(x, y, "ColumnTop", TileType.Wall);
                        ChangeTile(x, y - 1, "ColumnFront", TileType.Wall);
                    }
                }
            }
        }
        else
        {
            for (int x = room._x + 1; x < room._xMax; x++)
            {
                for (int y = room._y + 1; y < room._yMax; y++)
                {
                    if (y == room._yMax - 2 && x == room._x + 1)
                    {
                        ChangeTile(x, y - 1, "ColumnFront", TileType.Wall);
                        ChangeTile(x, y, "ColumnTop", TileType.Wall);
                    }

                    if (y == room._y + 1 && x == room._xMax - 2)
                    {
                        ChangeTile(x, y, "ColumnFront", TileType.Wall);
                        ChangeTile(x, y + 1, "ColumnTop", TileType.Wall);
                    }
                }
            }
        }
    }

    private void Wall(Room room)
    {
        room._isDeco = true;

        List<Tile> doors = FindDoor(room);
        // Find Door position 
        for (int i = 0; i < doors.Count; i++)
            if (doors[i].TILETYPE == TileType.SideDoor)
                return;       

        // Digging Wall
        int rand = Random.Range(room._y + 2, room._yMax - 2);
       
         if (_map.GetID(room._x - 2, rand + 1) != -1)
            //if (_map.GetID(room._x - 2, rand + 1) != -1)
                _map._tiles[room._x - 1, rand + 1].SetSprite(_sprites["WallBothCSW"]);
        else
            _map._tiles[room._x - 1, rand + 1].SetSprite(_sprites["WallCSW"]);

        for (int i = 0; i < 3; i++)
        {
            if (i == 2)
            {
                ChangeTile(room._x + i, rand, "WallNorth", TileType.Wall);
                ChangeTile(room._x + i, rand + 1, "WallSW", TileType.Wall);
            }
            else
            {
                ChangeTile(room._x + i, rand, "WallNorth", TileType.Wall);
                ChangeTile(room._x + i, rand + 1, "WallSouth", TileType.Wall);
            }

        }
            
        rand = Random.Range(room._y + 2, room._yMax - 2);
        for (int i = 3; i > 0; i--)
        {
            if (i == 3)
            {
                ChangeTile(room._xMax - i, rand, "WallNorth", TileType.Wall);
                ChangeTile(room._xMax - i, rand + 1, "WallSE", TileType.Wall);
            }
            else
            {
                ChangeTile(room._xMax - i, rand, "WallNorth", TileType.Wall);
                ChangeTile(room._xMax - i, rand + 1, "WallSouth", TileType.Wall);
            }
        }
        if (_map.GetID(room._xMax + 1, rand + 1) != -1)
            //if (_map._tiles[room._xMax + 1, rand + 1] != null)
                _map._tiles[room._xMax, rand + 1].SetSprite(_sprites["WallBothCSE"]);
        else
            _map._tiles[room._xMax, rand + 1].SetSprite(_sprites["WallCSE"]);
    }

    private void ChangeTile(int x, int y, string path, TileType type)
    {
        if (_map._tiles[x, y] != null)
        {
            _map._tiles[x, y].SetSprite(_sprites[path]);
            _map._tiles[x, y].TILETYPE = type;
        }
    }

    private List<Tile> FindDoor(Room room)
    {
        List<Tile> doors = new List<Tile>();
        //Debug.Log(room._x + "::" + room._y + "||" + room._xMax + "::" + room._yMax);

        for (int x = room._x; x < room._xMax; x++)
        {
            for (int y = room._y; y < room._yMax; y++)
            {
                if (_map.GetTile(x + 1, y).TILETYPE == TileType.SideDoor)
                    doors.Add(_map.GetTile(x + 1, y));
                if (_map.GetTile(x - 1, y).TILETYPE == TileType.SideDoor)
                    doors.Add(_map.GetTile(x - 1, y));
                if (_map.GetTile(x, y + 1).TILETYPE == TileType.FrontDoor)
                    doors.Add(_map.GetTile(x, y + 1));
                if (_map.GetTile(x, y - 1).TILETYPE == TileType.FrontDoor)
                    doors.Add(_map.GetTile(x, y - 1));
            }
        }
        return doors;
    }

}
