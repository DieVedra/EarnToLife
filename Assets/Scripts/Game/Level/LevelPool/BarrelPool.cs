using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BarrelPool : PoolMetods
{
    private readonly int _preloadExplosionEffectsCount = 2;
    private readonly ParticleSystem _barrelExplosionPrefab;
    private readonly ParticleSystem _burnEffectPrefab;
    private readonly Spawner _spawner;
    private readonly Transform _barrelPoolEffectsParent;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private PoolBase<ParticleSystem> _barrelExplosionsPool;
    private PoolBase<ParticleSystem> _barrelBurnPool;
    private PoolBase<DebrisEffect> _debrisBarrelEffectsPool;
    public BarrelPool(ParticleSystem barrelExplosionPrefab, ParticleSystem burnEffectPrefab, Transform barrelPoolEffectsParent)
    {
        _spawner = new Spawner();
        _barrelExplosionPrefab = barrelExplosionPrefab;
        _burnEffectPrefab = burnEffectPrefab;
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
        return _spawner.Spawn(_barrelExplosionPrefab, _barrelPoolEffectsParent, _barrelPoolEffectsParent);
    }
    private ParticleSystem CreateBarrelBurnEffect()
    {
        return _spawner.Spawn(_burnEffectPrefab, _barrelPoolEffectsParent, _barrelPoolEffectsParent);
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