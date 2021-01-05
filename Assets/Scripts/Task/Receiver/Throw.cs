using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : TaskReceiver<TaskThrow>
{
    private GameObject _go;
    private Tile _targetTile;
    private Item _item;
    private readonly float moveSpeed = 12.0f;

    public override void Execute(TaskThrow task)
    {
        _go = task.GO;
        Vector2Int start = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        _targetTile = Helper.RayCastWithBresenham(start, task.TargetTile.Position, task.Map);

        //_item = PoolMng.Instance.ItemPop(task.Slot._itemData._name);
        _item = GameMng.Pool.ItemPop(task.Slot._itemData._name);
        _item.gameObject.SetActive(true);
        _item.Visible = true;
        _item.transform.parent = GameMng.Map.CurrMap._holder.transform;

        if (task.Slot._slotType == Slot.SlotType.Equipment)
            task.Slot.ClearSlot();
        else
            task.Slot.SetCount(-1);
       
        StartCoroutine(IEMoveObject(_targetTile));
        UIMng.Instance.CallEvent(UIList.HUD, "ThrowAndShootUI", false);
        //UIMng.Instance.CallEvent(UIList.HUD, "CheckItemSlot", task.Slot._itemData._name);
    }

    private IEnumerator IEMoveObject(Tile tile)
    {
        _go.GetComponent<BaseChar>().IsDone = false;

        Vector2 curr = transform.position;
        Vector2 target = tile.Position;

        int x = (int)Mathf.Abs(curr.x - target.x);
        int y = (int)Mathf.Abs(curr.y - target.y);

        int moveCount = 0;

        if (x > y)
            moveCount = x;
        else
            moveCount = y;

        float elapsedTime = 0;
        float targetTime = 1f;

        while (elapsedTime < targetTime)
        {
            elapsedTime += Time.deltaTime * moveSpeed / moveCount;
            _item.transform.position = Vector2.Lerp(curr, target, elapsedTime);
            yield return null;
        }

        if (tile.TILETYPE == TileType.Trap)
            tile.GetComponent<Trap>().TrapActivate();

        //if (tile.TILETYPE == TileType.DoorSide)
        //    tile.GetComponent<Door>().Open();

        if (_item.Data._type == "Potion")
        {
            ItemMng.Instance.ThrowPotion(_item.Data._part, _item.transform.position);
            UIMng.Instance.CallEvent(UIList.Inventory, "CheckItemSlot", _item.Data._name);
            GameMng.Pool.ItemPush(_item.Data._name, _item);
            //PoolMng.Instance.ItemPush(_item.Data._name, _item);
            GameMng.Sound.SfxPlay("PosionBroken");
        }
        else if (tile.ORIGINTYPE == TileType.DownStairway || tile.ORIGINTYPE == TileType.UpStairway || tile.TILETYPE == TileType.Monster)
            StartCoroutine(IEObstacleBounce());
        else
            StartCoroutine(IEBounce());

    }

    private IEnumerator IEBounce()
    {
        float percent = 0;

        Vector3 origin = _item.transform.position;
        Vector3 target = _item.transform.position + new Vector3(0, 0.25f, 0);

        while (percent < 1)
        {
            percent += Time.deltaTime * 2;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            _item.transform.position = Vector2.Lerp(origin, target, interpolation);

            yield return null;
        }

        _targetTile.TileTypeStack.Push(TileType.Item);
        GameMng.CharMng.AddItem(_item);
        _go.GetComponent<BaseChar>().IsDone = true;
    }

    private IEnumerator IEObstacleBounce()
    {
        Map map = GameMng.Map.CurrMap;
        List<Tile> tiles = new List<Tile>();
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                Tile tile = map._tiles[_targetTile.X + dx, _targetTile.Y + dy];
                if (tile != null && tile.TILETYPE == TileType.Floor)
                    tiles.Add(tile);
            }
        }
        int rand = Random.Range(0, tiles.Count);
        Tile selectedTile = tiles[rand];
        Vector3 curr = _item.transform.position;

        Vector2 p1 = _item.transform.position;
        Vector2 p2 = _item.transform.position + new Vector3(0, 1, 0);
        Vector2 p3 = selectedTile.transform.position + new Vector3(0, 1, 0);
        Vector2 p4 = selectedTile.transform.position;

        float elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime * 2; 
            _item.transform.position = Helper.Bezier2D(elapsedTime, p1, p2, p3, p4);
            yield return null;
        }

        selectedTile.TileTypeStack.Push(TileType.Item);
        GameMng.CharMng.AddItem(_item);
        _go.GetComponent<BaseChar>().IsDone = true;
    }
}
