using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSearch : BaseTask
{
    public Tile Tile { get; private set; }
    public Map Map { get; private set; }
    public GameObject GO { get; private set; }
    public TaskSearch(Tile tile, Map map, GameObject go) : base("TaskSearch")
    {
        GO = go;
        Tile = tile;
        Map = map;
    }
}
