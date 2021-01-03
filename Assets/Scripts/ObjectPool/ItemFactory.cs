using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory<T> : IFactory<T> where T : Item
{
    private GameObject _prefab;
    private Transform _parent;
    private Data.ItemData _info;

    public ItemFactory(GameObject prefab, Transform parent, Data.ItemData info)
    {
        _prefab = prefab;
        _info = info;
        _parent = parent;
    }


    public T CreateObject()
    {
        GameObject obj = GameObject.Instantiate(_prefab) as GameObject;
        T t = obj.GetComponent<T>();
        t.transform.parent = _parent;
        obj.SetActive(false);
        return t;
    }
}
