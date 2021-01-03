using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePopUpUI : BaseUI
{
    protected float _xMax, _yMax, _xMin, _yMin;
    protected Rect _canvasRect, _uiRect;
    protected Image _baseImage;

    public override void Init()
    {
        _canvasRect = GetComponent<RectTransform>().rect;
        _baseImage = Helper.Find<Image>(transform, "Base");
        CalculateRect();
    }

 
    public virtual void Open()
    {

    }

    public virtual void Closed()
    {

    }

    protected void StartClosedCheck()
    {
        StartCoroutine(IEClosedCheck());
    }

    protected IEnumerator IEClosedCheck()
    {
        GameMng.Input.UIPress = true;

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 position = Input.mousePosition;
                if (position.x > _xMax || position.x < _xMin || position.y > _yMax || position.y < _yMin)
                    break;
            }
            yield return null;
        }
        Closed();
        yield return new WaitForSeconds(0.5f);
        GameMng.Input.UIPress = false;
    }

    protected void CalculateRect()
    {
        _uiRect = _baseImage.GetComponent<RectTransform>().rect;

        float xValue = (_canvasRect.width - _uiRect.width) * 0.5f;
        float yValue = (_canvasRect.height - _uiRect.height) * 0.5f;

        _xMax = xValue + _uiRect.width;
        _xMin = xValue;
        _yMax = yValue + _uiRect.height;
        _yMin = yValue;
    }
}
