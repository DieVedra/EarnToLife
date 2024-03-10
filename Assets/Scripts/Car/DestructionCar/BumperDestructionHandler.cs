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
    private int _previosDestructionIndex;
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
        SubscribeCollider(_collider2DBumperNormal, CheckCollision, TryDestruct);
    }
    protected override void TryDestruct()
    {
        if ( Speedometer.CurrentSpeedFloat > 10f/*NormalImpulseValue > MinStrength*/)
        {
            Debug.Log("ImpulseNormal: " + NormalImpulseValue);
            Debug.Log("Speed:  " + Speedometer.CurrentSpeedFloat);
            if (NormalImpulseValue > MaxStrength)
            {
                DestructionMode3();
            }
            else if (NormalImpulseValue > HalfStrength)
            {
                _previosDestructionIndex = 2;
                DestructionMode2();
            }
            else if (NormalImpulseValue > MinStrength)
            {
                _previosDestructionIndex = 1;
                DestructionMode1();
            }
        }
    }

    private void DestructionMode1()
    {
        // SubscribeColliderAndSwitchSprites();
        
        CompositeDisposable.Clear();
        SwitchSprites();
        SubscribeCollider(_collider2DBumperDamaged, CheckCollision, TryDestruct);
        Debug.Log("DestructionMode1");
    }

    private void DestructionMode2()
    {
        // SubscribeColliderAndSwitchSprites();
        
        CompositeDisposable.Clear();
        SwitchSprites();
        SubscribeCollider(_collider2DBumperDamaged, CheckCollision, TryDestruct);
        _hingeJoint2D.useMotor = false;
        Debug.Log("DestructionMode2");

    }

    private void DestructionMode3()
    {
        CompositeDisposable.Clear();
        SwitchSprites();
        Debug.Log("DestructionMode3");
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
}