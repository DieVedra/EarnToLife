﻿using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class RoofDestructionHandler : DestructionHandler, IDispose
{
    private readonly int _fallingContentLayer;
    private readonly Vector3 _scaleSupportRoofAfterHit = new Vector3(1f, 0.9f,1f);
    private readonly RoofRef _roofRef;
    private readonly FrontDoorDestructionHandler _frontDoorDestructionHandler;
    private readonly BackDoorDestructionHandler _backDoorDestructionHandler;
    private readonly GlassDestructionHandler _frontGlassDestructionHandler;
    private readonly GlassDestructionHandler _backGlassDestructionHandler;
    private readonly ArmoredBackFrameDestructionHandler _armoredBackFrameDestructionHandler;
    private readonly GunDestructionHandler _gunDestructionHandler;
    private readonly CabineDestructionHandler _cabineDestructionHandler;
    private readonly Action<float> _soundBends;
    private readonly CoupAnalyzer _coupAnalyzer;
    private readonly CarMass _carMass;
    private readonly Transform _roofNormal;
    private readonly Transform _roofDamaged1;
    private readonly Transform _roofDamaged2;
    private readonly Transform _roofDamaged3;
    private readonly Transform _supportRoof;

    private readonly SafetyFrameworkDestructionHandler _safetyFrameworkDestructionHandler;
    private readonly ArmoredRoofFrameRef _armoredRoofFrameRef;
    private readonly Transform _frameNormal;
    private readonly Transform _frameDamaged;
    private readonly CompositeDisposable _disposableCoupAnalyze = new CompositeDisposable();

    private Transform _currentRoof;
    private Transform _currentFrame;
    private bool _isArmored = false;
    private bool _safetyFrameworkInstalled = false;
    private bool _carHasBeenCoup = false;
    private bool _isCriticalDamaged = false;
    private bool _isСrushed = false;
    private DestructionMode _destructionMode = DestructionMode.ModeDefault;
    public RoofDestructionHandler(RoofRef roofRef, ArmoredRoofFrameRef armoredRoofFrameRef, SafetyFrameworkDestructionHandler safetyFrameworkDestructionHandler,
        CarMass carMass, CoupAnalyzer coupAnalyzer, ArmoredBackFrameDestructionHandler armoredBackFrameDestructionHandler,
        FrontDoorDestructionHandler frontDoorDestructionHandler, BackDoorDestructionHandler backDoorDestructionHandler,
        GlassDestructionHandler frontGlassDestructionHandler, GlassDestructionHandler backGlassDestructionHandler,
        GunDestructionHandler gunDestructionHandler, CabineDestructionHandler cabineDestructionHandler,
        DestructionHandlerContent destructionHandlerContent, Action<float> soundBends, Action<float> soundSoftHit,
        int totalStrengthRoof, int fallingContentLayer, bool isArmored, bool safetyFrameworkInstalled)
        : base(roofRef, destructionHandlerContent, soundSoftHit, totalStrengthRoof)
    {
        // Debug.Log($"TotalStrengthRoof: {totalStrengthRoof} | CarMass: {carMass.Mass}");
        _roofRef = roofRef;
        _coupAnalyzer = coupAnalyzer;
        _carMass = carMass;
        _frontDoorDestructionHandler = frontDoorDestructionHandler;
        _backDoorDestructionHandler = backDoorDestructionHandler;
        _frontGlassDestructionHandler = frontGlassDestructionHandler;
        _backGlassDestructionHandler = backGlassDestructionHandler;
        _armoredBackFrameDestructionHandler = armoredBackFrameDestructionHandler;
        _gunDestructionHandler = gunDestructionHandler;
        _cabineDestructionHandler = cabineDestructionHandler;
        _soundBends = soundBends;
        _roofNormal = roofRef.RoofNormal;
        _roofDamaged1 = roofRef.RoofDamaged1;
        _roofDamaged2 = roofRef.RoofDamaged2;
        _roofDamaged3 = roofRef.RoofDamaged3;
        _supportRoof = roofRef.SupportRoof;
        _currentRoof = _roofNormal;
        _fallingContentLayer = fallingContentLayer;
        _safetyFrameworkInstalled = safetyFrameworkInstalled;
        if (_safetyFrameworkInstalled == true)
        {
            _safetyFrameworkDestructionHandler = safetyFrameworkDestructionHandler;
        }
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _armoredRoofFrameRef = armoredRoofFrameRef;
            _frameNormal = armoredRoofFrameRef.FrameNormal;
            _frameDamaged = armoredRoofFrameRef.FrameDamaged;
            _currentFrame = _frameNormal;
            SubscribeCollider(_frameNormal.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
        else
        {
            SubscribeCollider(_roofNormal.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
    }

    public void Dispose()
    {
        _disposableCoupAnalyze.Clear();
        CompositeDisposable.Clear();
    }
    public void DestructNow()
    {
        CompositeDisposable.Clear();
        Dispose();
        _safetyFrameworkDestructionHandler?.TryThrow();
        TryThrowGlasses();
        if (_isArmored == true)
        {
            TryAddRigidBody(_currentFrame.gameObject);
            SetParentDebris(_armoredRoofFrameRef.transform);
        }
        TryAddRigidBody(_supportRoof.gameObject);
        TryAddRigidBody(_currentRoof.gameObject);

        SetParentDebris(_roofRef.transform);
    }
    protected override void TrySwitchMode()
    {
        if (ImpulseNormalValue > MaxStrength)
        {
            DestructionMode3();
        }
        else if (ImpulseNormalValue > HalfStrength)
        {
            DestructionMode2AndSubscribe();
            RecalculateStrength();
        }
        else if (ImpulseNormalValue > MinStrength)
        {
            DestructionMode1AndSubscribe();
            RecalculateStrength();
        }
        else
        {
            PlaySoftHitSound();
        }
    }

    private void PlayEffect()
    {
        _soundBends.Invoke(ImpulseNormalValue);
    }

    private void DestructionMode1AndSubscribe()
    {
        DestructionMode1();
        SubscribesDestructionMode1();
    }
    private void DestructionMode1()
    {
        CompositeDisposable.Clear();
        SwitchSprites1();
        _gunDestructionHandler?.TryDestruct();
        SetCurrentRoof(_roofDamaged1);
        _frontDoorDestructionHandler.TryDestructionMode1();
        _backDoorDestructionHandler.DestructionMode1();
        TryThrowGlasses();
        _armoredBackFrameDestructionHandler?.TryTakeDamageFromRoof();
        if (_isArmored == true)
        {
            _frameNormal.gameObject.SetActive(false);
            _frameDamaged.gameObject.SetActive(true);
            _currentFrame = _frameDamaged;
        }
        _destructionMode = DestructionMode.Mode1;
    }

    private void DestructionMode2AndSubscribe()
    {
        DestructionMode2();
        SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
    }
    private void DestructionMode2()
    {
        if (_destructionMode == DestructionMode.ModeDefault)
        {
            DestructionMode1();
        }
        CompositeDisposable.Clear();
        SwitchSprites2();
        SetCurrentRoof(_roofDamaged2);
        _frontDoorDestructionHandler.TryDestructionMode2();
        _backDoorDestructionHandler.TryDestructionMode2();
        _armoredBackFrameDestructionHandler?.TryTakeDamageFromRoof();
        _supportRoof.localScale = _scaleSupportRoofAfterHit;
        if (_safetyFrameworkInstalled == true)
        {
            _safetyFrameworkDestructionHandler.ChangeScale();
        }
        if (_isArmored == true)
        {
            _frameNormal.gameObject.SetActive(false);
            _frameDamaged.gameObject.SetActive(true);
            TryAddRigidBody(_frameDamaged.gameObject);
            SetParentDebris(_armoredRoofFrameRef.transform);
            _armoredBackFrameDestructionHandler?.TryThrow();
            _isArmored = false;
        }
        _destructionMode = DestructionMode.Mode2;
    }

    private /*async*/ void DestructionMode3()
    {
        if (_destructionMode != DestructionMode.Mode2 )
        {
            DestructionMode2();
        }
        CompositeDisposable.Clear();
        if (_safetyFrameworkInstalled == true && _isCriticalDamaged == false)
        {
            _isCriticalDamaged = true;
            SubscribeCoupAnalyze();
        }
        else
        {
            SwitchSprites3();
            SetCurrentRoof(_roofDamaged3);
            _safetyFrameworkDestructionHandler?.TryThrow();
            _frontDoorDestructionHandler.TryThrowDoor();
            _backDoorDestructionHandler.TryThrowDoor();
            SetParentDebris(_supportRoof);
            TryAddRigidBody(_supportRoof.gameObject);
            _cabineDestructionHandler.TryDriverDestruction();
        }
        _destructionMode = DestructionMode.Mode3;
    }

    protected override bool CollisionHandling(Collision2D collision)
    {
        bool result = false;
        if (1 << _fallingContentLayer == 1 << collision.gameObject.layer && _isСrushed == false)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                SetImpulseNormal(collision);
                ImpulseNormalValue += rigidbody2D.mass;
                if (rigidbody2D.mass > MaxStrength)
                {
                    _isCriticalDamaged = true;
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
        if (_coupAnalyzer.CarIsCoupCurrentValue == true)
        {
            if ((base.CheckCollision(collision) == true)
                && _carHasBeenCoup == false)
            {
                _carHasBeenCoup = true;
                SetImpulseNormal(collision);
                PlayEffect();
                ImpulseNormalValue += _carMass.Mass;
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            if (base.CheckCollisionAndMinSpeed(collision) == true)
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        return result;
    }

    private void SwitchSprites1()
    {
        _roofNormal.gameObject.SetActive(false);
        _roofDamaged1.gameObject.SetActive(true);
    }
    private void SwitchSprites2()
    {
        _roofDamaged1.gameObject.SetActive(false);
        _roofDamaged2.gameObject.SetActive(true);
    }
    private void SwitchSprites3()
    {
        _roofDamaged2.gameObject.SetActive(false);
        _roofDamaged3.gameObject.SetActive(true);
    }

    private void SetCurrentRoof(Transform roof)
    {
        _currentRoof = roof;
    }
    private void TryThrowGlasses()
    {
        _frontGlassDestructionHandler.TryThrowGlass();
        _backGlassDestructionHandler.TryThrowGlass();
    }
    private void SubscribeCoupAnalyze()
    {
        _coupAnalyzer.IsCoup.Subscribe(_ =>
        {
            TrySubscribeCollider();
        }).AddTo(_disposableCoupAnalyze);
    }

    private void TrySubscribeCollider()
    {
        if (_coupAnalyzer.CarIsCoupCurrentValue == false)
        {
            _carHasBeenCoup = false;
            _disposableCoupAnalyze.Clear();
            switch (_destructionMode)
            {
                case DestructionMode.Mode1:
                    SubscribesDestructionMode1();
                    break;
                case DestructionMode.Mode2:
                    SubscribesDestructionMode2();
                    break;
                case DestructionMode.Mode3:
                    SubscribesDestructionMode2();
                    break;
            }
        }
    }
    private void SubscribesDestructionMode1()
    {
        if (_isArmored == true)
        {
            SubscribeCollider(_frameDamaged.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
        else
        {
            SubscribeCollider(_roofDamaged1.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
    }
    private void SubscribesDestructionMode2()
    {
        if (_coupAnalyzer.CarIsCoupCurrentValue == false)
        {
            SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
    }
}