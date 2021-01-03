using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : BaseEffect
{
    private TextMeshProUGUI _text;
    private Color _originColor;
    private Vector3 _offset = new Vector3(0, 1, 0);
    private Vector3 _originPos;


    private void Awake()
    {
        _text = Helper.Find<TextMeshProUGUI>(transform, "Text");
        ColorUtility.TryParseHtmlString("#3d422b", out _originColor);
        _originPos = _text.transform.position;
        _text.color = _originColor;
        Name = "TextEffect";
    }

    public override void CallEvent(Vector3 position)
    {
        gameObject.SetActive(true);
        StartCoroutine(IETextMove(position));
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    private IEnumerator IETextMove(Vector3 position)
    {
        float elapsedTime = 0;
        float targetTime = 1;
        _text.color = _originColor;
        Color targetColor = _text.color;
        targetColor.a = 0;
        Vector3 start = position + _offset;
        Vector3 target = start + Vector3.up;

        while (elapsedTime <= targetTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, elapsedTime);

            if (elapsedTime >= 0.5f)
                _text.color = Color.Lerp(_originColor, targetColor, elapsedTime);

            yield return null;
        }
        Count = 0;
        EffectMng.Instance.Push(Name, this);
    }
}
