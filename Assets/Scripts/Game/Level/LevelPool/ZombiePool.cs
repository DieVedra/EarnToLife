
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ZombiePool : PoolMetods
{
    private readonly int _preloadEffectsCount = 8;

    private readonly ParticleSystem _bloodHitEffectPrefab;
    private readonly ParticleSystem _bloodEffectPrefab;
    private readonly Spawner _spawner;
    private readonly Transform _bloodEffectsPoolParent;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly PoolBase<ParticleSystem> _bloodHitEffectsPool;
    private readonly PoolBase<ParticleSystem> _bloodEffectsPool;
    public ZombiePool(ParticleSystem bloodHitEffectPrefab, ParticleSystem bloodEffectPrefab, Transform bloodEffectsPoolParent)
    {
        _spawner = new Spawner();
        _bloodHitEffectPrefab = bloodHitEffectPrefab;
        _bloodEffectPrefab = bloodEffectPrefab;
        _bloodEffectsPoolParent = bloodEffectsPoolParent;
        _bloodHitEffectsPool = new PoolBase<ParticleSystem>(CreateBloodHitEffect, GetAction,ReturnAction, _preloadEffectsCount);
        _bloodEffectsPool = new PoolBase<ParticleSystem>(CreateBloodEffect, GetAction,ReturnAction, _preloadEffectsCount);
    }
    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
    }
    public void PlayBloodHitEffect(Vector2 point)
    {
        PlayEffect(_bloodHitEffectsPool, point).Forget();
    }
    public void PlayBloodEffect(Transform parent)
    {
        PlayEffect(_bloodEffectsPool, parent).Forget();
    }
    private ParticleSystem CreateBloodHitEffect()
    {
        return _spawner.Spawn(_bloodHitEffectPrefab, _bloodEffectsPoolParent, _bloodEffectsPoolParent);
    }
    private ParticleSystem CreateBloodEffect()
    {
        return _spawner.Spawn(_bloodEffectPrefab, _bloodEffectsPoolParent, _bloodEffectsPoolParent);
    }
    private async UniTaskVoid PlayEffect(PoolBase<ParticleSystem> pool, Vector2 point, float duration = 0f)
    {
        var effect = pool.Get();
        effect.transform.position = point;
        effect.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(duration > 0f ? duration : effect.main.duration), cancellationToken: _cancellationTokenSource.Token);
        pool.Return(effect);
    }
    private async UniTaskVoid PlayEffect(PoolBase<ParticleSystem> pool, Transform parent, float duration = 0f)
    {
        var effect = pool.Get();
        effect.transform.position = parent.position;
        effect.transform.SetParent(parent);
        effect.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(duration > 0f ? duration : effect.main.duration), cancellationToken: _cancellationTokenSource.Token);
        effect.transform.SetParent(_bloodEffectsPoolParent);
        pool.Return(effect);
    }
}