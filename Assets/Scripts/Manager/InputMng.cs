using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMng 
{
    public Action<Define.TouchEvent> TouchAction = null;
    public bool UIPress { get; set; }
    private float _distance;
    private float _currTime;
    private float _prevTime;

    public void OnUpdate()
    {
        if (TouchAction != null)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _prevTime = Time.time;
                TouchAction.Invoke(Define.TouchEvent.Press);
            }

            if (Input.GetMouseButtonUp(0) && !UIPress)
            {
                _currTime = Time.time;

                if (Timer())
                    TouchAction.Invoke(Define.TouchEvent.End);
            }

            if (Input.GetMouseButton(0))
            {
                if (!GameMng.Camera.IsActive)
                {
                    _distance = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameMng.Camera.StartTouch).magnitude;

                    if (0.5f <= _distance && _distance <= 1)
                        GameMng.Camera.IsActive = true;
                }
                if (GameMng.Camera.IsActive)
                    TouchAction.Invoke(Define.TouchEvent.Drag);
            }

            if (Input.touchCount == 2)
                TouchAction.Invoke(Define.TouchEvent.Zoom);
        }
    }

    private bool Timer()
    {
        if (_currTime - _prevTime <= 1)
            return true;
        return false;
    }

    public void Clear()
    {
        TouchAction = null;
    }
}
