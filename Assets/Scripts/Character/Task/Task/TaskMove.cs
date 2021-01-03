using System.Collections;
using UnityEngine;

public class TaskMove : BaseTask
{
    public Tile Tile { get; private set; }
    public GameObject GO { get; private set; }
    public string FootStep { get; private set; }

    public TaskMove(Tile tile, GameObject go) : base("TaskMove")
    {
        Tile = tile;
        GO = go;
    }
}


