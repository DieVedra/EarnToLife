using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoosterDestructionHandler : DestructionHandler, IDispose
{
    private readonly float _switchLayerDelay = 1f;
    private readonly Booster _booster;
    private readonly BoosterRef _boosterRef;
    public BoosterDestructionHandler(BoosterRef boosterRef, Booster booster, DestructionHandlerContent destructionHandlerContent) 
        : base(boosterRef, destructionHandlerContent, boosterRef.StrengthBooster)
    {
        _booster = booster;
        _boosterRef = boosterRef;
        SubscribeColliders();
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }

    protected override void TrySwitchMode()
    {
        if (ValueNormalImpulse > MaxStrength)
        {
            Destruction();
        }
        else
        {
            RecalculateStrength();
        }
    }

    private void SubscribeColliders()
    {
        for (int i = 0; i < _boosterRef.BoosterParts.Length; i++)
        {
            SubscribeCollider(_boosterRef.BoosterParts[i].GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
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