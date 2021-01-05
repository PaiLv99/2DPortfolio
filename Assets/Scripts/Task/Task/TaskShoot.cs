using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskShoot : BaseTask
{
    public GameObject GO { get; private set; }
    public Tile Target { get; private set; }
    public Map Map { get; private set; }
    public Slot Slot { get; private set; }

    public TaskShoot(Tile tile, GameObject go, Map map, Slot slot) : base("TaskShoot")
    {
        GO = go;
        Target = tile;
        Map = map;
        Slot = slot;
    }
}
