using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTranslate : BaseTask
{
    public Map Map { get; private set; }
    public Tile Tile { get; private set; }
    public GameObject GO { get; private set; }
    public TaskTranslate(Map map, Tile tile, GameObject go) : base("TaskTranslate")
    {
        GO = go;
        Map = map;
        Tile = tile;
    }
}
