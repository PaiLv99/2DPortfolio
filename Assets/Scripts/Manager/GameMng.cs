using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : TSingleton<GameMng>
{
    //private TableMng _tableMng = new TableMng();
    //public static TableMng Table { get { return Instance._tableMng; } }

    //private DB _db = new DB();
    //static public DB DB { get { return Instance._db; } }

    private InputMng _input = new InputMng(); 
    public static InputMng Input { get { return Instance._input; } }

    private CameraMng _camera = new CameraMng();
    public static CameraMng Camera { get { return Instance._camera; } }

    private SpawnMng _spawn = new SpawnMng();
    public static SpawnMng Spawn { get { return Instance._spawn; } }

    private SoundMng _soundMng = new SoundMng();
    public static SoundMng Sound { get { return Instance._soundMng; } }

    private CharMng _charMng = new CharMng();
    public static CharMng CharMng { get { return Instance._charMng; } }

    private TaskMng _taskMng = new TaskMng();
    public static TaskMng Task { get { return Instance._taskMng; } }

    private MapMng _mapMng; // = new MapMng();
    public static MapMng Map { get { return Instance._mapMng; } }

    private PoolMng _poolMng = new PoolMng();
    public static PoolMng Pool { get { return Instance._poolMng; } }

    public override void Init()
    {
        //_tableMng.Init();
        //_db.Init();
        _poolMng.SetTransform(transform);
        _spawn.Init();
        _soundMng.Init();
        GameObject map = new GameObject { name = "MapMng" };
        map.transform.SetParent(transform);

        _mapMng = map.AddComponent<MapMng>();
        _mapMng.Init();

        CreateTaskReceiver();
    }

    private void CreateTaskReceiver()
    {
        GameObject receivers = new GameObject { name = "TaskReceiver" };
        receivers.transform.SetParent(transform);

        receivers.AddComponent<Attack>().Init(_taskMng);
        receivers.AddComponent<Move>().Init(_taskMng);
        receivers.AddComponent<PickUp>().Init(_taskMng);
        receivers.AddComponent<Translate>().Init(_taskMng);
        receivers.AddComponent<Throw>().Init(_taskMng);
        receivers.AddComponent<Shoot>().Init(_taskMng);
        receivers.AddComponent<Search>().Init(_taskMng);

    }

    private void Update()
    {
        if (Input != null)
            Input.OnUpdate();
    }

    private void LateUpdate()
    {
        Camera.CameraLateUpdate();
    }

    public void Clear()
    {
        Input.Clear();
        CharMng.Clear();
        Map.Clear();
        Camera.Clear();
        Task.TaskClear();
    }
}
