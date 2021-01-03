using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB : TSingleton<DB>
{
    private Dictionary<string, Data.ItemData> _itemInfos = new Dictionary<string, Data.ItemData>();
    private Dictionary<string, Data.HeroData> _heroInfos = new Dictionary<string, Data.HeroData>();
    private Dictionary<string, Data.MonsterData> _monsterInfos = new Dictionary<string, Data.MonsterData>();
    private Dictionary<string, Data.TileData> _tileInfos = new Dictionary<string, Data.TileData>();
    private Dictionary<string, Data.MapData> _mapInfos = new Dictionary<string, Data.MapData>();

    private Dictionary<string, Data.BaseData> _datas = new Dictionary<string, Data.BaseData>();

    public override void Init()
    {
        ItemDBRegist();
        MonsterDBRegist();
        HeroDBRegist();
        TileDBRegist();
        MapDBRegist();
    }

    private void ItemDBRegist()
    {
        LowText itemText = TableMng.Instance.Get(TableType.Item);

        //LowText itemText = GameMng.Table.Get(TableType.Item);

        for (int i = 0; i < itemText.GetCount(); i++)
        {
            Data.ItemData info = new Data.ItemData(itemText, i);
            _itemInfos.Add(info._name, info);
            _datas.Add(info._name, info);
        }
    }

    private void MonsterDBRegist()
    {
        //LowText monsterText = GameMng.Table.Get(TableType.Monster);
        LowText monsterText = TableMng.Instance.Get(TableType.Monster);


        for (int i= 0; i < monsterText.GetCount(); i++)
        {
            Data.MonsterData data = new Data.MonsterData(monsterText, i);
            _monsterInfos.Add(data._name, data);
            _datas.Add(data._name, data);
        }
    }

    private void HeroDBRegist()
    {
        //LowText heroText = GameMng.Table.Get(TableType.Hero);
        LowText heroText = TableMng.Instance.Get(TableType.Hero);
        for (int i = 0; i < heroText.GetCount(); i++)
        {
            Data.HeroData info = new Data.HeroData(heroText, i);
            _heroInfos.Add(info._name, info);
            _datas.Add(info._name, info);

        }
    }

    private void TileDBRegist()
    {
        //LowText tileText = GameMng.Table.Get(TableType.Tile);
        LowText tileText = TableMng.Instance.Get(TableType.Tile);

        for (int i = 0; i < tileText.GetCount(); i++)
        {
            Data.TileData info = new Data.TileData(tileText, i);
            _tileInfos.Add(info._name, info);
            _datas.Add(info._name, info);

        }
    }

    private void MapDBRegist()
    {
        //LowText mapsData = GameMng.Table.Get(TableType.Map);
        LowText mapsData = TableMng.Instance.Get(TableType.Map);
        for (int i =0; i < mapsData.GetCount(); i++)
        {
            Data.MapData mapData = new Data.MapData(mapsData, i);
            _mapInfos.Add(mapData._mapName, mapData);
            _datas.Add(mapData._mapName, mapData);

        }
    }

    public Data.ItemData GetItemData(string path)
    {
        if (_itemInfos.ContainsKey(path))
            return _itemInfos[path];

        return null;
    }

    public Data.MonsterData GetMonsterData(string path)
    {
        if (_monsterInfos.ContainsKey(path))
            return _monsterInfos[path];

        return null;
    }

    public Data.HeroData GetHeroData(string path)
    {
        if (_heroInfos.ContainsKey(path))
            return _heroInfos[path];
        return null;
    }

    public Data.TileData GetTileData(string path)
    {
        if (_tileInfos.ContainsKey(path))
            return _tileInfos[path];
        return null;
    }

    public Data.MapData GetMapData(string path)
    {
        if (_mapInfos.ContainsKey(path))
            return _mapInfos[path];
        return null;
    }

    public Data.BaseData GetData(string name)
    {
        if (_datas.ContainsKey(name))
            return _datas[name];
        return null;
    }

    public int GetCount(string path)
    {
        switch(path)
        {
            case "Monster": return _monsterInfos.Count;
            case "Item": return _itemInfos.Count;
            case "Hero": return _heroInfos.Count;
            case "Tile": return _tileInfos.Count;
            case "Map": return _mapInfos.Count;
        }

        return -1;
    }
}
