using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TweenType { None, Scale, Position, Rotation, }

public class UITween : TSingleton<UITween>
{
    private Tween<Vector3> _tween = new Tween<Vector3>();

    private System.Func<Vector3, Vector3, float, Vector3> _action;
    private TweenType _type = TweenType.None;

    private Vector3 _start;
    private Vector3 _end;
    private Vector3 _curr;

    private Image _image;

    private float _speed = 1.0f;

    public void Execute(Vector3 start, Vector3 end, float speed, System.Func<Vector3, Vector3, float, Vector3> action, TweenType type, Image image)
    {
        _start = start;
        _end = end;
        _speed = speed;
        _action = action;
        _type = type;
        _image = image;

        SelectTween(type);
    }

    private void SelectTween(TweenType type)
    {
        switch(type)
        {
            case TweenType.Scale: ScaleTween(); break;
            case TweenType.Position: PosTween(); break;
            case TweenType.Rotation: RotationUpdate(); break;
        }
    }

    private void ScaleTween()
    {
        _tween.SetTween(_start, _end, _speed, _action);
        StartCoroutine(ScaleUpdate());
    }

    private void PosTween()
    {
        _tween.SetTween(_start, _end, _speed, _action);
        StartCoroutine(PositionUpdate());
    }

    private void RotationUpdate()
    {
        _tween.SetTween(_start, _end, _speed, _action);
        StartCoroutine(RotateUpdate());
    }

    private IEnumerator ScaleUpdate()
    {
        while (_curr != _end)
        {
            _curr = _tween.Update();
            _image.transform.localScale = _curr;
            yield return null;
        }
    }

    private IEnumerator PositionUpdate()
    {
        while (_curr != _end)
        {
            _curr = _tween.Update();
            _image.transform.position = _curr;
            yield return null;
        }
    }

    private IEnumerator RotateUpdate()
    {
        while (_curr != _end)
        {
            _curr = _tween.Update();
            _image.transform.rotation = Quaternion.Euler(_curr);
            yield return null;
        }
    }

    private void ResetObject(Image image, Vector2 origin)
    {

        image.rectTransform.localScale = _start;
        image.rectTransform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
