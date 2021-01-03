using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private Animator _animator;

    public void Init(int x, int y)
    {
        _animator = GetComponent<Animator>();
    }

    public void CheckPlayer()
    {

    }

    private void Open()
    {
        _animator.SetBool("HoleOpen", true);
    }

    private void Closed()
    {
        _animator.SetBool("HoleOpen", false);

    }

}
