using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map
{
    public class MapValue
    {
        public int _width, _height, _min, _max;

        public MapValue(int w, int h, int min, int max)
        {
            _width = w; _height = h; _min = min; _max = max;
        }
    }
    public List<Room> _rooms = new List<Room>();
    public List<Door> _doors = new List<Door>();
    public Tile[,] _tiles;
    public Tile[,] Tiles => _tiles;
    public bool Spawned { get; set; }
    public int _roomCount;

    public int _width, _height, _min, _max;
    public int _roomID = 1;

    private Decoration _deco;
    //private TrapBuilder _trap;
    public GameObject _holder;

    public Vector2 _start = new Vector2();
    public Vector2 _end = new Vector2();

    public Map PrevMap;
    public Map NextMap;

    public string Name { get; private set; }

    public Map(GameObject holder, string str)
    {
        _rooms = new List<Room>();
        _doors = new List<Door>();
        _holder = holder;
        Create(RandomValue());
        Name = str;
    }

    private MapValue RandomValue()
    {
        int w = Random.Range(40, 52);
        int h = Random.Range(40, 52);
        w = w % 2 == 0 ? w + 1 : w;
        h = h % 2 == 0 ? h + 1 : h;

        int min = Random.Range(5, 10);
        int max = Random.Range(9, 12);
        min = min % 2 == 0 ? min + 1 : min;
        max = max % 2 == 0 ? max + 1 : max;

        MapValue value = new MapValue(w, h, min, max);

        return value;
    }

    private void Create(MapValue value)
    {
        _width = value._width;
        _height = value._height;
        _min = value._min;
        _max = value._max;

        _tiles = new Tile[_width, _height];
 
        _roomCount = Random.Range(9, 11);

        BuildTile();
        BuildRoom();
        BuildMaze();

        foreach (Room room in _rooms)
            room.Connect();

        DeleteTile();
        BuildWall();

        _deco = new Decoration(this);
        //_trap = new TrapBuilder(this);

        //foreach (var room in _rooms)
        //    _deco.BuildDecoration(room);

        //foreach (var room in _rooms)
        //{
        //    if (!room._isDeco)
        //        _trap.BuildTrap(room);
        //}
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
            return null;

        if (_tiles[x, y] == null)
            return null;

        return _tiles[x, y];        
    }

    private void TileBuilder(int x, int y, string path, bool transparent = true)
    {
        Tile tile = GameMng.Pool.TilePop(path);
        tile.TileBuild(x, y, transparent);
        _tiles[x, y] = tile;
    }

    private void BuildTile()
    {
        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
            {
                TileBuilder(x, y, "Floor");
                _tiles[x, y].ID = 0;
            }
    }

    private void BuildRoom()
    {
        Vector2Int left = new Vector2Int(1, Random.Range(1, _height - 6));
        Vector2Int right = new Vector2Int(_width - 6, Random.Range(1, _height - 6));

        float[] prob = { .5f, .5f };

        switch(Helper.Chosen(prob))
        {
            case 0: StairsRoom(left, "UpStairway", out _start); StairsRoom(right, "DownStairway", out _end); break;
            case 1: StairsRoom(right, "UpStairway", out _start); StairsRoom(left, "DownStairway", out _end); break;
        }

        for (int i = 0; i <= 100 && _rooms.Count <= _roomCount; i++)
        {
            Room room = new Room(this);
            bool flag = false;

            for (int index = 0; index < _rooms.Count; index++)
            {
                if (room.Intersert(_rooms[index]))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                room.TileIdChange();
                _rooms.Add(room);
                _roomID++;
            }
        }
    }

    private void BuildMaze()
    {
        _roomID++;

        for (int x = 1; x < _width - 1; x++)
        {
            for (int y = 1; y < _height - 1; y++)
            {
                Tile tile = GetTile(x, y);

                if (x % 2 == 1 && y % 2 == 1 && tile.ID == 0)
                {
                    Maze maze = new Maze(this);
                    maze.Digging(x, y);
                }
            }
        }
    }

    private void BuildWall()
    {
        //North
        for (int x = 1; x < _width - 1; x++)
            for (int y = 1; y < _height - 1; y++)
            {
                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
                    if (_tiles[x, y + 1] == null)
                    {
                        if (_tiles[x, y].TILETYPE == TileType.SideDoor)
                            TileBuilder(x, y + 1, "MiddleDownWall", false);
                        else
                            TileBuilder(x, y + 1, "NorthWall", false);
                    }
            }
        //South
        for (int x = 1; x < _width - 1; x++)
            for (int y = 1; y < _height - 1; y++)
            {
                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
                    if (_tiles[x, y - 1] == null)
                    {
                        // 양쪽이 복도일때 
                        if (_tiles[x + 1, y - 1] != null && _tiles[x + 1, y - 1].TILETYPE != TileType.Wall && _tiles[x - 1, y - 1] != null && _tiles[x - 1, y - 1].TILETYPE != TileType.Wall)
                            TileBuilder(x, y - 1, "MiddleUpWall", false);
                        // 양쪽이 북쪽 벽일 때
                        else if (GetTile(x + 1, y - 1) != null && GetTile(x + 1, y - 1).name == "NorthWall" && GetTile(x - 1, y - 1) != null && GetTile(x - 1, y - 1).name == "NorthWall")
                            TileBuilder(x, y - 1, "MiddleUpWall", false);
                        // 오른쪽이 복도일땐
                        else if (_tiles[x + 1, y - 1] != null && _tiles[x + 1, y - 1].TILETYPE != TileType.Wall)
                        {
                            // 만약 왼쪽이 북쪽 벽이라면 중
                            if (GetTile(x - 1, y - 1) != null && GetTile(x - 1, y - 1).name == "NorthWall")
                                TileBuilder(x, y - 1, "MiddleUpWall", false);
                            else
                                TileBuilder(x, y - 1, "SWWall", false);

                        }// 왼쪽이 복도일 때
                        else if (_tiles[x - 1, y - 1] != null && _tiles[x - 1, y - 1].TILETYPE != TileType.Wall)
                        {
                            // 오른쪽이 북쪽 벽이라면 
                            if (GetTile(x + 1, y - 1) != null && GetTile(x + 1, y - 1).name == "NorthWall")
                                TileBuilder(x, y - 1, "MiddleUpWall", false);
                            else
                                TileBuilder(x, y - 1, "SEWall", false);
                        }
                        else if (_tiles[x - 1, y - 1] != null && _tiles[x - 1, y - 1].name == "NorthWall")
                            TileBuilder(x, y - 1, "SEWall", false);
                        else if (_tiles[x + 1, y - 1] != null && _tiles[x + 1, y - 1].name == "NorthWall")
                            TileBuilder(x, y - 1, "SWWall", false);
                        else
                            TileBuilder(x, y - 1, "SouthWall", false);
                    }
            }
        //East
        for (int x = 1; x < _width - 1; x++)
            for (int y = 1; y < _height - 1; y++)
            {
                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
                    if (GetTile(x + 1, y) == null)
                    {
                        Tile tile = GetTile(x + 2, y);
                        // 왼쪽이 비어있지 않은 경우 
                        if (tile != null)
                        {
                            // 복도일 경우 || 북벽일 경우
                            if (tile.TILETYPE != TileType.Wall) // || GetTile(x + 2, y).name == "NorthWall")
                                TileBuilder(x + 1, y, "MiddleWall", false);
                            // 북벽일 경우
                            if (tile.name == "NorthWall")
                                TileBuilder(x + 1, y, "MiddleWall", false);
                            // 남쪽벽이거나 sw일경우
                            else if (tile.name == "SouthWall" || tile.name == "SWWall")
                                TileBuilder(x + 1, y, "CSWBothWall", false);
                        }
                        // 윗쪽이 비어있지 않고 남쪽 벽인경우 
                        //else if (GetTile(x + 1, y + 1) != null && GetTile(x + 1, y + 1).name == "SouthWall")
                        //{
                        //    GetTile(x + 1, y + 1).SetSprite(GameMng.Map._sprites["WallSE"]);
                        //}
                        else
                            TileBuilder(x + 1, y, "EastWall", false);
                    }
            }
        // West
        for (int x = 1; x < _width - 1; x++)
            for (int y = 1; y < _height - 1; y++)
            {
                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
                    if (_tiles[x - 1, y] == null)
                    {
                        if (GetTile(x - 2, y) != null)
                        {
                            if (GetTile(x - 2, y).TILETYPE != TileType.Wall || GetTile(x - 2, y).name == "NorthWall")
                                TileBuilder(x - 1, y, "MiddleWall", false);
                            else if (GetTile(x - 2, y).name == "SouthWall")
                                TileBuilder(x - 1, y, "CSEBothWall", false);
                            else
                                TileBuilder(x - 1, y, "WestWall", false);
                        }
                        else
                            TileBuilder(x - 1, y, "WestWall", false);
                    }
            }

        for (int x = 1; x < _width - 1; x++)
        {
            for (int y = 1; y < _height - 1; y++)
            {
                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
                {

                    // 왼쪽 대각선 아래
                    if (_tiles[x - 1, y - 1] == null)
                    {
                        if (GetTile(x - 2, y - 1) != null && GetTile(x - 2, y - 1).name == "NorthWall")
                            TileBuilder(x - 1, y - 1, "CSWBothWall", false);
                        else if (GetTile(x - 2, y - 1) != null && GetTile(x - 2, y - 1).name == "SouthWall")
                            TileBuilder(x - 1, y - 1, "SouthBothWall", false);
                        else if (GetTile(x - 2, y - 1) != null && GetTile(x - 2, y - 1).name == "SEWall")
                            TileBuilder(x - 1, y - 1, "CSEBothWall", false);
                        else if (GetTile(x - 2, y - 1) != null && GetTile(x - 2, y - 1).name == "EastWall")
                        {
                            TileBuilder(x - 1, y - 1, "WestWall", false);
                        }
                        else
                            TileBuilder(x - 1, y - 1, "CSWWall", false);
                    }

                    // 왼쪽 대각선 위
                    if (_tiles[x - 1, y + 1] == null)
                    {
                        if (GetTile(x - 2, y + 1) != null && GetTile(x - 2, y + 1).name == "NorthWall")
                            TileBuilder(x - 1, y + 1, "MiddleWall", false);
                        else if (GetTile(x - 2, y + 1) != null && GetTile(x - 2, y + 1).name == "SouthWall")
                            TileBuilder(x - 1, y + 1, "CSEBothWall", false);
                        else if (_tiles[x, y + 1] != null && _tiles[x, y + 1].name == "NorthWall")
                            TileBuilder(x - 1, y + 1, "WestWall", false);
                        //else
                        //    WallMaker(x - 1, y + 1, "WallCSW");
                        //continue;
                    }


                    // 오른쪽 대각선 아래
                    if (_tiles[x + 1, y - 1] == null)
                    {
                        if (GetTile(x + 2, y - 1) != null && GetTile(x + 2, y - 1).name == "SouthWall" || GetTile(x + 2, y - 1) != null && GetTile(x + 2, y - 1).name == "SWWall")
                            TileBuilder(x + 1, y - 1, "SouthBothWall", false);

                        else if (GetTile(x + 2, y - 1) != null && GetTile(x + 2, y - 1).name == "NorthWall")
                        {
                            if (GetTile(x, y - 1) != null && GetTile(x, y - 1).name == "NorthWall")
                                TileBuilder(x + 1, y - 1, "MiddleWall", false);
                            if (GetTile(x, y - 1) != null && GetTile(x, y - 1).name == "SouthWall")
                                TileBuilder(x + 1, y - 1, "CSEBothWall", false);
                        }
                        else
                            TileBuilder(x + 1, y - 1, "CSEWall", false);
                        //continue;
                    }

                    //// 오른쪽 대각선 위
                    if (_tiles[x + 1, y + 1] == null)
                    {
                        if (GetTile(x + 2, y + 1) != null && GetTile(x + 2, y + 1).name == "NotrhWall")
                            TileBuilder(x + 1, y + 1, "MiddleWall", false);
                        else if (GetTile(x + 2, y + 1) != null && GetTile(x + 2, y + 1).name == "SouthWall")
                            TileBuilder(x + 1, y + 1, "CSWBothWall");
                        else
                            TileBuilder(x + 1, y + 1, "EastWall", false);
                    }
                }
            }
        }
    }

    private void DeleteTile()
    {
        bool delete = true;
        while (delete)
        {
            delete = false;
            for (int x = 1; x < _width - 1; x++)
            {
                for (int y = 1; y < _height - 1; y++)
                {
                    if (GetID(x, y) != 0)
                    {
                        int count = 0;
                        for (int dx = -1; dx <= 1; dx += 2)
                            if (GetID(x + dx, y) == 0)
                                count++;

                        for (int dy = -1; dy <= 1; dy += 2)
                            if (GetID(x, y + dy) == 0)
                                count++;

                        if (count >= 3)
                        {
                            Tile tile = GetTile(x, y);
                            tile.ID = 0;
                            delete = true;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < _width; i++)
            for (int j = 0; j < _height; j++)
                if (GetID(i, j) == 0)
                {
                    Tile tile = GetTile(i, j);
                    GameMng.Pool.TilePush(tile, tile.name);
                    _tiles[i, j] = null;
                }
    }

    public int GetID(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
            return -1;

        if (_tiles[x, y] == null)
            return -1;

        return _tiles[x, y].ID;
    }

    private void StairsRoom(Vector2Int pos, string path, out Vector2 entr)
    {
        Room room = new Room(this);
        room.StartRoom(pos, path, out entr);
        _roomID++;

        room._isDeco = true;
    }

    public void SetVisible(int x, int y, bool state)
    {
        _tiles[x, y].Visible = state;
        _tiles[x, y].SetVisible();
    }

    public void SetExplored(int x, int y, bool state)
    {
        _tiles[x, y].Explored = state;
        _tiles[x, y].SetVisible();
    }
}
