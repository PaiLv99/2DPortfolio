using System.Collections;
using UnityEngine;

public class TaskAttack : BaseTask
{
    public GameObject GO { get; private set; }
    public Tile Tile { get; private set; }
    public int AP { get; private set; }
    public string Sound { get; private set; }

    public TaskAttack(Tile tile, GameObject go, int ap, string sound = "Hit") : base("TaskAttack")
    {
        GO = go;
        Tile = tile;
        AP = ap;
        Sound = sound;
    }
}