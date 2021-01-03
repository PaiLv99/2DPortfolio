using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class QuickSlot : MonoBehaviour, IPointerClickHandler
{
    private Image _itemSprite;
    public Data.ItemData _itemData;
    public Slot _slot;

    public bool IsActive { get; set; } = true;

    public void Init()
    {
        _itemSprite = Helper.Find<Image>(transform, "ItemImage");
        _itemSprite.color = Color.clear;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsActive)
        {
            if (_itemData == null)
            {
                UIMng.Instance.CallEvent(UIList.Inventory, "QuickInventoryOpen");
                UIMng.Instance.CallEvent(UIList.HUD, "ClickQuickSlot", this);
            }
            else
            {
                if (_itemData._type == "Potion" || _itemData._type == "Scroll")
                {
                    if (ItemMng.Instance.CheckIdentifiedList(_itemData._name))
                        switch (_itemData._name)
                        {
                            case "PotionOfFlame": ItemMng.Instance.Throw(_slot); break;
                            case "PotionOfFreeze": ItemMng.Instance.Throw(_slot); break;
                            default: ItemMng.Instance.Used(_slot); break;
                        }
                    else
                        ItemMng.Instance.Used(_slot);

                    UIMng.Instance.CallEvent(UIList.Inventory, "CheckItemSlot", _itemData._name);
                }
                if (_itemData._type == "Projectile")
                    ItemMng.Instance.Shoot(_slot);
                if (_itemData._type == "RangeWeapon")
                    ItemMng.Instance.Shoot(_slot);
            }
        }
    }

    public void AddSlot(Slot slot)
    {
        _slot = slot;
        _itemData = slot._itemData;
        _itemSprite.color = Color.white;
        _itemSprite.sprite = slot._itemData._sprite;
    }

    public void CheckSlot()
    {
        
    }

    public void RemoveData()
    {
        _itemData = null;
        _slot = null;
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;
    }

    public void CheckItem()
    {
        
    }
}
