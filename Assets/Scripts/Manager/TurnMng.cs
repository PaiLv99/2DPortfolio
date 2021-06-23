using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnMng : TSingleton<TurnMng>
{
    enum Turn { Player, Monster}

    public void HeroTurn()
    {
        EffectMng.Instance.Tick();
        BuffMng.Instance.Tick();
        StartCoroutine(IEHeroTurn());
    }

    IEnumerator IEHeroTurn()
    {
        Hero hero = GameMng.CharMng.GetHero();

        if (!hero.Freeze)
        {
            hero.IsDone = false;
            hero.FoV();

            if (hero.TemporaryStorage.Count > 0)
            {
                GameMng.Task.TaskRegister(hero.TemporaryStorage.Dequeue());
                GameMng.Task.Execute();
                StartCoroutine(IECancelCheck());
            }

            while (!hero.IsDone)
                yield return null;

            hero.TileTypeChange();
            hero.Idle();
            hero.FoV();
        }
        MonsterTurn();
    }

    private void MonsterTurn()
    {
        StartCoroutine(IEMonsterTurn());
    }

    IEnumerator IEMonsterTurn()
    {
        List<Monster> monsters = GameMng.CharMng.GetMonsters();

        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i].Freeze)
                continue;
            
            // monsters[i].StartAI();
            monsters[i].MonsterAI.Tick();
            GameMng.Task.Execute();

            while (!monsters[i].IsDone)
                yield return null;
            monsters[i].TileTypeChange();
            monsters[i].Idle();
        }

        HeroTurn();
    }

    public void HeroTurnOut()
    {
        StopAllCoroutines();
        MonsterTurn();
    }

    private void TurnExit()
    {
        StopAllCoroutines();
    }

    public void Clear()
    {
        StopAllCoroutines();
    }

    private IEnumerator IECancelCheck()
    {
        Hero hero = GameMng.CharMng.GetHero();
        while (!hero.IsDone)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                hero.TemporaryStorage.Clear();
                break;
            }
            yield return null;
        }
    }
}
