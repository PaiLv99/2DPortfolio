using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(DoorArea))]
public class Door : MonoBehaviour
{
    public enum DoorType { Vertical, Horizontal, Up, Down, Right, Left }
    public DoorType _type = DoorType.Vertical;

    private Tile _tile;

    private Dictionary<string, Sprite> _sprites;

    public int ID { get; private set; }

    public void Init(Tile tile)
    {

        _sprites = new Dictionary<string, Sprite>();

        _tile = tile;
        if (_tile.TILETYPE == TileType.FrontDoor)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Image/Map/Doors/Front");
            for (int i = 0; i < sprites.Length; i++)
                _sprites.Add(sprites[i].name, sprites[i]);
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Image/Map/Doors/Side");
            for (int i = 0; i < sprites.Length; i++)
                _sprites.Add(sprites[i].name, sprites[i]);
        }
    }

    public void Check()
    {
        BaseChar ch = GameMng.CharMng.GetChar(_tile);
        Item item = GameMng.CharMng.GetItem(_tile);

        if (ch != null)
            Open();
        else if (item != null)
            Open();
        else
            Closed();
    }

    private void Open()
    {
        //_renderer.sprite = _sprites["DoorOpen"];
        _tile.Transparent = true;
        //SoundMng.Instance.SfxPlay("Door");
        GameMng.Sound.SfxPlay("DoorOpen");

    }

    private void Closed()
    {
        //_renderer.sprite = _sprites["DoorClosed"];
        _tile.Transparent = false;
        //SoundMng.Instance.SfxPlay("Door");
        GameMng.Sound.SfxPlay("DoorClosed");
    }
}

