using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BarrelPool : PoolMetods
{
    private readonly int _preloadExplosionEffectsCount = 2;
    private readonly ParticleSystem _barrelExplosionPrefab;
    private readonly ParticleSystem _burnEffectPrefab;
    private readonly Factory _factory;
    private readonly Transform _barrelPoolEffectsParent;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private PoolBase<ParticleSystem> _barrelExplosionsPool;
    private PoolBase<ParticleSystem> _barrelBurnPool;
    private PoolBase<DebrisEffect> _debrisBarrelEffectsPool;
    public BarrelPool(ParticleSystem barrelExplosionPrefab, ParticleSystem burnEffectPrefab, 
        Factory factory, Transform barrelPoolEffectsParent)
    {
        _barrelExplosionPrefab = barrelExplosionPrefab;
        _burnEffectPrefab = burnEffectPrefab;
        _factory = factory;
        _barrelPoolEffectsParent = barrelPoolEffectsParent;
        _barrelExplosionsPool = new PoolBase<ParticleSystem>(CreateBarrelExplosionEffect, GetAction, ReturnAction, _preloadExplosionEffectsCount);
        _barrelBurnPool = new PoolBase<ParticleSystem>(CreateBarrelBurnEffect, GetAction, ReturnAction, _preloadExplosionEffectsCount);
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
    private ParticleSystem CreateBarrelExplosionEffect()
    {
        return _factory.CreateEffect(_barrelExplosionPrefab, _barrelPoolEffectsParent);
    }
    private ParticleSystem CreateBarrelBurnEffect()
    {
        return _factory.CreateEffect(_burnEffectPrefab, _barrelPoolEffectsParent);
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