using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoosterDestructionHandler : DestructionHandler, IDispose
{
    private readonly int _fallingContentLayer;
    private readonly Booster _booster;
    private readonly CoupAnalyzer _coupAnalyzer;
    private readonly CarMass _carMass;
    private readonly DestructionEffectsHandler _destructionEffectsHandler;
    private readonly BoosterRef _boosterRef;
    private bool _isСrushed;

    public BoosterDestructionHandler(BoosterRef boosterRef, Booster booster, CoupAnalyzer coupAnalyzer, CarMass carMass, DestructionHandlerContent destructionHandlerContent,
        DestructionEffectsHandler destructionEffectsHandler, DestructionAudioHandler destructionAudioHandler, int fallingContentLayer) 
        : base(boosterRef, destructionHandlerContent, " Booster ", destructionAudioHandler, boosterRef.StrengthBooster)
    {
        _fallingContentLayer = fallingContentLayer;
        _booster = booster;
        _coupAnalyzer = coupAnalyzer;
        _carMass = carMass;
        _destructionEffectsHandler = destructionEffectsHandler;
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
        _destructionEffectsHandler.HitBrokenEffect(HitPosition, ImpulseNormalValue);
    }

    private void SubscribeColliders()
    {
        for (int i = 0; i < _boosterRef.BoosterParts.Length; i++)
        {
            SubscribeCollider(_boosterRef.BoosterParts[i].GetComponent<Collider2D>(), BoosterCollisionHandling, TrySwitchMode);
        }
    }

    private bool BoosterCollisionHandling(Collision2D collision)
    {
        bool result;
        ImpulseNormalValue = SetImpulseNormalAndHitPosition(collision);
        if (1 << _fallingContentLayer == 1 << collision.gameObject.layer && _isСrushed == false)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                ImpulseNormalValue += rigidbody2D.mass;
                if (rigidbody2D.mass > MaxStrength)
                {
                    _isСrushed = true;
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        if (CollisionHandling(collision))
        {
            if (_coupAnalyzer.CarIsCoup == true || _coupAnalyzer.CarIsTilted == true)
            {
                ImpulseNormalValue += _carMass.Mass;
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else if (CheckCollisionAndMinSpeed(collision) == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }
    private void Destruction()
    {
        CompositeDisposable.Clear();
        _booster.BoosterDisable();
        for (int i = 0; i < _boosterRef.BoosterParts.Length; i++)
        {
            _boosterRef.BoosterParts[i].gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_boosterRef.BoosterParts[i]);
        }
        SetParentDebris();
        SetCarDebrisLayerNonInteractableWithCar();
    }
}