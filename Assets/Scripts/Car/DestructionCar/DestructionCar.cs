using System;
using NaughtyAttributes;
using UnityEngine;

public class DestructionCar : MonoBehaviour
{
    [SerializeField] private int _carDebrisLayer;
    [SerializeField] private LayerMask _canCollisionsLayerMasks;
    [SerializeField] private int _carRoofBrokenLayer;

    [SerializeField, BoxGroup("Settings")] private bool _bumpersDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _glassesDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _frontWingDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _backWingDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _roofDestructuonOn;
    [SerializeField, BoxGroup("Settings")] private bool _bottomDestructuonOn;

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
    
    
    [SerializeField, BoxGroup("Frame"), HorizontalLine(color:EColor.Black)] private ArmoredFrontFrameRef _armoredFrontFrameRef;
    [SerializeField, BoxGroup("Frame")] private ArmoredBackFrameRef _armoredBackFrameRef;
    [SerializeField, BoxGroup("Frame")] private ArmoredRoofFrameRef _armoredRoofFrameRef;
    [SerializeField, BoxGroup("Frame")] private SafetyFrameworkRef _safetyFrameworkRef;
    [SerializeField, BoxGroup("Frame")] private BottomRef _bottomRef;



    private CoupAnalyzer _coupAnalyzer;
    private BumperDestructionHandler _bumperDestructionFrontHandler;
    private BumperDestructionHandler _bumperDestructionBackHandler;
    private GunDestructionHandler _gunDestructionHandler;
    private BoosterDestructionHandler _boosterDestructionHandler;
    private FrontWingDestructionHandler _frontWingDestructionHandler;
    private BackWingDestructionHandler _backWingDestructionHandler;
    
    private GlassDestructionHandler _frontGlassDestructionHandler;
    private GlassDestructionHandler _backGlassDestructionHandler;
    
    private RoofDestructionHandler _roofDestructionHandler;
    private SafetyFrameworkDestructionHandler _safetyFrameworkDestructionHandler;

    private FrontDoorDestructionHandler _frontDoorDestructionHandler;
    private BackDoorDestructionHandler _backDoorDestructionHandler;
    
    private HotWheelDestructionHandler _hotWheelDestructionHandler;
    private ArmoredBackFrameDestructionHandler _armoredBackFrameHandler;
    private BottomDestructionHandler _bottomDestructionHandler;
    private BackCarDestructionHandler _backCarDestructionHandler;
    private DestructionHandlerContent _destructionHandlerContent;
    public void Construct(CarGun carGun, HotWheel hotWheel, CarMass carMass, Booster booster, Speedometer speedometer, CoupAnalyzer coupAnalyzer,
        HotWheelRef hotWheelRef, BoosterRef boosterRef, GunRef gunRef,
        Transform debrisParent)
    {
        _coupAnalyzer = coupAnalyzer;
        _destructionHandlerContent = new DestructionHandlerContent(speedometer, debrisParent, _canCollisionsLayerMasks, _carDebrisLayer);
        
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
    private void InitBumpersHandler()
    {
        if (_bumpersDestructuonOn == true)
        {
            if (CheckPart(_standartBumperRefFront))
            {
                _bumperDestructionFrontHandler = new BumperDestructionHandler(_standartBumperRefFront, _destructionHandlerContent);
            }

            if (CheckPart(_standartBumperRefBack))
            {
                _bumperDestructionBackHandler = new BumperDestructionHandler(_standartBumperRefBack, _destructionHandlerContent);
            }

            if (CheckPart(_armoredBumperRefFront))
            {
                _bumperDestructionFrontHandler = new BumperDestructionHandler(_armoredBumperRefFront, _destructionHandlerContent);
            }

            if (CheckPart(_armoredBumperRefBack))
            {
                _bumperDestructionBackHandler = new BumperDestructionHandler(_armoredBumperRefBack, _destructionHandlerContent);
            }
        }
    }
    private void InitGlassesHandler()
    {
        if (_glassesDestructuonOn == true)
        {
            if (CheckPart(_standartGlassRefFront))
            {
                _frontGlassDestructionHandler = new GlassDestructionHandler(_standartGlassRefFront, _destructionHandlerContent);
            }

            if (CheckPart(_standartGlassRefBack))
            {
                _backGlassDestructionHandler = new GlassDestructionHandler(_standartGlassRefBack, _destructionHandlerContent);
            }

            if (CheckPart(_armoredGlassRefFront))
            {
                _frontGlassDestructionHandler = new GlassDestructionHandler(_armoredGlassRefFront, _destructionHandlerContent);
            }

            if (CheckPart(_armoredGlassRefBack))
            {
                _backGlassDestructionHandler = new GlassDestructionHandler(_armoredGlassRefBack, _destructionHandlerContent);
            }
        }
    }
    private void InitBoosterHandler(BoosterRef boosterRef, Booster booster)
    {
        if (CheckPart(boosterRef))
        {
            _boosterDestructionHandler = new BoosterDestructionHandler(boosterRef, booster, _destructionHandlerContent);
        }
    }
    private void InitGunHandler(GunRef gunRef, CarGun carGun)
    {
        if (CheckPart(gunRef))
        {
            _gunDestructionHandler = new GunDestructionHandler(gunRef, carGun, _destructionHandlerContent);
        }
    }
    private void TryInitWingsHandler(BoosterRef boosterRef)
    {
        if (_frontWingDestructuonOn == true)
        {
            _frontWingDestructionHandler = new FrontWingDestructionHandler(_frontWingRef, _armoredFrontFrameRef,_frontGlassDestructionHandler,
                _hotWheelDestructionHandler, _destructionHandlerContent, CalculateStrengthFrontWing(), CheckPart(_armoredFrontFrameRef));
        }
        if (_backWingDestructuonOn == true)
        {
            _backWingDestructionHandler = new BackWingDestructionHandler(_backWingRef, _backGlassDestructionHandler, _armoredBackFrameHandler,
                _destructionHandlerContent, CalculateStrengthBackWing(), CheckPart(_armoredBackFrameRef), CheckPart(boosterRef));
        }
    }
    private void InitDoorsHandler()
    {
        if (CheckPart(_standartFrontDoorRef))
        {
            _frontDoorDestructionHandler = new FrontDoorDestructionHandler(_standartFrontDoorRef, _destructionHandlerContent);
        }
        else
        {
            _frontDoorDestructionHandler = new FrontDoorDestructionHandler(_armoredFrontDoorRef, _destructionHandlerContent, true);
        }
        if (CheckPart(_standartBackDoorRef))
        {
            _backDoorDestructionHandler = new BackDoorDestructionHandler(_standartBackDoorRef, _destructionHandlerContent);
        }
        else
        {
            _backDoorDestructionHandler = new BackDoorDestructionHandler(_armoredBackDoorRef, _destructionHandlerContent);
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
                _destructionHandlerContent, _carRoofBrokenLayer, CalculateStrengthRoof(),
                CheckPart(_armoredRoofFrameRef), CheckPart(_safetyFrameworkRef));
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
            _armoredBackFrameHandler = new ArmoredBackFrameDestructionHandler(_armoredBackFrameRef, _destructionHandlerContent);
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
        if (_bottomDestructuonOn == true)
        {
            _backCarDestructionHandler = new BackCarDestructionHandler(_backWingDestructionHandler, _bumperDestructionBackHandler);
            _bottomDestructionHandler = new BottomDestructionHandler(_bottomRef,
                _backCarDestructionHandler, _roofDestructionHandler,
                _frontDoorDestructionHandler, _backDoorDestructionHandler, _destructionHandlerContent,
                CalculateStrengthBottom(), CheckPart(_bottomRef.ArmoredBottom));
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
    private void OnDisable()
    {
        _roofDestructionHandler?.Dispose();
    }
}