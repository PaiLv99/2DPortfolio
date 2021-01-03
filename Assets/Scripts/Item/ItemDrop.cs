using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public void CreateItem(Vector2 position, string itemName)
    {
        //Item item = PoolMng.Instance.ItemPop(itemName);
        Item item = GameMng.Pool.ItemPop(itemName);
        item.gameObject.SetActive(true);
        item.transform.position = position;
    }

    public void DropButton()
    {
        UIMng.Instance.CallEvent(UIList.Inventory, "ItemDrop");

    }
}
