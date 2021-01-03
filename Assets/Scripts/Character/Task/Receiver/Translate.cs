using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : TaskReceiver<TaskTranslate>
{
    private GameObject _go;
    private Map _map;
    private Tile _tile;

    public override void Execute(TaskTranslate task)
    {
        _go = task.GO;
        _map = task.Map;
        _tile = task.Tile;
        StartCoroutine(IEProcessing());
    }

    private IEnumerator IEProcessing()
    {
        _go.GetComponent<BaseChar>().IsDone = false;

        float elapsedTime = 0;

        while (elapsedTime <= 0.2f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (_tile.ORIGINTYPE == TileType.UpStairway && _map.PrevMap != null)
            GameMng.Map.UpTranslate(_map.PrevMap);
        else if (_tile.ORIGINTYPE == TileType.DownStairway && _map.NextMap != null)
            GameMng.Map.DownTranslate(_map.NextMap);

        _go.GetComponent<BaseChar>().IsDone = true;
    }
}
