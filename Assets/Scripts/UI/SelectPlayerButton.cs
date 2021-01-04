using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectPlayerButton : MonoBehaviour, IPointerClickHandler
{
    private Image _image;
    private Select _controller;

    public void Init(Select controller)
    {
        _image = GetComponent<Image>();
        _image.color = Color.gray;
        _controller = controller;
    }

    public void Select()
    {
        _image.color = Color.white;
    }

    public void Reset()
    {
        _image.color = Color.gray;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _controller.OnClick(this);
    }
}
