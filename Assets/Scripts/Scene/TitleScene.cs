using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override void Enter()
    {
        //GameMng.Instance.ManagerClear();
        GameMng.Sound.BgmPlay("TitleBGM");

        UIMng.Instance.SetActive(UIList.Logo, false);
        UIMng.Instance.SetActive(UIList.Title, true);
        UIMng.Instance.CallEvent(UIList.Title, "Blink");
    }

    public override void Init()
    {
        AddChannel(Channel.Curr, SceneType.Title);
        AddChannel(Channel.Prev, SceneType.None);
    }

    public override void Exit()
    {
        UIMng.Instance.SetActive(UIList.Title, false);
        UIMng.Instance.SetActive(UIList.Loading, true);
    }

    public override void Prograss(float value)
    {
        UIMng.Instance.CallEvent(UIList.Loading, "LodingBar", value);
    }

    public void GoToTitle()
    {
    }
}
