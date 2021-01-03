﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : Status
{
    private Monster _monster;

    private HP _hp;
    private string[] _createItems;

    private float[] _itemProb = { 0.5f, 0.5f };

    public MonsterStatus(Data.MonsterData data, Monster mon, HP hp)
    {
        HP = data._hp;
        MAXHP = HP;
        EXP = data._exp;
        AP = data._ap;
        DP = data._dp;
        LEVEL = data._level;
        _monster = mon;
        _hp = hp;
        InstanceID = _monster.GetInstanceID();
        _createItems = data._createItem.Split('|');
        _hp.SetStatus(this);
    }

    public override int GetAp()
    {
        int value = Random.Range(1, AP);

        return value;
    }

    public override void Damage(int value)
    {
        TextEffect text = EffectMng.Instance.Pop("TextEffect") as TextEffect;

        //if (!_hpBar.IsOpen)
            //_hpBar.Open();

        int rand = Random.Range(0, DP + 1);
        value -= rand;

        if (value <= 0)
        {
            value = 0;
            text.SetText("막음");
            text.CallEvent(_monster.transform.position); 
            return;
        }

        HP -= value;
        if (HP <= 0)
        {
            MonsterDie();
        }
        else 
        {
            EffectMng.Instance.Pop("Blood").CallEvent(_monster.transform.position);
            text.SetText(value.ToString());
            text.CallEvent(_monster.transform.position);
            float hpBar = HP / MAXHP;
            //_hpBar.FillAmount(hpBar);
            _hp.UpdateHP();
        }

    }

    private void MonsterDie()
    {
        _monster.FindCurrTile().TILETYPE = TileType.Floor;
        GameMng.CharMng.GetHero().GetEXP(EXP);
        _monster.DeathEffect();
    }

    public void RandomItem()
    {
        if (Helper.Chosen(_itemProb) >= 0.5)
        {
            int index = Random.Range(0, _createItems.Length);
            string name = _createItems[index];

            //Item item = PoolMng.Instance.ItemPop(name);
            Item item = GameMng.Pool.ItemPop(name);
            item.transform.position = _monster.transform.position;
            item.CreateEffect();
            GameMng.CharMng.AddItem(item);
        }
    }
}