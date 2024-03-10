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
    // protected readonly float HalfStrength;
    // protected readonly float MinStrength;
    protected readonly Transform DebrisParent;
    protected readonly Speedometer Speedometer;
    protected readonly LayerMask CanCollisionsLayerMasks;
    protected readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();
    private readonly MonoBehaviour _monoBehaviour;
    protected float MaxStrength;
    protected float HalfStrength;
    protected float MinStrength;
    protected float NormalImpulseValue;

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
    protected virtual void TryDestruct(){}
    protected void ApplyDamage()
    {
        MaxStrength -= NormalImpulseValue;
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
        if (transform == null)
        {
            _monoBehaviour.gameObject.layer = CarDebrisLayer;
        }
        else
        {
            transform.gameObject.layer = CarDebrisLayer;
        }
    }
    protected virtual bool CheckCollision(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & CanCollisionsLayerMasks.value) == 1 << collision.gameObject.layer)
        {
            SetImpulseNormal(collision);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SetImpulseNormal(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (NormalImpulseValue < collision.contacts[i].normalImpulse)
            {
                NormalImpulseValue = collision.contacts[i].normalImpulse;
            }
        }

    }
    private void Dispose()
    {
        CompositeDisposable.Clear();
    }

    private void RecalculateStrength()
    {
        // CalculateStrength(int strength);
    }

    private void CalculateStrength(int strength)
    {
        if (strength > 0)
        {
            MaxStrength = strength;
            HalfStrength = strength * HalfStrengthMultiplier;
            MinStrength = strength * MinStrengthMultiplier;
        }
    }
}