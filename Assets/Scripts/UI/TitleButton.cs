using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour, IPointerExitHandler, IPointerClickHandler, IPointerEnterHandler
{
    private TitleButtonController _controller;
    public Image BackGround { get; private set; }
    public Action Action { get; private set; }

    public void Init(TitleButtonController controller, System.Action action)
    {
        BackGround = GetComponent<Image>();
        BackGround.color = new Color(0.5f, 0.5f, 0.5f, 1);
        _controller = controller;
        Action = action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _controller.OnButtonClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _controller.OnButtonEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _controller.OnButtonExit(this);
    }

    public void ClickEvent()
    {
        if (Action != null)
        Action();
    }
}
