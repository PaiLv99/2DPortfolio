using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : TaskReceiver<TaskMove>
{
    private readonly float _speed = 6.0f;

    private GameObject _go;
    private Tile _tile;
    private Vector2 _offset = new Vector2(0, 0.25f);


    public override void Execute(TaskMove task)
    {
        _go = task.GO;
        _tile = task.Tile;

        Vector2 start = task.GO.transform.position;
        Vector2 target = task.Tile.Position + _offset;
      
        if (task.Tile.Visible)
        {
            StartCoroutine(IEMove(start, target));
            GameMng.Sound.SfxPlay("FootStep");
        }
        else
            _go.transform.position = target;

        GameMng.Camera.SetCameraMode(Define.CameraMode.Follow);
        //UIMng.Instance.CallEvent(UIList.HUD, "HUDEnable", false);
    }

    private IEnumerator IEMove(Vector2 start, Vector2 target)
    {
        _go.GetComponent<BaseChar>().IsDone = false;

        float elapsedTime = 0;

        float dir = CalculateDir(start, target, out float speed);
        _go.transform.localScale = new Vector3(dir, 1, 1);
        _go.GetComponent<Animator>().SetBool("Move", true);
 
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime * _speed;
            _go.transform.position = Vector2.Lerp(start, target, elapsedTime);
            yield return null;
        }
        if (_tile.TILETYPE == TileType.Trap)
        {
            _tile.GetComponent<Trap>().TrapActivate();
        }

        _go.GetComponent<BaseChar>().IsDone = true;
        //UIMng.Instance.CallEvent(UIList.HUD, "HUDEnable", true);
    }

    private float CalculateDir(Vector2 start, Vector2 target, out float speed)
    {
        Vector2 delta = target - start;
        speed = 1;

        float distance = (start - target).magnitude;
        if (distance > 1 || distance < -1) 
            speed = distance;

        if (delta.x < 0)
            return -1;
        return 1;
    }
}
