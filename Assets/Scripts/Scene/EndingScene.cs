using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : BaseScene
{
    private Animator _animator;

    public override void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Prograss(float value)
    {
        UIMng.Instance.CallEvent(UIList.Loading, "LoadingBar", value);
    }
}
