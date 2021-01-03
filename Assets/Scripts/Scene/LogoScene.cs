using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScene : BaseScene
{

    public override void Init()
    {

    }

    public override void Prograss(float value)
    {

    }

    public override void Enter()
    {
        UIMng.Instance.CallEvent(UIList.Logo, "LogoAnimation");
    }

    public override void Exit()
    {
        UIMng.Instance.CallEvent(UIList.Logo, "SetActive", false);
    }
}
