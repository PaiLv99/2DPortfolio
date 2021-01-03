using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Inventory : BasePopUpUI
{
    private Slot[] _itemSlots;
    private Dictionary<string, Slot> _equipmentSlot;

    private RectTransform _itemSlotHolder;
    private RectTransform _equipmentHolder;

    private TextMeshProUGUI _name;

    public bool ItemPopUp;

    private bool IsOpen { get; set; } = true;

    public override void Init()
    {
        base.Init();
        CalculateRect();

        _itemSlotHolder = Helper.Find<RectTransform>(transform, "Base/ItemSlot");
        _equipmentHolder = Helper.Find<RectTransform>(transform, "Base/Equipment");
        _name = Helper.Find<TextMeshProUGUI>(transform, "Base/Name");

        _itemSlots = _itemSlotHolder.GetComponentsInChildren<Slot>();
        for (int i = 0; i < _itemSlots.Length; i++)
            _itemSlots[i].Init(Slot.SlotType.Item);

        Slot[] equipmentSlots = _equipmentHolder.GetComponentsInChildren<Slot>();
        _equipmentSlot = new Dictionary<string, Slot>();

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].Init(Slot.SlotType.Equipment);
            _equipmentSlot.Add(equipmentSlots[i].name, equipmentSlots[i]);
        }

        _baseImage.gameObject.SetActive(false);
    }

    private void OpenClosed()
    {
        if (!IsOpen)
            Closed();
        else
            Open();
    }

    public override void Closed()
    {
        _baseImage.gameObject.SetActive(false);
        ClickInit();
        IsOpen = true;

        UIMng.Instance.CallEvent(UIList.HUD, "HUDRelease");
    }

    public override void Open()
    {
        IsOpen = false;

        UIMng.Instance.CallEvent(UIList.HUD, "HUDLook");
        _baseImage.gameObject.SetActive(true);
        CalculateRect();
        StartClosedCheck();
    }

    private void AddSlot(Data.ItemData item)
    {
        if (item._type != "Equipment")
        {
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                if (_itemSlots[i]._itemData != null && _itemSlots[i]._itemData._name == item._name)
                {
                    _itemSlots[i].SetCount(item._count);
                    return;
                }
            }
        }
        // item type == equipment or AddItemSlot
        for (int i = 0; i < _itemSlots.Length; i++)
            if (_itemSlots[i]._itemData == null)
            {
                _itemSlots[i].AddItem(item, item._count);
                return;
            }
    }

    private void ItemDrop(Slot slot)
    {
        // 아이템을 생성할 좌표를 얻어온다.
        Transform pos = GameMng.CharMng.GetHero().transform;
        int x = (int)pos.position.x;
        int y = (int)pos.position.y;

        // 아이템 gameObject 생성
        //Item item = PoolMng.Instance.ItemPop(slot._itemData._name);
        Item item = GameMng.Pool.ItemPop(slot._itemData._name);
        item.Data._count = slot.Count;
        item.gameObject.SetActive(true);
        item.transform.position = new Vector3(x, y);
        item.CreateEffect();

        // 타일의 타입을 교체한다.
        // 아이템이 떨어지는 타일을 itemTile로 변경한다.
        Tile tile = GameMng.Map.CurrMap._tiles[x, y];
        if (tile != null)
            tile.TileTypeStack.Push(TileType.Item);

        slot.ClearSlot();
    }

    private void ItemEquip(Slot slot)
    {
        if (slot._itemData._type == "Weapon" || slot._itemData._type == "Armor" || slot._itemData._type == "Accessories1" || slot._itemData._type == "Accessories2")
        {
            string slotName = slot._itemData._type;
            if (_equipmentSlot[slotName]._itemData == null)
            {
                _equipmentSlot[slotName].AddItem(slot._itemData);
                ItemMng.Instance.SetEquipment(slot._itemData);
                slot.ClearSlot();
                return;
            }

            Data.ItemData temp = _equipmentSlot[slotName]._itemData;
            _equipmentSlot[slotName].AddItem(slot._itemData);
            ItemMng.Instance.SetEquipment(slot._itemData);
            slot.AddItem(temp);
        }
    }

    private void EquipStart(Data.ItemData data)
    {
        if (_equipmentSlot[data._type]._itemData == null)
            _equipmentSlot[data._type].AddItem(data);
    }

    private void ReleaseEquipment(Slot slot)
    {
        ItemMng.Instance.ReleaseEquipment(slot._itemData);
        Data.ItemData temp = slot._itemData;

        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i]._itemData == null)
            {
                _itemSlots[i].AddItem(temp, 1);
                slot.ClearSlot();
                return;
            }
        }
        ItemDrop(slot);
    }

    private void QuickInventoryOpen()
    {
        Open();

        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i]._itemData != null) 
            {
                if (_itemSlots[i]._itemData._type == "Weapon" || _itemSlots[i]._itemData._type == "Armor")
                    DisableClick(_itemSlots[i]);
                else
                    EnabledClick(_itemSlots[i], "QuickSlot");
            }
            else
                DisableClick(_itemSlots[i]);
        }
    }

    private void EnchantInventoryOpen()
    {
        Open();

        _name.text = "강화할 아이템을 선택하세요";
        foreach(var iter in _equipmentSlot)
        {
            if (iter.Value._itemData != null)
                EnabledClick(iter.Value, "Enchant");
            else
                DisableClick(iter.Value);
        }

        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i]._itemData != null)
            {
                if (_itemSlots[i]._itemData._type == "Weapon" || _itemSlots[i]._itemData._type == "Armor")
                    EnabledClick(_itemSlots[i], "Enchant");
                else
                    DisableClick(_itemSlots[i]);
            }
            else
                DisableClick(_itemSlots[i]);
        }
    }

    private void IdentifyInventoryOpen()
    {
        Open();

        _name.text = "식별할 아이템을 선택하세요";

        foreach (var iter in _equipmentSlot)
            DisableClick(iter.Value);

        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i]._itemData != null)
            {
                if (!ItemMng.Instance.CheckIdentifiedList(_itemSlots[i]._itemData._name))
                    EnabledClick(_itemSlots[i], "Identify");
                else
                    DisableClick(_itemSlots[i]);
            }
            else
                DisableClick(_itemSlots[i]);
        }
    }

    private void DisableClick(Slot slot)
    {
        slot.ClickType = null;
        slot.ChangeSlotColor("Gray");
    }

    private void EnabledClick(Slot slot, string str = null)
    {
        slot.ChangeSlotColor("White");
        slot.ClickType = str;
    }

    private void ClickInit()
    {
        _name.text = "배낭";
        for (int i = 0; i < _itemSlots.Length; i++)
            EnabledClick(_itemSlots[i]);

        foreach (var iter in _equipmentSlot)
            EnabledClick(iter.Value);
    }

    private void CheckItemSlot(string str)
    {
        // 아이템의 이름과 동일한 슬롯을 찾는다.
        // 만약 찾지 못한다면 삭제한다. 찾는다면 그냥 내비둔다.

        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i]._itemData != null && _itemSlots[i]._itemData._name == str)
                return;
        }
        UIMng.Instance.CallEvent(UIList.HUD, "RemoveQuickSlot", str);
    }

    private void CheckSlot(Slot slot)
    {
        for (int i = 0; i < _itemSlots.Length; i++)
            if (_itemSlots[i] == slot && _itemSlots[i]._itemData == null)
                UIMng.Instance.CallEvent(UIList.HUD, "RemoveQuickSlot", slot._itemData);
    }

    public bool CheckSlot()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
            if (_itemSlots[i]._itemData == null)
                return true;

        return false;
    }

    public Slot GetSlot(Data.ItemData itemInfo)
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i] != null && _itemSlots[i]._itemData == itemInfo)
                return _itemSlots[i];
        }

        return null;
    }
}