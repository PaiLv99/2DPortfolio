using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : BasePopUpUI
{
    private Image _backImage;
    private Dictionary<string, Button> _buttons;

    public override void Init()
    {
        base.Init();
        _backImage = Helper.Find<Image>(transform, "Back", false);
        RegistButton();
    }

    public override void Open()
    {
        _backImage.gameObject.SetActive(true);
        _baseImage.gameObject.SetActive(true);
        CalculateRect();
        StartClosedCheck();
        UIMng.Instance.CallEvent(UIList.HUD, "HUDLook");
    }

    public override void Closed()
    {
        _backImage.gameObject.SetActive(false);
        _baseImage.gameObject.SetActive(false);
        UIMng.Instance.CallEvent(UIList.HUD, "HUDRelease");
        //StartCoroutine(IEDelay());
        //SetActive(false);
    }

    private void RegistButton()
    {
        _buttons = new Dictionary<string, Button>();
        Button[] buttons = GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
            _buttons.Add(buttons[i].name, buttons[i]);

        _buttons["Option"].onClick.AddListener(Option);
        _buttons["GoToLobby"].onClick.AddListener(GoToLobby);
        _buttons["Continue"].onClick.AddListener(Continue);
        _buttons["Exit"].onClick.AddListener(Exit);
    }

    private void Option()
    {
        Closed();

        UIMng.Instance.CallEvent(UIList.HUD, "HUDRelease");
        //SetActive(false);

        UIMng.Instance.SetActive(UIList.Option, true);
        UIMng.Instance.CallEvent(UIList.Option, "Open");
    }

    private void GoToLobby()
    {
        Closed();
        // 게임 매니저 정보 클리어
        GameMng.Instance.Clear();
        SceneMng.Instance.SceneLoading(SceneType.Title);
    }

    private void Continue()
    {
        Closed();
    }

    private void Exit()
    {
        Application.Quit();
    }

    private IEnumerator IEDelay()
    {
        yield return new WaitForSeconds(0.3f);
        GameMng.Input.UIPress = false;
    }
}
