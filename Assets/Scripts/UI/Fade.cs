using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType { RandomPixel, GameBoy, StarWars}

public class Fade : BaseUI
{
    private Image _image;
    private Sprite _sprite;
    private Texture2D _texture;
    private List<Vector2Int> _coordList = new List<Vector2Int>();
    private Color _originColor;

    //[SerializeField]
    private Vector2Int _cellsize = new Vector2Int(8, 8);
    private int CellCount { get { return _cellsize.x * _cellsize.y; } }

    [SerializeField] private float _fadeTime = 0.25f;
    //private readonly float _alphaStart = 1.0f;
    private readonly float _alphaEnd = 0.0f;

    private int _count;

    public override void Init()
    {
        _image = Helper.Find<Image>(transform, "FadeImage");
        _image.rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;

        Rect rect = GetComponent<RectTransform>().rect;
        int x = (int)rect.xMax / _cellsize.x;
        int y = (int)rect.yMax / x;
        _cellsize = new Vector2Int(_cellsize.x, y);

        CreateTexture();
        CreateSprite();
    }

    #region Creator
    private void CreateTexture()
    {
        _texture = new Texture2D(_cellsize.x, _cellsize.y)
        {
            anisoLevel = 0,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        var pixels = _texture.GetPixels();

        ColorUtility.TryParseHtmlString("#1B1E13", out _originColor);

        for (int i = 0; i < CellCount; i++)
            pixels[i] = _originColor;

        _texture.SetPixels(pixels);
        _texture.Apply();

        for (int x = 0; x < _cellsize.x; x++)
            for (int y = 0; y < _cellsize.y; y++)
                _coordList.Add(new Vector2Int(x, y));
    }

    private void CreateSprite()
    {
        _sprite = Sprite.Create(_texture, new Rect(0, 0, _cellsize.x, _cellsize.y), Vector2.zero);
        _sprite.name = "FadeSprite";
        _image.sprite = _sprite;
    }
    #endregion

    public void FadeIn(FadeType type)
    {
        switch(type)
        {
            case FadeType.RandomPixel: StartCoroutine(IERandomFade(1, 0)); break;
            case FadeType.StarWars: StartCoroutine(IEStarWasFade(0, _cellsize.x, x => x < CellCount));  break;
        }
    }

    public void FadeOut(FadeType type)
    {
        switch (type)
        {
            case FadeType.RandomPixel: StartCoroutine(IERandomFade(0, 1)); break;
            case FadeType.StarWars: StartCoroutine(IEStarWasFade(CellCount - _cellsize.x, -_cellsize.x, x => x >= 0)); break;
        }
    }


    private IEnumerator IEStarWasFade(int start, int delta, System.Func<int, bool> func)
    {
        for (int i = start; func(i); i += delta)
        {
            StartCoroutine(IEStarSetPixels(i, delta, _fadeTime));
            yield return null;
        }

        //AlphaInit();
    }

    private IEnumerator IEStarSetPixels(int index, int delta, float duration)
    {
        float time = 0;
        while (time <= 1)
        {
            var pixels = _texture.GetPixels();

            for (int i = index; i < index + delta; i++)
                pixels[i].a = 1 - time;

            _texture.SetPixels(pixels);
            _texture.Apply();

            time += Time.deltaTime / duration;
            yield return null;
        }
        _count++;
        if (_count == _cellsize.y)
            AlphaInit();
    }

    #region RandomPixelsFade
    private IEnumerator IERandomFade(float start, float target)
    {
        Vector2Int[] randomcoords = Helper.ShuffleArray<Vector2Int>(_coordList.ToArray());

        foreach (var coord in randomcoords)
        {
            //StartCoroutine(SetPixels(coord.x, coord.y, _fadeTime, start, target));
            RandomFixelFade(coord.x, coord.y);
            yield return new WaitForSeconds(0.001f);
        }

        AlphaInit();
    }

    private void RandomFixelFade(int x, int y)
    {
        _texture.SetPixelsAlpha(x, y, 0);
    }

    //private IEnumerator SetPixels(int x, int y, float duration, float start, float target)
    //{
    //    float time = 0.0f;
    //    while (time < 1.0f)
    //    {
    //        var alpha = Mathf.Lerp(start, target, time);
    //        _texture.SetPixelsAlpha(x,y, alpha);
    //        time += Time.deltaTime * duration;
    //        yield return null;
    //    }
    //    _count++;
    //    _texture.SetPixelsAlpha(x,y, _alphaEnd);
    //    if (_count == CellCount)
    //        AlphaInit();
    //}
    #endregion

    private void AlphaInit()
    {
        var pixels = _texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = _originColor;

        _texture.SetPixels(pixels);
        _texture.Apply();

        gameObject.SetActive(false);
    }
}
