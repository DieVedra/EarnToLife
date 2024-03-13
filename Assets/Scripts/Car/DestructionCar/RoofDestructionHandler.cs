using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class RoofDestructionHandler : DestructionHandler, IDispose
{
    private readonly Vector3 _scaleSupportRoofAfterHit = new Vector3(1f, 0.9f,1f);
    private readonly RoofRef _roofRef;
    private readonly FrontDoorDestructionHandler _frontDoorDestructionHandler;
    private readonly BackDoorDestructionHandler _backDoorDestructionHandler;
    private readonly GlassDestructionHandler _frontGlassDestructionHandler;
    private readonly GlassDestructionHandler _backGlassDestructionHandler;
    private readonly ArmoredBackFrameDestructionHandler _armoredBackFrameDestructionHandler;
    private readonly GunDestructionHandler _gunDestructionHandler;
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
    private bool _frameHasDamaged;
    private bool _isCriticalDamaged = false;
    private DestructionMode _destructionMode = DestructionMode.ModeDefault;

    public RoofDestructionHandler(RoofRef roofRef, ArmoredRoofFrameRef armoredRoofFrameRef, SafetyFrameworkDestructionHandler safetyFrameworkDestructionHandler,
        CarMass carMass, CoupAnalyzer coupAnalyzer, ArmoredBackFrameDestructionHandler armoredBackFrameDestructionHandler,
        FrontDoorDestructionHandler frontDoorDestructionHandler, BackDoorDestructionHandler backDoorDestructionHandler,
        GlassDestructionHandler frontGlassDestructionHandler, GlassDestructionHandler backGlassDestructionHandler,
        GunDestructionHandler gunDestructionHandler, DestructionHandlerContent destructionHandlerContent,
        int totalStrengthRoof, bool isArmored, bool safetyFrameworkInstalled)
        : base(roofRef, destructionHandlerContent, totalStrengthRoof)
    {
        Debug.Log("TotalStrengthRoof:  " + totalStrengthRoof);
        _roofRef = roofRef;
        _coupAnalyzer = coupAnalyzer;
        _carMass = carMass;
        _frontDoorDestructionHandler = frontDoorDestructionHandler;
        _backDoorDestructionHandler = backDoorDestructionHandler;
        _frontGlassDestructionHandler = frontGlassDestructionHandler;
        _backGlassDestructionHandler = backGlassDestructionHandler;
        _armoredBackFrameDestructionHandler = armoredBackFrameDestructionHandler;
        _gunDestructionHandler = gunDestructionHandler;
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
        }
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _armoredRoofFrameRef = armoredRoofFrameRef;
            _frameNormal = armoredRoofFrameRef.FrameNormal;
            _frameDamaged = armoredRoofFrameRef.FrameDamaged;
            _currentFrame = _frameNormal;
            SubscribeCollider(_frameNormal.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
        }
        SubscribeCollider(_roofNormal.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
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
        if (_isArmored == true)
        {
            _currentFrame.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_armoredRoofFrameRef.transform);
        }
        _currentRoof.gameObject.AddComponent<Rigidbody2D>();
        SetParentDebris(_roofRef.transform);
    }
    protected override void TrySwitchMode()
    {
        if (ValueNormalImpulse > MaxStrength)
        {
            Debug.Log("DestructionMode3");
            DestructionMode3();
        }
        else if (ValueNormalImpulse > HalfStrength)
        {
            Debug.Log("DestructionMode2");
            DestructionMode2AndSubscribe();
            RecalculateStrength();
        }
        else if (ValueNormalImpulse > MinStrength)
        {
            Debug.Log("DestructionMode1");
            DestructionMode1AndSubscribe();
            RecalculateStrength();
        }
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
        SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
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
            _frameDamaged.gameObject.AddComponent<Rigidbody2D>();
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
        // SwitchSprites2();
        if (_safetyFrameworkInstalled == true && _isCriticalDamaged == false)
        {
            _isCriticalDamaged = true;
            SubscribeCoupAnalyze();
            // TrySubscribeCollider();
        }
        else
        {
            SwitchSprites3();
            SetCurrentRoof(_roofDamaged3);
            _safetyFrameworkDestructionHandler?.TryThrow();
            _frontDoorDestructionHandler.TryThrowDoor();
            _backDoorDestructionHandler.TryThrowDoor();
            SetParentDebris(_supportRoof);
            _supportRoof.gameObject.AddComponent<Rigidbody2D>();
            // await UniTask.Delay(TimeSpan.FromSeconds(_delay));
        
            //invoke game over driver crushed
            Debug.Log("game over driver crushed");
        }
        _destructionMode = DestructionMode.Mode3;
    }

    protected override bool CheckCollision(Collision2D collision)
    {
        bool result = false;
        if (_coupAnalyzer.CarIsCoupCurrentValue == true)
        {
            if ((1 << collision.gameObject.layer & CanCollisionsLayerMasks.value) == 1 << collision.gameObject.layer)
            {
                SetImpulseNormal(collision);
                ValueNormalImpulse += _carMass.Mass;
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            if (base.CheckCollision(collision))
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        Debug.Log("Impulse: " + ValueNormalImpulse);
        Debug.Log("result: " + result);
        Debug.Log("CarIsCoupCurrentValue: " + _coupAnalyzer.CarIsCoupCurrentValue);
        
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
            if (_coupAnalyzer.CarIsCoupCurrentValue == false)
            {
                TrySubscribeCollider();
            }
        }).AddTo(_disposableCoupAnalyze);
    }

    private void TrySubscribeCollider()
    {
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
    // private void TrySubscribeCollider()
    // {
    //     _disposableCoupAnalyze.Clear();
    //     if (_coupAnalyzer.CarIsCoupCurrentValue == false)
    //     {
    //         switch (_destructionMode)
    //         {
    //             case DestructionMode.Mode1:
    //                 SubscribesDestructionMode1();
    //                 break;
    //             case DestructionMode.Mode2:
    //                 SubscribesDestructionMode2();
    //                 break;
    //             case DestructionMode.Mode3:
    //                 SubscribesDestructionMode2();
    //                 break;
    //         }
    //     }
    //     else
    //     {
    //         SubscribeCoupAnalyze();
    //     }
    // }
    private void SubscribesDestructionMode1()
    {
        if (_isArmored == true)
        {
            SubscribeCollider(_frameDamaged.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
        }

        SubscribeCollider(_roofDamaged1.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
    }
    private void SubscribesDestructionMode2()
    {
        if (_coupAnalyzer.CarIsCoupCurrentValue == false)
        {
            SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
        }
    }
}