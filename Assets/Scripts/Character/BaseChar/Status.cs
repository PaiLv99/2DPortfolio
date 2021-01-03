using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public int HP { get; set; }
    public int AP { get; set; }
    public int EXP { get; set; }
    public int DP { get; set; }
    public int LEVEL { get; protected set; }
    public int MAXHP { get; protected set; }
    public int InstanceID { get; protected set; }

    public virtual int GetAp()
    {
        return -1;
    }

    public virtual void Damage(int value)
    {

    }

    public virtual void MagicDamage(int value)
    {

    }
}
