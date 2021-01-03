using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : BasePopUpUI
{
    private Image _backImage;

    public override void Init()
    {
        base.Init();
        _backImage = Helper.Find<Image>(transform, "Back");
        TabGroup tabs = GetComponentInChildren<TabGroup>();
        tabs.Init();
    }

    public override void Open()
    {
        _baseImage.gameObject.SetActive(true);
        _backImage.gameObject.SetActive(true);
        StartClosedCheck();

        UIMng.Instance.CallEvent(UIList.HUD, "HUDLook");
    }

    public override void Closed()
    {
        _baseImage.gameObject.SetActive(false);
        _backImage.gameObject.SetActive(false);
        UIMng.Instance.CallEvent(UIList.HUD, "HUDRelease");
    }
}
