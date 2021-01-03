using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiile : MonoBehaviour
{
    private float _speed, _damage, _chargeCount;

    public virtual void Init(ProjectileData data)
    {
        _speed = data._speed;

    }
}
