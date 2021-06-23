using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    private MonsterStatus _status;
    private Slider _hpSlider;
    private RectTransform _transform;
    private Image _backColor, _fillColor;

    private bool IsOpen;
    public int InstanceID { get; private set; }

    public void SetStatus(MonsterStatus status)
    {
        _hpSlider = Helper.Find<Slider>(transform, "HP");
        _backColor = Helper.Find<Image>(transform, "HP/Background");
        _fillColor = Helper.Find<Image>(transform, "HP/FillArea/Fill");

        _transform = _hpSlider.GetComponent<RectTransform>();
        _status = status;
        InstanceID = _status.InstanceID;
    }

    public void UpdateHP()
    {
        IsOpen = true;
        _hpSlider.value = (float)_status.Hp / _status.MaxHp;
    }

    public void UpdateLocalScale(Transform parentTransform)
    {
        _transform.localScale = parentTransform.localScale * 0.01f;
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

    private void SetAlpha(float a)
    {
        Color color = new Color(1, 1, 1, a);

        _fillColor.color = color;
        _backColor.color = color;
    }

    public void SetDieState()
    {
        IsOpen = false;
        SetAlpha(0);
    }
}
