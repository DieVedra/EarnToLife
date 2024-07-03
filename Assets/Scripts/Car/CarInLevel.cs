using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using NaughtyAttributes;
using UniRx;

[RequireComponent(typeof(Rigidbody2D))]
public class CarInLevel : Car
{
    [SerializeField, BoxGroup("Engine"), HorizontalLine(color:EColor.White)] private AnimationCurve _engineAccelerationCurve;
    [SerializeField, BoxGroup("Engine")] private ParticleSystem _exhaustParticleSystem;

    [SerializeField, Range(0f, 1f), BoxGroup("_gyroscope"), HorizontalLine(color:EColor.Green)] private float _gyroscopePower;

    [SerializeField, BoxGroup("WheelGroundInteraction"), HorizontalLine(color:EColor.Yellow)] private LayerMask _groundContactMask;
    [SerializeField, BoxGroup("WheelGroundInteraction"), Layer] private int _asphaltLayer;
    [SerializeField, BoxGroup("WheelGroundInteraction"), Layer] private int _groundLayer;
    [SerializeField, BoxGroup("WheelGroundInteraction"), Layer] private int _zombieBloodLayer;
    [SerializeField, BoxGroup("WheelGroundInteraction")] private AnimationCurve _particlesSpeedCurveGasState;
    [SerializeField, BoxGroup("WheelGroundInteraction")] private AnimationCurve _particlesSpeedCurveStopState;

    [SerializeField, BoxGroup("Gun"), HorizontalLine(color:EColor.Red)] private GunRef _gunRef;
    [SerializeField, BoxGroup("Gun")] private Transform _gunTransformRotate;
    [SerializeField, BoxGroup("Gun")] private Transform _defaultPointAiming;
    [SerializeField, BoxGroup("Gun")] private ParticleSystem _gunParticleSystem;
    [SerializeField, BoxGroup("Gun"), Range(0f,5f)] private float _speedLook = 2f;
    [SerializeField, BoxGroup("Gun"), Range(0f,15f)] private float _distanceDetectionValue = 6f;
    [SerializeField, BoxGroup("Gun"), Range(0f,15f)] private float _deadZoneDetectionValue = 2f;
    [SerializeField, BoxGroup("Gun")] private LayerMask _targetsGunLayerMask;
    [SerializeField, BoxGroup("Gun")] private float _rateFire = 1f;
    [SerializeField, BoxGroup("Gun")] private float _forceGun = 100f;

    [SerializeField, BoxGroup("Booster"), HorizontalLine(color:EColor.Green)] private BoosterRef _boosterRef;
    [SerializeField, BoxGroup("Booster"), Range(1f,50f)] private float _rotationSpeed;
    [SerializeField, BoxGroup("Booster"), Range(50f,1000f)] private float _force;
    [SerializeField, BoxGroup("Booster")] private ParticleSystem _boosterParticleSystem;
    [SerializeField, BoxGroup("Booster")] private Transform _screw;
    [SerializeField, BoxGroup("Booster")] private SpriteRenderer _blade1;
    [SerializeField, BoxGroup("Booster")] private SpriteRenderer _blade2;

    [SerializeField, BoxGroup("HotWheel"), HorizontalLine(color:EColor.Red)] private HotWheelRef _hotWheelRef;
    [SerializeField, BoxGroup("HotWheel"), Range(1f,50f)] private float _hotWheelRotationSpeed;
    [SerializeField, BoxGroup("HotWheel"), Range(0f,1f)] private float _radiusCastHotWheel;
    [SerializeField, BoxGroup("HotWheel"), Range(-1f,1f)] private float _positionYHotWheelCast;
    [SerializeField, BoxGroup("HotWheel"), Range(-1f,1f)] private float _positionXHotWheelCast;
    [SerializeField, BoxGroup("HotWheel")] private LayerMask _contactMask;
    [SerializeField, BoxGroup("HotWheel"), Layer] private int _layerAfterBreaking;

    [HorizontalLine(color:EColor.Orange)]
    [SerializeField, BoxGroup("Settings")] private LayerMask _checkToCanHit;
    [SerializeField, BoxGroup("Settings")] private SpeedEffect _speedEffect;
    [SerializeField, BoxGroup("Settings")] private Rigidbody2D _bodyRigidbody2D;
    [SerializeField, BoxGroup("Settings")] private Vector2 _centerMassOffset;
    [SerializeField, BoxGroup("Settings")] private Vector2 _centerMassAfterOnCarBrokenOffset;
    [SerializeField, BoxGroup("Settings")] private Transform _corpusTransform;
    [SerializeField, BoxGroup("Settings"), ProgressBar("Fuel", 1000, EColor.Green)] private float _currentFuelQuantity;
    [SerializeField, BoxGroup("Settings"), ProgressBar("BoosterFuel", 1000, EColor.Orange)] private float _currentBoosterFuelQuantity;
    [SerializeField, BoxGroup("Settings")] private float _currentSpeed;
    [SerializeField, BoxGroup("Settings")] bool _destructionActive = true;
    [SerializeField, BoxGroup("Settings")] bool _stopCauseHandling = true;
    [HorizontalLine(color:EColor.Blue)]
    [SerializeField, BoxGroup("CarAudio")] private CarAudio _carAudio;
    private bool _controlActive = false;
    [ShowNativeProperty] public bool ControlActive => _controlActive;

    private readonly float _defaultNormalImpulse = 1000f;
    private float _normalImpulse = 0f;
    private ContactPoint2D[] _points;
    private DestructionCar _destructionCar;
    private CarFSM _carFsm;
    private Engine _engine;
    private Transmission _transmission;
    private Exhaust _exhaust;
    private CarWheel _frontWheel;
    private CarWheel _backWheel;
    private Brakes _brakes;
    private NotificationsProvider _notificationsProvider;
    private LevelProgressCounter _levelProgressCounter;
    private PropulsionUnit _propulsionUnit;
    private CoupAnalyzer _coupAnalyzer;
    private MoveAnalyzer _moveAnalyzer;
    private HotWheel _hotWheel;
    private GroundAnalyzer _groundAnalyzer;
    private StopCauseHandler _stopCauseHandler;
    private Gyroscope _gyroscope;
    private CarMass _carMass;
    private ControlCar _controlCar;
    private SpeedEffectHandler _speedEffectHandler;
    public CarConfiguration CarConfiguration { get; private set; }
    public ControlCarUI ControlCarUI { get; private set; }
    public Speedometer Speedometer { get; private set; }
    public FuelTank FuelTank { get; private set; }
    public Booster Booster { get; private set; }
    public CarGun CarGun { get; private set; }
    public bool BoosterAvailable => CarConfiguration.BoosterCountFuelQuantity > 0f ? true : false;
    public bool GunAvailable => CarConfiguration.GunCountAmmo > 0 ? true : false;
    private ReactiveCommand _onCarBrokenIntoTwoPartsReactiveCommand = new ReactiveCommand();
    public void Construct(CarConfiguration carConfiguration, NotificationsProvider notificationsProvider,
        LevelProgressCounter levelProgressCounter, DebrisParent debrisParent,
        IGlobalAudio globalAudio, CarAudioClipProvider carAudioClipProvider, TimeScaleSignal timeScaleSignal,
        CarControlMethod carControlMethod)
    {
        CarConfiguration = carConfiguration;
        _carAudio.Construct(globalAudio, carAudioClipProvider, timeScaleSignal, _onCarBrokenIntoTwoPartsReactiveCommand);
        InitCustomizeCar();
        _bodyRigidbody2D = GetComponent<Rigidbody2D>();
        _destructionCar = GetComponent<DestructionCar>();
        _carMass = GetComponent<CarMass>();
        InitBase(_carAudio.SuspensionAudioHandler);
        _notificationsProvider = notificationsProvider;
        
        _levelProgressCounter = levelProgressCounter;
        _transmission = new Transmission(carConfiguration.GearRatio);
        Speedometer = new Speedometer(_transmission, _bodyRigidbody2D);
        _speedEffectHandler = new SpeedEffectHandler(Speedometer, _speedEffect);
        FuelTank = new FuelTank(carConfiguration.FuelQuantity, CarConfiguration.EngineOverclockingMultiplier);
        InitExhaust();
        _engine = new Engine(_engineAccelerationCurve, _carAudio.EngineAudioHandler, _exhaust, CarConfiguration.EngineOverclockingMultiplier);
        _propulsionUnit = new PropulsionUnit(_engine, _transmission, FuelTank);
        InitWheels();
        _groundAnalyzer = new GroundAnalyzer(_frontWheel, _backWheel, _onCarBrokenIntoTwoPartsReactiveCommand, _groundContactMask, _asphaltLayer, _groundLayer, _zombieBloodLayer);
        _brakes = new Brakes(_carAudio.WheelsAudioHandler.BrakeAudioHandler, Speedometer, _groundAnalyzer);
        _coupAnalyzer = new CoupAnalyzer(transform);
        _controlActive = true;
        _carAudio.SuspensionAudioHandler.Init(_groundAnalyzer, Speedometer);
        _carAudio.WheelsAudioHandler.Init(_groundAnalyzer);
        TryInitBooster();
        TryInitGun();
        TryInitHotWheel(_carAudio.HotWheelAudioHandler, debrisParent);
        InitCarFSM();
        _gyroscope = new Gyroscope(_groundAnalyzer, _bodyRigidbody2D, _carMass, _gyroscopePower);
        InitControlCar(carControlMethod);
        InitCarMass();
        TryInitDestructionCar(debrisParent);
        _moveAnalyzer = new MoveAnalyzer(Speedometer, _controlCar.DriveStarted);
        TryInitStopCauseHandler();
        _bodyRigidbody2D.centerOfMass += _centerMassOffset;
    }

    public void UpdateCar()
    {
        if (_controlActive == true)
        {
            _controlCar.Update();
            _coupAnalyzer.Update();
            _moveAnalyzer.Update();
            CarGun?.Update();
        }
        _carFsm.Update();
        if (Booster != null)
        {
            Booster.Update();
            _currentBoosterFuelQuantity = Booster.BoosterFuelTank.FuelQuantity;
        }
        _speedEffectHandler.Update();
        _groundAnalyzer.Update();
        FrontSuspension.Calculate();
        BackSuspension.Calculate();
        _currentFuelQuantity = FuelTank.FuelQuantity;
        _currentSpeed = Speedometer.CurrentSpeedInt;
        Speedometer.Update();
    }

    public void FixedUpdateCar()
    {
        if (_controlActive == true)
        {
            _hotWheel?.FixedUpdate();
        }

        _carFsm.FixedUpdate();
    }
    private void StopCarOther()
    {
        EndOfRide();
        _carFsm.SetState<StopState>();
        _carFsm.SetKeyLastState();
    }

    private void SoftStopCarOnDestinationPoint()
    {
        EndOfRide();
        _carFsm.SetState<SoftStopState>();
        _carFsm.SetKeyLastState();
    }

    private void StopCarOnFuelTankEmpty()
    {
        EndOfRide();
        _carFsm.SetState<SoftStopState>();
        _carFsm.SetKeyLastState();
    }

    private void EndOfRide()
    {
        _controlActive = false;
        _carAudio.EngineAudioHandler.PlaySoundStopEngine();
        _carAudio.HotWheelAudioHandler.StopPlaySoundRotateWheels().Forget();
        _carAudio.SuspensionAudioHandler.Dispose();
        _exhaust.StopPlayEffect();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & _checkToCanHit.value) == 1 << collision.gameObject.layer)
        {
            if (collision.gameObject.TryGetComponent(out IHitable hitable))
            {
                if (_coupAnalyzer.CarIsCoup == true)
                {
                    hitable.TryBreakOnImpact(_defaultNormalImpulse);
                }
                else
                {
                    _points = collision.contacts;
                    for (int i = 0; i < _points.Length; i++)
                    {
                        if (_normalImpulse < _points[i].normalImpulse)
                        {
                            _normalImpulse = _points[i].normalImpulse;
                        }
                    }
                    hitable.TryBreakOnImpact(_normalImpulse);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor)
        {
            if (Application.isPlaying == false)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere( _bodyRigidbody2D.centerOfMass + _centerMassOffset, 0.1f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_bodyRigidbody2D.centerOfMass + _centerMassAfterOnCarBrokenOffset, 0.1f);
                
            }else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere( transform.TransformPoint(_bodyRigidbody2D.centerOfMass), 0.1f);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_frontWheel.Position, _frontWheel.Radius);
                Gizmos.DrawWireSphere(_backWheel.Position, _backWheel.Radius);
            }

            if (_gunRef.gameObject.activeSelf == true)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_gunRef.transform.position, _distanceDetectionValue);
                Gizmos.DrawWireSphere(_gunRef.transform.position, _deadZoneDetectionValue);
            }

            if (_hotWheelRef.gameObject.activeSelf == true)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_hotWheelRef.Wheel2.position + new Vector3(_positionXHotWheelCast, _positionYHotWheelCast), _radiusCastHotWheel);
                // Gizmos.DrawWireSphere(_hotWheelRef.Wheel2.position, _radiusWheel2);
            }
        }
    }
    private void InitCustomizeCar()
    {
        CustomizeCar = GetComponent<CustomizeCar>();
        CustomizeCar.Construct(CarConfiguration);
    }

    private void InitBase(SuspensionAudioHandler suspensionAudioHandler)
    {
        SuspensionAudioHandler = suspensionAudioHandler;
        InitWheelsAndSuspension(CustomizeCar.UpgradesWheels[CarConfiguration.WheelsIndex].PartsToON);
    }
    private void TryInitStopCauseHandler()
    {
        if (_stopCauseHandling)
        {
            _stopCauseHandler = new StopCauseHandler(FuelTank,_levelProgressCounter, _destructionCar, _notificationsProvider, _moveAnalyzer,
                _coupAnalyzer, _groundAnalyzer, StopCarOther, SoftStopCarOnDestinationPoint, StopCarOnFuelTankEmpty);
        }
    }

    private void InitControlCar(CarControlMethod carControlMethod)
    {
        if (carControlMethod == CarControlMethod.KeyboardMethod)
        {
            _controlCar = new ControlCarKeyboard(new KeyboardInputMethod(BoosterAvailable, GunAvailable),
                _gyroscope, _carFsm, _propulsionUnit, Booster, CarGun, Speedometer);
        }
        else
        {
            ControlCarUI = new ControlCarUI(new UIInputMethod(), _gyroscope, _carFsm, _propulsionUnit, Booster,Speedometer);
            _controlCar = ControlCarUI;
        }
    }

    private void TryInitBooster()
    {
        if (BoosterAvailable)
        {
            Booster = new Booster(_carAudio.BoosterAudioHandler,
                new BoosterFuelTank(CarConfiguration.BoosterCountFuelQuantity),
                new BoosterScrew(_screw, _blade1, _blade2, _rotationSpeed),
                _boosterParticleSystem);
            Booster.BoosterFuelTank.OnTankEmpty += _notificationsProvider.BoosterEmpty;
            Booster.BoosterFuelTank.OnTankEmpty += Booster.StopBoosterOnOutFuel;
            Booster.OnBoosterDisable += BoosterDisable;
        }
        else
        {
            Booster = null;
        }
    }

    private void TryInitGun()
    {
        if (GunAvailable)
        {
            CarGun = new CarGun(_defaultPointAiming, _carAudio.GunAudioHandler,
                new CarGunDetector(_gunRef.transform, _targetsGunLayerMask, _distanceDetectionValue, _deadZoneDetectionValue),
                new GunGuidance(_gunTransformRotate, _speedLook), _gunParticleSystem,
                CarConfiguration.GunCountAmmo, _rateFire, _forceGun);
            CarGun.OnGunDestruct += GunDestruct;
        }
    }
    private void InitCarFSM()
    {
        Dictionary<Type, CarState> dictionaryStates = new Dictionary<Type, CarState>
        {
            {typeof(GasState), new GasState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, Booster, GetGasStateWheelGroundInteraction(), _onCarBrokenIntoTwoPartsReactiveCommand)},
            {typeof(StopState), new StopState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, _brakes, GetStopStateWheelGroundInteraction(), Booster, _onCarBrokenIntoTwoPartsReactiveCommand)},
            {typeof(RollState), new RollState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, Booster, _onCarBrokenIntoTwoPartsReactiveCommand)},
            {typeof(FlyState), new FlyState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, Booster, _bodyRigidbody2D, _onCarBrokenIntoTwoPartsReactiveCommand, _force)},
            {typeof(SoftStopState), new SoftStopState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, Booster, _onCarBrokenIntoTwoPartsReactiveCommand)}

        };
        _carFsm = new CarFSM(dictionaryStates);
        _carFsm.SetState<StopState>();
    }
    private StopStateWheelGroundInteraction GetStopStateWheelGroundInteraction()
    {
        return new StopStateWheelGroundInteraction(_groundAnalyzer, Speedometer, _frontWheel, _backWheel,
            _particlesSpeedCurveStopState, _onCarBrokenIntoTwoPartsReactiveCommand);
    }
    private GasStateWheelGroundInteraction GetGasStateWheelGroundInteraction()
    {
        return new GasStateWheelGroundInteraction(_groundAnalyzer, Speedometer, _propulsionUnit.Transmission, _frontWheel, _backWheel,
            _particlesSpeedCurveGasState, _onCarBrokenIntoTwoPartsReactiveCommand);
    }
    private void TryInitDestructionCar(DebrisParent debrisParent)
    {
        if (_destructionActive == true)
        {
            _destructionCar.Construct(_carAudio.DestructionAudioHandler, _exhaust, CarGun, _hotWheel ,_carMass, Booster, Speedometer, _coupAnalyzer, _hotWheelRef,
                _boosterRef, _gunRef,  debrisParent);
            if (_destructionCar.BottomDestructionOn == true)
            {
                _destructionCar.OnCarBrokenIntoTwoParts += CarBrokenIntoTwoParts;
            }
        }
    }

    private void InitCarMass()
    {
        _carMass.Construct(_boosterRef.transform, _gunRef.transform, _hotWheelRef.transform, Booster, FuelTank);
    }

    private void InitWheels()
    {
        _frontWheel = new CarWheel(FrontWheelJoint, FrontWheelCarValues.WheelRigidbody2D, _corpusTransform, 
            FrontWheelCarValues.SmokeWheelParticleSystem, FrontWheelCarValues.DirtWheelParticleSystem, FrontWheelCarValues.BloodWheelParticleSystem);
        _backWheel = new CarWheel(BackWheelJoint, BackWheelCarValues.WheelRigidbody2D, _corpusTransform,
            BackWheelCarValues.SmokeWheelParticleSystem, BackWheelCarValues.DirtWheelParticleSystem, BackWheelCarValues.BloodWheelParticleSystem);
    }

    private void InitExhaust()
    {
        _exhaust = new Exhaust(_exhaustParticleSystem);
        FuelTank.OnTankEmpty += _exhaust.StopPlayEffect;
    }
    private void TryInitHotWheel(HotWheelAudioHandler hotWheelAudioHandler, DebrisParent debrisParent)
    {
        if (_hotWheelRef.gameObject.activeSelf == true)
        {
            _hotWheel = new HotWheel(_hotWheelRef, hotWheelAudioHandler, debrisParent, _contactMask, new Vector3(_positionXHotWheelCast, _positionYHotWheelCast),
                _layerAfterBreaking, _hotWheelRotationSpeed, _radiusCastHotWheel);
        } 
    }

    private void GunDestruct()
    {
        CarGun.OnGunDestruct -= GunDestruct;
        _controlCar.TryTurnOffCheckGun();
        CarGun = null;
    }

    private void BoosterDisable()
    {
        Booster.OnBoosterDisable -= BoosterDisable;
        Booster.BoosterFuelTank.OnTankEmpty -= Booster.StopBoosterOnOutFuel;
        Booster.BoosterFuelTank.OnTankEmpty -= _notificationsProvider.BoosterEmpty;
        _controlCar.TryTurnOffCheckBooster();
        Booster = null;
    }
    private void CarBrokenIntoTwoParts(WheelJoint2D joint2D, WheelCarValues wheelCarValues)
    {
        ReinitBackWheelAndSuspensionOnCarBrokenIntoTwoParts(joint2D, wheelCarValues);
        _controlCar.TryTurnOffCheckBooster();
        _onCarBrokenIntoTwoPartsReactiveCommand.Execute();
        _bodyRigidbody2D.centerOfMass += _centerMassAfterOnCarBrokenOffset;
    }
    private void OnDisable()
    {
        FuelTank.OnTankEmpty -= _exhaust.StopPlayEffect;
        _stopCauseHandler.Dispose();
        _coupAnalyzer.Dispose();
        _groundAnalyzer.Dispose();
        _onCarBrokenIntoTwoPartsReactiveCommand.Dispose();
        if (_controlCar.DriveStarted != null)
        {
            _controlCar.DriveStarted.Dispose();
        }

        _carFsm.Dispose();
        _carAudio.Dispose();
        if (Booster != null)
        {
            BoosterDisable();
        }
        if (CarGun != null)
        {
            GunDestruct();
        }

        if (_hotWheel != null)
        {
            _hotWheel.Dispose();
        }

        if (_destructionActive == true && _destructionCar.BottomDestructionOn == true)
        {
            _destructionCar.OnCarBrokenIntoTwoParts -= CarBrokenIntoTwoParts;
        }
    }
}
