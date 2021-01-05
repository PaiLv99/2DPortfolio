using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPickUp : BaseTask
{
    //public Vector2 Dir { get; private set; }
    //public Map Map { get; private set; }
    public GameObject GO { get; private set; }
    public Tile Tile { get; private set; }
    public TaskPickUp(Tile tile, GameObject go) : base("TaskPickUp")
    {
        GO = go;
        Tile = tile;
    }
}
