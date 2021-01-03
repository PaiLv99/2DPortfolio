using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTween : TSingleton<ColorTween>
{
    private Tween<Color> _tween = new Tween<Color>();
    private System.Func<Color, Color, float, Color> _func;

    private Color _startColor;
    private Color _targetColor;
    private Color _currColor;

    private float _speed;

    private SpriteRenderer _sRenderer;


    public override void Init()
    {

    }

    public void Execute(Color start, Color target, float delta, System.Func<Color, Color, float, Color> func, SpriteRenderer renderer)
    {
        _startColor = start;
        _targetColor = target;
        _speed = delta;
        _func = func;
        _sRenderer = renderer;

        _tween.SetTween(start, target, delta, func);
        StartCoroutine(IEUpdate());
    }

    private IEnumerator IEUpdate()
    {
        Debug.Log("In");
        while (_currColor != _targetColor)
        {
            yield return null;
            _currColor = _tween.Update();
            _sRenderer.color = _currColor;
        }
    }
}
