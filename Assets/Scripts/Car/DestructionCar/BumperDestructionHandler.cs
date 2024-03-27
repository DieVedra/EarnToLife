using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BumperDestructionHandler : DestructionHandler, IDispose
{
    private readonly Action<Vector2> _effect;
    private readonly float _delay = 2f;
    private readonly Transform _bumperNormal;
    private readonly Transform _bumperDamaged;
    private readonly Collider2D _collider2DBumperNormal;
    private readonly Collider2D _collider2DBumperDamaged;
    private HingeJoint2D _hingeJoint2D;
    private Transform _currentbumper;
    private DestructionMode _destructionMode = DestructionMode.ModeDefault;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private bool _isBroken = false;
    public BumperDestructionHandler(BumperRef bumperRef, DestructionHandlerContent destructionHandlerContent,
        Action<Vector2> effect, Action sound)
    :base(bumperRef, destructionHandlerContent, sound, bumperRef.StrengthBumper)
    {
        _effect = effect;
        _collider2DBumperNormal = bumperRef.BumperNormal.GetComponent<Collider2D>();
        _collider2DBumperDamaged = bumperRef.BumperDamaged.GetComponent<Collider2D>();
        _bumperNormal = bumperRef.BumperNormal;
        _bumperDamaged = bumperRef.BumperDamaged;
        _bumperNormal.gameObject.SetActive(true);
        _bumperDamaged.gameObject.SetActive(false);
        _hingeJoint2D = _bumperDamaged.GetComponent<HingeJoint2D>();
        _hingeJoint2D.useMotor = true;
        _currentbumper = _bumperNormal;
        SubscribeCollider(_collider2DBumperNormal, CollisionHandling, TrySwitchMode);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
        _cancellationTokenSource.Cancel();
    }
    protected override void TrySwitchMode()
    {
        // Debug.Log($"BumperDestructionEnter      ValueNormalImpulse: {ValueNormalImpulse}");
        if (ValueNormalImpulse > MaxStrength)
        {
            _effect.Invoke(HitPosition);
            DestructionMode3();
            // Debug.Log($"            BumperDestructionMode1  MaxStrength     ValueNormalImpulse: {ValueNormalImpulse}");

        }
        else if (ValueNormalImpulse > HalfStrength)
        {
            _effect.Invoke(HitPosition);
            RecalculateStrength();
            DestructionMode2();
            // Debug.Log($"             BumperDestructionMode2  HalfStrength     ValueNormalImpulse: {ValueNormalImpulse}");

        }
        else if (ValueNormalImpulse > MinStrength)
        {
            _effect.Invoke(HitPosition);
            RecalculateStrength();
            if (_destructionMode == DestructionMode.Mode2)
            {
                DestructionMode2();
            }
            else
            {
                DestructionMode1();
            }
            Debug.Log($"             BumperDestructionMode  MinStrength     ValueNormalImpulse: {ValueNormalImpulse}");
        }
        else
        {
            RecalculateStrength();

        }
    }
    private void DestructionMode1()
    {
        SubscribeColliderAndSwitchSprites();
        _destructionMode = DestructionMode.Mode1;
    }

    private void DestructionMode2()
    {
        if (_destructionMode == DestructionMode.ModeDefault)
        {
            DestructionMode1();
        }
        _hingeJoint2D.useMotor = false;
        _destructionMode = DestructionMode.Mode2;
    }
    private void DestructionMode3()
    {
        CompositeDisposable.Clear();
        if (_destructionMode != DestructionMode.Mode2)
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
            SetCarDebrisLayer();
        }
    }

    private void SubscribeColliderAndSwitchSprites()
    {
        CompositeDisposable.Clear();
        SwitchSprites();
        SubscribeCollider(_collider2DBumperDamaged, CollisionHandling, TrySwitchMode);
    }
}