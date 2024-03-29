﻿using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class DestructionCar : MonoBehaviour
{
    [SerializeField] private int _carDebrisLayer;
    [SerializeField] private int _fallingContentLayer;

    [SerializeField] private LayerMask _canCollisionsLayerMasks;
    
    [SerializeField, BoxGroup("Effects")] private ParticleSystem _glassBrokenEffectPrefab;
    [SerializeField, BoxGroup("Effects")] private ParticleSystem _hitEffectPrefab;
    [SerializeField, BoxGroup("Effects")] private ParticleSystem _engineSmokeEffect;
    [SerializeField, BoxGroup("Effects")] private ParticleSystem _engineBurnEffect;
    [SerializeField, BoxGroup("Effects")] private Transform _effectsParent;
    [SerializeField, BoxGroup("Effects")] private AnimationCurve _soundsCurve;    //time 0 - 200

    [SerializeField, BoxGroup("Settings")] private bool _bumpersDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _glassesDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _frontWingDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _backWingDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _roofDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _bottomDestructionOn;

    [SerializeField, BoxGroup("Exhaust")] private Transform _point1;
    [SerializeField, BoxGroup("Exhaust")] private Transform _point2;


    [SerializeField, BoxGroup("Bumpers"), HorizontalLine(color:EColor.Red)] private BumperRef _standartBumperRefFront;
    [SerializeField, BoxGroup("Bumpers")] private BumperRef _standartBumperRefBack;
    [SerializeField, BoxGroup("Bumpers")] private BumperRef _armoredBumperRefFront;
    [SerializeField, BoxGroup("Bumpers")] private BumperRef _armoredBumperRefBack;
    
    [SerializeField, BoxGroup("Glasses"), HorizontalLine(color:EColor.White)] private GlassRef _standartGlassRefFront;
    [SerializeField, BoxGroup("Glasses")] private GlassRef _standartGlassRefBack;
    [SerializeField, BoxGroup("Glasses")] private GlassRef _armoredGlassRefFront;
    [SerializeField, BoxGroup("Glasses")] private GlassRef _armoredGlassRefBack;

    [SerializeField, BoxGroup("Doors"), HorizontalLine(color:EColor.Green)] private DoorRef _standartFrontDoorRef;
    [SerializeField, BoxGroup("Doors")] private DoorRef _standartBackDoorRef;
    [SerializeField, BoxGroup("Doors")] private DoorRef _armoredFrontDoorRef;
    [SerializeField, BoxGroup("Doors")] private DoorRef _armoredBackDoorRef;
    
    [SerializeField, BoxGroup("Wings"), HorizontalLine(color:EColor.Green)] private FrontWingRef _frontWingRef;
    [SerializeField, BoxGroup("Wings")] private BackWingRef _backWingRef;

    [SerializeField, BoxGroup("Roof"), HorizontalLine(color:EColor.Blue)] private RoofRef _roofRef;
    [SerializeField, BoxGroup("Cabine"), HorizontalLine(color:EColor.Orange)] private CabineRef _cabineRef;
    
    [SerializeField, BoxGroup("Frame"), HorizontalLine(color:EColor.Black)] private ArmoredFrontFrameRef _armoredFrontFrameRef;
    [SerializeField, BoxGroup("Frame")] private ArmoredBackFrameRef _armoredBackFrameRef;
    [SerializeField, BoxGroup("Frame")] private ArmoredRoofFrameRef _armoredRoofFrameRef;
    [SerializeField, BoxGroup("Frame")] private SafetyFrameworkRef _safetyFrameworkRef;
    [SerializeField, BoxGroup("Frame")] private BottomRef _bottomRef;


    private DestructionEffectsHandler _destructionEffectsHandler;
    private DestructionAudioHandler _destructionAudioHandler;
    private CoupAnalyzer _coupAnalyzer;
    private CarMass _carMass;
    private BumperDestructionHandler _frontBumperDestructionHandler;
    private BumperDestructionHandler _backBumperDestructionHandler;
    private GunDestructionHandler _gunDestructionHandler;
    private BoosterDestructionHandler _boosterDestructionHandler;
    private BackWingDestructionHandler _backWingDestructionHandler;
    private GlassDestructionHandler _frontGlassDestructionHandler;
    private GlassDestructionHandler _backGlassDestructionHandler;
    private ExhaustHandler _exhaustHandler;
    private RoofDestructionHandler _roofDestructionHandler;
    private SafetyFrameworkDestructionHandler _safetyFrameworkDestructionHandler;

    private FrontDoorDestructionHandler _frontDoorDestructionHandler;
    private BackDoorDestructionHandler _backDoorDestructionHandler;

    private HotWheelDestructionHandler _hotWheelDestructionHandler;
    private ArmoredBackFrameDestructionHandler _armoredBackFrameHandler;
    private BottomDestructionHandler _bottomDestructionHandler;
    private BackCarHandler _backCarHandler;
    private DestructionHandlerContent _destructionHandlerContent;
    private List<IDispose> _disposes = new List<IDispose>();
    public FrontWingDestructionHandler FrontWingDestructionHandler { get; private set; }
    public CabineDestructionHandler CabineDestructionHandler { get; private set; }

    public event Action<WheelJoint2D, WheelCarValues> OnCarBrokenIntoTwoParts;
    public bool BottomDestructionOn => _bottomDestructionOn;
    public void Construct(DestructionAudioHandler destructionAudioHandler, Exhaust exhaust, CarGun carGun, HotWheel hotWheel, CarMass carMass, Booster booster, Speedometer speedometer, CoupAnalyzer coupAnalyzer,
        HotWheelRef hotWheelRef, BoosterRef boosterRef, GunRef gunRef,
        Transform debrisParent)
    {
        _destructionAudioHandler = destructionAudioHandler;
        _destructionAudioHandler.Init(_soundsCurve);
        InitDestructionEffectsHandler();
        _coupAnalyzer = coupAnalyzer;
        _destructionHandlerContent = new DestructionHandlerContent(speedometer, debrisParent, _canCollisionsLayerMasks, _carDebrisLayer);
        _carMass = carMass;
        _exhaustHandler = new ExhaustHandler(exhaust, _point1, _point2);
        InitCabineHandler();
        TryInitSafetyFrameworkDestructionHandler(debrisParent);
        InitBumpersHandler();
        InitGlassesHandler();
        InitHotWheel(hotWheel, hotWheelRef);
        InitBoosterHandler(boosterRef, booster);
        InitGunHandler(gunRef, carGun);
        InitDoorsHandler();
        InitArmoredBackFrameHandler();
        TryInitWingsHandler(boosterRef);
        InitRoofHandler(carMass, coupAnalyzer);
        InitBottomHandler();
    }

    private void InitDestructionEffectsHandler()
    {
        _destructionEffectsHandler = new DestructionEffectsHandler(_destructionAudioHandler, _glassBrokenEffectPrefab, _hitEffectPrefab, _engineSmokeEffect, _engineBurnEffect, _effectsParent);
        _disposes.Add(_destructionEffectsHandler);
    }
    private void InitCabineHandler()
    {
        CabineDestructionHandler = new CabineDestructionHandler(_cabineRef, _destructionHandlerContent);
        AddToDispose(CabineDestructionHandler);
    }

    private void InitBumpersHandler()
    {
        if (_bumpersDestructuonOn == true)
        {
            if (CheckPart(_standartBumperRefFront))
            {
                _frontBumperDestructionHandler = new BumperDestructionHandler(_standartBumperRefFront, _destructionHandlerContent,
                    _destructionEffectsHandler.HitBrokenEffect, _destructionAudioHandler.PlaySoftHit);
            }

            if (CheckPart(_standartBumperRefBack))
            {
                _backBumperDestructionHandler = new BumperDestructionHandler(_standartBumperRefBack, _destructionHandlerContent,
                    _destructionEffectsHandler.HitBrokenEffect, _destructionAudioHandler.PlaySoftHit);
            }

            if (CheckPart(_armoredBumperRefFront))
            {
                _frontBumperDestructionHandler = new BumperDestructionHandler(_armoredBumperRefFront, _destructionHandlerContent,
                    _destructionEffectsHandler.HitBrokenEffect, _destructionAudioHandler.PlaySoftHit);
            }

            if (CheckPart(_armoredBumperRefBack))
            {
                _backBumperDestructionHandler = new BumperDestructionHandler(_armoredBumperRefBack, _destructionHandlerContent,
                    _destructionEffectsHandler.HitBrokenEffect, _destructionAudioHandler.PlaySoftHit);
            }
            AddToDispose(_frontBumperDestructionHandler);
            AddToDispose(_backBumperDestructionHandler);
        }
    }
    private void InitGlassesHandler()
    {
        if (_glassesDestructuonOn == true)
        {
            if (CheckPart(_standartGlassRefFront))
            {
                _frontGlassDestructionHandler = new GlassDestructionHandler(_standartGlassRefFront, _destructionHandlerContent, _destructionEffectsHandler.GlassBrokenEffect);
            }

            if (CheckPart(_standartGlassRefBack))
            {
                _backGlassDestructionHandler = new GlassDestructionHandler(_standartGlassRefBack, _destructionHandlerContent, _destructionEffectsHandler.GlassBrokenEffect);
            }

            if (CheckPart(_armoredGlassRefFront))
            {
                _frontGlassDestructionHandler = new GlassDestructionHandler(_armoredGlassRefFront, _destructionHandlerContent, _destructionEffectsHandler.HitBrokenEffect);
            }

            if (CheckPart(_armoredGlassRefBack))
            {
                _backGlassDestructionHandler = new GlassDestructionHandler(_armoredGlassRefBack, _destructionHandlerContent, _destructionEffectsHandler.HitBrokenEffect);
            }
            AddToDispose(_frontGlassDestructionHandler);
            AddToDispose(_backGlassDestructionHandler);
        }
    }
    private void InitBoosterHandler(BoosterRef boosterRef, Booster booster)
    {
        if (CheckPart(boosterRef))
        {
            _boosterDestructionHandler = new BoosterDestructionHandler(boosterRef, booster, _destructionHandlerContent, _destructionEffectsHandler.HitBrokenEffect, _destructionAudioHandler.PlaySoftHit);
            AddToDispose(_boosterDestructionHandler);
        }
    }
    private void InitGunHandler(GunRef gunRef, CarGun carGun)
    {
        if (CheckPart(gunRef))
        {
            _gunDestructionHandler = new GunDestructionHandler(gunRef, carGun, _destructionHandlerContent, _destructionEffectsHandler.HitBrokenEffect);
            AddToDispose(_gunDestructionHandler);
        }
    }
    private void TryInitWingsHandler(BoosterRef boosterRef)
    {
        if (_frontWingDestructuonOn == true)
        {
            FrontWingDestructionHandler = new FrontWingDestructionHandler(_frontWingRef, _armoredFrontFrameRef,_frontGlassDestructionHandler,
                _hotWheelDestructionHandler, _frontBumperDestructionHandler, _destructionHandlerContent, 
                _destructionEffectsHandler.HitBrokenEffect, _destructionEffectsHandler.TryPlayEngineSmokeEffect,
                _destructionEffectsHandler.TryPlayEngineBurnEffect, _destructionAudioHandler.PlaySoftHit,
                CalculateStrengthFrontWing(), CheckPart(_armoredFrontFrameRef));
            AddToDispose(FrontWingDestructionHandler);
        }
        if (_backWingDestructuonOn == true)
        {
            _backWingDestructionHandler = new BackWingDestructionHandler(_backWingRef, _backGlassDestructionHandler, _armoredBackFrameHandler,
                _backBumperDestructionHandler, _exhaustHandler, _destructionEffectsHandler.HitBrokenEffect, _destructionAudioHandler.PlaySoftHit, _destructionHandlerContent, 
                CalculateStrengthBackWing(), CheckPart(_armoredBackFrameRef), CheckPart(boosterRef));
            AddToDispose(_backWingDestructionHandler);
        }
    }
    private void InitDoorsHandler()
    {
        if (CheckPart(_standartFrontDoorRef))
        {
            _frontDoorDestructionHandler = new FrontDoorDestructionHandler(_standartFrontDoorRef, _destructionHandlerContent, _destructionEffectsHandler.GlassBrokenEffect);
        }
        else
        {
            _frontDoorDestructionHandler = new FrontDoorDestructionHandler(_armoredFrontDoorRef, _destructionHandlerContent, _destructionEffectsHandler.HitBrokenEffect,true);
        }
        if (CheckPart(_standartBackDoorRef))
        {
            _backDoorDestructionHandler = new BackDoorDestructionHandler(_standartBackDoorRef, _destructionHandlerContent, _destructionEffectsHandler.GlassBrokenEffect);
        }
        else
        {
            _backDoorDestructionHandler = new BackDoorDestructionHandler(_armoredBackDoorRef, _destructionHandlerContent, _destructionEffectsHandler.HitBrokenEffect);
        }
    }
    private void InitRoofHandler(CarMass carMass, CoupAnalyzer coupAnalyzer)
    {
        if (_roofDestructuonOn == true)
        {
            _roofDestructionHandler = new RoofDestructionHandler(
                _roofRef, _armoredRoofFrameRef, _safetyFrameworkDestructionHandler,
                carMass, coupAnalyzer, _armoredBackFrameHandler, 
                _frontDoorDestructionHandler, _backDoorDestructionHandler,
                _frontGlassDestructionHandler, _backGlassDestructionHandler,
                _gunDestructionHandler, CabineDestructionHandler, _destructionHandlerContent, _destructionAudioHandler.PlayRoofBends, _destructionAudioHandler.PlaySoftHit, CalculateStrengthRoof(), _fallingContentLayer,
                CheckPart(_armoredRoofFrameRef), CheckPart(_safetyFrameworkRef));
            AddToDispose(_roofDestructionHandler);
        }
    }
    private void InitHotWheel(HotWheel hotWheel, HotWheelRef hotWheelRef)
    {
        if (CheckPart(hotWheelRef))
        {
            _hotWheelDestructionHandler = new HotWheelDestructionHandler(hotWheelRef, hotWheel);
        }
    }
    private void InitArmoredBackFrameHandler()
    {
        if (CheckPart(_armoredBackFrameRef))
        {
            _armoredBackFrameHandler = new ArmoredBackFrameDestructionHandler(_armoredBackFrameRef, _destructionHandlerContent, _destructionAudioHandler.PlaySoftHit);
        }
    }

    private void TryInitSafetyFrameworkDestructionHandler(Transform debrisParent)
    {
        if (CheckPart(_safetyFrameworkRef))
        {
            _safetyFrameworkDestructionHandler = new SafetyFrameworkDestructionHandler(_safetyFrameworkRef, debrisParent);
        }
    }

    private void InitBottomHandler()
    {
        if (_bottomDestructionOn == true)
        {
            _backCarHandler = new BackCarHandler(_bottomRef, _backWingDestructionHandler);
            _bottomDestructionHandler = new BottomDestructionHandler(_bottomRef,
                _backCarHandler, _roofDestructionHandler,
                _frontDoorDestructionHandler, _backDoorDestructionHandler, _exhaustHandler,
                _destructionHandlerContent, _destructionAudioHandler.PlayHardHit, _destructionAudioHandler.PlaySoftHit,
                CalculateStrengthBottom(), CheckPart(_bottomRef.ArmoredBottom));
            AddToDispose(_bottomDestructionHandler);
            _backCarHandler.OnCarBrokenIntoTwoParts += CarBrokenIntoTwoParts;
        }
    }
    private bool CheckPart(Transform part)
    {
        if (part.gameObject.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckPart(MonoBehaviour part)
    {
        if (part.gameObject.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int CalculateStrengthRoof()
    {
        int total = _roofRef.StrengthRoof;
        if (CheckPart(_safetyFrameworkRef))
        {
            total += _safetyFrameworkRef.StrengthSafetyFramework;
        }
        if (CheckPart(_armoredRoofFrameRef))
        {
            total += _armoredRoofFrameRef.StrengthArmoredRoof;
        }
        total += _frontDoorDestructionHandler.StrengthDoor;
        total += _backDoorDestructionHandler.StrengthDoor;
        return total;
    }
    private int CalculateStrengthFrontWing()
    {
        int total = _frontWingRef.StrengthWing;
        if (CheckPart(_armoredFrontFrameRef))
        {
            total += _armoredFrontFrameRef.StrengthArmoredFront;
        }
        return total;
    }
    private int CalculateStrengthBackWing()
    {
        int total = _backWingRef.StrengthWing;
        if (CheckPart(_armoredBackFrameRef))
        {
            total += _armoredBackFrameRef.StrengthArmoredBack;
        }
        return total;
    }

    private int CalculateStrengthBottom()
    {
        int total = _bottomRef.StrengthBottom;
        if (CheckPart(_bottomRef.ArmoredBottom))
        {
            total += _bottomRef.StrengthArmoredBottom;
        }
        return total;
    }

    private void AddToDispose(IDispose dispose)
    {
        _disposes.Add(dispose);
    }

    private void CarBrokenIntoTwoParts(WheelJoint2D joint2D, WheelCarValues wheelCarValues)
    {
        OnCarBrokenIntoTwoParts?.Invoke(joint2D, wheelCarValues);
        _carMass.ChangeMassOnCarBrokenIntoTwoParts();
    }

    private void OnDisable()
    {
        foreach (var dispose in _disposes)
        {
            dispose.Dispose();
        }
        if (_bottomDestructionOn == true)
        {
            _backCarHandler.OnCarBrokenIntoTwoParts -= CarBrokenIntoTwoParts;
        }
    }
}