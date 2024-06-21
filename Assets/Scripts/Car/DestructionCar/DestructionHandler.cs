using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract class DestructionHandler
{
    protected readonly CompositeDisposable CompositeDisposable = new CompositeDisposable();
    private int _carDebrisLayerNonInteractionWithCar;
    private int _carDebrisInteractingWithCar;
    private readonly float _backwardMoveDamageMultiplier = 0.42f;
    private readonly float _halfStrengthMultiplier = 0.7f;
    private readonly float _minStrengthMultiplier = 0.2f;
    private readonly Transform _debrisParent;
    private readonly Speedometer _speedometer;
    private readonly LayerMask _canCollisionsLayerMasks;
    private readonly MonoBehaviour _monoBehaviour;
    private readonly float _minSpeedForDestruct = 10f;
    private readonly float _reducingStrengthMultiplier = 0.7f;
    protected DestructionMode DestructionMode = DestructionMode.ModeDefault;
    protected Vector2 HitPosition;
    protected float MaxStrength;
    protected float HalfStrength;
    protected float MinStrength;
    protected float ImpulseNormalValue;
    private string _name;
    private readonly DestructionAudioHandler _destructionAudioHandler;

    protected DestructionHandler(MonoBehaviour monoBehaviour, DestructionHandlerContent destructionHandlerContent,
        string name,
        DestructionAudioHandler destructionAudioHandler = null,
        int maxStrength = 0)
    {
        _name = name;
        _destructionAudioHandler = destructionAudioHandler;
        _carDebrisLayerNonInteractionWithCar = destructionHandlerContent.CarDebrisLayerNonInteractionWithCar;
        _carDebrisInteractingWithCar = destructionHandlerContent.CarDebrisInteractingWithCar;
        CalculateStrength(maxStrength);
        _debrisParent = destructionHandlerContent.DebrisParent;
        _speedometer = destructionHandlerContent.Speedometer;
        _canCollisionsLayerMasks = destructionHandlerContent.CanCollisionsLayerMasks;
        _monoBehaviour = monoBehaviour;
    }
    protected virtual void TrySwitchMode(){}
    protected void PlaySoftHitSound()
    {
        _destructionAudioHandler?.PlaySoftHit(ImpulseNormalValue, _name);
    }

    protected bool CollisionHandling(Collision2D collision)
    {
        ImpulseNormalValue = SetImpulseNormalAndHitPosition(collision);
        if (CheckCollisionAndMinSpeed(collision) == true)
        {
            return true;
        }
        else
        {
            PlaySoftHitSound();
            return false;
        }
    }
    protected void SubscribeCollider(Collider2D collider2D, Predicate<Collision2D> condition /*= null*/, Action operation/* = null*/)
    {
        collider2D.OnCollisionEnter2DAsObservable()
            .Where(condition.Invoke)
            .Subscribe(_ =>
            {
                operation?.Invoke();
            })
            .AddTo(CompositeDisposable);
    }

    protected void SetParentDebris(Transform transform = null)
    {
        if (transform == null)
        {
            _monoBehaviour.transform.SetParent(_debrisParent);
        }
        else
        {
            transform.SetParent(_debrisParent);
        }
    }
    protected void SetCarDebrisLayerNonInteractableWithCar(Transform transform = null)
    {
        SetCarDebrisLayer(_carDebrisLayerNonInteractionWithCar, transform);
    }
    protected void SetCarDebrisLayerInteractableWithCar(Transform transform = null)
    {
        SetCarDebrisLayer(_carDebrisInteractingWithCar, transform);
    }

    protected bool CheckCollision(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & _canCollisionsLayerMasks.value) == 1 << collision.gameObject.layer)
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
        if ((1 << collision.gameObject.layer & _canCollisionsLayerMasks.value) == 1 << collision.gameObject.layer
            && _speedometer.CurrentSpeedFloat >= _minSpeedForDestruct)
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

    protected float SetImpulseNormalAndHitPosition(Collision2D collision)
    {
        float result = 0;
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (result < collision.contacts[i].normalImpulse)
            {
                result = collision.contacts[i].normalImpulse;
                SetHitPosition(collision.contacts[i].point);
            }
        }

        if (_speedometer.IsMovementForward == false)
        {
            result *= _backwardMoveDamageMultiplier;
        }
        return result;
    }

    protected void TryAddRigidBody(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody2D rigidbody2D) == false)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
    }
    protected Rigidbody2D GetNotSimulatedRigidBodyOrTryAdd(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Rigidbody2D rigidbody2D) == true)
        {
            rigidbody2D.simulated = false;
            return rigidbody2D;
        }
        else
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.simulated = false;
            return rb;
        }
    }
    protected void RigidBodySetSimulated(Rigidbody2D rigidbody2D)
    {
        rigidbody2D.simulated = true;
    }
    private void CalculateStrength(float strength)
    {
        if (strength > 0)
        {
            MaxStrength = strength;
            HalfStrength = strength * _halfStrengthMultiplier;
            MinStrength = strength * _minStrengthMultiplier;
        }
    }

    private void SetHitPosition(Vector2 pos)
    {
        HitPosition = pos;
    }

    private void SetCarDebrisLayer(int layer, Transform transform)
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
                transformCarPart.GetChild(i).gameObject.layer = layer;
            }
        }
        else
        {
            transformCarPart.gameObject.layer = layer;
        }
    }
}