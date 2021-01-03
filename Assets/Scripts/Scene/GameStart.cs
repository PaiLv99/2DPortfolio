using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    private void Awake()
    {
        SceneRegist();
        DataRegist();
        UIRegist();
        RegistPool();
        UIMng.Instance.CallEvent(UIList.Logo, "LogoAnimation");
    }

    private void SceneRegist()
    {
        SceneMng.Instance.AddScene<TitleScene>(SceneType.Title, false);
        SceneMng.Instance.AddScene<GameScene>(SceneType.Game, false);
        SceneMng.Instance.AddScene<EndingScene>(SceneType.Ending, false);
    }

    // DataMng.Instance.AddData
    private void DataRegist()
    {
        //GameMng.Table.AddTable(TableType.Hero);
        //GameMng.Table.AddTable(TableType.Monster);
        //GameMng.Table.AddTable(TableType.Item);
        //GameMng.Table.AddTable(TableType.Tile);
        //GameMng.Table.AddTable(TableType.Map);
        TableMng.Instance.AddTable(TableType.Hero);
        TableMng.Instance.AddTable(TableType.Monster);
        TableMng.Instance.AddTable(TableType.Item);
        TableMng.Instance.AddTable(TableType.Tile);
        TableMng.Instance.AddTable(TableType.Map);
    }

    // private void UIRegist()
    private void UIRegist()
    {
        UIMng.Instance.Add(UIList.Logo);
        UIMng.Instance.Add(UIList.Title, false);
        UIMng.Instance.Add(UIList.Select, false);
        UIMng.Instance.Add(UIList.PauseMenu, false);
        UIMng.Instance.Add(UIList.Option, false);
        UIMng.Instance.Add(UIList.HUD, false);
        UIMng.Instance.Add(UIList.GameOver, false);
        UIMng.Instance.Add(UIList.Inventory);
        UIMng.Instance.Add(UIList.ItemPopUp);
        UIMng.Instance.Add(UIList.StatusPopUp);
        UIMng.Instance.Add(UIList.SearchPopUp);
        UIMng.Instance.Add(UIList.Ending, false);
    }

    private void RegistPool()
    {
        GameMng.Pool.RegistPool();
        //PoolMng.Instance.RegistPool();
        EffectMng.Instance.RegistPool();
    }
}
