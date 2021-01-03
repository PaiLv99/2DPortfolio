using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoy : Item
{
    private Shader _colorShader;
    private Shader _gameBoyShader;
    private bool _isChecked;

    //public override void Init(ItemData data)
    //{
    //    base.Init(data);
    //    _colorShader = Shader.Find("Sprites/Default");
    //    _gameBoyShader = Shader.Find("Shader/GameBoyShader");

    //    GameMng.Instance.AddItem(this);
    //}

    public override void Init(Data.BaseData data)
    {
        base.Init(data);
        _colorShader = Shader.Find("Sprites/Default");
        _gameBoyShader = Shader.Find("Shader/GameBoyShader");

        GameMng.CharMng.AddItem(this);
    }

    private void ShaderChange()
    {
        GameObject[] allGO = FindObjectsOfType<GameObject>();
        for (int i = 0; i < allGO.Length; i++)
        {
            allGO[i].GetComponent<SpriteRenderer>().material.shader = _colorShader;
        }
    }

    public void HeroCheck()
    {
        StartCoroutine(IECheck());
    }

    private IEnumerator IECheck()
    {
        Hero hero = GameMng.CharMng.GetHero();
        while (!_isChecked)
        {
            yield return null;
            if (NotifyPosition() == hero.NotifyPosition())
            {
                _isChecked = true;
                // gameEnding Effect
            }
        }

        UIMng.Instance.SetActive(UIList.Ending, true);
    }
}
