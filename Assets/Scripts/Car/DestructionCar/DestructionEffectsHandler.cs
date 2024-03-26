using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DestructionEffectsHandler :IDispose
{
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
        _glassEffects = new PoolBase<ParticleSystem>(PreloadGlassEffect, GetAction, ReturnAction, 4);
        _hitEffects = new PoolBase<ParticleSystem>(PreloadHitEffect, GetAction, ReturnAction, 4);
    }

    public void Dispose()
    {
        _engineSmokeEffect.Stop();
        _engineBurnEffect.Stop();
        _destructionAudioHandler.StopPlayEngineBurn();

    }

    public void EngineSmokeEffects()
    {
        if (_engineSmokeEffect.isPlaying == false)
        {
            _engineSmokeEffect.Play();
        }
    }

    public void EngineBurnEffects()
    {
        _engineBurnEffect.Play();
        _destructionAudioHandler.PlayEngineBurn();
    }

    public void GlassBrokenEffects(Vector2 position)
    {
        PlayEffect(_glassEffects, () => { _destructionAudioHandler.PlayGlassBreak();}, position);
    }

    public void HitBrokenEffects(Vector2 position)
    {
        PlayEffect(_hitEffects, () => { _destructionAudioHandler.PlayHit();}, position);
    }

    public ParticleSystem PreloadGlassEffect()
    {
        return _spawner.Spawn(_glassBrokenEffectPrefab, _effectsParent, _effectsParent);
    }

    public ParticleSystem PreloadHitEffect()
    {
        return _spawner.Spawn(_hitEffectPrefab, _effectsParent, _effectsParent);
    }

    public void GetAction(ParticleSystem effect)
    {
        effect.gameObject.SetActive(true);
    }

    public void ReturnAction(ParticleSystem effect)
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
}