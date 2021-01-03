using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UIList { Logo, Loading, Fade, Title, Select, HUD, GameOver, Option, Inventory, ItemPopUp, Ending, StatusPopUp, SearchPopUp, PauseMenu }

public class UIMng : TSingleton<UIMng>
{
    public Dictionary<UIList, BaseUI> _uiDic = new Dictionary<UIList, BaseUI>();
    private readonly string Path = "Prefabs/UI/";

    public bool Open { get; set; }

    public override void Init()
    {
        gameObject.AddComponent<StandaloneInputModule>();

        Add(UIList.Fade, false);
        Add(UIList.Loading, false);
    }

    public BaseUI Add(UIList name, bool state = true)
    {
        if (_uiDic.ContainsKey(name))
            return _uiDic[name];

        BaseUI ui = Helper.Instantiate<BaseUI>(Path + name.ToString(), transform.position, Quaternion.identity, true, transform);

        if (ui != null)
        {
            ui.gameObject.SetActive(state);
            _uiDic.Add(name, ui);
        }

        return ui;
    }

    public void DestroyUI(UIList name)
    {
        if (_uiDic.ContainsKey(name))
        {
            Destroy(_uiDic[name].gameObject);
            _uiDic.Remove(name);
        }
    }

    public void CallEvent(UIList name, string func, object value = null)
    {
        if (_uiDic.ContainsKey(name))
        {
            _uiDic[name].SendMessage(func, value, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void SetActive(UIList name, bool state)
    {
        if (_uiDic.ContainsKey(name))
            _uiDic[name].gameObject.SetActive(state);
    }

    public T Get<T>(UIList list) where T : BaseUI
    {
        if (_uiDic.ContainsKey(list))
        {
            return _uiDic[list].GetComponent<T>();
        }

        return null;
    }

    public void FadeIn(FadeType fadeType)
    {
        SetActive(UIList.Fade, true);
        Fade fade = Get<Fade>(UIList.Fade);

        fade.FadeIn(fadeType);
    }

    public void FadeOut(FadeType fadeType)
    {
        SetActive(UIList.Fade, true);
        Fade fade = Get<Fade>(UIList.Fade);

        fade.FadeOut(fadeType);
    }
}
