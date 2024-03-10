using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoosterDestructionHandler : DestructionHandler
{
    private readonly float _switchLayerDelay = 1f;
    private Booster _booster;
    private BoosterRef _boosterRef;
    public BoosterDestructionHandler(BoosterRef boosterRef, Booster booster, DestructionHandlerContent destructionHandlerContent) 
        : base(boosterRef, destructionHandlerContent, boosterRef.StrengthBooster)
    {
        _booster = booster;
        _boosterRef = boosterRef;
        SubscribeColliders();
    }
    protected override void TryDestruct()
    {
        ApplyDamage();
        if (MaxStrength <= StrengthForDestruct)
        {
            Destruction();
        }
    }
    private void SubscribeColliders()
    {
        for (int i = 0; i < _boosterRef.BoosterParts.Length; i++)
        {
            SubscribeCollider(_boosterRef.BoosterParts[i].GetComponent<Collider2D>(), CheckCollision, TryDestruct);
        }
    }

    private async void Destruction()
    {
        CompositeDisposable.Clear();
        _booster.BoosterDisable();
        SetParentDebris();
        for (int i = 0; i < _boosterRef.BoosterParts.Length; i++)
        {
            _boosterRef.BoosterParts[i].gameObject.AddComponent<Rigidbody2D>();
        }
        await UniTask.Delay(TimeSpan.FromSeconds(_switchLayerDelay));
        SetCarDebrisLayer();
    }
}