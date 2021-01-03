using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMng : TSingleton<EffectMng>
{
    private BaseEffect[] _effectPrefabs;
    private Dictionary<string, TPool<BaseEffect>> _effectPool = new Dictionary<string, TPool<BaseEffect>>();
    private List<BaseEffect> _activeEffect = new List<BaseEffect>();

    public override void Init()
    {
        
    }

    public void RegistPool()
    {
        _effectPrefabs = Resources.LoadAll<BaseEffect>("Prefabs/Effects/SpriteEffects");

        foreach (var prefab in _effectPrefabs)
            _effectPool.Add(prefab.name, new TPool<BaseEffect>(new Factory<BaseEffect>(prefab, prefab.name, transform), 5));
    }

    public void Push(string type, BaseEffect effect)
    {
        if (_effectPool.ContainsKey(type))
        {
            _effectPool[type].Push(effect);
            _activeEffect.Remove(effect);
            effect.transform.SetParent(transform);
            effect.gameObject.SetActive(false);
        }
    }

    public BaseEffect Pop(string type)
    {
        if (_effectPool.ContainsKey(type))
        {
            BaseEffect effect = _effectPool[type].Pop();
            _activeEffect.Add(effect);
            return effect;
        }
        return null;
    }

    public void Tick()
    {
        if (_activeEffect.Count > 0)
            for (int i = _activeEffect.Count - 1; i >= 0; i--)
            {
                if (_activeEffect[i].Count <= 0)
                    _activeEffect[i].StopEffect();
            }
   
        for (int i = 0; i < _activeEffect.Count; i++)
            _activeEffect[i].Count -= 1;
    }
}
