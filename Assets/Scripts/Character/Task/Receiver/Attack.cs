using UnityEngine;
using System.Collections;

public class Attack : TaskReceiver<TaskAttack>
{
    private GameObject _go;

    private Tile _tile;
    private Vector2 _offset = new Vector2(0, 0.25f);
    private string _hitSound;

    public override void Execute(TaskAttack task)
    {
        _go = task.GO;
        _tile = task.Tile;
        _hitSound = task.Sound;
        StartCoroutine(IEAttack(task.Tile.Position + _offset, task.AP));
    }

    private IEnumerator IEAttack(Vector2 diraction, int value)
    {
        _go.GetComponent<BaseChar>().IsDone = false;

        Vector2 start = _go.transform.position;

        // 방향 전환
        float dir = CalculateDir(start, diraction);
        _go.transform.localScale = new Vector3(dir, 1,1);
        float delayTime = 0;
        // 애니메이션
        _go.GetComponent<Animator>().SetTrigger("Attack");

        while (delayTime < 0.5)
        {
            delayTime += Time.deltaTime;
            yield return null;
        }

        BaseChar ch = GameMng.CharMng.GetChar(_tile);
        if (ch != null)
        {
            ch.Damage(value);
            GameMng.Sound.SfxPlay(_hitSound);
        }

        _go.GetComponent<BaseChar>().IsDone = true;
    }

    private float CalculateDir(Vector2 start, Vector2 target)
    {
        Vector2 delta = target - start;
        if (delta.x < 0)
            return -1;
        return 1;
    }
}