using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMarkEffect : BaseEffect
{
    private Coroutine _coroutine;

    private void Awake()
    {
        _sprites = Resources.LoadAll<Sprite>("Image/Effect/ExclamtionMark");
        _renderer = GetComponent<SpriteRenderer>();
        Name = "ExclamationMark";
    }

    public override void CallEvent(Vector3 position)
    {
        Count = 1;
        gameObject.SetActive(true);
        transform.position = position;
        StartEffect();
    }

    public override void StopEffect()
    {
        StopCoroutine(_coroutine);
        EffectMng.Instance.Push(Name, this);
    }

    private void StartEffect()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(IEScaleEffect());
    }

    private IEnumerator IEScaleEffect()
    {
        Vector3 target = transform.localScale * 1.2f;
        Vector3 start = transform.localScale;

        while (true)
        {
            float percent = 0;
            while (percent <= 1)
            {
                percent += Time.deltaTime * 2.0f;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.localScale = Vector3.Lerp(start, target, interpolation);
                yield return null;
            }
            yield return null;
        }
    }
}
