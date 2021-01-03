using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    public float _damage, _speed, _chargeCount;

    public ProjectileData(float damage, float speed, float chargeCount)
    {
        _damage = damage;
        _speed = speed;
        _chargeCount = chargeCount;
    }
}
