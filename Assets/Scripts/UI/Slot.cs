using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public enum SlotType { Equipment, Item }

    private Image _slotSprite, _itemSprite, _iconSprite;
    public int Count;
    private TextMeshProUGUI _textCount, _textEnchant;
    //public ItemData _itemData;
    public Data.ItemData _itemData = null;
    public SlotType _slotType;
    public string ClickType { get; set; }
    //public bool OnCanClick { get; set; }

    public void Init(SlotType type)
    {
        _slotSprite = GetComponent<Image>();
        _itemSprite = Helper.Find<Image>(transform, "ItemImage");
        _iconSprite = Helper.Find<Image>(transform, "IconImage");
        _textCount = Helper.Find<TextMeshProUGUI>(transform, "Count");
        _textEnchant = Helper.Find<TextMeshProUGUI>(transform, "Enchant");
        _itemData = null;
        _slotType = type;
    }

    public void AddItem(Data.ItemData item, int count = 0)
    {
        if (item._type == "Potion" || item._type == "Scroll")
        {
            if (ItemMng.Instance.CheckIdentifiedList(item._name))
                _iconSprite.sprite = item._icon;
            else
                _iconSprite.sprite = Resources.Load<Sprite>("Image/Icon/QIcon");
        }
        else
            _iconSprite.sprite = null;

        _itemData = item;
        _itemSprite.sprite = item._sprite;
        SetColor(1);
        if (item._type == "Weapon" || item._type == "Armor" || item._type == "RangeWeapon")
        {
            Count = count;
            _textCount.text = "";

            if (item._enchant >= 1)
                _textEnchant.text = item._enchant.ToString();
        }
        else
        {
            SetCount(count);
        }
    }

    public void SetCount(int count = 0)
    {
        Count += count;

        if (Count <= 0)
            _textCount.text = "";
        else
            _textCount.text = Count.ToString();

        if (Count == 0)
            ClearSlot();
    }

    private void SetColor(float value)
    {
        Color alpha = _itemSprite.color;
        alpha.a = value;
        _itemSprite.color = alpha;
        if (_iconSprite.sprite == null)
            _iconSprite.color = Color.clear;
        else
            _iconSprite.color = alpha;
    }

    public void ClearSlot()
    {
        _itemData = null;
        _itemSprite.sprite = null;
        _iconSprite.sprite = null;
        Count = 0;
        _textCount.text = "";
        _textEnchant.text = "";
        SetColor(0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_itemData != null)
        {
            //if (OnCanClick)
            //{
                switch (ClickType)
                {
                    case "Enchant": 
                        _itemData._enchant = _itemData._enchant + 1; 
                        _textEnchant.text = _itemData._enchant.ToString();
                        GameMng.Sound.SfxPlay("Enchant");
                        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 " + _itemData._uiName + "(을)를 강화했다.");
                        break;
                    case "QuickSlot":
                        UIMng.Instance.CallEvent(UIList.HUD, "AddQuickSlot", this);
                        UIMng.Instance.CallEvent(UIList.Inventory, "Closed");
                        break;
                    case "Identify":
                        IdentifyIcon();
                        ItemMng.Instance.AddIdentifiedList(_itemData._name);
                        GameMng.Sound.SfxPlay("Identify");  
                        UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "당신은 " + _itemData._uiName + "(을)를 식별했다.");
                        break;
                    default: UIMng.Instance.CallEvent(UIList.ItemPopUp, "PopUp", this); break;
                }
            //}
            UIMng.Instance.CallEvent(UIList.Inventory, "ClickInit");
        }
    }

    public void ChangeSlotColor(string str)
    {
        switch(str)
        {
            case "White": _slotSprite.color = Color.white; break;
            case "Gray": _slotSprite.color = Color.gray; break;
        }
    }
    
    public void IdentifyIcon()
    {
        if (_slotType == SlotType.Item)
            _iconSprite.sprite = _itemData._icon;
    }
    //public void ChangeSlotIcon(string str)
    //{
    //    switch(str)
    //    {
    //        case "White": break;
    //        case "Gray": break;
    //    }
    //}
}
