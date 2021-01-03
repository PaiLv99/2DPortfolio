using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventMng : TSingleton<GameEventMng>
{
    public event Action<int> OnDoorTriggerEnter;
    public void DoorTriggerEnter(int id)
    {
        if (OnDoorTriggerEnter != null)
            OnDoorTriggerEnter(id);
    }

    public event Action<int> OnDoorTriggerExit;
    public void DoorTriggerExit(int id)
    {
        if (OnDoorTriggerExit != null)
            OnDoorTriggerExit(id);
    }

    public event Action GoToGameScene;
    public void OnClickStartButton()
    {
        if (GoToGameScene != null)
            GoToGameScene();
    }

    public event Action GoToOption;
    public void OnClickOptionButton()
    {
        if (GoToOption != null)
            GoToOption();
    }
}
