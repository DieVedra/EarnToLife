using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class DestructionHandler
{
    protected readonly int CarDebrisLayer;
    // protected readonly float StrengthForDestruct = 0;
    protected readonly float HalfStrengthMultiplier = 0.7f;
    protected readonly float MinStrengthMultiplier = 0.2f;
    protected readonly Transform DebrisParent;
    protected readonly Speedometer Speedometer;
    protected readonly LayerMask CanCollisionsLayerMasks;
    protected readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();
    private readonly MonoBehaviour _monoBehaviour;
    private readonly Action<float> _soundSoftHit;
    private readonly float _minSpeedForDestruct = 10f;
    private readonly float _reducingStrengthMultiplier = 0.7f;
    protected Vector2 HitPosition;
    protected float MaxStrength;
    protected float HalfStrength;
    protected float MinStrength;
    protected float ImpulseNormalValue;

    protected DestructionHandler(MonoBehaviour monoBehaviour, DestructionHandlerContent destructionHandlerContent, Action<float> soundSoftHit = null,
        int maxStrength = 0)
    {
        CarDebrisLayer = destructionHandlerContent.CarDebrisLayer;
        CalculateStrength(maxStrength);
        DebrisParent = destructionHandlerContent.DebrisParent;
        Speedometer = destructionHandlerContent.Speedometer;
        CanCollisionsLayerMasks = destructionHandlerContent.CanCollisionsLayerMasks;
        _monoBehaviour = monoBehaviour;
        _soundSoftHit = soundSoftHit;
    }
    protected virtual void TrySwitchMode(){}

    protected void PlaySoftHitSound()
    {
        _soundSoftHit?.Invoke(ImpulseNormalValue);
    }

    protected virtual bool CollisionHandling(Collision2D collision)
    {
        if (CheckCollisionAndMinSpeed(collision) == true)
        {
            SetImpulseNormal(collision);
            return true;
        }
        else
        {
            PlaySoftHitSound();
            return false;
        }
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

    protected bool CheckCollision(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & CanCollisionsLayerMasks.value) == 1 << collision.gameObject.layer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool CheckCollisionAndMinSpeed(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & CanCollisionsLayerMasks.value) == 1 << collision.gameObject.layer
            && Speedometer.CurrentSpeedFloat >= _minSpeedForDestruct)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void RecalculateStrength()
    {
        CalculateStrength(MaxStrength - ImpulseNormalValue * _reducingStrengthMultiplier);
    }

    protected void SetImpulseNormal(Collision2D collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (ImpulseNormalValue < collision.contacts[i].normalImpulse)
            {
                ImpulseNormalValue = collision.contacts[i].normalImpulse;
                SetHitPosition(collision.contacts[i].point);
            }
        }

    }

    protected void TryAddRigidBody(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody2D rigidbody2D) == false)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
    }

    private void CalculateStrength(float strength)
    {
        if (strength > 0)
        {
            MaxStrength = strength;
            HalfStrength = strength * HalfStrengthMultiplier;
            MinStrength = strength * MinStrengthMultiplier;
            // Debug.Log($"MaxStrength: {MaxStrength}    HalfStrength: {HalfStrength}    MinStrength: {MinStrength}");
        }
    }

    private void SetHitPosition(Vector2 pos)
    {
        HitPosition = pos;
    }
}