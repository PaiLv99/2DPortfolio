using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : BaseUI
{
    private Button _goToTitle;

    public override void Init()
    {
        _goToTitle = Helper.Find<Button>(transform, "TitleButton");

        _goToTitle.onClick.AddListener(GoToTitle);
    }

    private void GoToTitle()
    {
        GameMng.Instance.Clear();
        UIMng.Instance.SetActive(UIList.Loading, true);
        UIMng.Instance.CallEvent(UIList.Loading, "GoToTitle");
    }

}
