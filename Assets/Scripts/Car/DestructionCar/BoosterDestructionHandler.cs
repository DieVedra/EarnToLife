using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoosterDestructionHandler : DestructionHandler, IDispose
{
    private readonly float _switchLayerDelay = 1f;
    private readonly Booster _booster;
    private readonly Action<Vector2, float> _effect;
    private readonly BoosterRef _boosterRef;
    public BoosterDestructionHandler(BoosterRef boosterRef, Booster booster, DestructionHandlerContent destructionHandlerContent,
        Action<Vector2, float> effect, Action<float> soundSoftHit) 
        : base(boosterRef, destructionHandlerContent, soundSoftHit, boosterRef.StrengthBooster)
    {
        _booster = booster;
        _effect = effect;
        _boosterRef = boosterRef;
        SubscribeColliders();
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }

    protected override void TrySwitchMode()
    {
        if (ImpulseNormalValue > MaxStrength)
        {
            Debug.Log($"Booster                  ImpulseNormalValue: {ImpulseNormalValue}  MaxStrength: {MaxStrength}");
            PlayEffect();
            Destruction();
        }
        else
        {
            PlaySoftHitSound();
            RecalculateStrength();
        }
    }

    private void PlayEffect()
    {
        _effect.Invoke(HitPosition, ImpulseNormalValue);
    }

    private void SubscribeColliders()
    {
        for (int i = 0; i < _boosterRef.BoosterParts.Length; i++)
        {
            SubscribeCollider(_boosterRef.BoosterParts[i].GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
    }

    private async void Destruction()
    {
        CompositeDisposable.Clear();
        _booster.BoosterDisable();
        for (int i = 0; i < _boosterRef.BoosterParts.Length; i++)
        {
            _boosterRef.BoosterParts[i].gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_boosterRef.BoosterParts[i]);
        }
        SetParentDebris();
        await UniTask.Delay(TimeSpan.FromSeconds(_switchLayerDelay));
        SetCarDebrisLayer();
    }
}