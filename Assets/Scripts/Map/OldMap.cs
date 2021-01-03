//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Random = UnityEngine.Random;

//public class OldMap : MonoBehaviour
//{
//    public class MapValue
//    {
//        public int _width, _height, _min, _max;

//        public MapValue(int w, int h, int min, int max)
//        {
//            _width = w; _height = h; _min = min; _max = max;
//        }
//    }

//    public Dictionary<string, Tile> _tileDic; // = new Dictionary<string, Tile>();
//    public Dictionary<string, Sprite> _images; // = new Dictionary<string, Sprite>();
//    public List<Room> _rooms;

//    public Tile[,] _tiles;

//    public int _roomCount;

//    public int _width, _height, _min, _max;
//    public int _roomID = 1;

//    private Decoration _deco;
//    private TrapBuilder _trap;
//    //private Spawner _spawner;
//    private GameObject _mapHolder;

//    public Vector2 _start = new Vector2();
//    public Vector2 _end = new Vector2();

//    private readonly string _tilePath = "Prefabs/Map/Maze";
//    private readonly string _spritePath = "Image/Map";

//    private void Start()
//    {
//        _mapHolder = new GameObject("TileHolder");
//        _mapHolder.transform.parent = gameObject.transform;

//        _tileDic = new Dictionary<string, Tile>();
//        _images = new Dictionary<string, Sprite>();
//        _rooms = new List<Room>();

//        RegistResourse();
//        CreateMap(RandomValue());
//    }

//    public void Generator()
//    {
//        _mapHolder = new GameObject("TileHolder");
//        _mapHolder.transform.parent = gameObject.transform;

//        _tileDic = new Dictionary<string, Tile>();
//        _images = new Dictionary<string, Sprite>();
//        _rooms = new List<Room>();

//        RegistResourse();
//        CreateMap(RandomValue());
//    }

//    private void RegistResourse()
//    {
//        Tile[] tiles = Resources.LoadAll<Tile>(_tilePath);

//        for (int i = 0; i < tiles.Length; i++)
//        {
//            tiles[i].SetData(DB.Instance.GetTileData(tiles[i].name));
//            _tileDic.Add(tiles[i].name, tiles[i]);
//        }

//        Sprite[] sprites = Resources.LoadAll<Sprite>(_spritePath);

//        for (int i = 0; i < sprites.Length; i++)
//            _images.Add(sprites[i].name, sprites[i]);
//    }

//    private MapValue RandomValue()
//    {
//        int w = Random.Range(40, 52);
//        int h = Random.Range(40, 52);
//        w = w % 2 == 0 ? w + 1 : w;
//        h = h % 2 == 0 ? h + 1 : h;

//        int min = Random.Range(5, 10);
//        int max = Random.Range(9, 12);
//        min = min % 2 == 0 ? min + 1 : min;
//        max = max % 2 == 0 ? max + 1 : max;

//        MapValue value = new MapValue(w, h, min, max);

//        return value;
//    }

//    public void CreateMap(MapValue value)
//    {
//        _width = value._width;
//        _height = value._height;
//        _min = value._min;
//        _max = value._max;

//        _tiles = new Tile[_width, _height];
//        _deco = new Decoration(this);
//        _trap = new TrapBuilder(this);
//        //_spawner = new Spawner(this);

//        _roomCount = Random.Range(9, 11);
//        CreateRoom();

//        foreach (var room in _rooms)
//            _deco.BuildDecoration(room);

//        foreach (var room in _rooms)
//        {
//            if (!room._isDeco)
//                _trap.BuildTrap(room);
//        }

//        //foreach (var room in _rooms)
//            //_spawner.CreateMonster(room);
//    }

//    private void CreateRoom()
//    {
//        BuildTile();
//        BuildRoom();    
//        BuildMaze();

//        foreach (Room room in _rooms)
//            room.Connect();

//        DeleteTile();
//        BuildWall();
//    }

//    public void SetVisible(int x, int y, bool state)
//    {
//        _tiles[x, y].Visible = state;
//        _tiles[x, y].SetVisible();
//    }

//    public void SetExplored(int x, int y, bool state)
//    {
//        _tiles[x, y].Explored = state;
//        _tiles[x, y].SetVisible();
//    }

//    public Tile GetTile(int x, int y)
//    {
//        //if (_tiles[x, y] == null)
//            //return null;

//        if (x < 0 || x >= _width || y < 0 || y >= _height)
//            return null;

//        return _tiles[x, y];        
//    }

//    public int GetID(int x, int y)
//    {
//        if (x < 0 || x >= _width || y < 0 || y >= _height)
//            return -1;

//        if (_tiles[x, y] == null)
//            return -1;

//        return _tiles[x, y].ID;
//    }

//    private void DeleteTile()
//    {
//        bool delete = true;
//        while(delete)
//        {
//            delete = false;
//            for (int x = 1; x < _width - 1 ; x++)
//            {
//                for (int y = 1; y < _height - 1; y++)
//                {
//                    if (GetID(x, y) != 0 ) // && GetTile(x,y).TILETYPE != TileType.DoorFront || GetTile(x,y).TILETYPE != TileType.DoorSide)
//                    {
//                        int count = 0;
//                        for (int dx = -1; dx <= 1; dx += 2)
//                            if (GetID(x + dx, y) == 0)
//                                count++;

//                        for (int dy = -1; dy <= 1; dy += 2)
//                            if (GetID(x, y + dy) == 0)
//                                count++;

//                        if (count >= 3)
//                        {
//                            Tile tile = GetTile(x, y);
//                            tile.ID = 0;
//                            delete = true;
//                        }
//                    }
//                }
//            }
//        }

//        for (int i = 0; i < _width; i++)
//            for (int j = 0; j < _height; j++)
//                if (GetID(i, j) == 0)
//                {
//                    Destroy(GetTile(i, j).gameObject);
//                    _tiles[i, j] = null;
//                }
//    }

//    public Tile GetTile(Vector2 pos)
//    {
//        return _tiles[(int)pos.x, (int)pos.y];
//    }

//    private void BuildTile()
//    {
//        for (int x = 0; x < _width; x++)
//        {
//            for (int y = 0; y < _height; y++)
//            {
//                Tile tile = GameObject.Instantiate(_tileDic["Tile"], new Vector2(x, y), Quaternion.identity, _mapHolder.transform);
//                tile.name = "Tile :" + x.ToString() + " , " + y.ToString();
//                tile.TILETYPE = TileType.Floor;
//                tile.Transparent = true;
//                tile.Init(x, y);
//                tile.ID = 0;
//                _tiles[x, y] = tile;
//            }
//        }
//    }

//    private void BuildRoom()
//    {
//        Vector2Int a = new Vector2Int(1, Random.Range(1, _height - 6));
//        Vector2Int b = new Vector2Int(_width - 6, Random.Range(1, _height - 6));

//        float[] prob = { .5f, .5f };

//        switch(Helper.Chosen(prob))
//        {
//            case 0: StairsRoom(a, "StartStairs", out _start); StairsRoom(b, "EndStairs", out _end); break;
//            case 1: StairsRoom(b, "StartStairs", out _start); StairsRoom(a, "EndStairs", out _end); break;
//        }

//        for (int i = 0; i <= 100 && _rooms.Count <= _roomCount; i++)
//        {
//            Room room = new Room(this);
//            bool flag = false;

//            for (int index = 0; index < _rooms.Count; index++)
//            {
//                if (room.Intersert(_rooms[index]))
//                {
//                    flag = true;
//                    break;
//                }
//            }
//            if (!flag)
//            {
//                room.BuildTile();
//                _rooms.Add(room);
//                _roomID++;
//            }
//        }
//    }

//    private void StairsRoom(Vector2Int pos, string path, out Vector2 entr)
//    {
//        Room room = new Room(this);
//        room.StartRoom(pos);
//        room._isDeco = true;
//        room.BuildStairs(path, out entr);
//    }

//    private void BuildMaze()
//    {
//        _roomID++;

//        for (int x = 1; x < _width - 1; x++)
//        {
//            for (int y = 1; y < _height - 1; y++)
//            {
//                Tile tile = GetTile(x, y);

//                if (x % 2 == 1 && y % 2 == 1 && tile.ID == 0)
//                {
//                    Maze maze = new Maze(this);
//                    maze.Digging(x, y);
//                }
//            }
//        }
//    }

//    private void BuildWall()
//    {
//        //North
//        for (int x = 1; x < _width - 1; x++)
//            for (int y = 1; y < _height - 1; y++)
//            {
//                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
//                    if (_tiles[x, y + 1] == null)
//                        if (_tiles[x, y].TILETYPE == TileType.DoorSide)
//                            WallMaker(x, y + 1, "WallMiddleSouth");
//                        else
//                            WallMaker(x, y + 1, "WallNorth");
//            }
//        //South
//        for (int x = 1; x < _width - 1; x++)
//            for (int y = 1; y < _height - 1; y++)
//            {
//                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
//                {
//                    if (_tiles[x, y - 1] == null)
//                    {
//                        if (_tiles[x + 1, y - 1] != null && _tiles[x + 1, y - 1].TILETYPE != TileType.Wall &&
//                            _tiles[x - 1, y - 1] != null && _tiles[x - 1, y - 1].TILETYPE != TileType.Wall)
//                            WallMaker(x, y - 1, "WallMiddle");
//                        else if (_tiles[x + 1, y - 1] != null && _tiles[x + 1, y - 1].TILETYPE != TileType.Wall)
//                        {
//                            if (GetTile(x - 1, y - 1) != null && GetTile(x - 1, y - 1).name == "WallNorth(Clone)")
//                                WallMaker(x, y - 1, "WallMiddle");
//                            else
//                                WallMaker(x, y - 1, "WallSW");

//                        }
//                        else if (_tiles[x - 1, y - 1] != null && _tiles[x - 1, y - 1].TILETYPE != TileType.Wall)
//                        {
//                            if (GetTile(x + 1, y - 1) != null && GetTile(x + 1, y - 1).name == "WallNorth(Clone)")
//                                WallMaker(x, y - 1, "WallMiddle");
//                            else
//                                WallMaker(x, y - 1, "WallSE");
//                        }
//                        else
//                        {
//                            if (GetTile(x + 1, y - 1) != null && GetTile(x + 1, y - 1).name == "WallNorth(Clone)" && GetTile(x - 1, y - 1) != null && GetTile(x - 1, y - 1).name == "WallNorth(Clone)")
//                                WallMaker(x, y - 1, "WallMiddle");
//                            else
//                                WallMaker(x, y - 1, "WallSouth");
//                        }
//                        if (GetTile(x + 1, y) != null && GetTile(x + 1, y).name == "WallNorth(Clone)")
//                            _tiles[x + 1, y].SetSprite(_images["WallSW"]);
//                    }

//                }
//                // South
             
                    
//            }
//        //East
//        for (int x = 1; x < _width - 1; x++)
//            for (int y = 1; y < _height - 1; y++)
//            {
//                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
//                    if (GetTile(x + 1, y) == null)
//                    {
//                        if (GetTile(x + 2, y) != null)
//                        {
//                            if (GetTile(x + 2, y).TILETYPE != TileType.Wall)
//                                WallMaker(x + 1, y, "WallBoth");
//                            if (GetTile(x + 2, y).name == "WallNorth(Clone)")
//                                WallMaker(x + 1, y, "WallBoth");
//                            else if ( GetTile(x + 2, y).name == "WallSouth(Clone)" || GetTile(x + 2, y).name == "WallSW(Clone)")
//                                WallMaker(x + 1, y, "WallBothCSW");
//                        }           
//                        else if (GetTile(x + 1, y + 1) != null && GetTile(x + 1, y + 1).name == "WallSouth(Clone)")
//                        {
//                            GetTile(x + 1, y + 1).SetSprite(_images["WallSE"]);
//                        }
//                        else
//                            WallMaker(x + 1, y, "WallEast");
//                    }
//            }
//        // West
//        for (int x = 1; x < _width - 1; x++)
//            for (int y = 1; y < _height - 1; y++)
//            {
//                if (_tiles[x, y] != null && _tiles[x, y].TILETYPE != TileType.Wall)
//                    if (_tiles[x - 1, y] == null)
//                    {
//                        if (GetTile(x - 2, y) != null)
//                        {
//                            if (GetTile(x - 2, y).TILETYPE != TileType.Wall)
//                                WallMaker(x - 1, y, "WallBoth");
//                            if (GetTile(x - 2, y).name == "WallNorth(Clone)")
//                                WallMaker(x - 1, y, "WallBoth");
//                            else if (GetTile(x - 2, y).name == "WallSouth(Clone)")
//                                WallMaker(x - 1, y, "WallBothCSE");
//                        }
//                        else if (GetTile(x - 1, y + 1) != null && GetTile(x - 1, y + 1).name == "WallSouth(Clone)")
//                        {
//                            GetTile(x - 1, y + 1).SetSprite(_images["WallSW"]);
//                        }
//                        else
//                            WallMaker(x - 1, y, "WallWest");
//                    }
//            }

//        for (int x = 0; x < _width - 1; x++)
//            for (int y = 0; y < _height; y++)
//            {
//                if (_tiles[x,y] != null && _tiles[x,y].TILETYPE != TileType.Wall)
//                {
//                    if (GetTile(x + 1, y + 1) == null)
//                    {
//                        if (GetTile(x + 2, y + 1) != null && GetTile(x + 2, y + 1).name == "WallNorth(Clone)")
//                            WallMaker(x + 1, y + 1, "WallBoth");
//                        else if (GetTile(x + 2, y + 1) != null && GetTile(x + 2, y + 1).name == "WallSouth(Clone)")
//                            WallMaker(x + 1, y + 1, "WallBothCSW");
//                        else if (GetTile(x + 2, y + 1) != null && GetTile(x + 2, y + 1).TILETYPE == TileType.Floor)
//                            WallMaker(x + 1, y + 1, "WallBothCSW");
//                        else
//                            WallMaker(x + 1, y + 1, "WallEast");
//                        continue;
//                    }

//                    if (GetTile(x - 1, y + 1) == null)
//                    {
//                        WallMaker(x - 1, y + 1, "WallWest");
//                        continue;
//                    }

//                    if (GetTile(x + 1, y - 1) == null)
//                    {
//                        if (GetTile(x + 2, y - 1) != null && GetTile(x + 2, y - 1).name == "WallSouth(Clone)")
//                            WallMaker(x + 1, y - 1, "WallBothSouth");
//                        else if (GetTile(x + 2, y - 1) != null && GetTile(x + 2, y - 1).name == "WallNorth(Clone)")
//                            WallMaker(x + 1, y - 1, "WallBothCSE");
//                        else if (GetTile(x + 2, y - 1) != null && GetTile(x + 2, y - 1).TILETYPE == TileType.Floor)
//                            WallMaker(x + 1, y - 1, "WallBothCSE");
//                        else
//                            WallMaker(x + 1, y - 1, "WallCSE");
//                        continue;
//                    }

//                    if (GetTile(x - 1, y - 1) == null)
//                    {
//                        if (GetTile(x, y - 1) != null && GetTile(x, y - 1).name == "Tile")
//                            WallMaker(x - 1, y - 1, "WallWest");
//                        else
//                            WallMaker(x - 1, y - 1, "WallCSW");
//                        continue;
//                    }
//                }
//            }
//    }

//    private void WallMaker(int x, int y, string path)
//    {
//        Tile tile = GameObject.Instantiate(_tileDic[path], new Vector2(x, y), Quaternion.identity, _mapHolder.transform);
//        tile.Init(x, y);
//        tile.TILETYPE = TileType.Wall;
//        tile.Transparent = false;
//        tile.gameObject.layer = LayerMask.NameToLayer("Wall");
//        _tiles[x, y] = tile;
//    }
//}
