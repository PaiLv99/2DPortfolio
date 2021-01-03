using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween<T>
{
    private T _start;
    private T _end;
    private T _curr;

    private bool _state;
    private float _speed;
    private float _elapsedTime;
    private float _delayElapsedTime;
    private float _delayTargetTime;

    private System.Func<T, T, float, T> _action;
    private System.Action _delay;

    public T Curr() { return _curr; }
    public bool IsEnd() { return _state; }
    public void SetState(bool state) { _state = state; }

    private void Delay()
    {
        _delayElapsedTime += Time.deltaTime / _delayTargetTime;

        _delayElapsedTime = Mathf.Clamp01(_delayElapsedTime);

        if (_delayElapsedTime >= 1)
        {
            _delayElapsedTime = 0;
            _delay = null;
        }
    }

    // 변경된 값을 반환하는 메서드.
    public T Update()
    {
        if (_state)
            return _end;

        if (_delay != null)
        {
            _delay();
            return _start;
        }

        if (_action != null)
        {
            _elapsedTime += Time.deltaTime * _speed;
            _elapsedTime = Mathf.Clamp01(_elapsedTime);
            _curr = _action(_start, _end, _elapsedTime);

            if (_elapsedTime >= 1)
            {
                _state = true;
            }
        }
        return _curr;
    }

    public void SetTween(T start, T end, float speed, System.Func<T, T, float, T> action, float delayTime = -1f)
    {
        _start = start;
        _end = end;
        _speed = speed;
        _action = action;
        _elapsedTime = 0;
        _delayTargetTime = delayTime;

        if (delayTime > 0)
            _delay = Delay;
    }
}
