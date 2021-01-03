using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Image _valueImage;
    private Image _borderImage;
    private Transform _monTtransform;
    private Vector3 _offset;
    private float _prevValue;
    public bool IsOpen { get; set; }

    public void Init(Transform holder)
    {
        _offset = new Vector3(0, 0.5f);

        _monTtransform = holder;
        _valueImage = Helper.Find<Image>(transform, "Value");
        _borderImage = Helper.Find<Image>(transform, "Border");

        _valueImage.rectTransform.position += _offset;
        _borderImage.rectTransform.position += _offset;

        _prevValue = 1;
        SetAlpha(0);
    }

    private void SetAlpha(float alpha)
    {
        Color color = new Color(1, 1, 1, alpha);
        _borderImage.color = color;
        _valueImage.color = color;
    }

    public void Open()
    {
        IsOpen = true;
        SetAlpha(1);
    }

    public void Clear()
    {
        _valueImage.fillAmount = 1.0f;
        IsOpen = false;
        SetAlpha(0);
    }

    public void FillAmount(float value)
    {
        StartCoroutine(IEFillAmount(value));
        UpdateDir();
        //_valueImage.fillAmount = value;
    }

    private void UpdateDir()
    {
        _valueImage.rectTransform.localScale = _monTtransform.localScale;
        _borderImage.rectTransform.localScale = _monTtransform.localScale;
    }

    private IEnumerator IEFillAmount(float value)
    {
        //SetAlpha(1);

        yield return new WaitForSeconds(0.25f);

        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            _valueImage.fillAmount = Mathf.Lerp(_prevValue, value, elapsedTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        _prevValue = value;
        //SetAlpha(0);
    }

    public void Visible(bool state)
    {
        if (state)
        {
            if (IsOpen)
                SetAlpha(1);
        }
        else
            SetAlpha(0);
    }
}
