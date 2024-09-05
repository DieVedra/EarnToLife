using UnityEngine;

public class DebrisPool : PoolMetods
{
    private readonly int _preloadDebrisEffectsCount = 8;

    private readonly ParticleSystem _burnEffectPrefab;
    private readonly ParticleSystem _smokeEffectPrefab;
    private readonly DebrisEffect _debrisEffectPrefab;
    private readonly Spawner _spawner;
    private readonly Transform _debrisPoolEffectsParent;
    private readonly PoolBase<DebrisEffect> _debrisBarrelEffectsPool;

    public DebrisPool(ParticleSystem burnEffectPrefab, ParticleSystem smokeEffectPrefab, DebrisEffect debrisEffectPrefab, Transform debrisPoolEffectsParent)
    {
        _burnEffectPrefab = burnEffectPrefab;
        _smokeEffectPrefab = smokeEffectPrefab;
        _debrisEffectPrefab = debrisEffectPrefab;
        _spawner = new Spawner();
        _debrisPoolEffectsParent = debrisPoolEffectsParent;
        
        _debrisBarrelEffectsPool = new PoolBase<DebrisEffect>(CreateDebrisEffect, GetAction, this.ReturnAction, _preloadDebrisEffectsCount);

    }
    private DebrisEffect CreateDebrisEffect()
    {
        DebrisEffect debrisEffect = _spawner.Spawn(_debrisEffectPrefab, _debrisPoolEffectsParent, _debrisPoolEffectsParent);
        debrisEffect.Construct(
            _spawner.Spawn(_smokeEffectPrefab, debrisEffect.transform, debrisEffect.transform),
            _spawner.Spawn(_burnEffectPrefab, debrisEffect.transform, debrisEffect.transform),
            this.ReturnAction);

        return debrisEffect;
    }
    protected override void ReturnAction(DebrisEffect effect)
    {
        effect.transform.SetParent(_debrisPoolEffectsParent);
        base.ReturnAction(effect);
    }
    public DebrisEffect GetDebrisBarrelEffect()
    {
        return _debrisBarrelEffectsPool.Get();
    }
}