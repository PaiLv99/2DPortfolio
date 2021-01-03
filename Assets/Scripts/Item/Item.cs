using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : BaseProduct
{
    [SerializeField]
    public Data.ItemData Data {get; private set;}
    private SpriteRenderer _renderer;

    public bool Visible { get; set; } = false;
    public bool Explored { get; set; } = false;

    public override void Init(Data.BaseData data)
    {
        if (Data == null)
        {
            Data = data as Data.ItemData;
            Data._uniqueID = GetInstanceID();
            _renderer = GetComponent<SpriteRenderer>();
        }
    }

    public void VisibleChange(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (NotifyPosition() == list[i])
            {
                _renderer.color = Color.white;
                return;
            }
        }
        if (Explored)
            _renderer.color = Color.grey;
        else
            _renderer.color = Color.black;
    }

    public Vector2Int NotifyPosition()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        Vector2Int position = new Vector2Int(x, y);
        return position;
    }

    public void CreateEffect()
    {
        StartCoroutine(IECreateEffect());
    }

    private IEnumerator IECreateEffect()
    {
        Tile tile = GameMng.Map.GetTile(transform.position);
        tile.TILETYPE = TileType.Item;

        Vector3 origin = transform.position - new Vector3(0, 0.25f, 0);
        Vector3 target = transform.position;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 2;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(origin, target, interpolation);

            yield return null;
        }

        GameMng.CharMng.AddItem(this);
    }
}
