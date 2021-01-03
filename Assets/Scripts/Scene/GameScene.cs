using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene :BaseScene
{
    public override void Enter()
    {
        GameMng.Map.CreateMap(1);

        //UIMng.Instance.CallEvent(UIList.Loading, "StopTextAnimation");
        //UIMng.Instance.SetActive(UIList.Loading, false);
        //UIMng.Instance.SetActive(UIList.Fade, true);
        //UIMng.Instance.FadeIn(FadeType.StarWars);
        //UIMng.Instance.CallEvent(UIList.Select, "CreatePlayer");
        //CameraMng.Instance.CameraSetting();

        UIMng.Instance.CallEvent(UIList.Select, "ResetSelectPlayer");

        UIMng.Instance.SetActive(UIList.HUD, true);
        UIMng.Instance.SetActive(UIList.Select, false);

        //GameMng.Instance.InToDungeon();
        //MapMng.Instance.InToDungeon();
        //InToDungeon();
    }
    
    public override void Prograss(float value)
    {
        UIMng.Instance.CallEvent(UIList.Loading, "LoadingBarProgress", value);
    }

    public override void Init()
    {
        AddChannel(Channel.Prev, SceneType.Title);
        AddChannel(Channel.Curr, SceneType.Game);
    }

    public override void Exit()
    {

    }
}
