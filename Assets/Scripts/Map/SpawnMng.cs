using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnMng
{
    private Map _map;
    private List<Room> _rooms = new List<Room>();
    private readonly int _maxCount = 5;

    private Dictionary<string, string[]> _monsterSqawnTable = new Dictionary<string, string[]>();

    public void Init()
    {
        for (int i = 0; i < DB.Instance.GetCount("Map"); i++)
        {
            string[] monsters;

            Data.MapData mapData = DB.Instance.GetMapData("Map" + i);
            monsters = mapData._sqawnedMonsters.Split('|');
            _monsterSqawnTable.Add(mapData._mapName, monsters);
        }
    }

    public void SetMap(Map map)
    {
        _map = map;
        _rooms = _map._rooms;
    }

    public void MonsterSpawn()
    {
        if (!_map.Spawned)
        {
            foreach (var room in _rooms)
                CreateMonster(room);

            _map.Spawned = true;
        }
    }

    private void CreateMonster(Room room)
    {
        if (room._width <= 5 && room._height <= 5)
        {
            // 이 조건을 만족 하려면 StartRoom 이거나 EndRoom 밖에 없다. 
            return;
        }
            

        int rand = Random.Range(2, _maxCount);
        Vector2[] coords = RandomCoord(room);

        for (int i = 0; i < rand && i < coords.Length; i++)
        {
            int index = Random.Range(0, _monsterSqawnTable[_map.Name].Length);
            string name = _monsterSqawnTable[_map.Name][index];
            PopMon(name, coords[i], _map);
        }
    }    

    private Vector2[] RandomCoord(Room room)
    {
        List<Vector2> coords = new List<Vector2>();

        for (int x = room._x; x < room._xMax; x++)
            for (int y = room._y; y < room._yMax; y++)
            {
                if (_map.GetTile(x,y).TILETYPE == TileType.Floor)
                    coords.Add(new Vector2(x, y));
            }

        Vector2[] coordArray = Helper.ShuffleArray(coords.ToArray());

        return coordArray;
    }

    private void PopMon(string path, Vector2 pos, Map map)
    {
        //Monster mon = PoolMng.Instance.MonsterPop(path);
        Monster mon = GameMng.Pool.MonsterPop(path);
        mon.transform.position = pos + new Vector2(0, 0.25f);
        mon.transform.parent = map._holder.transform;
        mon.SetMap(map);
        mon.gameObject.SetActive(true);
        //mon.Init(DB.Instance.GetMonsterData(path));

        mon.TileTypeChange();
        GameMng.CharMng.AddChar(mon.UniqueID, mon);
    }

    //public void CreateItem(string path, Vector2 coord, Map map)
    //{
    //    Item item = PoolMng.Instance.ItemPop(path);
    //    item.transform.position = coord;
    //    //item.VisibleChange();
    //    //item.Init(DB.Instance.GetItemData(path));
    //    item.transform.parent = map._holder.transform;
    //    item.gameObject.SetActive(true);

    //    Tile tile = _map.GetTile(coord);
    //    if (tile != null)
    //        tile.TILETYPE = TileType.Item;
            
    //    GameMng.CharMng.AddItem(item);
    //}
}
