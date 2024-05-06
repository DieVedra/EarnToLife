using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class DestructionEffectsHandler :IDispose
{
    private readonly int _preloadCount = 4;
    private readonly Color _smokeEffectMainModuleType2StartColor = new Color(1f,1f,1f,0.7f);
    private readonly Color _smokeEffectMainModuleType1StartColor = new Color(1f,1f,1f,0.2f);

    private readonly DestructionAudioHandler _destructionAudioHandler;
    private readonly ParticleSystem _glassBrokenEffectPrefab;
    private readonly ParticleSystem _hitEffectPrefab;
    private readonly ParticleSystem _engineSmokeEffect;
    private readonly ParticleSystem _engineBurnEffect;

    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
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
        _compositeDisposable.Clear();
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

    private void PlayEffect(PoolBase<ParticleSystem> pool, Action audioOperation, Vector2 position)
    {
        var effect = pool.Get();
        effect.transform.position = position;
        effect.Play();
        audioOperation.Invoke();

        Observable.Timer(TimeSpan.FromSeconds(effect.main.duration)).Subscribe(_ =>
        {
            pool.Return(effect);
            _compositeDisposable.Clear();
        }).AddTo(_compositeDisposable);
    }

    private void SetSmokeEffectMainModuleType1()
    {
        var mainModule = _engineSmokeEffect.main;
        mainModule.startColor = _smokeEffectMainModuleType1StartColor;
    }
    private void SetSmokeEffectMainModuleType2()
    {
        var mainModule = _engineSmokeEffect.main;
        mainModule.startColor = _smokeEffectMainModuleType2StartColor;
    }
}