using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : TaskReceiver<TaskShoot>
{
    private Tile _targetTile;
    private Item _projectile;
    private string _itemName;
    private int _damageValue;

    private GameObject _go;

    private readonly float moveSpeed = 10.0f;

    public override void Execute(TaskShoot task)
    {
        _go = task.GO;

        _itemName = task.Slot._itemData._name;

        Vector2Int start = new Vector2Int((int)_go.transform.position.x, (int)_go.transform.position.y);
        _targetTile = Helper.RayCastWithBresenham(start, task.Target.Position, task.Map);

        if (task.Slot._itemData._name == "Bow")
        {
            //_projectile = PoolMng.Instance.ItemPop("Arrow");
            _projectile = GameMng.Pool.ItemPop("Arrow");
            int min = 1 + task.Slot._itemData._tear + task.Slot._itemData._enchant;
            int max = task.Slot._itemData._value + (1 + task.Slot._itemData._tear) * task.Slot._itemData._enchant + 1;
            _damageValue = Random.Range(min, max);
        }
        else if (task.Slot._itemData._type == "Wand")
        {
            //_projectile = PoolMng.Instance.ItemPop("Magic");
            _projectile = GameMng.Pool.ItemPop("Magic");

            int min = 1 + task.Slot._itemData._tear + task.Slot._itemData._enchant;
            int max = task.Slot._itemData._value + (1 + task.Slot._itemData._tear) * task.Slot._itemData._enchant + 1;
            _damageValue = Random.Range(min, max);
        }
        else
        {
            //_projectile = PoolMng.Instance.ItemPop(task.Slot._itemData._name);
            _projectile = GameMng.Pool.ItemPop(task.Slot._itemData._name);

            _damageValue = Random.Range(_projectile.Data._tear + _projectile.Data._enchant, _projectile.Data._value + 1);
            task.Slot.SetCount(-1);
        }
        _projectile.gameObject.SetActive(true);

        StartCoroutine(IEMovingObject(_targetTile));

        // UI process 인벤토리를 체크한다.
        UIMng.Instance.CallEvent(UIList.Inventory, "CheckItemSlot", _itemName);
        UIMng.Instance.CallEvent(UIList.HUD, "ThrowAndShootUI", false);

        // Shoot sound 
        GameMng.Sound.SfxPlay(_projectile.Data._name + "Shoot");
    }

    private IEnumerator IEMovingObject(Tile tile)
    {
        _go.GetComponent<BaseChar>().IsDone = false;
        _go.GetComponent<Animator>().SetTrigger("Attack");

        Vector2 curr = _go.transform.position;
        Vector2 target = tile.Position;

        _projectile.transform.position = curr;
        CalculateDir(curr, target);

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
            _projectile.transform.position = Vector2.Lerp(curr, target, elapsedTime);
            yield return null;
        }

        BaseChar ch = GameMng.CharMng.GetChar(tile);
        if (ch != null)
        {
            ch.Damage(_damageValue);
            GameMng.Sound.SfxPlay(_projectile.Data._itemSound);
        }

        // Projectile Object Pool push
        GameMng.Pool.ItemPush(_projectile.Data._name, _projectile);
        //PoolMng.Instance.ItemPush(_projectile.Data._name, _projectile);
        _go.GetComponent<BaseChar>().IsDone = true;
    }

    private void CalculateDir(Vector2 start, Vector2 target)
    {
        Vector2 delta = target - start;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + 270;

        _projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
