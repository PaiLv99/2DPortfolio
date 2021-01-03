using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;

public class Select : BaseUI
{
    private Button _startButton;
    private SelectPlayerButton _selectedPlayer;
    private SelectPlayerButton[] _selectPlayers;

    private Vector3 _mousePosition;
    private GraphicRaycaster _graphicRay;
    private PointerEventData _point;

    private Image _fireImage;
    private Coroutine _runningCoroutine;
    private int _currIndex;
    private float _timePerFrame;
    private float _frameRate = 8.0f;
    private Dictionary<string, Sprite> _fireSprites;

    private Coroutine _newGameCoroutine;
    private TextMeshProUGUI _newGameText;

    public override void Init()
    {
        _graphicRay = GetComponent<GraphicRaycaster>();
        _point = new PointerEventData(null);

        _fireImage = Helper.Find<Image>(transform, "Fire");

        _startButton = Helper.Find<Button>(transform, "NewGame");
        _startButton.onClick.AddListener(StartGame);
        _startButton.image.color = new Color(1, 1, 1, 0);

        _newGameText = Helper.Find<TextMeshProUGUI>(transform, "NewGame/Text");
        _newGameText.alpha = 0;

        _selectPlayers = GetComponentsInChildren<SelectPlayerButton>();

        for (int i = 0; i < _selectPlayers.Length; i++)
            _selectPlayers[i].Init(this);

        _timePerFrame = 1.0f / _frameRate;
        _fireSprites = new Dictionary<string, Sprite>();
        Sprite[] fires = Resources.LoadAll<Sprite>("Image/Title/Fire");
        for (int i = 0; i < fires.Length; i++)
        {
            _fireSprites.Add(fires[i].name, fires[i]);
        }

        _fireSprites.OrderBy(x => x.Key);
    }

    public void StartSelectInput()
    {
        FireAnimation();
    }

    private void StartGame()
    {
        if (_selectedPlayer != null)
        {
            StopCoroutine(_runningCoroutine);
            SceneMng.Instance.SceneLoading(SceneType.Game);

            StartCoroutine(IENewGameFade(1, 0));
        }
    }

    private void ResetSelectPlayer()
    {
        for (int i = 0; i < _selectPlayers.Length; i++)
            _selectPlayers[i].Reset();

        _selectedPlayer = null;
        _startButton.image.color = Color.clear;
    }

    //private void ResetSelect()
    //{
    //    for (int i = 0; i < _selectPlayers.Length; i++)
    //    {
    //        if (_selectPlayers[i] == _selectedPlayer)
    //            continue;

    //        _selectPlayers[i].Reset();
    //    }
    //}

    public void OnClick(SelectPlayerButton selectedButton)
    {
        for (int i = 0; i < _selectPlayers.Length; i++)
            _selectPlayers[i].Reset();

        selectedButton.Select();
        _selectedPlayer = selectedButton;
        GameMng.CharMng.HeroName = _selectedPlayer.name;
        //GameMng.Instance.SetHero(_selectedPlayer.name);

        if (_startButton.image.color != Color.white)
            NewGameFade();
    }

    private void NewGameFade()
    {
        if (_newGameCoroutine != null)
            StopCoroutine(_newGameCoroutine);

        _newGameCoroutine = StartCoroutine(IENewGameFade(0,1));
    }

    private IEnumerator IENewGameFade(float start, float target)
    {
        Color color = _startButton.image.color;
        float elapsedTIme = 0;
        while (elapsedTIme <= 1)
        {
            elapsedTIme += Time.deltaTime;
            float alpha = Mathf.Lerp(start, target, elapsedTIme);
            color.a = alpha;

            _startButton.image.color = color;
            _newGameText.alpha = alpha;
            yield return null;
        }
    }

    private void FireAnimation()
    {
        if (_runningCoroutine != null)
            StopCoroutine(_runningCoroutine);

        _runningCoroutine = StartCoroutine(IEFireAnimation());
    }

    private IEnumerator IEFireAnimation()
    {
        float elapsedTime = 0;

        while (true)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= _timePerFrame)
            {
                elapsedTime = 0;
                ++_currIndex;

                if (_currIndex > _fireSprites.Count)
                    _currIndex = 1;

                _fireImage.sprite = _fireSprites["Fire" + _currIndex.ToString()];
            }
            yield return null;
        }
    }
}
