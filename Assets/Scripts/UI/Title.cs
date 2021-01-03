using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Title : BaseUI
{
    //private Button _startButton;
    //private TextMeshProUGUI _text;
    //private Coroutine _runningCoroutine;
    private TitleButtonController _buttonController;

    public override void Init()
    {
        //_startButton = Helper.Find<Button>(transform, "StartButton");
        //_text = Helper.Find<TextMeshProUGUI>(transform, "StartButton/Text");
        //_startButton.onClick.AddListener(StartButton)
        
        _buttonController = Helper.Find<TitleButtonController>(transform, "Image");
        _buttonController.Init();
    }


        //private void StartButton()
        //{
        //    StopCoroutine(_runningCoroutine);
        //    UIMng.Instance.SetActive(UIList.Select, true);
        //    UIMng.Instance.SetActive(UIList.Title, false);
        //    UIMng.Instance.CallEvent(UIList.Select, "StartRoutine");
        //    //UIMng.Instance.CallEvent(UIList.Fade, "FadeIn", FadeType.StarWars);
        //}

        //private void Blink()
        //{
        //    Helper.Blink(_text.transform);
        //    if (_runningCoroutine != null)
        //        StopCoroutine(_runningCoroutine);

        //    _runningCoroutine = StartCoroutine(IEBlink());
        //}

        //private IEnumerator IEBlink()
        //{
        //    float blinkSpeed = 0.5f;
        //    Color alpha = Color.white;
        //    float alphas = 0; 
        //    while (true)
        //    {
        //        float percent = 0;
        //        while (percent <= 1)
        //        {
        //            percent += Time.deltaTime * blinkSpeed;
        //            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
        //            alphas = Mathf.Lerp(0, 1, interpolation);
        //            alpha.a = alphas;
        //            _text.color = alpha;
        //            yield return null;
        //        }
        //        yield return null;
        //    }
        //}
    }
