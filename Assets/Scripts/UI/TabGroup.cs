using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    private List<TabButton> _taps;
    private List<Image> _pages;

    private Sprite _tabIdle, _tabActive, _tabHover;
    private TabButton _selectedTab;

    private Transform _tabHolder, _pageHolder;

    public void Init()
    {
        _taps = new List<TabButton>();
        _tabHolder = Helper.Find<Transform>(transform, "TabArea");
        _taps.AddRange(_tabHolder.GetComponentsInChildren<TabButton>());

        for (int i = 0; i < _taps.Count; i++)
            _taps[i].Init(this);

        _pages = new List<Image>();
        _pageHolder = Helper.Find<Transform>(transform, "PageArea");

        //_pages.AddRange(_pageHolder.GetComponentsInChildren<Image>());

        _pages.Add(Helper.Find<Image>(_pageHolder.transform, "Sound"));
        _pages.Add(Helper.Find<Image>(_pageHolder.transform, "Interface"));

        _tabIdle = Resources.Load<Sprite>("Image/UI/Option/ButtonIdle");
        _tabActive = Resources.Load<Sprite>("Image/UI/Option/ButtonActive");
        _tabHover = Resources.Load<Sprite>("Image/UI/Option/ButtonHover");

        foreach (var tap in _taps)
        {
            tap.Init(this);

            if (tap.name == "Interface")
            {
                tap.BackGround.sprite = _tabActive;
                _selectedTab = tap;
            }
        }
    }

    //public void AddButton(TabButton button)
    //{
    //    _buttons.Add(button);
    //}

    public void OnTabEnter(TabButton button)
    {
        ResetTabButtons();
        if (_selectedTab == null || _selectedTab != button)
            button.BackGround.sprite = _tabHover;
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabButtons();
    }

    public void OnTabSelected(TabButton button)
    {
        // Selected Tab Function
        if (_selectedTab != null)
            _selectedTab.Deselect();

        _selectedTab = button;
        _selectedTab.Select();

        // UI Image Change
        ResetTabButtons();
        button.BackGround.sprite = _tabActive;

        // Page Active
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < _pages.Count; i++)
        {
            if (i == index)
                _pages[i].gameObject.SetActive(true);
            else
                _pages[i].gameObject.SetActive(false);
        }
    }

    private void ResetTabButtons()
    {
        foreach (var button in _taps)
        {
            if (_selectedTab != null && button == _selectedTab)
                continue;

            button.BackGround.sprite = _tabIdle;
        }
    }
}
