using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPool<T> 
{
    public Queue<T> _pool = new Queue<T>();
    private IFactory<T> _factory;

    public TPool(IFactory<T> factory, int count)
    {
        _factory = factory;
        for (int i = 0; i < count; i++)
        {
            T t = _factory.CreateObject();
            _pool.Enqueue(t);
        }
    }

    private void Create()
    {
        T t = _factory.CreateObject();
        _pool.Enqueue(t);
    }

    public T Pop()
    {
        if (_pool.Count <= 0)
            Create();
        return _pool.Dequeue();
    }

    public void Push(T t)
    {
        _pool.Enqueue(t);
    }
}


