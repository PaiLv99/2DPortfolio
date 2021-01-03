using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory<T> : IFactory<T> where T : BaseProduct
{
    private T _prefab;
    private Transform _holder;
    private string _name;

    public Factory(T prefab, string name, Transform holder)
    {
        _holder = holder;
        _prefab = prefab;
        _name = name;
    }

    public T CreateObject()
    {
        T obj = GameObject.Instantiate(_prefab);
        obj.Init(DB.Instance.GetData(_name));
        Transform t = obj.GetComponent<Transform>();
        t.transform.SetParent(_holder);
        t.gameObject.SetActive(false);

        return obj;
    }
}



//public Factory(GameObject prefab, Transform holder)
//{
//    _holder = holder;
//    _prefab = prefab;
//}

//public T CreateObject()
//{
//    GameObject obj = GameObject.Instantiate(_prefab) as GameObject;
//    obj.transform.SetParent(_holder);
//    T t = obj.GetComponent<T>();
//    obj.SetActive(false);
//    return t;
//}
