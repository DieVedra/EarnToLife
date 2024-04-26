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
    private readonly DestructionAudioHandler _destructionAudioHandler;
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
    public RoofDestructionHandler(RoofRef roofRef, ArmoredRoofFrameRef armoredRoofFrameRef, SafetyFrameworkDestructionHandler safetyFrameworkDestructionHandler,
        CarMass carMass, CoupAnalyzer coupAnalyzer, ArmoredBackFrameDestructionHandler armoredBackFrameDestructionHandler,
        FrontDoorDestructionHandler frontDoorDestructionHandler, BackDoorDestructionHandler backDoorDestructionHandler,
        GlassDestructionHandler frontGlassDestructionHandler, GlassDestructionHandler backGlassDestructionHandler,
        GunDestructionHandler gunDestructionHandler, CabineDestructionHandler cabineDestructionHandler,
        DestructionHandlerContent destructionHandlerContent,  DestructionAudioHandler destructionAudioHandler/*Action<float> soundBends, Action<float> soundHardHit, Action<float, string> soundSoftHit*/,
        int totalStrengthRoof, int fallingContentLayer, bool isArmored, bool safetyFrameworkInstalled)
        : base(roofRef, destructionHandlerContent, " roof ",destructionAudioHandler, totalStrengthRoof)
    {
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
        _destructionAudioHandler = destructionAudioHandler;
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
            SubscribeCollider(_frameNormal.GetComponent<Collider2D>(), CollisionHandlingRoof, TrySwitchMode);
        }
        else
        {
            SubscribeCollider(_roofNormal.GetComponent<Collider2D>(), CollisionHandlingRoof, TrySwitchMode);
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
            SetCarDebrisLayerNonInteractableWithCar(_armoredRoofFrameRef.transform);
        }
        TryAddRigidBody(_supportRoof.gameObject);
        TryAddRigidBody(_currentRoof.gameObject);
        PlaySoundHardHit();
        PlaySoundBends();
        SetParentDebris(_roofRef.transform);
        SetCarDebrisLayerInteractableWithCar(_roofRef.transform);
    }
    protected override void TrySwitchMode()
    {
        if (ImpulseNormalValue > MaxStrength)
        {
            PlaySoundHardHit();
            DestructionMode3();
        }
        else if (ImpulseNormalValue > HalfStrength)
        {
            PlaySoundHardHit();
            DestructionMode2AndSubscribe();
            RecalculateStrength();
        }
        else if (ImpulseNormalValue > MinStrength)
        {
            PlaySoundHardHit();
            DestructionMode1AndSubscribe();
            RecalculateStrength();
        }
        else
        {
            PlaySoftHitSound();
        }
    }

    private void PlaySoundHardHit()
    {
        _destructionAudioHandler.PlayHardHit(ImpulseNormalValue);
    }
    private void PlaySoundBends()
    {
        _destructionAudioHandler.PlayRoofBends();
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
        DestructionMode = DestructionMode.Mode1;
    }

    private void DestructionMode2AndSubscribe()
    {
        DestructionMode2();
        SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), CollisionHandlingRoof, TrySwitchMode);
    }
    private void DestructionMode2()
    {
        if (DestructionMode == DestructionMode.ModeDefault)
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
            SetCarDebrisLayerNonInteractableWithCar(_armoredRoofFrameRef.transform);
            _armoredBackFrameDestructionHandler?.TryThrow();
            _isArmored = false;
        }
        DestructionMode = DestructionMode.Mode2;
    }

    private void DestructionMode3()
    {
        if (DestructionMode != DestructionMode.Mode2 )
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
            SetCarDebrisLayerNonInteractableWithCar(_supportRoof);
            _cabineDestructionHandler.TryDriverDestruction();
        }
        DestructionMode = DestructionMode.Mode3;
    }

    private bool CollisionHandlingRoof(Collision2D collision)
    {
        Debug.Log($"Overriding CollisionHandling roof ");

        bool result = false;
        if (1 << _fallingContentLayer == 1 << collision.gameObject.layer && _isСrushed == false)
        {
            if (collision.gameObject.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                SetImpulseNormalAndHitPosition(collision);
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
        if (_coupAnalyzer.CarIsCoup == true)
        {
            Debug.Log($"Overriding CollisionHandling roof   CarIsCoup");

            if ((base.CheckCollision(collision) == true)
                && _carHasBeenCoup == false)
            {
                _carHasBeenCoup = true;
                SetImpulseNormalAndHitPosition(collision);
                PlaySoundBends();
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
                SetImpulseNormalAndHitPosition(collision);
                PlaySoundHardHit();
                result = true;
            }
            else
            {
                SetImpulseNormalAndHitPosition(collision);
                PlaySoftHitSound();
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
        if (_coupAnalyzer.CarIsCoup == false)
        {
            _carHasBeenCoup = false;
            _disposableCoupAnalyze.Clear();
            switch (DestructionMode)
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
            SubscribeCollider(_frameDamaged.GetComponent<Collider2D>(), CollisionHandlingRoof, TrySwitchMode);
        }
        else
        {
            SubscribeCollider(_roofDamaged1.GetComponent<Collider2D>(), CollisionHandlingRoof, TrySwitchMode);
        }
    }
    private void SubscribesDestructionMode2()
    {
        if (_coupAnalyzer.CarIsCoup == false)
        {
            SubscribeCollider(_roofDamaged2.GetComponent<Collider2D>(), this.CollisionHandling, TrySwitchMode);
        }
    }
}