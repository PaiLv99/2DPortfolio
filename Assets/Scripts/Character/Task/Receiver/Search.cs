using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : TaskReceiver<TaskSearch>
{
    private Vector2Int _position;
    private Map _map;
    private GameObject _go;

    public override void Execute(TaskSearch task)
    {
        _go = task.GO;
        _position = task.Tile.Position;
        _map = task.Map;
        StartCoroutine(IESearch());
    }

    private IEnumerator IESearch()
    {
        _go.GetComponent<BaseChar>().IsDone = false;

        // effect
        // animation
        Searching();

        float elapedTime = 0;

        while (elapedTime <= 0.2f)
        {
            elapedTime += Time.deltaTime;
            yield return null;
        }

        _go.GetComponent<BaseChar>().IsDone = true;
    }

    private void Searching()
    {
        int x = _position.x;
        int y = _position.y;

        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                Tile tile = _map._tiles[x + dx, y + dy];
                CheckTile(tile);
                EffectMng.Instance.Pop("Searching").CallEvent(new Vector3(x + dx, y + dy));
            }
        }

    }

    private void CheckTile(Tile tile)
    {
        if (tile.IsHiding)
        {
            tile.SetSprite(Resources.Load<Sprite>("Image/Map/Wall/" + tile.TILETYPE));
        }
    }
}
