using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharPopUp : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Color _color;
    private Transform _targetTransform;
    private Vector3 _offset;

    public void Init(Transform transform)
    {
        _targetTransform = transform;
        _offset = new Vector3(0, 0.5f, 0);
        _text = GetComponentInChildren<TextMeshProUGUI>();
        ColorUtility.TryParseHtmlString("#3d422b", out _color);
        _text.color = _color;
        _text.gameObject.SetActive(false);
    }

    public void DamagePop(int value)
    {
        _text.text = value.ToString();
        StartCoroutine(IETextMove());
    }

    public void BlockPop()
    {
        _text.text = "막음";
        StartCoroutine(IETextMove());
    }

    public void EXPPopUp(int value)
    {
        _text.text = string.Format("+{0}경험치", value.ToString());
        StartCoroutine(IETextMove());
    }

    public void LevelPopUp()
    {
        _text.text = "레벨업";
        StartCoroutine(IETextMove());
    }

    public void TurnOut()
    {
        _text.text = "...";
        StartCoroutine(IETextMove());
    }


    private IEnumerator IETextMove()
    {
        float delayTime = 0;
        float targetTime = 1;
        _text.rectTransform.localScale = _targetTransform.localScale;
        _text.gameObject.SetActive(true);
        //Vector3 start = Camera.main.WorldToScreenPoint(_transform.position);
        Vector3 start = _targetTransform.position + _offset;
        Vector3 target = _targetTransform.position + Vector3.up; //Camera.main.WorldToScreenPoint(_transform.position + Vector3.up);
        Color color = _text.color;
        color.a = 0;

        while (delayTime <= targetTime)
        {
            delayTime += Time.deltaTime;
            _text.transform.position = Vector3.Lerp(start, target, delayTime);

            if (delayTime >= 0.5f)
                _text.color = Color.Lerp(_color, color, delayTime);

            yield return null;
        }

        _text.color = _color;
        _text.gameObject.SetActive(false);
    }
}
