using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BaseChar
{
    private Hero _enemy, _lockTarget;
    private Tile _prevTile, _currTile;
    public SpriteRenderer Renderer { get; private set; }

    // 8.25 
    //public bool Visible { get; set; }
    private int SightRadius { get; set; }
    private bool IsAwaken { get; set; }

    private MonsterStatus _status;
    public Data.MonsterData Data;
    private float[] _itemCreateProv = { 0.3f, 0.7f };
    private HP _hp;

    private int _awakenCount;

    public Queue<BaseTask> TempStorage { get; private set; } = new Queue<BaseTask>();
    private Tile _prevTargetTile;

    public override void Init(Data.BaseData info)
    {
        Data = info as Data.MonsterData;

        _animator = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
        _hp = Instantiate(Resources.Load<HP>("Prefabs/UI/World/HP"), transform.position + new Vector3(0,0.75f,0), transform.rotation, transform);
        _status = new MonsterStatus(Data, this, _hp);

        SightRadius = 3;
    }

    public override void Damage(int value)
    {
        IsAwaken = true;
        _animator.SetTrigger("Damage");
        _status.Damage(value);
        Helper.BlinkCount(transform, 3);

        _lockTarget = GameMng.CharMng.GetHero();
    }

    public override void MagicDamage(int value)
    {
        IsAwaken = true;
        _status.MagicDamage(value);
        _animator.SetTrigger("Damage");
        Helper.BlinkCount(transform, 3);

        _lockTarget = GameMng.CharMng.GetHero();
    }

    //private void Move()
    //{
    //    Tile targetTile = GameMng.CharMng.GetHero().FindCurrTile();

    //    List<Tile> path = PathFinder.Instance.PathFinding(FindCurrTile(), targetTile);
    //    if (path.Count > 0)
    //    {
    //        GameMng.Task.TaskRegister(new TaskMove(path[0], gameObject));
    //    }
    //}

    private void Move()
    {
        TempStorage.Clear();

        Tile targetTile = GameMng.CharMng.GetHero().FindCurrTile();
        _prevTargetTile = targetTile;

        List<Tile> path = GameMng.Map.PathFinding(FindCurrTile(), targetTile);
        if (path.Count > 0)
        {
            for (int i = 0; i < path.Count; i++)
                TempStorage.Enqueue(new TaskMove(path[0], gameObject));
            GameMng.Task.TaskRegister(TempStorage.Dequeue());
        }
    }

    public void PlayAnim(string str)
    {
        _animator.SetTrigger(str);
    }

    private void Attack()
    {
        if (_lockTarget != null)
            _lockTarget = null;

        Tile targetTile = GameMng.CharMng.GetHero().FindCurrTile();
        GameMng.Task.TaskRegister(new TaskAttack(targetTile, gameObject, _status.AP));
    }

    private void Flee()
    {
        List<Vector2Int> coords = new List<Vector2Int>();
        List<Tile> tiles = new List<Tile>();
        for (int x = 0; x < _map._width; x++)
            for (int y = 0; y < _map._height; y++)
                if (_map._tiles[x,y] != null && _map.GetTile(x, y).TILETYPE == TileType.Floor)
                {
                    coords.Add(new Vector2Int(x, y));
                    tiles.Add(_map._tiles[x, y]);
                }

        Vector2Int coord = Helper.GetShuffleRandomValue(coords.ToArray());

        Tile targetTile = Helper.GetShuffleRandomValue(tiles.ToArray());

        if (CheckTileSurrond(targetTile.Position))
        {
            List<Tile> path = GameMng.Map.PathFinding(FindCurrTile(), targetTile);
            if (path.Count > 0)
            {
                for (int i = 0; i < path.Count; i++)
                    TempStorage.Enqueue(new TaskMove(path[i], gameObject));
            }
        }
    }


    private bool CheckAttack()
    {
        Vector2Int pos = NotifyPosition();

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Tile tile = _map.GetTile(pos.x + x, pos.y + y);
                if (tile != null && tile.TILETYPE == TileType.Hero)
                    return true;
            }
        }

        return false;
    }

    public void StartAI()
    {
        if (IsAwaken)
        {
            if (CheckAttack())
                Attack();
            else if (IsSightInHero() || _enemy != null)
            {
                if (TempStorage.Count > 0)
                    GameMng.Task.TaskRegister(TempStorage.Dequeue());
                else
                    Move();
            }
            else
                Flee();
        }
        else
            Awaken();
    }

    public void Idle()
    {
        _animator.SetBool("Move", false);
    }

    // target position에 갈 수 있는지 없는지 판별하는 함수(PathFinder의 부하를 줄이기 위해) 
    private bool CheckTileSurrond(Vector2Int target)
    {
        for (int x = -1; x < 2; x++)
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Tile tile = _map._tiles[target.x + x, target.y + y];
                if (tile != null && tile.TILETYPE == TileType.Floor || tile.Position == NotifyPosition())
                    return true;
                
            }
        return false;
    }

    private bool IsSightInHero()
    {
        Vector2Int heroPos = Vector2Int.zero;
        List<Vector2Int> tiles = new List<Vector2Int>();
        Vector2Int pos = NotifyPosition();
        ShadowCaster.ComputeFOVWithShadowCast(pos.x, pos.y, SightRadius,
                                               (x, y) => _map._tiles[x, y].Transparent == false,
                                               (x, y) => { tiles.Add(new Vector2Int(x, y)); });

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] == GameMng.CharMng.GetHero().NotifyPosition())
            {
                return true;
            }
        }

        return false;
    }

    private void Awaken()
    {
        Vector2Int heroPos = Vector2Int.zero;
        List<Vector2Int> tiles = new List<Vector2Int>();
        Vector2Int pos = NotifyPosition();
        ShadowCaster.ComputeFOVWithShadowCast(pos.x, pos.y, SightRadius,
                                               (x, y) => _map._tiles[x, y].Transparent == false,
                                               (x, y) => { tiles.Add(new Vector2Int(x, y)); });
        
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] == GameMng.CharMng.GetHero().NotifyPosition())
            {
                _awakenCount++;
                break;
            }
        }

        if (_awakenCount >= 2)
        {
            Vector2 delta = NotifyPosition() - heroPos;
            Vector3 offset;
            if (delta.x < 0)
                offset = new Vector3(-1, 1);
            else
                offset = new Vector3(1, 1);
            IsAwaken = true;
            transform.localScale = offset;
            EffectMng.Instance.Pop("ExclamationMark").CallEvent(transform.position + offset * 0.5f);
        }

        IsDone = true;
    }

    public void TileTypeChange()
    {
        if (_currTile != null)
        {
            _prevTile = _currTile;

            if (_currTile.TileTypeStack.Count > 0)
                _prevTile.TILETYPE = _currTile.TileTypeStack.Pop();
            else
                _prevTile.TILETYPE = _currTile.ORIGINTYPE;
        }

        _currTile = FindCurrTile();
        _currTile.TILETYPE = TileType.Monster;

        _hp.UpdateLocalScale(transform);
    }

    public void VisibleChange(List<Vector2Int> list)
    {
        for (int i= 0; i< list.Count; i++)
        {
            if (NotifyPosition() == list[i])
            {
                Renderer.color = Color.white;
                _hp.Visible(true);
                return;
            }
        }

        Renderer.color = Color.clear;
        _hp.Visible(false);
    }

    #region Monster Death Effect
    public void DeathEffect()
    {
        StartCoroutine(IEDeathEffect());
    }

    private IEnumerator IEDeathEffect()
    {
        _hp.SetDieState();
        GameMng.CharMng.RemoveChar(UniqueID);

        _animator.SetTrigger("Die");
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            yield return null;


        float elapsedTime = 0;
        float targetTime = 1.6f;

        FindCurrTile().TILETYPE = FindCurrTile().ORIGINTYPE;

        _status.RandomItem();
        Color originColor = Renderer.color;

        while (elapsedTime <= targetTime)
        {
            elapsedTime += Time.deltaTime;
            Renderer.color = Color.Lerp(originColor, Color.clear, elapsedTime);
            yield return null;
        }

        GameMng.Pool.MonsterPush(Data._name, this);
        //PoolMng.Instance.MonsterPush(Data._name, this);
        Renderer.color = originColor;
    }
    #endregion
}
