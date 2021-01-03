using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameEffect : BaseEffect
{
    private int _currFrame = 0;
    private float _timePerFrame;
    private int _frameRate = 12;
    private Coroutine _coroutine;

    private void Awake()
    {
        _sprites = Resources.LoadAll<Sprite>("Image/Effect/FireEffectAtlas");
        _renderer = GetComponent<SpriteRenderer>();
        Name = "FlameEffect";
    }

    public override void CallEvent(Vector3 position)
    {
        gameObject.SetActive(true);
        this.transform.position = position;
        _coroutine = StartCoroutine(IESpriteEffect());
    }

    public void Parent(Transform t)
    {
        transform.SetParent(t);
    }

    public override void StopEffect()
    {
        StopCoroutine(_coroutine);
        EffectMng.Instance.Push(Name, this);
    }

    private IEnumerator IESpriteEffect()
    {
        _timePerFrame = 1f / _frameRate;
        float elapsedTime = 0;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= _timePerFrame)
            {
                elapsedTime = 0;

                if (_currFrame > _sprites.Length)
                        _currFrame = 0;

                if (_currFrame < _sprites.Length)
                    _renderer.sprite = _sprites[_currFrame];
                ++_currFrame;
            }
            yield return null;
        }
    }
}
