using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BaseChar
{
    private Hero mHero;
    private Tile _prevTile, _currTile;
    public SpriteRenderer Renderer { get; private set; }

    bool isAwaken;
    public bool IsAwaken => Status.HPRatio < 1 || isAwaken != false || awakenCount >= 0;

    int radius;
    public int Radius => radius;

    private MonsterStatus _status;
    public MonsterStatus Status => _status;

    int awakenCount = -1;
    public int AwakenCount => awakenCount;

    public int fleeIndex = 0;
    public List<Tile> fleeTiles = new List<Tile>();

    public Data.MonsterData Data;
    private float[] _itemCreateProv = { 0.3f, 0.7f };
    private HP _hp;

    public int CURRHP;

    MonsterAI monsterAI;
    public MonsterAI MonsterAI => monsterAI;

    public override void Init(Data.BaseData info)
    {
        Data = info as Data.MonsterData;

        _animator = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
        _hp = Instantiate(Resources.Load<HP>("Prefabs/UI/World/HP"), transform.position + new Vector3(0,0.75f,0), transform.rotation, transform);
        _status = new MonsterStatus(Data, this, _hp);

        radius = 3;

        CURRHP = _status.Hp;

        mHero = GameMng.CharMng.GetHero();

        monsterAI = new MonsterAI(this, mHero);
    }

    public override void Damage(int value)
    {
        _animator.SetTrigger("Damage");
        _status.Damage(value);
        CURRHP -= value;
        Helper.BlinkCount(transform, 3);
    }

    public override void MagicDamage(int value)
    {
        _status.MagicDamage(value);
        _animator.SetTrigger("Damage");
        CURRHP -= value;
        Helper.BlinkCount(transform, 3);
    }

    public void PlayAnim(string str)
    {
        _animator.SetTrigger(str);
    }

    /*
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
    */

    public void Idle()
    {
        _animator.SetBool("Move", false);
    }

    /*
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
    */


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

    public void SetAwakenCount(int count)
    {
        awakenCount += count;
    }

    #region Monster Death Effect
    public void DeathEffect()
    {
        FindCurrTile().TILETYPE = FindCurrTile().ORIGINTYPE;

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


        _status.RandomItem();
        Color originColor = Renderer.color;

        while (elapsedTime <= targetTime)
        {
            elapsedTime += Time.deltaTime;
            Renderer.color = Color.Lerp(originColor, Color.clear, elapsedTime);
            yield return null;
        }

        GameMng.Pool.MonsterPush(Data._name, this);

        Renderer.color = originColor;
    }
    #endregion
}
