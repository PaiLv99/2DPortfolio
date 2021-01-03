using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMng
{
    private Define.CameraMode _currMode = Define.CameraMode.None;

    private Camera _camera;
    private Transform _targetPos;
    private readonly float _smoothTime = 0.001f;
    private readonly float _zoomMin = 3;
    private readonly float _zoomMax = 16;
    public Vector3 StartTouch { get; private set; }
    //private Hero _player;
    public bool IsActive { get; set; }

    public void Init()
    {
        GameMng.Input.TouchAction += TouchStart;
        GameMng.Input.TouchAction += TouchDrag;
        GameMng.Input.TouchAction += TouchZoom;
    }

    public void CameraSetting()
    {
        _camera = GameObject.FindObjectOfType<Camera>();
        _targetPos = GameObject.FindGameObjectWithTag("Player").transform;
        _camera.transform.position = _targetPos.position + new Vector3(0, 0, -10);
    }

    public void SetCameraMode(Define.CameraMode mode)
    {
        if (_currMode == mode)
            return;

        _currMode = mode;
    }

    private void FollowCamera()
    {
        if (_targetPos == null)
            return;

        Vector3 value = Vector3.zero;
        Vector3 pos = Vector3.SmoothDamp(_camera.transform.position, _targetPos.position, ref value, _smoothTime);
        pos.z = -10;

        _camera.transform.position = pos;
    }

    public void CameraLateUpdate()
    {
        if (_currMode == Define.CameraMode.Follow)
            FollowCamera();
    }

    private void TouchStart(Define.TouchEvent evt)
    {
        if (evt != Define.TouchEvent.Press)
            return;

        IsActive = false;
        StartTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void TouchDrag(Define.TouchEvent evt)
    {
        if (evt != Define.TouchEvent.Drag || _camera == null)
            return;

        _currMode = Define.CameraMode.Drag;
        Vector3 dir = StartTouch - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _camera.transform.position += dir;
    }

    private void TouchZoom(Define.TouchEvent evt)
    {
        if (evt != Define.TouchEvent.Zoom || _camera == null)
            return;

        IsActive = true;

        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 zeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 onePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevM = (zeroPrevPos - onePrevPos).magnitude;
        float m = (touchZero.position - touchOne.position).magnitude;

        float difference = m - prevM;
        Zoom(difference * 0.01f);
    }

    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _zoomMin, _zoomMax);
    }

    public void Clear()
    {
        _camera = null;
        _currMode = Define.CameraMode.None;
    }
}
