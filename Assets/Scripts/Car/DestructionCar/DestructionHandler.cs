using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class DestructionHandler
{
    protected readonly int CarDebrisLayer;
    protected readonly float StrengthForDestruct = 0;
    protected readonly float HalfStrengthMultiplier = 0.7f;
    protected readonly float MinStrengthMultiplier = 0.2f;
    protected readonly Transform DebrisParent;
    protected readonly Speedometer Speedometer;
    protected readonly LayerMask CanCollisionsLayerMasks;
    protected readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();
    private readonly MonoBehaviour _monoBehaviour;
    private readonly float _minSpeedForDestruct = 10f;
    private readonly float _reducingStrengthMultiplier = 0.7f;
    protected float MaxStrength;
    protected float HalfStrength;
    protected float MinStrength;
    protected float ValueNormalImpulse;

    protected DestructionHandler(MonoBehaviour monoBehaviour, DestructionHandlerContent destructionHandlerContent,
        int maxStrength = 0)
    {
        CarDebrisLayer = destructionHandlerContent.CarDebrisLayer;
        CalculateStrength(maxStrength);
        DebrisParent = destructionHandlerContent.DebrisParent;
        Speedometer = destructionHandlerContent.Speedometer;
        CanCollisionsLayerMasks = destructionHandlerContent.CanCollisionsLayerMasks;
        _monoBehaviour = monoBehaviour;
        monoBehaviour.OnDestroyAsObservable()
            .Subscribe(_ =>
            {
                Dispose();
            }).AddTo(monoBehaviour);
    }
    protected virtual void TrySwitchMode(){}
    protected void ApplyDamage()
    {
        MaxStrength -= ValueNormalImpulse;
    }
    protected virtual void SubscribeCollider(Collider2D collider2D, Predicate<Collision2D> condition = null, Action operation = null)
    {
        collider2D.OnCollisionEnter2DAsObservable()
            .Where(condition.Invoke)
            .Subscribe(_ =>
            {
                operation();
            })
            .AddTo(CompositeDisposable);
    }
    protected void SetParentDebris(Transform transform = null)
    {
        if (transform == null)
        {
            _monoBehaviour.transform.SetParent(DebrisParent);
        }
        else
        {
            transform.SetParent(DebrisParent);
        }
    }
    protected void SetCarDebrisLayer(Transform transform = null)
    {
        Transform transformCarPart;
        if (transform == null)
        {
            transformCarPart = _monoBehaviour.transform;
        }
        else
        {
            transformCarPart = transform;
        }

        if (transformCarPart.childCount > 0)
        {
            for (int i = 0; i < transformCarPart.childCount; i++)
            {
                transformCarPart.GetChild(i).gameObject.layer = CarDebrisLayer;
            }
        }
        else
        {
            transformCarPart.gameObject.layer = CarDebrisLayer;
        }
    }
    protected virtual bool CheckCollision(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & CanCollisionsLayerMasks.value) == 1 << collision.gameObject.layer
            && Speedometer.CurrentSpeedFloat >= _minSpeedForDestruct)
        {
            SetImpulseNormal(collision);
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void RecalculateStrength()
    {
        CalculateStrength(MaxStrength - ValueNormalImpulse * _reducingStrengthMultiplier);
    }

    protected void SetImpulseNormal(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (ValueNormalImpulse < collision.contacts[i].normalImpulse)
            {
                ValueNormalImpulse = collision.contacts[i].normalImpulse;
            }
        }

    }

    private void CalculateStrength(float strength)
    {
        if (strength > 0)
        {
            MaxStrength = strength;
            HalfStrength = strength * HalfStrengthMultiplier;
            MinStrength = strength * MinStrengthMultiplier;
        }
    }

    private void Dispose()
    {
        CompositeDisposable.Clear();
    }
}