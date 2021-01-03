using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private TabGroup _group;
    public Image BackGround { get; private set; }
    public UnityEvent _selected;
    public UnityEvent _deSelected;

    public void Init(TabGroup group)
    {
        _group = group;
        BackGround = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _group.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _group.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _group.OnTabExit(this);
    }

    public void Select()
    {
        if (_selected != null)
            _selected.Invoke();
    }

    public void Deselect()
    {
        if (_deSelected != null)
            _deSelected.Invoke();
    }

}
