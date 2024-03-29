using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DestructionEffectsHandler :IDispose
{
    private readonly int _preloadCount = 4;
    private readonly ParticleSystem.MinMaxCurve _smokeEffectMainModuleType2StartSize = new ParticleSystem.MinMaxCurve(2f,3f);
    private readonly Color _smokeEffectMainModuleType2StartColor = new Color(1f,1f,1f,0.9f);
    private readonly ParticleSystem.MinMaxCurve _smokeEffectMainModuleType2StartLifetime  = new ParticleSystem.MinMaxCurve(2f,3f);
    
    
    private readonly float _smokeEffectMainModuleType1StartSize = 1f;
    private readonly Color _smokeEffectMainModuleType1StartColor = new Color(1f,1f,1f,0.1f);
    private readonly ParticleSystem.MinMaxCurve _smokeEffectMainModuleType1StartLifetime  = new ParticleSystem.MinMaxCurve(1f,3f);

    private readonly DestructionAudioHandler _destructionAudioHandler;
    private readonly ParticleSystem _glassBrokenEffectPrefab;
    private readonly ParticleSystem _hitEffectPrefab;
    private readonly ParticleSystem _engineSmokeEffect;
    private readonly ParticleSystem _engineBurnEffect;


    private readonly Transform _effectsParent;
    private readonly Spawner _spawner;
    private readonly PoolBase<ParticleSystem> _glassEffects;
    private readonly PoolBase<ParticleSystem> _hitEffects;

    public DestructionEffectsHandler(DestructionAudioHandler destructionAudioHandler, ParticleSystem glassBrokenEffectPrefab, ParticleSystem hitEffectPrefab,
        ParticleSystem engineSmokeEffect, ParticleSystem engineBurnEffect, Transform effectsParent)
    {
        _destructionAudioHandler = destructionAudioHandler;
        _glassBrokenEffectPrefab = glassBrokenEffectPrefab;
        _hitEffectPrefab = hitEffectPrefab;
        _engineSmokeEffect = engineSmokeEffect;
        _engineBurnEffect = engineBurnEffect;
        _effectsParent = effectsParent;
        _spawner = new Spawner();
        _glassEffects = new PoolBase<ParticleSystem>(PreloadGlassEffect, GetAction, ReturnAction, _preloadCount);
        _hitEffects = new PoolBase<ParticleSystem>(PreloadHitEffect, GetAction, ReturnAction, _preloadCount);
    }

    public void Dispose()
    {
        _engineSmokeEffect.Stop();
        _engineBurnEffect.Stop();
    }

    public void TryPlayEngineSmokeEffect(DestructionMode destructionMode)
    {
        _engineSmokeEffect.gameObject.SetActive(true);
        if (destructionMode == DestructionMode.Mode1)
        {
            SetSmokeEffectMainModuleType1();
            _engineSmokeEffect.Play();
        }
        else if (destructionMode == DestructionMode.Mode2)
        {
            SetSmokeEffectMainModuleType2();
            _engineSmokeEffect.Play();
        }
    }

    public void TryPlayEngineBurnEffect()
    {
        _engineBurnEffect.gameObject.SetActive(true);
        if (_engineBurnEffect.isPlaying == false)
        {
            _engineBurnEffect.Play();
            _destructionAudioHandler.PlayEngineBurn();
        }
    }
    public void GlassBrokenEffect(Vector2 position, float force)
    {
        PlayEffect(_glassEffects, () => { _destructionAudioHandler.PlayGlassBreak();}, position);
    }

    public void HitBrokenEffect(Vector2 position, float force)
    {
        PlayEffect(_hitEffects, () => { _destructionAudioHandler.PlayHardHit(force);}, position);
    }
    private ParticleSystem PreloadGlassEffect()
    {
        return _spawner.Spawn(_glassBrokenEffectPrefab, _effectsParent, _effectsParent);
    }

    private ParticleSystem PreloadHitEffect()
    {
        return _spawner.Spawn(_hitEffectPrefab, _effectsParent, _effectsParent);
    }

    private void GetAction(ParticleSystem effect)
    {
        effect.gameObject.SetActive(true);
    }

    private void ReturnAction(ParticleSystem effect)
    {
        effect.gameObject.SetActive(false);
    }

    private async void PlayEffect(PoolBase<ParticleSystem> pool, Action operation, Vector2 position)
    {
        var effect = pool.Get();
        float duration = effect.main.duration;
        effect.transform.position = position;
        effect.Play();
        operation.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        pool.Return(effect);
    }

    private void SetSmokeEffectMainModuleType1()
    {
        var mainModule = _engineSmokeEffect.main;
        mainModule.startSize = _smokeEffectMainModuleType1StartSize;
        mainModule.startColor = _smokeEffectMainModuleType1StartColor;
        mainModule.startLifetime = _smokeEffectMainModuleType1StartLifetime;
    }
    private void SetSmokeEffectMainModuleType2()
    {
        var mainModule = _engineSmokeEffect.main;
        mainModule.startSize = _smokeEffectMainModuleType2StartSize;
        mainModule.startColor = _smokeEffectMainModuleType2StartColor;
        mainModule.startLifetime = _smokeEffectMainModuleType2StartLifetime;
    }
}