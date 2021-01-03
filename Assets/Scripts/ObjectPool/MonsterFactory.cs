using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory<T> : IFactory<T> where T : Monster
{
    private GameObject _prefab;
    private Transform _parent;
    private Data.MonsterData Data;

    public MonsterFactory(GameObject prefab, Transform parent , Data.MonsterData data)
    {
        _prefab = prefab;
        _parent = parent;
        Data = data;
    }

    public T CreateObject()
    {
        GameObject obj = GameObject.Instantiate(_prefab) as GameObject;
        T t = obj.GetComponent<T>();
        t.Init(Data);
        t.transform.parent = _parent;
        obj.SetActive(false);
        return t;
    }

}
