using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSound : MonoBehaviour
{
    private Slider _bgmSlider, _sfxSlider;
    private Dictionary<string, Button> _buttons;
    private Sprite _checkedSprite, _idleSprite;
    private bool _isBgmMute, _isSfxMute;

    private void Start()
    {
        _bgmSlider = Helper.Find<Slider>(transform, "BGMSlider");
        _bgmSlider.onValueChanged.AddListener(BGMSlider);
        _sfxSlider = Helper.Find<Slider>(transform, "SFXSlider");
        _sfxSlider.onValueChanged.AddListener(SFXSlider);

        _buttons = new Dictionary<string, Button>();
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
            _buttons.Add(buttons[i].name, buttons[i]);

        _buttons["BGMMute"].onClick.AddListener(BGMMuteButton);
        _buttons["SFXMute"].onClick.AddListener(SFXMuteButton);

        _checkedSprite = Resources.Load<Sprite>("Image/UI/Option/CheckedButton");
        _idleSprite = Resources.Load<Sprite>("Image/UI/Option/CheckButton");
    }

    private void BGMSlider(float value)
    {
        float temp = _bgmSlider.value * 10;
        _bgmSlider.value = Mathf.Round(temp) * 0.1f;

        GameMng.Sound.ControllBgmMasterVolume(_bgmSlider.value);
    }

    private void SFXSlider(float value)
    {
        float temp = _sfxSlider.value * 10;
        _sfxSlider.value = Mathf.Round(temp) * 0.1f;

        GameMng.Sound.ControllSfxMasterVolume(_sfxSlider.value);
    }

    private void BGMMuteButton()
    {
        if (!_isBgmMute)
        {
            _isBgmMute = true;
            _bgmSlider.interactable = false;
            _buttons["BGMMute"].image.sprite = _checkedSprite;
            GameMng.Sound.ControllBgmMasterVolume(0);
        }
        else
        {
            _isBgmMute = false;
            _bgmSlider.interactable = true;
            _buttons["BGMMute"].image.sprite = _idleSprite;
            GameMng.Sound.ControllBgmMasterVolume(_bgmSlider.value);
        }
    }

    private void SFXMuteButton()
    {
        if (!_isSfxMute)
        {
            _isSfxMute = true;
            _sfxSlider.interactable = false;
            _buttons["SFXMute"].image.sprite = _checkedSprite;
            GameMng.Sound.ControllSfxMasterVolume(0);
        }
        else
        {
            _isSfxMute = false;
            _sfxSlider.interactable = true;
            _buttons["SFXMute"].image.sprite = _idleSprite;
            GameMng.Sound.ControllSfxMasterVolume(_sfxSlider.value);
        }
    }
}
