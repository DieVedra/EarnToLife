using UnityEngine;

public class DebrisPool : PoolMetods
{
    private readonly int _preloadDebrisEffectsCount = 8;

    private readonly ParticleSystem _burnEffectPrefab;
    private readonly ParticleSystem _smokeEffectPrefab;
    private readonly DebrisEffect _debrisEffectPrefab;
    private readonly Factory _factory;
    private readonly Transform _debrisPoolEffectsParent;
    private PoolBase<DebrisEffect> _debrisBarrelEffectsPool;

    public DebrisPool(ParticleSystem burnEffectPrefab, ParticleSystem smokeEffectPrefab, DebrisEffect debrisEffectPrefab,
        Factory factory, Transform debrisPoolEffectsParent)
    {
        _burnEffectPrefab = burnEffectPrefab;
        _smokeEffectPrefab = smokeEffectPrefab;
        _debrisEffectPrefab = debrisEffectPrefab;
        _factory = factory;
        _debrisPoolEffectsParent = debrisPoolEffectsParent;
        
        _debrisBarrelEffectsPool = new PoolBase<DebrisEffect>(CreateDebrisEffect, GetAction, ReturnAction, _preloadDebrisEffectsCount);

    }
    private DebrisEffect CreateDebrisEffect()
    {
        DebrisEffect debrisEffect = _factory.CreateEffect(_debrisEffectPrefab, _debrisPoolEffectsParent);
        debrisEffect.Construct(
            _factory.CreateEffect(_smokeEffectPrefab, debrisEffect.transform),
            _factory.CreateEffect(_burnEffectPrefab, debrisEffect.transform),
            _debrisBarrelEffectsPool.Return);

        return debrisEffect;
    }
    public DebrisEffect GetDebrisBarrelEffect()
    {
        return _debrisBarrelEffectsPool.Get();
    }
}