using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searching : BaseEffect
{
    //public override void Init()
    //{
    //    Name = "Searching";
    //}

    public void Awake()
    {
        Name = "Searching";
    }

    public override void CallEvent(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        StartCoroutine(IEEffect());
    }

    private IEnumerator IEEffect()
    {
        float elapsedTime = 0;

        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime * 2;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime);
            yield return null;
        }

        EffectMng.Instance.Push(Name, this);
    }

    public override void StopEffect()
    {

    }
}
