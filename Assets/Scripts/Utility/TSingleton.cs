using UnityEngine;

public class TSingleton<T> : MonoBehaviour where T : TSingleton<T>
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Helper.CreateObject<T>(null);
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    public virtual void Init()
    {

    }
}
