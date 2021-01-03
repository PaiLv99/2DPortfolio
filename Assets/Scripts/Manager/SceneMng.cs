using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType { None, Title, Lobby, Game, Ending }
public enum Channel { Prev, Curr }

public class SceneMng : TSingleton<SceneMng>
{
    private Dictionary<SceneType, BaseScene> _sceneDic = new Dictionary<SceneType, BaseScene>();
    private SceneType _currScene = SceneType.None;

    public override void Init()
    {

    }

    public T AddScene<T>(SceneType scene, bool state = false) where T : BaseScene
    {
        if (!_sceneDic.ContainsKey(scene))
        {
            T t = Helper.CreateObject<T>(transform, false);
            t.enabled = state;
            _sceneDic.Add(scene, t);
            return t;
        }
        return null;
    }

    public void SceneLoading(SceneType scene, bool falseLoading = false)
    {
        if (_sceneDic.ContainsKey(_currScene))
            _sceneDic[_currScene].Exit();

        if (_sceneDic.ContainsKey(scene))
        {
            _currScene = scene;
            _sceneDic[scene].LoadScene(scene, falseLoading);
        }
    }
}
