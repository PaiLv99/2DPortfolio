using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sprite _buttonIdle;
    private Sprite _buttonHover;
    private Sprite _buttonClick;

    private Image _gameOverButton;
    private Coroutine _coroutine;

    private System.Action Action;

    private void Awake()
    {
        _buttonClick = Resources.Load<Sprite>("Image/UI/GameOverClick");
        _buttonHover = Resources.Load<Sprite>("Image/UI/GameOverHover");
        _buttonIdle = Resources.Load<Sprite>("Image/UI/GameOverIdle");

        _gameOverButton = GetComponent<Image>();
    }

    public void Init(System.Action action)
    {
        Action = action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameOverButton.sprite = _buttonClick;

        if (Action != null)
            Action.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _gameOverButton.sprite = _buttonHover;
        _coroutine = StartCoroutine(Helper.UIBlink(transform));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _gameOverButton.sprite = _buttonIdle;

        StopAllCoroutines();
    }
}
