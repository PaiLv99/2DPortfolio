using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class ItemData
{
    public int _tableID;
    public int _uniqueID;
    public Sprite _sprite;
    public string _type;
    public string _loar;
    public string _name;
    public string _uiName;
    public string _itemSound;
    public string _part;
    public int _value;
    public int _count = 1;
    public int _tear = 0;
    public int _enchant = 0;

    public ItemData(Data.ItemData info)
    {
        _tableID = info._tableID;
        _sprite = info._sprite;
        _type = info._type;
        _loar = info._loar;
        _name = info._name;
        _uiName = info._uiName;
        _itemSound = info._itemSound;
        _part = info._part;
        _value = info._value;
        _tear = info._tear;
        _enchant = 0;
    }
}
