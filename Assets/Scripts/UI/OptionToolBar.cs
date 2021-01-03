using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionToolBar : MonoBehaviour
{
    private Dictionary<string, Button> _buttons;
    private Sprite _checkIdle, _checkActive;
    private string _state = "Right";
    private bool _isReverse;

    private void Start()
    {
        _buttons = new Dictionary<string, Button>();
        Button[] buttons = GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
            _buttons.Add(buttons[i].name, buttons[i]);

        ButtonsRegist();

        _checkIdle = Resources.Load<Sprite>("Image/UI/Option/CheckIdle");
        _checkActive = Resources.Load<Sprite>("Image/UI/Option/CheckActive");

        //Sprite[] sprites = Resources.LoadAll<Sprite>("Image/UI/Option/CheckButton");
        //for (int i = 0; i < sprites.Length; i++)
            //_sprites.Add(sprites[i].name, sprites[i]);
    }

    private void ButtonsRegist()
    {
        _buttons["Middle"].onClick.AddListener( () => { _state = "Middle"; ToolBarPosition(); });
        _buttons["Right"].onClick.AddListener(() => { _state = "Right"; ToolBarPosition(); });
        _buttons["Left"].onClick.AddListener(() => { _state = "Left"; ToolBarPosition(); });
        _buttons["Flip"].onClick.AddListener(Flip);
    }

    private void ToolBarPosition()
    {
        if (_isReverse)
            UIMng.Instance.CallEvent(UIList.HUD, "ToolBarPositionReverce", _state);
        else
            UIMng.Instance.CallEvent(UIList.HUD, "ToolBarPosition", _state);
    }

    private void Flip()
    {
        if (!_isReverse)
        {
            _buttons["Flip"].image.sprite = _checkActive;
            UIMng.Instance.CallEvent(UIList.HUD, "ToolBarFlipReverse", _state);
            _isReverse = true;
        }
        else
        {
            _buttons["Flip"].image.sprite = _checkIdle;
            UIMng.Instance.CallEvent(UIList.HUD, "ToolBarFlip");
            _isReverse = false;
        }
    }
}
