using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BarrelPool
{
    private readonly int _preloadExplosionEffectsCount = 2;
    private readonly int _preloadDebrisBarrelEffectsCount = 8;
    private readonly ParticleSystem _barrelExplosionPrefab;
    private readonly ParticleSystem _barrelBurnPrefab;
    private readonly DebrisBarrelEffect _debrisBarrelEffectPrefab;
    private readonly Factory _factory;
    private readonly Transform _barrelPoolEffectsParent;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private PoolBase<ParticleSystem> _barrelExplosionsPool;
    private PoolBase<ParticleSystem> _barrelBurnPool;
    private PoolBase<DebrisBarrelEffect> _debrisBarrelEffectsPool;
    public BarrelPool(ParticleSystem barrelExplosionPrefab, ParticleSystem barrelBurnPrefab, DebrisBarrelEffect debrisBarrelEffectPrefab,
        Factory factory, Transform barrelPoolEffectsParent)
    {
        _barrelExplosionPrefab = barrelExplosionPrefab;
        _debrisBarrelEffectPrefab = debrisBarrelEffectPrefab;
        _barrelBurnPrefab = barrelBurnPrefab;
        _factory = factory;
        _barrelPoolEffectsParent = barrelPoolEffectsParent;
        _barrelExplosionsPool = new PoolBase<ParticleSystem>(CreateBarrelExplosionEffect, GetAction, ReturnAction, _preloadExplosionEffectsCount);
        _barrelBurnPool = new PoolBase<ParticleSystem>(CreateBarrelBurnEffect, GetAction, ReturnAction, _preloadExplosionEffectsCount);
        _debrisBarrelEffectsPool = new PoolBase<DebrisBarrelEffect>(CreateDebrisBarrelEffect, GetAction, ReturnAction, _preloadDebrisBarrelEffectsCount);
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
    public void PlayBarrelExplosionEffect(Vector2 point)
    {
        PlayEffect(_barrelExplosionsPool, point).Forget();
    }
    public void PlayBarrelBurnEffect(Vector2 point, float duration)
    {
        PlayEffect(_barrelBurnPool, point, duration).Forget();
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
    private ParticleSystem CreateBarrelBurnEffect()
    {
        return _factory.CreateEffect(_barrelBurnPrefab, _barrelPoolEffectsParent);
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

    private async UniTaskVoid PlayEffect(PoolBase<ParticleSystem> pool, Vector2 point, float duration = 0f)
    {
        var effect = pool.Get();
        effect.transform.position = point;
        effect.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(duration > 0f ? duration : effect.main.duration), cancellationToken: _cancellationTokenSource.Token);
        pool.Return(effect);
    }
}