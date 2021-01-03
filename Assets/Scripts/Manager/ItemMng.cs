using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemMng : TSingleton<ItemMng>
{
    private HeroStatus _status;

    private Dictionary<string, bool> _identifiedItems;

    public override void Init()
    {
        _identifiedItems = new Dictionary<string, bool>();
        
        //LowText items = GameMng.Table.Get(TableType.Item);
        LowText items = TableMng.Instance.Get(TableType.Item);
        for (int i = 0; i < items.GetCount(); i++)
            _identifiedItems.Add(items.ToS(i,"NAME"), false);
    }

    public void SetStatus(HeroStatus status)
    {
        _status = status;
    }

    public void Used(Slot slot)
    {
        switch (slot._itemData._part)
        {
            case "Healing": Healing(slot); break;
            case "LevelUp": LevelUp(slot); break;
            case "Invisibility": Invisibility(slot); break;
            case "Vision": Vision(slot); break;
            case "Mapping": Mapping(slot); break;
            case "Teleportation": Teleportation(slot); break;

            case "Flame": AddIdentifiedList(slot._itemData._name); Flame(GameMng.CharMng.GetHero().transform.position); break;
            case "Freeze": AddIdentifiedList(slot._itemData._name); Freeze(GameMng.CharMng.GetHero().transform.position); break;
            case "Enchant": AddIdentifiedList(slot._itemData._name); Enchant(); break;
            case "Identify": AddIdentifiedList(slot._itemData._name); Identify(); break;
        }
        slot.IdentifyIcon();
        slot.SetCount(-1);

        //GameMng.Instance.HeroTurnOut();
        TurnMng.Instance.HeroTurnOut();
    }

    public void ThrowPotion(string str, Vector3 pos)
    {
        switch(str)
        {
            case "Flame": Flame(pos); return;
            case "Freeze": Freeze(pos); return;
        }

        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "아무런 일도 일어나지 않았다.");
        GameMng.Sound.SfxPlay("EmptyPosion");
    }

    private void Healing(Slot slot)
    {
        AddIdentifiedList(slot._itemData._name); 
        _status.Healing(slot._itemData._value);
        GameMng.Sound.SfxPlay(slot._itemData._itemSound);

    }

    private void LevelUp(Slot slot)
    {
        AddIdentifiedList(slot._itemData._name);
        _status.LevelUpPotion();
        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신의 경험이 상승했다.");
        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "레벨 업! 공격력 +1! 방어력 + 1!");
        GameMng.Sound.SfxPlay(slot._itemData._itemSound);

    }

    private void Flame(Vector3 pos)
    {
        Map map = GameMng.Map.GetCurrentMap();

        for (int x = -1; x < 2; x++)
            for (int y = -1; y < 2; y++)
            {
                Tile tile = map._tiles[x + (int)pos.x, y + (int)pos.y];
                BaseChar ch = GameMng.CharMng.GetChar(tile);
                //BaseChar ch = GameMng.Instance.GetChar(tile);
                FlameEffect effect = EffectMng.Instance.Pop("FlameEffect") as FlameEffect;
                Buff buff = new Buff("Flame", 5);
                Debug.Log(effect);
                if (ch != null)
                {
                    BuffMng.Instance.AddBuff(ch, buff);

                    effect.Count = 5;
                    effect.CallEvent(ch.transform.position);
                    effect.Parent(ch.transform);
                }
                else
                {
                    effect.Count = 1;
                    effect.CallEvent(map._tiles[x + (int)pos.x, y + (int)pos.y].transform.position);
                    Debug.Log(effect.Count);
                }
            }


    }

    private void Freeze(Vector3 pos)
    {
        Map map = GameMng.Map.GetCurrentMap();

        for (int x = -1; x < 2; x++)
            for (int y = -1; y < 2; y++)
            {
                Tile tile = map._tiles[x + (int)pos.x, y + (int)pos.y];
                BaseChar ch = GameMng.CharMng.GetChar(tile);
                //BaseChar ch = GameMng.Instance.GetChar(tile);
                if (ch != null)
                {
                    Buff buff = new Buff("Freeze", 5);
                    BuffMng.Instance.AddBuff(ch, buff);
                }
            }
    }

    private void Invisibility(Slot slot)
    {
        AddIdentifiedList(slot._itemData._name);
        Buff buff = new Buff("Invisibility", 5);
        BuffMng.Instance.AddBuff(GameMng.CharMng.GetHero(), buff);

        Color alpha = new Color(1, 1, 1, 0.5f);
        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 투명해졌다.");
    }

    private void Vision(Slot slot)
    {
        AddIdentifiedList(slot._itemData._name);

        //Monster[] mons = GameMng.Instance.GetAllMonster();
        Monster[] mons = GameMng.CharMng.GetMonsters().ToArray();
        for (int i = 0; i < mons.Length; i++)
        {
            Buff buff = new Buff("Vision", 10);
            BuffMng.Instance.AddBuff(mons[i], buff);
        }

        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 다른 존재들을 느낀다.");
    }

    private void Enchant()
    {
        UIMng.Instance.CallEvent(UIList.Inventory, "EnchantInventoryOpen");
    }

    private void Identify()
    {
        UIMng.Instance.CallEvent(UIList.Inventory, "IdentifyInventoryOpen");
    }

    private void Teleportation(Slot slot)
    {
        AddIdentifiedList(slot._itemData._name);

        Map map = GameMng.Map.GetCurrentMap();
        List<Vector2> coord = new List<Vector2>();

        for (int x = 0; x < map._tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map._tiles.GetLength(1); y++)
            {
                if (map._tiles[x, y] != null && map._tiles[x, y].TILETYPE == TileType.Floor)
                    coord.Add(new Vector2Int(x, y));
            }
        }

        Vector2[] randomCoord = Helper.ShuffleArray(coord.ToArray());
        Hero hero = GameMng.CharMng.GetHero();
        hero.transform.position = randomCoord[0];
        hero.FoV();
        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 이층의 다른 지역으로 이동했다.");
        GameMng.Sound.SfxPlay(slot._itemData._itemSound);
    }

    private void Mapping(Slot slot)
    {
        AddIdentifiedList(slot._itemData._name);

        Map map = GameMng.Map.GetCurrentMap();
        for (int x = 0; x < map._tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map._tiles.GetLength(1); y++)
            {
                if (map._tiles[x,y] != null)
                    map._tiles[x, y].Explored = true;
            }
        }

        GameMng.CharMng.GetHero().FoV();
        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 현재층의 구조를 알게 되었다.");
        GameMng.Sound.SfxPlay(slot._itemData._itemSound);
    }

    //private void MonsterDamage()
    //{
    //    // 보이는 타일에 몬스터가 있다면 몬스터에게 magic damage를 준다.
    //    // 현재 hp의 1/3을 준다.
    //    for (int i = 0; i < GameMng.Instance._visibleTile.Count; i++)
    //    {
    //        GameMng.Instance.
    //    }
    //}

    public bool CheckIdentifiedList(string name)
    {
        if (_identifiedItems.ContainsKey(name))
            return _identifiedItems[name];

        return false;
    }

    public void AddIdentifiedList(string str)
    {
        if (_identifiedItems.ContainsKey(str))
            _identifiedItems[str] = true;
    }

    public void Throw(Slot slot)
    {
        GameMng.Input.UIPress = false;

        UIMng.Instance.CallEvent(UIList.Inventory, "Closed");
        UIMng.Instance.CallEvent(UIList.HUD, "ThrowAndShootUI", true);
        Hero hero = GameMng.CharMng.GetHero();
        hero.SelectedSlot = slot;
        hero.IsThrow = true;
    }

    public void Shoot(Slot slot)
    {
        GameMng.Input.UIPress = false;

        UIMng.Instance.CallEvent(UIList.Inventory, "Closed");
        UIMng.Instance.CallEvent(UIList.HUD, "ThrowAndShootUI", true);
        Hero hero = GameMng.CharMng.GetHero();
        hero.SelectedSlot = slot;
        hero.IsShoot = true;
    }

    public void CheckAllItem()
    {
        List<string> keys = new List<string>();
        foreach (var iter in _identifiedItems)
            keys.Add(iter.Key);

        for (int i = 0; i < keys.Count; i++)
            _identifiedItems[keys[i]] = true;
    }

    public void SetEquipment(Data.ItemData item)
    {
        switch (item._type)
        {
            case "Weapon": _status.Weapon = item; break;
            case "Armor": _status.Armor = item; break;
        }
    }

    public void ReleaseEquipment(Data.ItemData item)
    {
        switch (item._type)
        {
            case "Weapon": _status.Weapon = null; break;
            case "Armor": _status.Armor = null; break;
        }
    }

    public void Clear()
    {
        LowText items = TableMng.Instance.Get(TableType.Item);
        //LowText item = GameMng.Table.Get(TableType.Item);

        for (int i = 0; i < items.GetCount(); i++)
        {
            _identifiedItems[items.ToS(i, "NAME")] = false;
        }
    }
}
