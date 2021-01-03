using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int _x, _y, _xMax, _yMax, _width, _height;
    private Map _map;

    public int roomID;

    public bool _isDeco = false;

    public Room(Map map)
    {
        _map = map;

        int x = Random.Range(1, map._width - map._max - 1);
        int y = Random.Range(1, map._height - map._max - 1);
        int w = Random.Range(map._min, map._max);
        int h = Random.Range(map._min, map._max);

        _x = x % 2 == 0 ? x + 1 : x;
        _y = y % 2 == 0 ? y + 1 : y;
        _width = w; 
        _height = h;

        _xMax = _x + _width;
        _yMax = _y + _height;

        _xMax = _xMax % 2 == 0 ? _xMax : _xMax  - 1;
        _yMax = _yMax % 2 == 0 ? _yMax : _yMax - 1;
   }

    public bool Intersert(Room room)
    {
        return _x <= room._xMax + 2 && _xMax >= room._x - 2 && _y <= room._yMax + 2 && _yMax >= room._y - 2;
    }

    // 생성된 타일에 아이디를 부과한다.
    public void TileIdChange()
    {
        for (int x = _x; x < _xMax; x++)
        {
            for (int y = _y; y < _yMax; y++)
            {
                Tile tile = _map.GetTile(x, y);
                tile.ID = _map._roomID;
                roomID = _map._roomID;
            }
        }
    }

    public void Connect()
    {
        Dictionary<Door.DoorType, List<Tile>> doorDic = new Dictionary<Door.DoorType, List<Tile>>();
        // Door Front
        for (int x = _x + 1; x < _xMax - 1; x++)
        {
            if (_map.GetID(x, _yMax + 1) != -1 && _map.GetID(x, _yMax + 1) != 0)
            {
                if (!doorDic.ContainsKey(Door.DoorType.Up))
                    doorDic.Add(Door.DoorType.Up, new List<Tile>());

                doorDic[Door.DoorType.Up].Add(_map.GetTile(x, _yMax));
            }

            if (_map.GetID(x, _y - 2) != -1 && _map.GetID(x, _y - 2) != 0 )
            {
                if (!doorDic.ContainsKey(Door.DoorType.Down))
                    doorDic.Add(Door.DoorType.Down, new List<Tile>());

                doorDic[Door.DoorType.Down].Add(_map.GetTile(x, _y - 1));
            }
        }
        // Door Side
        for (int y = _y + 1; y < _yMax - 1; y++)
        {
            if (_map.GetID(_x - 2, y) != -1 && _map.GetID(_x - 2, y) != 0 )
            {
                if (!doorDic.ContainsKey(Door.DoorType.Left))
                    doorDic.Add(Door.DoorType.Left, new List<Tile>());

                doorDic[Door.DoorType.Left].Add(_map.GetTile(_x - 1, y));
            }

            if (_map.GetID(_xMax + 1, y) != -1 && _map.GetID(_xMax + 1, y) != 0)
            {
                if (!doorDic.ContainsKey(Door.DoorType.Right))
                    doorDic.Add(Door.DoorType.Right, new List<Tile>());

                doorDic[Door.DoorType.Right].Add(_map.GetTile(_xMax, y));
            }
        }

        int iterCount = 0;
        int targetCount = Random.Range(1, 4);

        foreach (var iter in doorDic)
        {
            // 연결되는 문 개수 랜덤 
            if (iterCount >= targetCount)
                break;

            int rand = Random.Range(0, iter.Value.Count);

            Tile tile = iter.Value[rand];
            tile.ID = 100;

            if (iter.Key == Door.DoorType.Up || iter.Key == Door.DoorType.Down)
            {
                MakeDoor(tile, TileType.FrontDoor);
                iterCount++;
            }
            else
            {
                MakeDoor(tile, TileType.SideDoor);
                iterCount++;
            }
        }
    }

    private void MakeDoor(Tile tile, TileType path)
    {
        //Tile doorTile = PoolMng.Instance.TilePop(path.ToString());
        Tile doorTile = GameMng.Pool.TilePop(path.ToString());
        doorTile.ID = 100;

        doorTile.TileBuild(tile.X, tile.Y, false);
        _map._tiles[doorTile.X, doorTile.Y] = doorTile;

        Door door = doorTile.gameObject.AddComponent<Door>();
        door.Init(doorTile);
        _map._doors.Add(door);

        GameObject.Destroy(tile.gameObject);
    }

    public void StartRoom(Vector2Int pos, string path, out Vector2 entr)
    {
        roomID = _map._roomID;
        _x = pos.x % 2 == 0 ? pos.x + 1 : pos.x;
        _y = pos.y % 2 == 0 ? pos.y + 1 : pos.y;

        _width = Random.Range(0, 2) >= 1 ? 3 : 5;
        _height = _width > 3 ? 3 : 5;
        _xMax = _x + _width;
        _yMax = _y + _height;

        TileIdChange();
        _map._rooms.Add(this);
        BuildStairs(path, out entr);
    } 

    private void BuildStairs(string path, out Vector2 pos)
    {
        int x = _x + _width / 2;
        int y = _y + _height / 2;

        Tile tile = _map.GetTile(x, y);

        //Tile stairwayTile = PoolMng.Instance.TilePop(path);
        Tile stairwayTile = GameMng.Pool.TilePop(path);

        stairwayTile.TileBuild(x, y);
        stairwayTile.ID = tile.ID;
        _map._tiles[x, y] = stairwayTile;

        GameObject.Destroy(tile.gameObject);
        pos = new Vector2(x, y);
    }
}
