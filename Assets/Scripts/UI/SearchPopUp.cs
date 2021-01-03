using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchPopUp : BasePopUpUI
{
    private Image _iconImage;
    private TextMeshProUGUI _loar, _name;

    private float _width;
    private Vector2 _offset = new Vector2(0, 128); 

    public override void Init()
    {
        base.Init();
        _baseImage = Helper.Find<Image>(transform, "Base", false);
        _loar = Helper.Find<TextMeshProUGUI>(transform, "Base/Loar");
        _name = Helper.Find<TextMeshProUGUI>(transform, "Base/Name");
        _iconImage = Helper.Find<Image>(transform, "Base/Icon");

        _width = _baseImage.rectTransform.rect.width;
    }

    public override void Closed()
    {
        _baseImage.gameObject.SetActive(false);
        UIMng.Instance.CallEvent(UIList.HUD, "HUDRelease");
    }

    public override void Open()
    {
        //_baseImage.gameObject.SetActive(true);
        //UIMng.Instance.CallEvent(UIList.HUD, "HUDLook");
    }

    private void MonsterInfo(Data.MonsterData mon)
    {
        _iconImage.sprite = mon._sprite;
        _name.text = mon._uiName;
        _loar.text = mon._loar;

        ChangeRect();
    }

    private void ItemInfo(Data.ItemData item)
    {
        _iconImage.sprite = item._sprite;
        _name.text = item._uiName;
        _loar.text = item._loar;

        ChangeRect();
    }

    private void TileInfo(Data.TileData data)
    {
        _iconImage.sprite = data._sprite;
        _name.text = data._uiName;
        _loar.text = data._loar;
        ChangeRect();
    }

    private void ChangeRect()
    {
        GameMng.Input.UIPress = true;
        _baseImage.rectTransform.sizeDelta = new Vector2(_width, _loar.preferredHeight) + _offset;
        CalculateRect();
        StartClosedCheck();
        _baseImage.gameObject.SetActive(true);
        //UIMng.Instance.CallEvent(UIList.HUD, "HUDLook");
    }
}
