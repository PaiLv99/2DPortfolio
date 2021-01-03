using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Logo : BaseUI
{
    private Image _logoImage;
    private Image _fadeImage;
    private TextMeshProUGUI _text;

    private float _timePerFrame;
    private int _frameRate = 8;
    private int _currFrame = 0;
    private float _elapsedTime;

    private Dictionary<string, Sprite> _fadeSprites;
    private Dictionary<string, Sprite> _iconSprites;

    public override void Init()
    {
        _fadeSprites = new Dictionary<string, Sprite>();
        _iconSprites = new Dictionary<string, Sprite>();


        Sprite[] sprites = Resources.LoadAll<Sprite>("Image/Logo/Fade");
        for (int i = 0; i < sprites.Length; i++)
            _fadeSprites.Add(sprites[i].name, sprites[i]);
        _fadeSprites.OrderBy(x => x.Key);


        sprites = Resources.LoadAll<Sprite>("Image/Logo/Icon");
        for (int i = 0; i < sprites.Length; i++)
            _iconSprites.Add(sprites[i].name, sprites[i]);
        _iconSprites.OrderBy(x => x.Key);

        _logoImage = Helper.Find<Image>(transform, "LogoImage");
        _fadeImage = Helper.Find<Image>(transform, "FadeImage", false);
        _text = Helper.Find<TextMeshProUGUI>(transform, "Text");

        _timePerFrame = 1f / _frameRate;
    }


    public void LogoAnimation()
    {
        StartCoroutine(IELogoFade());
    }

    private IEnumerator IELogoFade()
    {
        _logoImage.color = Color.black;
        _text.color = Color.black;

        ColorUtility.TryParseHtmlString("#212517", out Color color);
        _text.color = color;
        _logoImage.color = new Color(.25f, .25f, .25f, 1);

        yield return new WaitForSeconds(0.25f);

        ColorUtility.TryParseHtmlString("#747d56", out color);
        _text.color = color;
        _logoImage.color = new Color(.5f, .5f, .5f, 1);
        yield return new WaitForSeconds(0.25f);

        ColorUtility.TryParseHtmlString("#c6cfa5", out  color);
        _text.color = color;
        _logoImage.color = new Color(.75f, .75f, .75f, 1);
        yield return new WaitForSeconds(0.25f);

        ColorUtility.TryParseHtmlString("#e7f0cb", out color);
        _text.color = color;
        _logoImage.color = Color.white;

        yield return new WaitForSeconds(0.25f);

        GameMng.Sound.SfxPlay("StartBeep");
        while (_currFrame <= _iconSprites.Count)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _timePerFrame)
            {
                _elapsedTime = 0;
                ++_currFrame;

                if (_currFrame <= _iconSprites.Count)
                    _logoImage.sprite = _iconSprites[_currFrame.ToString()];
            }
            yield return null;
        }


        _fadeImage.gameObject.SetActive(true);
        _currFrame = 1;
                                                                                                                                                                 
        while (_currFrame <= _fadeSprites.Count)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _timePerFrame)
            {
                _elapsedTime = 0;
                ++_currFrame;

                if (_currFrame <= _fadeSprites.Count)
                    _fadeImage.sprite = _fadeSprites[_currFrame.ToString()];
            }
            yield return null;
        }

        SceneMng.Instance.SceneLoading(SceneType.Title);
    }
}
