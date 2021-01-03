using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffect : BaseEffect
{
    private int _currFrame = 0;
    private float _timePerFrame;
    private int _frameRate = 8;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _sprites = Resources.LoadAll<Sprite>("Image/Effect/Blood");
        Name = "Blood";
    }

    public override void CallEvent(Vector3 position)
    {
        gameObject.SetActive(true);
        this.transform.position = position;
        StartCoroutine(IESpriteEffect());
    }

    private IEnumerator IESpriteEffect()
    {
        _timePerFrame = 1f / _frameRate;

        float elapsedTime = 0;
        while (_currFrame < _sprites.Length)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= _timePerFrame)
            {
                elapsedTime = 0;
                if (_currFrame < _sprites.Length)
                    _renderer.sprite = _sprites[_currFrame];
                ++_currFrame;
            }
            yield return null;
        }
        _currFrame = 0;
        EffectMng.Instance.Push(Name, this);
    }
}
