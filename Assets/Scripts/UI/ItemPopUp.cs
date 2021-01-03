using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemPopUp : BasePopUpUI
{
    private Image _iconImage, _backImage;
    private TextMeshProUGUI _loar, _itemName;
    //private Rect _canvasRect, _bgRect;
    private Slot _slot;
    private Dictionary<string, Button> _buttons;
    private Dictionary<string, Sprite> _sprites;

    private readonly float _offset = 160.0f;
    //private float _xMax, _xMin, _yMax, _yMin;

    private Data.ItemData _itemData;

    public override void Init()
    {
        base.Init();
        ComponentRegist();
        _sprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Image/Item");
        for (int i = 0; i < sprites.Length; i++)
            _sprites.Add(sprites[i].name, sprites[i]);

        _baseImage.gameObject.SetActive(false);
    }

    public override void Closed()
    {
        _baseImage.gameObject.SetActive(false);
        _backImage.gameObject.SetActive(false);
    }

    public override void Open()
    {
        UIMng.Instance.Get<Inventory>(UIList.Inventory).ItemPopUp = true;
        _baseImage.gameObject.SetActive(true);
        _backImage.gameObject.SetActive(true);
        CalculateRect();
        StartClosedCheck();
   }

    private void ComponentRegist()
    {
        _canvasRect = GetComponent<RectTransform>().rect;
        _iconImage = Helper.Find<Image>(transform, "Base/Icon");
        _backImage = Helper.Find<Image>(transform, "Back", false);
        _backImage.rectTransform.sizeDelta = _canvasRect.size;
        _loar = Helper.Find<TextMeshProUGUI>(transform, "Base/Loar");
        _itemName = Helper.Find<TextMeshProUGUI>(transform, "Base/ItemName");

        _buttons = new Dictionary<string, Button>();
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
            _buttons.Add(buttons[i].name, buttons[i]);
            
        _buttons["Drop"].onClick.AddListener(ItemDrop);
        _buttons["Throw"].onClick.AddListener(ItemThrow);
        _buttons["Used"].onClick.AddListener(ItemUsed);
    }

    private void PopUp(Slot slot)
    {
        _itemData = slot._itemData;

        if (_itemData._type == "Potion" || _itemData._type == "Scroll")
        {
            if (ItemMng.Instance.CheckIdentifiedList(_itemData._name))
                IdentifiedPopUp(slot);
            else
                FalsePopUp(slot);
        }
        else
            EquipmentPopUp(slot);
    }

    private void EquipmentPopUp(Slot slot)
    {
        _slot = slot;
        _itemData = slot._itemData;
        int min;
        int max; 

        if (_itemData._type == "Weapon")
        {
            min = 1 + _itemData._tear + _itemData._enchant;
            max = _itemData._value + (1 + _itemData._tear) * _itemData._enchant + 1;
            _loar.text = slot._itemData._loar + string.Format("\n{0}은 {1} 과 {2} 사이의 피해를 준다.", _itemData._uiName, min, max);
        }
        if (_itemData._type == "Armor")
        {
            min = _itemData._enchant;
            max = _itemData._tear * (2 * _itemData._enchant) + 1;
            _loar.text = slot._itemData._loar + string.Format("\n{0}은 {1} 과 {2} 사이의 피해를 막는다.", _itemData._uiName, min, max);
        }
        if (slot._slotType == Slot.SlotType.Item)
            _buttons["Used"].GetComponentInChildren<TextMeshProUGUI>().text = "착용한다";
        else if (slot._slotType == Slot.SlotType.Equipment)
            _buttons["Used"].GetComponentInChildren<TextMeshProUGUI>().text = "해제한다";

        _itemName.text = _itemData._uiName;
        _iconImage.sprite = _itemData._sprite;
        _baseImage.rectTransform.sizeDelta = new Vector2(_baseImage.rectTransform.rect.width, _loar.preferredHeight + _offset);

        Open();
    }

    private void IdentifiedPopUp(Slot slot)
    {
        _slot = slot;
        _itemData = slot._itemData;

        if (_itemData._type == "Weapon" || _itemData._type == "Armor")
        {
            int min = (int)(_itemData._value * .5f);
            int max = _itemData._value + min;

            if (_itemData._type == "Weapon")
                _loar.text = string.Format("{0}은 {1} 과 {2} 사이의 피해를 줍니다.", _itemData._uiName, min.ToString(), max.ToString());
            if (_itemData._type == "Armor")
                _loar.text = string.Format("{0}은 {1} 과 {2} 사이의 피해를 막습니다.", _itemData._uiName, min.ToString(), max.ToString());

            if (slot._slotType == Slot.SlotType.Item)
                _buttons["Used"].GetComponentInChildren<TextMeshProUGUI>().text = "착용한다";
            else if (slot._slotType == Slot.SlotType.Equipment)
            _buttons["Used"].GetComponentInChildren<TextMeshProUGUI>().text = "해제한다";

            ChangeAndOpen(_itemData);
        }
        else if (_itemData._type == "Projectile")
        {
            _buttons["Used"].GetComponentInChildren<TextMeshProUGUI>().text = "발사한다";
            ChangeAndOpen(_itemData);
        }
        else if (_itemData._type == "Potion" || _itemData._type == "Scroll")
        {
            _buttons["Used"].GetComponentInChildren<TextMeshProUGUI>().text = "사용한다";
            ChangeAndOpen(_itemData);
        }
    }

    private void FalsePopUp(Slot slot)
    {
        _slot = slot;
        _buttons["Used"].GetComponentInChildren<TextMeshProUGUI>().text = "사용한다";

        switch (_slot._itemData._type)
        {
            case "Potion": PotionText(); break;
            case "Scroll": ScrollText(); break;
            case "": break;
        }
    }

    private void PotionText()
    {
        _itemName.text = "정체불명의 물약";
        _loar.text = "이 물약은 아직 확인되지 않았습니다. 이 물약을 마시거나 던졌을때 무슨일이 일어날까요?";
        _iconImage.sprite = _sprites["Potion"];
        _baseImage.rectTransform.sizeDelta = new Vector2(_baseImage.rectTransform.rect.width, _loar.preferredHeight + _offset);
        Open();
    }

    private void ScrollText()
    {
        _itemName.text = "정체불명의 주문서";
        _loar.text = "이 주문서는 해독되지 않은 글귀들로 가득 쓰여 있습니다. 이 주문서를 읽으면 어떤 일이 일어날까요?";
        _iconImage.sprite = _sprites["Scroll"];
        _baseImage.rectTransform.sizeDelta = new Vector2(_baseImage.rectTransform.rect.width, _loar.preferredHeight + _offset);
        Open();
    }

    private void ChangeAndOpen(Data.ItemData item)
    {
        _itemName.text = item._uiName;
        _loar.text = item._loar;
        _iconImage.sprite = item._sprite;
        _baseImage.rectTransform.sizeDelta = new Vector2(_baseImage.rectTransform.rect.width, _loar.preferredHeight + _offset);
        Open();
    }

    #region Item Interaction
    private void ItemDrop()
    {
        Closed();
        UIMng.Instance.CallEvent(UIList.Inventory, "ItemDrop", _slot);
    }

    private void ItemThrow()
    {
        Closed();
        ItemMng.Instance.Throw(_slot);
    }

    private void ItemUsed()
    {
        if (_slot._slotType == Slot.SlotType.Item)
        {
            if (_slot._itemData._type == "Potion" || _slot._itemData._type == "Scroll")
                ItemMng.Instance.Used(_slot);
            else if (_slot._itemData._type == "Projectile")
                ItemMng.Instance.Shoot(_slot);
            else if (_slot._itemData._type == "Weapon" || _slot._itemData._type == "Armor")
                UIMng.Instance.CallEvent(UIList.Inventory, "ItemEquip", _slot);
        }
        else
            UIMng.Instance.CallEvent(UIList.Inventory, "ReleaseEquipment", _slot);

        Closed();
    }
    #endregion
}
