using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    protected Animator _animator;
    public Map _map;
    private bool _isActive;
    private Vector2Int _coord;
    private int _damage = 5;

    public void Init(int x, int y)
    {
        _animator = GetComponent<Animator>();
        _coord = new Vector2Int(x, y);
    }

    public void TrapActivate()
    {
        if (GameMng.CharMng.GetHero().NotifyPosition() == _coord)
        {
            if (!_isActive)
            {
                _isActive = true;
                _animator.SetTrigger("Trap");
                GameMng.Map.CurrMap._tiles[_coord.x, _coord.y].TILETYPE = TileType.Floor;
                // 플레이어의 체력에 비례해서 만들어지는 공격력의 공식은 무었일까??
                GameMng.CharMng.GetHero().Damage(_damage);
            }
        }
       
    }
}
