using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonController : MonoBehaviour
{
    private Dictionary<string, Sprite> _sprites;
    private Dictionary<string, TitleButton> _buttons;

    private TitleButton _hoverButton;
    private TitleButton _clickButton;
    private Coroutine _runningCoroutine;

    public void Init()
    {
        _sprites = new Dictionary<string, Sprite>();
        _buttons = new Dictionary<string, TitleButton>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Image/Title/Button");
        for (int i = 0; i < sprites.Length; i++)
            _sprites.Add(sprites[i].name, sprites[i]);

        TitleButton[] buttons = GetComponentsInChildren<TitleButton>();
        for (int i = 0; i < buttons.Length; i++)
        {
            _buttons.Add(buttons[i].name, buttons[i]);
        }

        _buttons["Start"].Init(this, GoToGameScene);
        _buttons["Option"].Init(this, GoToOption);

    }

    public void OnButtonEnter(TitleButton button)
    {
        _hoverButton = button;
        ResetButton();

        button.BackGround.sprite = _sprites[button.name + "Hover"];
        button.BackGround.color = Color.white;
        Blick();
    }

    public void OnButtonExit(TitleButton button)
    {
        if (_hoverButton != null && _hoverButton == button)
            StopCoroutine(_runningCoroutine);

        ResetButton();
    }

    public void OnButtonClick(TitleButton button)
    {
        _clickButton = button;
        _clickButton.ClickEvent();
        //button.BackGround.sprite = _sprites[button.name + "Click"];
    }

    private void ResetButton()
    {
        foreach(var button in _buttons)
        {
            //if (button.Value == _clickButton)
                //continue;

            button.Value.BackGround.sprite = _sprites[button.Value.name + "Idle"];
            button.Value.BackGround.color = new Color(0.5f,0.5f,0.5f,1);
        }
    }

    private void Blick()
    {
        if (_runningCoroutine != null)
            StopCoroutine(_runningCoroutine);
        _runningCoroutine = StartCoroutine(IEBlick());
    }

    private IEnumerator IEBlick()
    {
        float blinkSpeed = 1f;
        Color alpha = _hoverButton.BackGround.color;
        float alphas = 0;

        while (true)
        {
            float percent = 0;
            while (percent <= 1)
            {
                percent += Time.deltaTime * blinkSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                alphas = Mathf.Lerp(0, 1, interpolation);
                alpha.a = alphas;
                _hoverButton.BackGround.color = alpha;
                yield return null;
            }
            yield return null;
        }
    }

    private void GoToOption()
    {
        UIMng.Instance.SetActive(UIList.Option, true);
        UIMng.Instance.CallEvent(UIList.Option, "Open");

        if (_runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine);
            _hoverButton.BackGround.color = Color.white;
        }
    }

    private void GoToGameScene()
    {
        UIMng.Instance.SetActive(UIList.Title, false);
        UIMng.Instance.SetActive(UIList.Select, true);
        UIMng.Instance.CallEvent(UIList.Select, "StartSelectInput");
    }
}
