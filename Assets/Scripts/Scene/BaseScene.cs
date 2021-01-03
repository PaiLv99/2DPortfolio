using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BaseScene : MonoBehaviour
{
    private Dictionary<Channel, SceneType> _channelDic = new Dictionary<Channel, SceneType>();

    public virtual void Enter() { }
    public virtual void Prograss(float value) { }
    public virtual void Exit() { }
    public virtual void Init() { }

    public void AddChannel(Channel channel, SceneType scene)
    {
        if (!_channelDic.ContainsKey(channel))
            _channelDic.Add(channel, scene);
    }

    public SceneType GetScene(Channel channel)
    {
        if (_channelDic.ContainsKey(channel))
            return _channelDic[channel];
        return SceneType.None;
    }

    public void LoadScene(SceneType scene, bool falseLoading = false, float targetTime = 1.0f)
    {
        if (!falseLoading)
            StartCoroutine(IEAsyncScene(scene, Prograss));
        else
            StartCoroutine(IEFalseAsyncScene(scene, targetTime, Prograss));
    }

    private IEnumerator IEAsyncScene(SceneType scene, System.Action<float> action = null)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());

        bool state = false;

        while(!state)
        {
            if (action != null)
                action(operation.progress);

            if (operation.isDone)
            {
                state = true;
                Enter();
            }
            yield return null;
        }
    }

    private IEnumerator IEFalseAsyncScene(SceneType scene, float targetTime = 1.0f, System.Action<float> action = null)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());

        bool state = false;
        float elapsedTime = 0;

        while (!state)
        {
            elapsedTime += Time.deltaTime / targetTime;
            elapsedTime = Mathf.Clamp01(elapsedTime);

            if (elapsedTime >= 1.0f)
            {
                state = true;
                Enter();
            }
            action(elapsedTime);
            yield return null;
        }
    }
}
