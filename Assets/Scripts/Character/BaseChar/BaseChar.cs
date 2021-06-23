using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChar : BaseProduct
{
    protected Animator _animator;
    protected Map _map;
    public Map Map => _map;
    private readonly Vector2 _offset = new Vector2(0, 0.25f);

    public bool Freeze { get; set; }
    public bool Invisibility { get; set; }
    public bool Vision { get; set; }
    public int UniqueID { get; private set; }
    public bool IsDone { get; set; } = true;
    int sightRadius;
    public int SightRadius => sightRadius;

    private void Awake()
    {
        UniqueID = GetInstanceID();
    }

    public Tile FindCurrTile()
    {
        Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Tile tile = _map.GetTile(pos.x, pos.y);
        if (tile != null)
            return tile;

        return null;
    }

    public Tile FindTargetTile(Vector2 pos)
    {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);

        if (_map.GetTile(x,y) != null && _map._tiles[x,y].TILETYPE != TileType.Wall)
            return _map.GetTile(x,y);

        return null;
    }

    public virtual void SetMap(Map map)
    {
        _map = map;
    }

    public virtual void Damage(int value)
    {

    }

    public virtual void MagicDamage(int value)
    {

    }

    public void Visible()
    {
        if (Vision)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        else
            GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public Vector2Int NotifyPosition()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        Vector2Int position = new Vector2Int(x, y);
        return position;
    }

}
