using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TableType { Hero, Monster, Item, Dialogue, Tile, Map }

public class TableMng : TSingleton<TableMng>
{
    private Dictionary<TableType, LowText> _tables = new Dictionary<TableType, LowText>();

    public LowText Get(TableType type)
    {
        if (_tables.ContainsKey(type))
        {
            return _tables[type];
        }
        else if (Enum.TryParse(type.ToString(), out TableType t))
        {
            AddTable(t);
            return _tables[type];
        }
        return null;
    }

    public void AddTable(TableType type)
    {
        if (!_tables.ContainsKey(type))
        {
            LowText low = new LowText();
            low.Load(type.ToString());
            _tables.Add(type, low);
        }
    }
}










//public int ToI(TableType type, int mainKey, string subKey)
//{
//    if (_table.ContainsKey(type))
//        return _table[type].ToI(mainKey, subKey);
//    return -1;
//}

//public float ToF(TableType type, int mainKey, string subKey)
//{
//    if (_table.ContainsKey(type))
//        return _table[type].ToF(mainKey, subKey);
//    return -1.0f;
//}

//public string ToS(TableType type, int mainKey, string subKey)
//{
//    if (_table.ContainsKey(type))
//        return _table[type].ToS(mainKey, subKey);
//    return string.Empty;
//}

