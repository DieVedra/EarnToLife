using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BumperDestructionHandler : DestructionHandler
{
    private readonly float _switchLayerDelay = 1f;
    private Transform _bumperNormal;
    private Transform _bumperDamaged;
    private Collider2D _collider2DBumperNormal;
    private Collider2D _collider2DBumperDamaged;
    private HingeJoint2D _hingeJoint2D;
    private DestructionMode _destructionMode = DestructionMode.ModeDefault;
    public BumperDestructionHandler(BumperRef bumperRef, DestructionHandlerContent destructionHandlerContent)
    :base(bumperRef, destructionHandlerContent, bumperRef.StrengthBumper)
    {
        _collider2DBumperNormal = bumperRef.BumperNormal.GetComponent<Collider2D>();
        _collider2DBumperDamaged = bumperRef.BumperDamaged.GetComponent<Collider2D>();
        _bumperNormal = bumperRef.BumperNormal;
        _bumperDamaged = bumperRef.BumperDamaged;
        _bumperNormal.gameObject.SetActive(true);
        _bumperDamaged.gameObject.SetActive(false);
        _hingeJoint2D = _bumperDamaged.GetComponent<HingeJoint2D>();
        _hingeJoint2D.useMotor = true;
        SubscribeCollider(_collider2DBumperNormal, CheckCollision, TrySwitchMode);
    }
    protected override void TrySwitchMode()
    {
        if (ValueNormalImpulse > MaxStrength)
        {
            DestructionMode3();
        }
        else if (ValueNormalImpulse > HalfStrength)
        {
            RecalculateStrength();
            DestructionMode2();
        }
        else if (ValueNormalImpulse > MinStrength)
        {
            RecalculateStrength();
            if (_destructionMode == DestructionMode.Mode2)
            {
                DestructionMode2();
            }
            else
            {
                DestructionMode1();
            }
        }
    }
    private void DestructionMode1()
    {
        SubscribeColliderAndSwitchSprites();
        _destructionMode = DestructionMode.Mode1;
    }

    private void DestructionMode2()
    {
        SubscribeColliderAndSwitchSprites();
        _hingeJoint2D.useMotor = false;
        _destructionMode = DestructionMode.Mode2;
    }
    private void DestructionMode3()
    {
        CompositeDisposable.Clear();
        SwitchSprites();
        Throw();
    }
    private void SwitchSprites()
    {
        _bumperNormal.gameObject.SetActive(false);
        _bumperDamaged.gameObject.SetActive(true);
    }
    private async void Throw()
    {
        _hingeJoint2D.enabled = false;
        SetParentDebris();
        await UniTask.Delay(TimeSpan.FromSeconds(_switchLayerDelay));
        SetCarDebrisLayer();
    }

    private void SubscribeColliderAndSwitchSprites()
    {
        CompositeDisposable.Clear();
        SwitchSprites();
        SubscribeCollider(_collider2DBumperDamaged, CheckCollision, TrySwitchMode);
    }
}