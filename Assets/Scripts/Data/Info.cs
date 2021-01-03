using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class BaseData
    {
        //protected string _name;
    }

    public class MonsterData : BaseData
    {
        public string _name;
        public string _uiName;
        public string _loar;
        public string _prefab;
        public string _hitSound;
        public Sprite _sprite;
        public int _hp;
        public int _ap;
        public int _dp;
        public int _exp;
        public string _createItem;
        public int _sightRadius;
        public int _level;

        public MonsterData(LowText low, int mainKey)
        {
            _name = low.ToS(mainKey, "NAME");
            _uiName = low.ToS(mainKey, "UINAME");
            _loar = low.ToS(mainKey, "LOAR");
            _hitSound = low.ToS(mainKey, "HITSOUND");
            _hp = low.ToI(mainKey, "HP");
            _exp = low.ToI(mainKey, "EXP");
            _ap = low.ToI(mainKey, "AP");
            _dp = low.ToI(mainKey, "DP");
            _prefab = low.ToS(mainKey, "PREFAB");
            _sprite = Resources.Load<Sprite>(low.ToS(mainKey, "SPRITE"));
            _createItem = low.ToS(mainKey, "CREATEITEM");
            _level = low.ToI(mainKey, "LEVEL");
            _sightRadius = low.ToI(mainKey, "SIGHTRADIUS");
        }
    }

    public class HeroData : BaseData
    {
        public string _name;
        public string _uiName;
        public string _prefab;
        public string _startWeapon;
        public string _startArmor;
        public string _startProjectile;
        public Sprite _sprite;
        public Sprite _statusSprite;
        public int _maxHp;
        public int _ap;
        public int _dp;
        public int _exp;
        public int _level;

        public HeroData(LowText low, int mainKey)
        {
            _name = low.ToS(mainKey, "NAME");
            _uiName = low.ToS(mainKey, "UINAME");
            _maxHp = low.ToI(mainKey, "HP");
            _level = low.ToI(mainKey, "LEVEL");
            _ap = low.ToI(mainKey, "AP");
            _dp = low.ToI(mainKey, "DP");
            _exp = low.ToI(mainKey, "EXP");
            _prefab = low.ToS(mainKey, "PREFAB");
            _startWeapon = low.ToS(mainKey, "STARTWEAPON");
            _startArmor = low.ToS(mainKey, "STARTARMOR");
            _startProjectile = low.ToS(mainKey, "STARTPROJECTILE");
            _sprite = Resources.Load<Sprite>(low.ToS(mainKey, "SPRITE"));
            _statusSprite = Resources.Load<Sprite>(low.ToS(mainKey, "STATUSSPRITE"));
        }
    }

    public class ItemData : BaseData
    {
        public int _tableID;
        public string _prefab;
        public Sprite _sprite;
        public Sprite _icon;
        public string _type;
        public string _name;
        public string _uiName;
        public string _loar;
        public string _itemSound;
        public string _part;
        public int _value;
        public int _enchant;
        public int _tear;
        public int _uniqueID;
        public int _count = 1;

        public ItemData(LowText low, int mainKey)
        {
            _tableID = low.ToI(mainKey, "TABLEID");
            _prefab = low.ToS(mainKey, "PREFAB");
            _type = low.ToS(mainKey, "TYPE");
            _sprite = Resources.Load<Sprite>(low.ToS(mainKey, "SPRITE"));
            _icon = Resources.Load<Sprite>(low.ToS(mainKey, "ICON"));
            _name = low.ToS(mainKey, "NAME");
            _uiName = low.ToS(mainKey, "UINAME");
            _loar = low.ToS(mainKey, "LOAR");
            _part = low.ToS(mainKey, "PART");
            _value = low.ToI(mainKey, "VALUE");
            _itemSound = low.ToS(mainKey, "ITEMSOUND");
            _enchant = low.ToI(mainKey, "ENCHANT");
            _tear = low.ToI(mainKey, "TEAR");
        }
    }

    public class TileData : BaseData
    {
        public Sprite _sprite;
        public string _prefab;
        public string _uiName;
        public string _type;
        public string _loar;
        public string _name;

        public TileData(LowText low, int mainKey)
        {
            _name = low.ToS(mainKey, "NAME");
            _uiName = low.ToS(mainKey, "UINAME");
            _loar = low.ToS(mainKey, "LOAR");
            _sprite = Resources.Load<Sprite>(low.ToS(mainKey, "SPRITE"));
            _type = low.ToS(mainKey, "TILETYPE");
            _prefab = low.ToS(mainKey, "PREFAB");
        }
    }

    public class MapData : BaseData
    {
        public string _mapName;
        public string _sqawnedMonsters;

        public MapData(LowText low, int mainKey)
        {
            _mapName = low.ToS(mainKey, "MAPNAME");
            _sqawnedMonsters = low.ToS(mainKey, "MONSTERS");
        }
    }
}


