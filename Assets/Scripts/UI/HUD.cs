using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HUD : BaseUI
{
    private Dictionary<string, Button> _buttons;
    private QuickSlot[] _quickSlots;
    private QuickSlot _clickedQuickSlot;

    private RectTransform _quickSlotHolder;
    private Vector3 _originPos;

    private HeroStatus _status;
    private Image _hp, _exp, _statImage, _itemPickUp, _throwInfo, _searchPage;

    //private Image _hpDamage;
    private TextMeshProUGUI _levelText;

    private bool IsSearch { get; set; }

    private Coroutine _runningCoroutine;
    private RectTransform _toolBarHolder;


    private TextMeshProUGUI[] _textBox;
    private List<string> _texts = new List<string>(); 

    public override void Init()
    {
        ComponentRegist();
        ButtonRegist();

        _originPos = _itemPickUp.rectTransform.position;
    }

    private void ComponentRegist()
    {
        //_hpDamage = Helper.Find<Image>(transform, "Status/HPDamage");
        //_hpCut = Helper.Find<Image>(transform, "Status/HPCut");

        _hp = Helper.Find<Image>(transform, "Status/HP");
        _exp = Helper.Find<Image>(transform, "Status/EXP");
        _damageBarTemplate = Helper.Find<Transform>(transform, "Status/HPCut", false);
        _statImage = Helper.Find<Image>(transform, "Status");
        _throwInfo = Helper.Find<Image>(transform, "Throw", false);
        _searchPage = Helper.Find<Image>(transform, "Search", false);
        _itemPickUp = Helper.Find<Image>(transform, "ActionImage", false);
        _levelText = Helper.Find<TextMeshProUGUI>(transform, "Status/Level");

        _toolBarHolder = Helper.Find<RectTransform>(transform, "ToolBar");
        _quickSlotHolder = Helper.Find<RectTransform>(_toolBarHolder.transform, "QuickSlotHolder");

        _textBox = Helper.Find<RectTransform>(transform, "TextBox").GetComponentsInChildren<TextMeshProUGUI>();

        _quickSlots = _quickSlotHolder.GetComponentsInChildren<QuickSlot>();
        for (int i = 0; i < _quickSlots.Length; i++)
            _quickSlots[i].Init();
    }

    private void RegistHero(Hero hero)
    {
        _status = hero.GetStatus();
        _statImage.sprite = _status.STATUSSPRITE;
        UpdateHP();
        UpdateEXP();
        UpdateLevel();
    }

    private void ButtonRegist()
    {
        _buttons = new Dictionary<string, Button>();

        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
            _buttons.Add(buttons[i].name, buttons[i]);

        Button throwCancel = Helper.Find<Button>(transform, "Throw/ThrowCancel");
        _buttons.Add(throwCancel.name, throwCancel);

        Button searchCancel = Helper.Find<Button>(transform, "Search/SearchCancel");
        _buttons.Add(searchCancel.name, searchCancel);

        //Button statusButton = Helper.Find<Button>(transform, "Status/StatusButton");
        //_buttons.Add(statusButton.name, statusButton);

        _buttons["StatusButton"].onClick.AddListener(() => UIMng.Instance.CallEvent(UIList.StatusPopUp, "OpenClosed"));
        _buttons["Inventory"].onClick.AddListener(() => UIMng.Instance.CallEvent(UIList.Inventory, "OpenClosed"));
        _buttons["ThrowCancel"].onClick.AddListener(ThrowCancel);
        _buttons["SearchCancel"].onClick.AddListener(SearchCancel);
        _buttons["Turn"].onClick.AddListener(Turn);
        _buttons["Search"].onClick.AddListener(Search);
        _buttons["Pause"].onClick.AddListener(PauseMenu);
        //_buttons["Attack"].onClick.AddListener(AttackUI);
    }

    private void TextBox(string str)
    {
        Stack<string> temp = new Stack<string>();

        _texts.Add(str);

        if (_texts.Count > 4)
            _texts.RemoveAt(0);

        for (int i = 0; i < _texts.Count; i++)
            temp.Push(_texts[i]);

        for (int i = 0; i < _texts.Count; i++)
            _textBox[i].text = temp.Pop();
    }


    private void SearchCancel()
    {
        IsSearch = false;
        _searchPage.gameObject.SetActive(false);
    }

    private void ThrowCancel()
    {
        GameMng.CharMng.GetHero().IsShoot = false;
        GameMng.CharMng.GetHero().IsThrow = false;

        _throwInfo.gameObject.SetActive(false);
    }

    private void PauseMenu()
    {
        UIMng.Instance.SetActive(UIList.PauseMenu, true);
        UIMng.Instance.CallEvent(UIList.PauseMenu, "Open");
    }

    private void Turn()
    {
        if (GameMng.CharMng.GetHero() != null)
        {
            //GameMng.Instance.HeroTurnOut();
            TurnMng.Instance.HeroTurnOut();
            //TurnMng.Instance.HeroTurnOut();
            TextEffect effect = EffectMng.Instance.Pop("TextEffect") as TextEffect;
            effect.Count = 1;
            effect.SetText("...");
            effect.CallEvent(GameMng.CharMng.GetHero().transform.position);
        }
    }

    #region 써치 버튼 관련 함수 
    private void Search()
    {
        if (IsSearch)
        {
            _searchPage.gameObject.SetActive(false);
            IsSearch = false;
            GameMng.Task.CurrentExecute(new TaskSearch(GameMng.CharMng.GetHero().FindCurrTile(), GameMng.Map.CurrMap, GameMng.CharMng.GetHero().gameObject));
        }
        else
        {
            IsSearch = true;
            _searchPage.gameObject.SetActive(true);
            SeachInput();
        }
    }

    private void SeachInput()
    {
        if (GameMng.CharMng.GetHero() != null)
        {
            if (_runningCoroutine != null)
                StopCoroutine(_runningCoroutine);

            _runningCoroutine = StartCoroutine(IESearchInput());
        }
    }

    private IEnumerator IESearchInput()
    {
        GameMng.Input.UIPress = true;

        while (IsSearch)
        {
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(pos.x);
                int y = Mathf.RoundToInt(pos.y);
                Vector2Int position = new Vector2Int(x, y);
                Tile tile = GameMng.Map.GetTile(position);

                if (tile != null && tile.Explored)
                {
                    if (tile.TILETYPE == TileType.Monster)
                        UIMng.Instance.CallEvent(UIList.SearchPopUp, "MonsterInfo", GameMng.CharMng.GetChar(tile).GetComponent<Monster>().Data);
                    else if (tile.TILETYPE == TileType.Item)
                        UIMng.Instance.CallEvent(UIList.SearchPopUp, "ItemInfo", GameMng.CharMng.GetItem(tile).Data);
                    else
                    {
                        if (tile.IsHiding)
                        {
                            if (tile.TILETYPE == TileType.SideDoor || tile.TILETYPE == TileType.FrontDoor)
                                UIMng.Instance.CallEvent(UIList.SearchPopUp, "TileInfo", DB.Instance.GetTileData("WallTile"));
                            if (tile.TILETYPE == TileType.Trap)
                                UIMng.Instance.CallEvent(UIList.SearchPopUp, "TileInfo", DB.Instance.GetTileData("FloorTile"));
                        }
                        else
                            UIMng.Instance.CallEvent(UIList.SearchPopUp, "TileInfo", tile.Data);
                    }
                }

                _searchPage.gameObject.SetActive(false);
                IsSearch = false;
            }
            yield return null;
        }
        //GameMng.Input.UIPress = false;
    }
    #endregion

    #region HUD 버튼 인터렉션 관리
    // task, ui 실행중일때 입력을 받지 못하게 하기위한 함수.
    private void HUDEnable(bool state)
    {
        foreach (var button in _buttons)
            button.Value.interactable = state;
        for (int i = 0; i < _quickSlots.Length; i++)
            _quickSlots[i].IsActive = state;
    }

    private void HUDLook()
    {
        StartCoroutine(IELookAndRelease(false));
    }

    private void HUDRelease()
    {
        StartCoroutine(IELookAndRelease(true));
    }

    private IEnumerator IELookAndRelease(bool state)
    {
        float delayTime = 0;
        while (delayTime < 0.2f)
        {
            delayTime += Time.deltaTime;
            yield return null;
        }

        foreach (var button in _buttons)
            button.Value.interactable = state;
        for (int i = 0; i < _quickSlots.Length; i++)
            _quickSlots[i].IsActive = state;
    }

    #endregion

    private void ThrowAndShootUI(bool flag)
    {
        _throwInfo.gameObject.SetActive(flag);
    }

    #region 퀵슬롯 
    private void ClickQuickSlot(QuickSlot slot)
    {
        _clickedQuickSlot = slot;
    }

    private void AddQuickSlot(Slot slot)
    {
        for (int i = 0; i < _quickSlots.Length; i++)
        {
            if (_quickSlots[i]._slot == slot)
                _quickSlots[i].RemoveData();

            if (_quickSlots[i]._slot == null)
            {
                _quickSlots[i].AddSlot(slot);
                return;
            }
        }
    }

    private void RemoveQuickSlot(string str)
    {
        for (int i = 0; i < _quickSlots.Length; i++)
            if (_quickSlots[i]._itemData != null && _quickSlots[i]._itemData._name == str)
                _quickSlots[i].RemoveData();
    }
    #endregion

    #region 아이템 인벤토리 이동 효과
    private void ItemImageMove(Sprite sprite)
    {
        _itemPickUp.sprite = sprite;
        StartCoroutine(IEItemImageMove());
    }

    private IEnumerator IEItemImageMove()
    {
        _itemPickUp.gameObject.SetActive(true);
        float elapsedTime = 0;
        Vector3 origin = _itemPickUp.rectTransform.position;

        Vector3 target = _buttons["Inventory"].transform.position;

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime * 2.0f;
            elapsedTime = Mathf.Clamp01(elapsedTime);
            _itemPickUp.rectTransform.position = Vector3.Lerp(origin, target, elapsedTime);
            _itemPickUp.rectTransform.localScale = Vector3.Lerp(Vector2.one, Vector2.zero, elapsedTime);

            yield return null;
        }

        _itemPickUp.rectTransform.position = origin;
        _itemPickUp.rectTransform.localScale = Vector3.one;
        _itemPickUp.gameObject.SetActive(false);
    }
    #endregion


    #region 툴바 옵션
    private void ToolBarPosition(string path)
    {
        switch(path)
        {
            case "Middle": 
                _toolBarHolder.anchorMin = new Vector2(0.5f, 0);
                _toolBarHolder.anchorMax = new Vector2(0.5f, 0);
                _toolBarHolder.pivot = new Vector2(0.5f,0);
                break;
            case "Right":
                _toolBarHolder.anchorMin = new Vector2(1, 0);
                _toolBarHolder.anchorMax = new Vector2(1,0);
                _toolBarHolder.pivot = new Vector2(1,0);
                break;
            case "Left":
                _toolBarHolder.anchorMin = new Vector2(0, 0);
                _toolBarHolder.anchorMax = new Vector2(0,0);
                _toolBarHolder.pivot = new Vector2(0,0);
                break;
        }
    }

    private void ToolBarPositionReverce(string path)
    {
        switch (path)
        {
            case "Middle":
                _toolBarHolder.anchorMin = new Vector2(0.5f, 0);
                _toolBarHolder.anchorMax = new Vector2(0.5f, 0);
                _toolBarHolder.pivot = new Vector2(0.5f, 0);
                _toolBarHolder.localScale = new Vector3(-1, 1, 1);
                _toolBarHolder.anchoredPosition = new Vector2(0, 0);
                break;
            case "Right":
                _toolBarHolder.anchorMin = new Vector2(1, 0);
                _toolBarHolder.anchorMax = new Vector2(1, 0);
                _toolBarHolder.pivot = new Vector2(1, 0);
                _toolBarHolder.localScale = new Vector3(-1, 1, 1);
                _toolBarHolder.anchoredPosition = new Vector2(-540, 0);
                break;
            case "Left":
                _toolBarHolder.anchorMin = new Vector2(0, 0);
                _toolBarHolder.anchorMax = new Vector2(0, 0);
                _toolBarHolder.pivot = new Vector2(0, 0);
                _toolBarHolder.localScale = new Vector3(-1, 1, 1);
                _toolBarHolder.anchoredPosition = new Vector2(540, 0);
                break;
        }
    }

    private void ToolBarFlip()
    {
        _toolBarHolder.localScale = new Vector3(1,1,1);
        _toolBarHolder.anchoredPosition = new Vector2(0, 0);
    }

    private void ToolBarFlipReverse(string path)
    {
        switch (path)
        {
            case "Middle":
                _toolBarHolder.localScale = new Vector3(-1, 1, 1);
                _toolBarHolder.anchoredPosition = new Vector2(0, 0);
                break;
            case "Right":
                _toolBarHolder.localScale = new Vector3(-1, 1, 1);
                _toolBarHolder.anchoredPosition = new Vector2(-540, 0);
                break;
            case "Left":
                _toolBarHolder.localScale = new Vector3(-1, 1, 1);
                _toolBarHolder.anchoredPosition = new Vector2(540, 0);
                break;
        }
    }
    #endregion

    #region 스테이터스
    private Transform _damageBarTemplate;
    private const float HPCut_WIDTH = 236.0f;

    private void UpdateHP()
    {
        _hp.fillAmount = (float)_status.HP / _status.MAXHP;
    }

    private void HPCut(float prevValue)
    {
        Transform damageBar = Instantiate(_damageBarTemplate, _hp.transform);
        damageBar.gameObject.SetActive(true);
        damageBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(_hp.fillAmount * HPCut_WIDTH, 0);
        damageBar.GetComponent<Image>().fillAmount = prevValue - _hp.fillAmount;

        StartCoroutine(IEMoveHPCurBar(damageBar));
    }

    private IEnumerator IEMoveHPCurBar(Transform target)
    {
        float elapsedTime = 0;

        Vector2 p1 = target.position;
        Vector2 p2 = target.position + new Vector3(0, 16, 0);
        Vector2 p3 = target.position + new Vector3(40, 16, 0);
        Vector2 p4 = target.position + new Vector3(40, -60, 0);

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime * 2;
            target.transform.position = Helper.Bezier2D(elapsedTime, p1, p2, p3, p4);
            yield return null;
        }

        Destroy(target.gameObject);
    }

    private void UpdateEXP()
    {
        _exp.fillAmount = (float)_status.EXP / _status.MAXEXP;
    }

    //private void DamageUpdateHP(float prevValue)
    //{
    //    StartCoroutine(IEDamage(prevValue));
    //}

    //private IEnumerator IEDamage(float startValue)
    //{
    //    float elasedTime = 0;
    //    float targetTime = 1;

    //    while (elasedTime <= targetTime)
    //    {
    //        elasedTime += Time.deltaTime;
    //        _hpDamage.fillAmount = Mathf.Lerp(startValue, _hp.fillAmount, elasedTime);
    //        yield return null;
    //    }

    //    _hpDamage.fillAmount = (float)_status.HP / _status.MAXHP;
    //}

    private void UpdateLevel()
    {
        _levelText.text = _status.LEVEL.ToString();
    }
    //Buff Icon 
    #endregion
}
