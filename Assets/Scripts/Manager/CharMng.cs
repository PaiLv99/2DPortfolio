using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMng 
{
    private Dictionary<int, BaseChar> _registChar = new Dictionary<int, BaseChar>();
    private List<Item> _registItem = new List<Item>();
    public string HeroName { get; set; }

    public void AddItem(Item item)
    {
        if (!_registItem.Contains(item))
            _registItem.Add(item);
    }

    public void RemoveItem(Item item)
    {
        if (_registItem.Contains(item))
            _registItem.Remove(item);
    }

    public Item GetItem(Tile tile)
    {
        for (int i = 0; i < _registItem.Count; i++)
            if (_registItem[i].NotifyPosition() == tile.Position)
                return _registItem[i];

        return null;
    }

    public void AddChar(int uniqueID, BaseChar ch)
    {
        if (!_registChar.ContainsKey(uniqueID))
            _registChar.Add(uniqueID, ch);
    }

    public void RemoveChar(int uniqueID)
    {
        if (_registChar.ContainsKey(uniqueID))
            _registChar.Remove(uniqueID);
    }

    public Hero GetHero()
    {
        Hero hero = null;
        foreach(var iter in _registChar)
        {
            hero = iter.Value as Hero;
            if (hero != null)
                break;
        }
        return hero;
    }

    public BaseChar GetChar(Tile tile)
    {
        BaseChar ch = null;

        foreach(var iter in _registChar)
        {
            if (iter.Value != null && iter.Value.NotifyPosition() == tile.Position)
                ch = iter.Value;
        }
        return ch;
    }

    public List<Monster> GetMonsters()
    {
        List<Monster> monsters = new List<Monster>();

        foreach(var iter in _registChar)
        {
            Monster mon = iter.Value as Monster;

            if (mon != null)
                monsters.Add(mon);
        }

        return monsters;
    }

    public List<Item> GetItems()
    {
        return _registItem;
    }

    public void MonsterClear()
    {
        Hero hero = GetHero();
        _registChar.Clear();
        _registChar.Add(hero.UniqueID, hero);
    }

    public void Clear()
    {
        foreach(var iter in _registChar)
        {
            Monster mon = iter.Value as Monster;
            if (mon != null)
                GameMng.Pool.MonsterPush(mon.Data._name, mon);
        }

        for (int i = 0; i < _registItem.Count; i++)
        {
            GameMng.Pool.ItemPush(_registItem[i].Data._name, _registItem[i]);
        }

        _registChar.Clear();
        _registItem.Clear();
    }

    public void OnUpdate(int uniqueID)
    {
        if (_registChar.ContainsKey(uniqueID))
        {

        }
    }
}
