using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorArea : MonoBehaviour
{
    private int _id;
    private BoxCollider2D _collider;

    public void Init(int id)
    {
        _id = id;
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Check()
    {
        _collider.enabled = false;
        RaycastHit2D[] hit = Physics2D.LinecastAll(transform.position, transform.position);
        _collider.enabled = true;

        if (hit.Length > 0)
            GameEventMng.Instance.DoorTriggerEnter(_id);
        else
            GameEventMng.Instance.DoorTriggerExit(_id);
    }
}
