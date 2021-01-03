using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMng : TSingleton<BuffMng>
{
    private Dictionary<BaseChar, List<Buff>> _buffChar;

    public override void Init()
    {
        _buffChar = new Dictionary<BaseChar, List<Buff>>();
    }

    public void AddBuff(BaseChar ch, Buff buff)
    {
        if (!_buffChar.ContainsKey(ch))
        {
            _buffChar.Add(ch, new List<Buff>());
        }
        _buffChar[ch].Add(buff);
    }

    public void Tick()
    {
        foreach (var iter in _buffChar)
        {
            List<Buff> buffs = iter.Value;
            BaseChar ch = iter.Key;

            for (int i =0;i < buffs.Count; i++)
            {
                EffectTick(buffs[i].Name, ch);
                buffs[i].Count -= 1;

                if (buffs[i].Count <= 0)
                {
                    ClearBuff(buffs[i].Name, ch);
                    buffs.Remove(buffs[i]);
                }
            }
        }
    }

    private void EffectTick(string name, BaseChar ch)
    {
        switch(name)
        {
            case "Flame": Flame(ch); break;
            case "Freeze": Freeze(ch); break;
            case "Invisibility": Invisibility(ch); break;
            case "Vision": Vision(ch); break;

        }
    }

    private void ClearBuff(string name, BaseChar ch)
    {
        switch (name)
        {
            case "Flame": break;
            case "Freeze": DeFreeze(ch); break;
            case "Invisibility": DeInvisibility(ch); break;
            case "Vision": DeVision(ch); break;
        }
    }

    private void Flame(BaseChar ch)
    {
        ch.MagicDamage(1);
    }

    private void Freeze(BaseChar ch)
    {
        ch.Freeze = true;
    }

    private void DeFreeze(BaseChar ch)
    {
        ch.Freeze = false;
    }

    private void Invisibility(BaseChar ch)
    {
        ch.Invisibility = true;
        ch.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    private void DeInvisibility(BaseChar ch)
    {
        ch.Invisibility = false;
        ch.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    private void Vision(BaseChar ch)
    {
        ch.Vision = true;
   
        for (int x = -1; x < 2; x++)
            for (int y = -1; y < 2; y++)
            {
                Tile tile = GameMng.Map.CurrMap._tiles[ch.NotifyPosition().x + x, ch.NotifyPosition().y + y];
                tile.Visible = true;
                tile.Explored = true;
                tile.SetVisible();
            }

        ch.Visible();
    }

    private void DeVision(BaseChar ch)
    {
        ch.Vision = false;

        for (int x = -1; x < 2; x++)
            for (int y = -1; y < 2; y++)
            {
                Tile tile = GameMng.Map.CurrMap._tiles[ch.NotifyPosition().x + x, ch.NotifyPosition().y + y];
                tile.Visible = false;
                tile.Explored = true;
                tile.SetVisible();
            }

        ch.Visible();
    }
}