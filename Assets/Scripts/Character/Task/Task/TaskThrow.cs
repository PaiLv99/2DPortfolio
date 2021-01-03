using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskThrow : BaseTask
{
    public Tile TargetTile { get; private set; }
    public GameObject GO { get; private set; }
    public Map Map { get; private set; }
    public Slot Slot { get; set; }

    public TaskThrow(Tile target, GameObject go, Map map, Slot slot) : base("TaskThrow")
    {
        GO = go;
        TargetTile = target;
        Map = map;
        Slot = slot;
    }
}
