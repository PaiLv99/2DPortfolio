using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    List<T> _heap = new List<T>();
    public int Count { get { return _heap.Count; } }

    public void Push(T item)
    {
        _heap.Add(item);
        int now = _heap.Count - 1;

        while(now > 0)
        {
            int next = (now - 1) / 2;

            if (_heap[now].CompareTo(_heap[next]) < 0)
                break;

            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            now = next;
        }
    }

    public T Pop()
    {
        T ret = _heap[0];

        int lastIdx = _heap.Count - 1;
        _heap[0] = _heap[lastIdx];
        _heap.RemoveAt(lastIdx);
        lastIdx--;

        int now = 0;

        while (true)
        {
            int left = 2 * now + 1;
            int right = 2 * now + 2;

            int next = now;

            if (left <= lastIdx && _heap[now].CompareTo(_heap[left]) < 0)
                next = left;

            if (right <= lastIdx && _heap[now].CompareTo(_heap[right]) < 0)
                next = right;

            if (next == now)
                break;

            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            now = next;
        }
        return ret;
    }
}
