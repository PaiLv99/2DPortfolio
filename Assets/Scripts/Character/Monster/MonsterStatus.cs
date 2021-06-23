using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : Status
{
    private Monster _monster;

    public int AP { get; set; }
    public int EXP { get; set; }
    public int DP { get; set; }
    public int LEVEL { get; protected set; }
    public int MAXHP { get; protected set; }
    public int InstanceID { get; protected set; }


    private HP _hp;
    private string[] _createItems;

    float stackHeal;

    private float[] _itemProb = { 0.5f, 0.5f };

    public float HPRatio => mHp / (float)MaxHp;

    int mHp;
    public int Hp => mHp;

    int maxHp;
    public int MaxHp => maxHp;

    public int Threshold = 3;

    public MonsterStatus(Data.MonsterData data, Monster mon, HP hp)
    {
        mHp = data._hp;
        maxHp = mHp;
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

        mHp -= value;
        if (mHp <= 0)
        {
            MonsterDie();
        }
        else 
        {
            EffectMng.Instance.Pop("Blood").CallEvent(_monster.transform.position + new Vector3(0.5f, 0.5f) );
            text.SetText(value.ToString());
            text.CallEvent(_monster.transform.position);
            _hp.UpdateHP();
        }

    }

    private void MonsterDie()
    {
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

    public void Heal(float heal)
    {
        if (heal < 1)
        {
            stackHeal += heal;
            if (stackHeal >= 1)
            {
                mHp += (int)stackHeal;
                stackHeal = 0;
            }
        }
        else
        {
            mHp += (int)heal;

        }

        if (mHp >= MaxHp )
        {
            mHp = MaxHp;
        }
    }
}
