using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BumperDestructionHandler : DestructionHandler, IDispose
{
    private readonly DestructionEffectsHandler _destructionEffectsHandler;
    private readonly float _delay = 2f;
    private readonly Transform _bumperNormal;
    private readonly Transform _bumperDamaged;
    private readonly Collider2D _collider2DBumperNormal;
    private HingeJoint2D _hingeJoint2D;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private bool _isBroken = false;
    public BumperDestructionHandler(BumperRef bumperRef, DestructionHandlerContent destructionHandlerContent,
        DestructionEffectsHandler destructionEffectsHandler, DestructionAudioHandler destructionAudioHandler)
    :base(bumperRef, destructionHandlerContent, " bumper ", destructionAudioHandler, bumperRef.StrengthBumper)
    {
        _destructionEffectsHandler = destructionEffectsHandler;
        _collider2DBumperNormal = bumperRef.BumperNormal.GetComponent<Collider2D>();
        _bumperNormal = bumperRef.BumperNormal;
        _bumperDamaged = bumperRef.BumperDamaged;
        _bumperNormal.gameObject.SetActive(true);
        _bumperDamaged.gameObject.SetActive(false);
        _hingeJoint2D = _bumperDamaged.GetComponent<HingeJoint2D>();
        _hingeJoint2D.useMotor = true;
        SubscribeCollider(_collider2DBumperNormal, CollisionHandling, TrySwitchMode);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
        _cancellationTokenSource.Cancel();
    }
    protected override void TrySwitchMode()
    {
        if (ImpulseNormalValue > MaxStrength)
        {
            PlayEffect();
            DestructionMode3();
        }
        else if (ImpulseNormalValue > HalfStrength)
        {
            PlayEffect();
            RecalculateStrength();
            DestructionMode2();
        }
        else if (ImpulseNormalValue > MinStrength)
        {
            PlayEffect();
            RecalculateStrength();
            if (DestructionMode == DestructionMode.Mode2)
            {
                DestructionMode2();
            }
            else
            {
                DestructionMode1();
            }
        }
        else
        {
            PlaySoftHitSound();
        }
    }

    private void PlayEffect()
    {
        _destructionEffectsHandler.HitBrokenEffect(HitPosition, ImpulseNormalValue);
    }
    private void DestructionMode1()
    {
        SubscribeColliderAndSwitchSprites();
        DestructionMode = DestructionMode.Mode1;
    }

    private void DestructionMode2()
    {
        if (DestructionMode == DestructionMode.ModeDefault)
        {
            DestructionMode1();
        }
        _hingeJoint2D.useMotor = false;
        DestructionMode = DestructionMode.Mode2;
    }

    private void DestructionMode3()
    {
        CompositeDisposable.Clear();
        if (DestructionMode != DestructionMode.Mode2)
        {
            DestructionMode2();
        }
        TryThrow().Forget();
    }

    private void SwitchSprites()
    {
        _bumperNormal.gameObject.SetActive(false);
        _bumperDamaged.gameObject.SetActive(true);
    }

    public async UniTaskVoid TryThrow()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            Dispose();
            _hingeJoint2D.enabled = false;
            SetParentDebris();
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), cancellationToken: _cancellationTokenSource.Token);
            SetCarDebrisLayerNonInteractableWithCar();
        }
    }

    private void SubscribeColliderAndSwitchSprites()
    {
        CompositeDisposable.Clear();
        SwitchSprites();
    }
}