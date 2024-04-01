using System;
using UnityEngine;

public class FrontWingDestructionHandler : DestructionHandler, IDispose
{
    private readonly ArmoredFrontFrameRef _armoredFrontFrameRef;
    private readonly GlassDestructionHandler _glassDestructionHandler;
    private readonly HotWheelDestructionHandler _hotWheelDestructionHandler;
    private readonly BumperDestructionHandler _bumperDestructionFrontHandler;
    private readonly Action<Vector2, float> _effectHit;
    private readonly Action<DestructionMode> _effectSmoke;
    private readonly Action _effectBurn;
    private readonly Transform _wingNormal;
    private readonly Transform _lighterWingNormal;
    private readonly Transform _wingDamaged1;
    private readonly Transform _wingDamaged2;
    private readonly Transform _bottomNormal;
    private readonly Transform _bottomDamaged;
    private readonly Transform _armoredFrontNormal;
    private readonly Transform _armoredFrontDamaged;
    private readonly Collider2D _wingDamaged1Collider;
    private readonly Collider2D _wingDamaged2Collider;
    private readonly Collider2D _armoredFrontDamagedCollider;
    private readonly bool _isArmored;
    private Transform _damaged2Hood;
    private bool _lighterBroken;
    public event Action OnEngineBroken;

    public FrontWingDestructionHandler(FrontWingRef frontWingRef, ArmoredFrontFrameRef armoredFrontFrameRef, 
        GlassDestructionHandler glassDestructionHandler, HotWheelDestructionHandler hotWheelDestructionHandler,
        BumperDestructionHandler bumperDestructionFrontHandler, DestructionHandlerContent destructionHandlerContent,
        Action<Vector2, float> effectHit, Action<DestructionMode> effectSmoke, Action effectBurn, Action<float> soundSoftHitHit2,
        int strength, bool isArmored)
        :base(frontWingRef, destructionHandlerContent, soundSoftHitHit2, strength)
    {
        _glassDestructionHandler = glassDestructionHandler;
        _hotWheelDestructionHandler = hotWheelDestructionHandler;
        _bumperDestructionFrontHandler = bumperDestructionFrontHandler;
        _effectHit = effectHit;
        _effectSmoke = effectSmoke;
        _effectBurn = effectBurn;
        _wingNormal = frontWingRef.WingNormal;
        _lighterWingNormal = frontWingRef.LighterWingNormal;
        _lighterWingNormal.gameObject.SetActive(false);
        _damaged2Hood = frontWingRef.WingFrontDamaged2TrunkCover;
        _wingDamaged1 = frontWingRef.WingDamaged1;
        _wingDamaged2 = frontWingRef.WingDamaged2;
        _bottomNormal = frontWingRef.BottomNormal;
        _bottomDamaged = frontWingRef.BottomDamaged;
        _wingDamaged1Collider = _wingDamaged1.GetComponent<Collider2D>();
        _wingDamaged2Collider = _wingDamaged2.GetComponent<Collider2D>();
        _bottomDamaged.gameObject.SetActive(false);
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _armoredFrontFrameRef = armoredFrontFrameRef;
            _armoredFrontNormal = armoredFrontFrameRef.ArmoredFrontNormal;
            _armoredFrontDamaged = armoredFrontFrameRef.ArmoredFrontDamaged;
            _armoredFrontDamagedCollider = _armoredFrontDamaged.GetComponent<Collider2D>();
            SubscribeCollider(_armoredFrontNormal.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
        SubscribeCollider(_wingNormal.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
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
            DestructionMode3AndSubscribe();
        }
        else if (ImpulseNormalValue > HalfStrength)
        {
            PlayEffect();
            RecalculateStrength();
            DestructionMode2AndSubscribe();
        }
        else if (ImpulseNormalValue > MinStrength)
        {
            PlayEffect();
            RecalculateStrength();
            if (DestructionMode == DestructionMode.Mode2)
            {
                DestructionMode2AndSubscribe();
            }
            else
            {
                DestructionMode1AndSubscribe();
            }
        }
        else
        {
            PlaySoftHitSound();
        }
    }

    private void PlayEffect()
    {
        _effectHit.Invoke(HitPosition, ImpulseNormalValue);
    }

    private void DestructionMode1AndSubscribe()
    {
        DestructionMode1();
        if (_isArmored == true)
        {
            SubscribeCollider(_armoredFrontDamagedCollider, CollisionHandling, TrySwitchMode);
        }
        SubscribeCollider(_wingDamaged1Collider, CollisionHandling, TrySwitchMode);
    }
    private void DestructionMode1()
    {
        CompositeDisposable.Clear();
        _bumperDestructionFrontHandler.TryThrow().Forget();
        ThrowLighter();
        SwitchSprites1();
        TrySwitchSpriteArmoredFrame();
        _glassDestructionHandler?.TryBreakGlassFromWings();
        DestructionMode = DestructionMode.Mode1;
        StartSmoke(DestructionMode);
    }

    private void DestructionMode2AndSubscribe()
    {
        DestructionMode2();
        SubscribeCollider(_wingDamaged2Collider, CollisionHandling, TrySwitchMode);
    }
    private void DestructionMode2()
    {
        if (DestructionMode == DestructionMode.ModeDefault)
        {
            DestructionMode1();
        }
        CompositeDisposable.Clear();
        SwitchSprites2();
        SwitchBottom();
        TrySwitchSpriteArmoredFrame();
        if (_isArmored == true)
        {
            TryAddRigidBody(_armoredFrontDamaged.gameObject);
            SetParentDebris(_armoredFrontFrameRef.transform);
        }
        if (_hotWheelDestructionHandler != null)
        {
            _hotWheelDestructionHandler.Destruct();
            SetParentDebris(_hotWheelDestructionHandler.HotWheelRef.transform);
        }
        _glassDestructionHandler?.TryThrowGlass();
        ThrowHood();
        DestructionMode = DestructionMode.Mode2;
        StartSmoke(DestructionMode);
    }
    private void DestructionMode3AndSubscribe()
    {
        if (DestructionMode != DestructionMode.Mode2)
        {
            DestructionMode2();
        }
        DestructionMode = DestructionMode.Mode3;
        StartFire();
        OnEngineBroken?.Invoke();
    }
    private void SwitchSprites1()
    {
        _wingNormal.gameObject.SetActive(false);
        _wingDamaged1.gameObject.SetActive(true);
    }
    private void SwitchSprites2()
    {
        _wingDamaged1.gameObject.SetActive(false);
        _wingDamaged2.gameObject.SetActive(true);
    }

    private void TrySwitchSpriteArmoredFrame()
    {
        if (_isArmored == true)
        {
            _armoredFrontNormal.gameObject.SetActive(false);
            _armoredFrontDamaged.gameObject.SetActive(true);
        }
    }
    private void ThrowLighter()
    {
        if (_lighterBroken == false)
        {
            _lighterBroken = true;
            _lighterWingNormal.gameObject.SetActive(true);
            TryAddRigidBody(_lighterWingNormal.gameObject);
            SetParentDebris(_lighterWingNormal);
        }
    }

    private void ThrowHood()
    {
        TryAddRigidBody(_damaged2Hood.gameObject);
        SetParentDebris(_damaged2Hood);
        SetCarDebrisLayer(_damaged2Hood);
    }
    private void SwitchBottom()
    {
        _bottomNormal.gameObject.SetActive(false);
        _bottomDamaged.gameObject.SetActive(true);
    }

    private void StartSmoke(DestructionMode destructionMode)
    {
        _effectSmoke.Invoke(destructionMode);
    }
    private void StartFire()
    {
        _effectBurn.Invoke();
    }
}