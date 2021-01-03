using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {  None, Floor, Wall, FrontDoor, SideDoor, Hole, Trap, Hero, Monster, Item, UpStairway, DownStairway }

public class Tile : BaseProduct
{
    public Stack<TileType> TileTypeStack = new Stack<TileType>();

    public Data.TileData Data;
    public bool IsHiding { get; set; }
    private SpriteRenderer _renderer;
    public TileType TILETYPE { get; set; }
    public TileType ORIGINTYPE { get; set; }
    public Tile PARENT { get; set; }
    public int ID { get; set; } = 0;
    public int X,Y;
    public Vector2Int Position { get; private set; }

    // 8.20 fov algorithm
    public bool Visible { get; set; }
    public bool Explored{ get; set; }
    public bool Transparent { get; set; }

    public override void Init(Data.BaseData data)
    {
        _renderer = GetComponent<SpriteRenderer>();
        Data = data as Data.TileData;
        TileType type;
        System.Enum.TryParse<TileType>(Data._type, out type);
        TILETYPE = type;
        ORIGINTYPE = type;
        name = Data._name;
    }

    public void SetVisible()
    {
        if (Visible)
            _renderer.color = Color.white;
        else
        {
            if (Explored)
                _renderer.color = Color.gray;
            else
                _renderer.color = Color.black;
        }
    }

    public void TileBuild(int x, int y, bool transparent = true)
    {
        X = x;
        Y = y;
        Position = new Vector2Int(x, y);
        Transparent = transparent;
        transform.position = new Vector3(x,y);
        gameObject.SetActive(true);
    }

    public void SetSprite(Sprite sprite)
    {
        _renderer = GetComponent<SpriteRenderer>();

        _renderer.sprite = sprite;
    }
}
