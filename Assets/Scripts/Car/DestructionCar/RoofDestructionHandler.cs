using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class RoofDestructionHandler : DestructionHandler
{
    private readonly int _carRoofBrokenLayer;
    private readonly float _strength1AfterHit;
    private readonly float _strength2AfterHit;
    // private readonly float _delay = 1f;
    private readonly Vector3 _scaleSupportRoofAfterHit = new Vector3(1f, 0.9f,1f);
    private readonly RoofRef _roofRef;
    private readonly FrontDoorDestructionHandler _frontDoorDestructionHandler;
    private readonly BackDoorDestructionHandler _backDoorDestructionHandler;
    private readonly GlassDestructionHandler _frontGlassDestructionHandler;
    private readonly GlassDestructionHandler _backGlassDestructionHandler;
    private readonly ArmoredBackFrameDestructionHandler _armoredBackFrameDestructionHandler;
    private readonly CoupAnalyzer _coupAnalyzer;
    private readonly CarMass _carMass;
    private readonly Transform _roofNormal;
    private readonly Transform _roofDamaged1;
    private readonly Transform _roofDamaged2;
    private readonly Transform _roofDamaged3;
    private readonly Transform _supportRoof;

    private readonly SafetyFrameworkDestructionHandler _safetyFrameworkDestructionHandler;
    private readonly SafetyFrameworkRef _safetyFramework;
    private readonly ArmoredRoofFrameRef _armoredRoofFrameRef;
    private readonly Transform _frameNormal;
    private readonly Transform _frameDamaged;
    private readonly CompositeDisposable _disposableCoupAnalyze = new CompositeDisposable();

    private Transform _currentRoof;
    private Transform _currentFrame;
    private int _lastDestructionMode = 0;
    private bool _isArmored = false;
    private bool _safetyFrameworkInstalled = false;
    private bool _frameHasDamaged;
    private bool _isCriticalDamaged = false;

    public RoofDestructionHandler(RoofRef roofRef, ArmoredRoofFrameRef armoredRoofFrameRef, SafetyFrameworkDestructionHandler safetyFrameworkDestructionHandler,
        CarMass carMass, CoupAnalyzer coupAnalyzer, ArmoredBackFrameDestructionHandler armoredBackFrameDestructionHandler,
        FrontDoorDestructionHandler frontDoorDestructionHandler, BackDoorDestructionHandler backDoorDestructionHandler,
        GlassDestructionHandler frontGlassDestructionHandler, GlassDestructionHandler backGlassDestructionHandler,
        DestructionHandlerContent destructionHandlerContent,
        int carRoofBrokenLayer, int totalStrengthRoof, bool isArmored, bool safetyFrameworkInstalled)
        : base(roofRef, destructionHandlerContent, totalStrengthRoof)
    {
        Debug.Log("TotalStrengthRoof:  " + totalStrengthRoof);
        _carRoofBrokenLayer = carRoofBrokenLayer;
        _strength1AfterHit = totalStrengthRoof * HalfStrengthMultiplier;
        _strength2AfterHit = totalStrengthRoof * MinStrengthMultiplier;
        _roofRef = roofRef;
        _coupAnalyzer = coupAnalyzer;
        _carMass = carMass;
        _frontDoorDestructionHandler = frontDoorDestructionHandler;
        _backDoorDestructionHandler = backDoorDestructionHandler;
        _frontGlassDestructionHandler = frontGlassDestructionHandler;
        _backGlassDestructionHandler = backGlassDestructionHandler;
        _armoredBackFrameDestructionHandler = armoredBackFrameDestructionHandler;
        _roofNormal = roofRef.RoofNormal;
        _roofDamaged1 = roofRef.RoofDamaged1;
        _roofDamaged2 = roofRef.RoofDamaged2;
        _roofDamaged3 = roofRef.RoofDamaged3;
        _supportRoof = roofRef.SupportRoof;
        _currentRoof = _roofNormal;
        _safetyFrameworkInstalled = safetyFrameworkInstalled;
        if (_safetyFrameworkInstalled == true)
        {
            _safetyFrameworkDestructionHandler = safetyFrameworkDestructionHandler;
            _safetyFramework = safetyFrameworkDestructionHandler.SafetyFrameworkRef;
        }
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _armoredRoofFrameRef = armoredRoofFrameRef;
            _frameNormal = armoredRoofFrameRef.FrameNormal;
            _frameDamaged = armoredRoofFrameRef.FrameDamaged;
            _currentFrame = _frameNormal;
            SubscribeCollider(_frameNormal.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
        }
        SubscribeCollider(_roofNormal.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
    }

    public void Dispose()
    {
        _disposableCoupAnalyze.Clear();
    }
    public void DestructNow()
    {
        CompositeDisposable.Clear();
        Dispose();
        _safetyFrameworkDestructionHandler?.TryThrow();
        if (_isArmored == true)
        {
            _currentFrame.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_armoredRoofFrameRef.transform);
        }
        _currentRoof.gameObject.AddComponent<Rigidbody2D>();
        SetParentDebris(_roofRef.transform);
    }
    protected override void TryDestruct()
    {
        // if (_coupAnalyzer.CarIsCoup == true)
        // {
        //     if (Speedometer.CurrentSpeedFloat > MinImpulseForDamage)
        //     {
        //         ApplyDamageCarMass();
        //         ApplyDamage();
        //     }
        //     else
        //     {
        //         ApplyDamageCarMass();
        //     }
        // }
        // else
        // {
        //     if (Speedometer.CurrentSpeedFloat > MinImpulseForDamage)
        //     {
        //         ApplyDamage();
        //     }
        // }
        if (MaxStrength <= StrengthForDestruct)
        {
            DestructionMode3();
        }
        else if (MaxStrength <= _strength2AfterHit)
        {
            DestructionMode2();
        }
        else if (MaxStrength <= _strength1AfterHit)
        {
            DestructionMode1();
        }
    }
    
    private void DestructionMode1()
    {
        CompositeDisposable.Clear();
        _lastDestructionMode = 1;
        SwitchSprites1();
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
        SubscribesDestructionMode1();
    }

    private void DestructionMode2()
    {
        CompositeDisposable.Clear();
        _lastDestructionMode = 2;
        SwitchSprites2();
        _frontDoorDestructionHandler.TryDestructionMode2();
        _backDoorDestructionHandler.TryDestructionMode2();
        TryThrowGlasses();
        _armoredBackFrameDestructionHandler.TryTakeDamageFromRoof();
        _supportRoof.localScale = _scaleSupportRoofAfterHit;
        if (_safetyFrameworkInstalled == true)
        {
            _safetyFrameworkDestructionHandler.ChangeScale();
        }

        if (_isArmored == true)
        {
            _frameNormal.gameObject.SetActive(false);
            _frameDamaged.gameObject.SetActive(true);
            _frameDamaged.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_armoredRoofFrameRef.transform);
            _armoredBackFrameDestructionHandler?.TryThrow();
            _isArmored = false;
        }
        SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
    }

    private /*async*/ void DestructionMode3()
    {
        CompositeDisposable.Clear();
        _lastDestructionMode = 3;
        SwitchSprites2();
        _frontDoorDestructionHandler.TryDestructionMode2();
        _backDoorDestructionHandler.TryDestructionMode2();
        TryThrowGlasses();
        if (_safetyFrameworkInstalled == true && _isCriticalDamaged == false)
        {
            _isCriticalDamaged = true;
            TrySubscribeCollider();
        }
        else
        {
            SwitchSprites3();
            _safetyFrameworkDestructionHandler.TryThrow();
            _frontDoorDestructionHandler.TryThrowDoor();
            _backDoorDestructionHandler.TryThrowDoor();
            SetParentDebris(_supportRoof);
            _supportRoof.gameObject.AddComponent<Rigidbody2D>();
            _roofRef.gameObject.layer = _carRoofBrokenLayer;
            // await UniTask.Delay(TimeSpan.FromSeconds(_delay));
        
            //invoke game over driver crushed
        }
    }

    protected override bool CheckCollision(Collision2D collision)
    {
        return (1 << collision.gameObject.layer & CanCollisionsLayerMasks.value) == 1 << collision.gameObject.layer;
    }

    private void SwitchSprites1()
    {
        _roofNormal.gameObject.SetActive(false);
        _roofDamaged1.gameObject.SetActive(true);
        _currentRoof = _roofDamaged1;
    }
    private void SwitchSprites2()
    {
        _roofNormal.gameObject.SetActive(false);
        _roofDamaged1.gameObject.SetActive(false);
        _roofDamaged2.gameObject.SetActive(true);
        _currentRoof = _roofDamaged2;
    }
    private void SwitchSprites3()
    {
        _roofNormal.gameObject.SetActive(false);
        _roofDamaged1.gameObject.SetActive(false);
        _roofDamaged2.gameObject.SetActive(false);
        _roofDamaged3.gameObject.SetActive(true);
        _currentRoof = _roofDamaged3;
    }
    private void TryThrowGlasses()
    {
        _frontGlassDestructionHandler.TryThrowGlass();
        _backGlassDestructionHandler.TryThrowGlass();
    }
    private void ApplyDamageCarMass()
    {
        MaxStrength -= _carMass.Mass;
    }
    private void SubscribeCoupAnalyze()
    {
        _coupAnalyzer.IsCoup.Subscribe(_ =>
        {
            if (_coupAnalyzer.CarIsCoup == false)
            {
                TrySubscribeCollider();
            }
        }).AddTo(_disposableCoupAnalyze);
    }

    private void TrySubscribeCollider()
    {
        _disposableCoupAnalyze.Clear();
        if (_coupAnalyzer.CarIsCoup == false)
        {
            switch (_lastDestructionMode)
            {
                case 1:
                    SubscribesDestructionMode1();
                    break;
                case 2:
                    SubscribesDestructionMode2();
                    break;
                case 3:
                    SubscribesDestructionMode2();
                    break;
            }
        }
        else
        {
            SubscribeCoupAnalyze();
        }
    }
    private void SubscribesDestructionMode1()
    {
        if (_isArmored == true)
        {
            SubscribeCollider(_frameDamaged.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
        }

        SubscribeCollider(_roofDamaged1.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
    }
    private void SubscribesDestructionMode2()
    {
        if (_coupAnalyzer.CarIsCoup == false)
        {
            SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
        }
    }
}