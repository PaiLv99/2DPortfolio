using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : BaseProduct
{
    protected SpriteRenderer _renderer;
    protected Sprite[] _sprites;
    public bool IsActive { get; set; }
    public new string Name  { get; protected set; }
    public int Count { get; set; } = 1;

    public virtual void CallEvent(Vector3 position)
    {

    }

    public virtual void StopEffect()
    {

    }
}
