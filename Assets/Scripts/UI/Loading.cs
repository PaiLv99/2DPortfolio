using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Loading : BaseUI
{
    private Image _loadingBar;
    private TextMeshProUGUI _text;
    private bool _flag = true;

    private float _mapPregress;
    private float _sceneProgress;

    private Coroutine _textCoroutine;

    public override void Init()
    {
        _loadingBar = Helper.Find<Image>(transform, "Loading/LoadingValue");
        _text = Helper.Find<TextMeshProUGUI>(transform, "LoadingText");
    }

    public override void SetActive(bool state)
    {
        base.SetActive(state);
    }

    private void LoadingBarProgress(float value)
    {
        //_loadingBar.fillAmount = value / 2.0f;
        _sceneProgress = value;
        if (_flag)
        {
            _textCoroutine = StartCoroutine(IEText());
            StartCoroutine(IEGetTotalProgress(value));
            _flag = false;
        }
    }

    private IEnumerator IEGetTotalProgress(float value)
    {
        float totalProgress = 0;
        while (!GameMng.Map.IsDone)
        {
            _mapPregress = GameMng.Map.progress;

            totalProgress = (_mapPregress + _sceneProgress) / 2.0f;
            _loadingBar.fillAmount = totalProgress;
            Debug.Log("InIN");
            yield return null;
        }

        Debug.Log("종료");

        GameMng.Map.IsDone = false;
        StopCoroutine(_textCoroutine);
        UIMng.Instance.FadeIn(FadeType.RandomPixel);
        gameObject.SetActive(false);
        _flag = true;
    }

    private IEnumerator IEText()
    {
        Color origin = _text.color;
        Color targetColor = _text.color;
        targetColor.a = 0.5f;

        while (true)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * 2;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 2; 
                _text.color = Color.Lerp(origin, targetColor, interpolation);
                yield return null;
            }
            yield return null;
        }
    }

    private void GoToTitle()
    {
        StartCoroutine(IETitle());
    }

    private IEnumerator IETitle()
    {
        float elapsedTime = 0;

        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * 2f;
            elapsedTime = Mathf.Clamp01(elapsedTime);
            //LoadingBar(elapsedTime);
            _loadingBar.fillAmount = Mathf.Lerp(0, 1, elapsedTime);
            yield return null;
        }
        UIMng.Instance.FadeIn(FadeType.StarWars);
        SceneMng.Instance.SceneLoading(SceneType.Title);
        gameObject.SetActive(false);
    }
}
