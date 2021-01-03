using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff //: MonoBehaviour
{
    //private BaseChar _ch;
    //private Status _status;

    //public bool Flame { get; set; }
    //public int FlameCount { get; set; }
    //public bool Invisibility { get; set; }
    //public int InvisibilityCount { get; set; }
    //public bool Vision { get; set; }
    //public int VisionCount { get; set; }
    //public bool Freeze { get; set; }
    //public int FreezeCount { get; set; }

    public string Name { get; private set; }
    public int Count { get; set; }

    public Buff(string name, int count)
    {
        Name = name;
        Count = count;
    }

    //public void Init( BaseChar ch, Status status )
    //{
    //    _ch = ch;
    //    _status = status;
    //}

    //public void DisCount()
    //{
    //    if (Invisibility)
    //    {
    //        InvisibilityCount -= 1;
    //    }

    //    if (Vision)
    //    {
    //        VisionCount -= 1;
    //    }

    //    if (Flame)
    //    {
    //        FlameCount -= 1;
    //        int damage = 1 + (_status.LEVEL / 5);
    //        _ch.MagicDamage(damage);
    //    }

    //    if (Freeze)
    //    {
    //        FreezeCount -= 1;
    //    }

    //    Check();
    //}

    //private void Check()
    //{
    //    if (InvisibilityCount == 0)
    //    {
    //        Invisibility = false;
    //        Color alpha = Color.white;
    //        _ch.GetComponent<SpriteRenderer>().color = alpha;
    //    }

    //    if (VisionCount == 0)
    //    {
    //        Vision = false;
    //        Monster[] mons = GameMng.Instance.GetAllMonster();
    //        for (int i = 0; i < mons.Length; i++)
    //            mons[i].Visible = false;
    //    }

    //    if (FlameCount == 0)
    //    {
    //        Flame = false;
    //    }

    //    if (FreezeCount == 0)
    //    {
    //        Freeze = false;
    //    }

    //}
}
