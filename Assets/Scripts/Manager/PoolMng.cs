using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMng
{
    private Dictionary<string, TPool<Item>> _itemPools = new Dictionary<string, TPool<Item>>();
    private Dictionary<string, TPool<Monster>> _monsterPools = new Dictionary<string, TPool<Monster>>();
    private Dictionary<string, TPool<Tile>> _tilesPools = new Dictionary<string, TPool<Tile>>();

    private Transform transform;
    private GameObject monParent;
    private GameObject tileParent;
    private GameObject itemParent;

    public void SetTransform(Transform t)
    {
        transform = t;
    }

    public void RegistPool()
    {
        LowText monsters = TableMng.Instance.Get(TableType.Monster);
        //LowText monsters = GameMng.Table.Get(TableType.Monster);

         monParent = new GameObject("Monsters");
        monParent.transform.SetParent(transform);

        for (int i = 0; i < monsters.GetCount(); i++)
        {
            Monster prefab = Resources.Load<Monster>(monsters.ToS(i, "PREFAB"));
            string name = monsters.ToS(i, "NAME");
            _monsterPools.Add(name, new TPool<Monster>(new Factory<Monster>(prefab, name, monParent.transform), 5));
        }
        LowText items = TableMng.Instance.Get(TableType.Item);

        //LowText items = GameMng.Table.Get(TableType.Item);


         itemParent = new GameObject("Items");
        itemParent.transform.SetParent(transform);

        for (int i = 0; i < items.GetCount(); i++)
        {
            Item prefab = Resources.Load<Item>(items.ToS(i, "PREFAB"));
            string name = items.ToS(i, "NAME");
            _itemPools.Add(name, new TPool<Item>(new Factory<Item>(prefab, name, itemParent.transform), 3));
        }
        LowText tiles = TableMng.Instance.Get(TableType.Tile);

        //LowText tiles = GameMng.Table.Get(TableType.Tile);

         tileParent = new GameObject("Tiles");
        tileParent.transform.SetParent(transform);

        for (int i = 0; i < tiles.GetCount(); i++)
        {
            Tile prefab = Resources.Load<Tile>(tiles.ToS(i, "PREFAB"));
            string name = tiles.ToS(i, "NAME");
            _tilesPools.Add(name, new TPool<Tile>(new Factory<Tile>(prefab, name, tileParent.transform), 1));
        }
    }

    public Tile TilePop(string path)
    {
        if (_tilesPools.ContainsKey(path))
            return _tilesPools[path].Pop();

        return null;
    }

    public void TilePush(Tile tile, string path)
    {
        if (_tilesPools.ContainsKey(path))
        {
            _tilesPools[path].Push(tile);
            tile.gameObject.SetActive(false);
            tile.transform.SetParent(tileParent.transform);
            tile.transform.position = Vector2.zero;
        }
    }

    public Monster MonsterPop(string path)
    {
        if (_monsterPools.ContainsKey(path))
            return _monsterPools[path].Pop();
        return null;
    }

    public void MonsterPush(string path, Monster mon)
    {
        if (_monsterPools.ContainsKey(path))
        {
            _monsterPools[path].Push(mon);
            mon.transform.position = Vector2.zero;
            mon.transform.SetParent(monParent.transform);
            mon.gameObject.SetActive(false);
        }
    }

    public Item ItemPop(string path)
    {
        if (_itemPools.ContainsKey(path))
        {
            Item item = _itemPools[path].Pop();
            item.gameObject.SetActive(true);
            item.transform.SetParent(itemParent.transform);
            item.Init(DB.Instance.GetItemData(path));
            return item;
        }
        return null;
    }

    public void ItemPush(string path, Item item)
    {
        if (_itemPools.ContainsKey(path))
        {
            _itemPools[path].Push(item);
            item.transform.SetParent(transform);
            item.transform.position = Vector2.zero;
            item.gameObject.SetActive(false);
        }
    }
}
