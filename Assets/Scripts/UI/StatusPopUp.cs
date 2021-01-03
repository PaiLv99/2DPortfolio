using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusPopUp : BasePopUpUI
{
    private TextMeshProUGUI _level, _hp, _exp, _attackPower, _defensivePower, _name;
    private HeroStatus _status;
    private Image _heroIcon;

    private bool IsOpen { get; set; }

    public override void Init()
    {
        base.Init();
        RegistComponent();
        _baseImage.gameObject.SetActive(false);
    }

    public override void Closed()
    {
        UIMng.Instance.CallEvent(UIList.HUD, "HUDRelease");
        _baseImage.gameObject.SetActive(false);
        IsOpen = false;
    }

    public override void Open()
    {
        IsOpen = true;
        UIMng.Instance.CallEvent(UIList.HUD, "HUDLook");
        _baseImage.gameObject.SetActive(true);
        CalculateRect();
        PopUp();
        StartClosedCheck();
    }


    private void OpenClosed()
    {
        if (!IsOpen)
            Open();
    }

    private void RegistComponent()
    {
        //_baseImage = Helper.Find<Image>(transform, "Base");
        _uiRect = _baseImage.GetComponent<RectTransform>().rect;
        _level = Helper.Find<TextMeshProUGUI>(transform, "Base/Level");
        _hp = Helper.Find<TextMeshProUGUI>(transform, "Base/HP");
        _exp = Helper.Find<TextMeshProUGUI>(transform, "Base/EXP");
        _attackPower = Helper.Find<TextMeshProUGUI>(transform, "Base/AP");
        _defensivePower = Helper.Find<TextMeshProUGUI>(transform, "Base/DP");
        _name = Helper.Find<TextMeshProUGUI>(transform, "Base/Name");
        _heroIcon = Helper.Find<Image>(transform, "Base/HeroIcon");
    }

    private void PopUp()
    {
        if (_status == null)
            _status = GameMng.CharMng.GetHero().GetStatus();

        _level.text = string.Format("레벨\t{0}", _status.LEVEL.ToString());
        _hp.text = string.Format("체력\t{0}/{1}", _status.HP.ToString(), _status.MAXHP.ToString());
        _exp.text = string.Format("경험치\t{0}/{1}", _status.EXP.ToString(), _status.MAXEXP.ToString());
        _attackPower.text = string.Format("공격력\t{0}", _status.AP.ToString());
        _defensivePower.text = string.Format("방어력\t{0}", _status.DP.ToString());
        _heroIcon.sprite = _status.SPRITE;
        _name.text = _status.Data._uiName;
    }
}
