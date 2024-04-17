using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BarrelPool
{
    private readonly int _preloadExplosionEffectsCount = 2;
    private readonly int _preloadDebrisBarrelEffectsCount = 8;
    private readonly ParticleSystem _barrelExplosionPrefab;
    private readonly DebrisBarrelEffect _debrisBarrelEffectPrefab;
    private readonly Factory _factory;
    private readonly Transform _barrelPoolEffectsParent;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private PoolBase<ParticleSystem> _barrelExplosionsPool;
    private PoolBase<DebrisBarrelEffect> _debrisBarrelEffectsPool;
    public BarrelPool(ParticleSystem barrelExplosionPrefab, DebrisBarrelEffect debrisBarrelEffectPrefab, Factory factory, Transform barrelPoolEffectsParent)
    {
        _barrelExplosionPrefab = barrelExplosionPrefab;
        _debrisBarrelEffectPrefab = debrisBarrelEffectPrefab;
        _factory = factory;
        _barrelPoolEffectsParent = barrelPoolEffectsParent;
        _barrelExplosionsPool = new PoolBase<ParticleSystem>(CreateBarrelExplosionEffect, GetAction, ReturnAction, _preloadExplosionEffectsCount);
        _debrisBarrelEffectsPool = new PoolBase<DebrisBarrelEffect>(CreateDebrisBarrelEffect, GetAction, ReturnAction, _preloadDebrisBarrelEffectsCount);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public async UniTaskVoid PlayBarrelExplosionEffect(Vector2 point)
    {
        var effect = _barrelExplosionsPool.Get();
        effect.transform.position = point;
        effect.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(effect.main.duration), cancellationToken: _cancellationTokenSource.Token);
        _barrelExplosionsPool.Return(effect);
    }
    public DebrisBarrelEffect GetDebrisBarrelEffect(Transform parent)
    {
        var effect = _debrisBarrelEffectsPool.Get();
        effect.transform.position = parent.position;
        effect.transform.SetParent(parent);
        effect.Init(parent, _debrisBarrelEffectsPool.Return);
        return effect;
    }
    private ParticleSystem CreateBarrelExplosionEffect()
    {
        return _factory.CreateEffect(_barrelExplosionPrefab, _barrelPoolEffectsParent);
    }
    private DebrisBarrelEffect CreateDebrisBarrelEffect()
    {
        return _factory.CreateEffect(_debrisBarrelEffectPrefab, _barrelPoolEffectsParent);
    }
    private void GetAction(DebrisBarrelEffect effect)
    {
        effect.gameObject.SetActive(true);
    }
    private void GetAction(ParticleSystem effect)
    {
        effect.gameObject.SetActive(true);
    }
    private void ReturnAction(DebrisBarrelEffect effect)
    {
        effect.transform.SetParent(_barrelPoolEffectsParent);
        effect.gameObject.SetActive(false);
    }
    private void ReturnAction(ParticleSystem effect)
    {
        effect.gameObject.SetActive(false);
    }
}