using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : TaskReceiver<TaskPickUp>
{
    private GameObject _go;

    public override void Execute(TaskPickUp task)
    {
        _go = task.GO;
        if (UIMng.Instance.Get<Inventory>(UIList.Inventory).CheckSlot())
        {
            Item item = GameMng.CharMng.GetItem(task.Tile);
            if (item != null)
            {
                UIMng.Instance.CallEvent(UIList.Inventory, "AddSlot", item.Data);
                UIMng.Instance.CallEvent(UIList.HUD, "ItemImageMove", item.Data._sprite);

                GameMng.Pool.ItemPush(item.Data._name, item);
                //PoolMng.Instance.ItemPush(item.Data._name, item);
                GameMng.CharMng.RemoveItem(item);
            }
            StartCoroutine(IEProcessing());
            // Sound Process;
            //SoundMng.Instance.SfxPlay("ItemPickUp");
            GameMng.Sound.SfxPlay("ItemPickUp");
        }
        else
            UIMng.Instance.CallEvent(UIList.HUD, "TextBox", "가방이 가득차있다.");

    }

    private IEnumerator IEProcessing()
    {
        _go.GetComponent<BaseChar>().IsDone = false;

        float delayTime = 0;
        while (delayTime < 0.2f)
        {
            delayTime += Time.deltaTime;
            yield return null;
        }
        _go.GetComponent<BaseChar>().IsDone = true;
    }
}
