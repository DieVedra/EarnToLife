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

    [SerializeField, Range(0f, 10f), BoxGroup("Gyroscope"), HorizontalLine(color:EColor.Green)] private float _gyroscopePower;

    [SerializeField, BoxGroup("Brake"), HorizontalLine(color:EColor.Yellow)] private LayerMask _ground;
    [SerializeField, BoxGroup("Brake")] private AnimationCurve _brakeVolumeCurve;

    [SerializeField, BoxGroup("Gun"), HorizontalLine(color:EColor.Red)] private GunRef _gunRef;
    [SerializeField, BoxGroup("Gun")] private Transform _gunTransformRotate;
    [SerializeField, BoxGroup("Gun")] private Transform _defaultPointAiming;
    [SerializeField, BoxGroup("Gun"), Range(0f,5f)] private float _speedLook = 2f;
    [SerializeField, BoxGroup("Gun"), Range(0f,15f)] private float _distanceDetectionValue = 6f;
    [SerializeField, BoxGroup("Gun"), Range(0f,15f)] private float _deadZoneDetectionValue = 2f;
    [SerializeField, BoxGroup("Gun")] private LayerMask _targetsGunLayerMask;
    [SerializeField, BoxGroup("Gun")] private float _rateFire = 1f;
    [SerializeField, BoxGroup("Gun")] private float _forceGun = 100f;

    [SerializeField, BoxGroup("Booster"), HorizontalLine(color:EColor.Green)] private BoosterRef _boosterRef;
    [SerializeField, BoxGroup("Booster"), Range(1f,50f)] private float _rotationSpeed;
    [SerializeField, BoxGroup("Booster"), Range(50f,1000f)] private float _force;
    [SerializeField, BoxGroup("Booster")] private Transform _screw;
    [SerializeField, BoxGroup("Booster")] private SpriteRenderer _blade1;
    [SerializeField, BoxGroup("Booster")] private SpriteRenderer _blade2;

    [SerializeField, BoxGroup("HotWheel"), HorizontalLine(color:EColor.Red)] private HotWheelRef _hotWheelRef;
    [SerializeField, BoxGroup("HotWheel"), Range(1f,50f)] private float _hotWheelRotationSpeed;

    [HorizontalLine(color:EColor.Orange)]
    [SerializeField, BoxGroup("Settings")] private Transform _corpusTransform;
    [SerializeField, BoxGroup("Settings"), ProgressBar("Fuel", 1000, EColor.Green)] private float _currentFuelQuantity;
    [SerializeField, BoxGroup("Settings"), ProgressBar("BoosterFuel", 1000, EColor.Orange)] private float _currentBoosterFuelQuantity;
    [SerializeField, BoxGroup("Settings")] private float _currentSpeed;
    [SerializeField, BoxGroup("Settings")] bool _destructionActive = true;
    private bool _controlActive = false;
    [ShowNativeProperty] public bool ControlActive => _controlActive;

    private DestructionCar _destructionCar;
    private Rigidbody2D _bodyRigidbody2D;
    private CarFSM _carFsm;
    private Engine _engine;
    private CarAudioHandler _carAudioHandler;
    private CarWheel _frontWheel;
    private CarWheel _backWheel;
    private Brakes _brakes;
    private NotificationsProvider _notificationsProvider;
    private LevelProgressCounter _levelProgressCounter;
    private PropulsionUnit _propulsionUnit;
    private CoupAnalyzer _coupAnalyzer;
    private HotWheel _hotWheel;
    public CarConfiguration CarConfiguration { get; private set; }
    // public CompositeDisposable CompositeDisposable { get; private set; } = new CompositeDisposable();
    public CarMass CarMass { get; private set; }

    public ControlCar ControlCar { get; private set; }
    public ControlCarUI ControlCarUI { get; private set; }
    public Speedometer Speedometer { get; private set; }
    public FuelTank FuelTank { get; private set; }
    public Gyroscope Gyroscope { get; private set; }
    public Booster Booster { get; private set; }
    public CarGun CarGun { get; private set; }
    public bool BoosterAvailable => CarConfiguration.BoosterCountFuelQuantity > 0f ? true : false;
    public bool GunAvailable => CarConfiguration.GunCountAmmo > 0 ? true : false;
    public void Construct(CarConfiguration carConfiguration, NotificationsProvider notificationsProvider,
        CarAudioHandler carAudioHandler, LevelProgressCounter levelProgressCounter, Transform debrisParent, CarControlMethod carControlMethod)
    {
        CustomizeCar = GetComponent<CustomizeCar>();
        _bodyRigidbody2D = GetComponent<Rigidbody2D>();
        _destructionCar = GetComponent<DestructionCar>();
        CarMass = GetComponent<CarMass>();
        CarConfiguration = carConfiguration;
        CustomizeCar.Construct(carConfiguration);
        _notificationsProvider = notificationsProvider;
        _carAudioHandler = carAudioHandler;
        _levelProgressCounter = levelProgressCounter;
        _coupAnalyzer = new CoupAnalyzer(transform);
        FuelTank = new FuelTank(carConfiguration.FuelQuantity, CarConfiguration.EngineOverclockingMultiplier);
        Speedometer = new Speedometer(_bodyRigidbody2D);
        _engine = new Engine(_engineAccelerationCurve, _carAudioHandler, CarConfiguration.EngineOverclockingMultiplier);
        _propulsionUnit = new PropulsionUnit(_engine, FuelTank, carConfiguration.GearRatio);
        _frontWheel = new CarWheel(FrontWheelJoint, FrontWheelCarValues.WheelRigidbody2D, _corpusTransform);
        _backWheel = new CarWheel(BackWheelJoint, BackWheelCarValues.WheelRigidbody2D, _corpusTransform);
        Gyroscope = new Gyroscope(_bodyRigidbody2D, _gyroscopePower);
        _brakes = new Brakes(_carAudioHandler, Speedometer, _frontWheel, _backWheel, _ground, _brakeVolumeCurve);

        _controlActive = true;
        SubscribeActions();
        TryInitBooster();
        TryInitGun();
        TryInitHotWheel();
        InitCarFSM();
        InitControlCar(carControlMethod);
        InitCarMass();
        TryInitDestructionCar(debrisParent);
        _carAudioHandler.PlayStartEngine();
    }
    public void UpdateCar()
    {
        if (_controlActive == true)
        {
            ControlCar.Update();
            _carFsm.Update();
            _coupAnalyzer.Update();
        }
        else
        {
            _carFsm.SetState<StopState>();
        }
        if (Booster != null)
        {
            _currentBoosterFuelQuantity = Booster.BoosterFuelTank.FuelQuantity;
        }
        CarGun?.Update();
        // Debug.Log(Speedometer.CurrentSpeedInt);
        FrontSuspension.Calculate();
        BackSuspension.Calculate();
        _currentFuelQuantity = FuelTank.FuelQuantity;
        // FuelTank.Update();
        _currentSpeed = Speedometer.CurrentSpeedInt;
        Speedometer.Update();
    }
    private void StopCar()
    {
        _controlActive = false;
        _carAudioHandler.StopPlayEngine();
        _carAudioHandler.PlaySoundStopEngine();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IKnockable knockable))
        {
            knockable.Destruct(Speedometer.CurrentSpeedFloat * CarMass.Mass);
        }
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(_backWheel.Position, _backWheel.Radius);
            Gizmos.DrawWireSphere(_frontWheel.Position, _frontWheel.Radius);
        }
        if (_gunRef.gameObject.activeSelf == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_gunRef.transform.position, _distanceDetectionValue);
            Gizmos.DrawWireSphere(_gunRef.transform.position, _deadZoneDetectionValue);
        }
    }

    private void SubscribeActions()
    {
        FuelTank.OnTankEmpty += _notificationsProvider.FuelTankEmpty;
        FuelTank.OnTankEmpty += StopCar;
        _levelProgressCounter.OnGotPointDestination += StopCar;
    }

    private void InitControlCar(CarControlMethod carControlMethod)
    {
        if (carControlMethod == CarControlMethod.KeyboardMethod)
        {
            ControlCar = new ControlCarKeyboard(new KeyboardInputMethod(BoosterAvailable, GunAvailable),
                Gyroscope, _carFsm, _propulsionUnit, Booster, CarGun, Speedometer);
        }
        else
        {
            ControlCarUI = new ControlCarUI(new UIInputMethod(), Gyroscope, _carFsm, _propulsionUnit, Booster,Speedometer);
            ControlCar = ControlCarUI;
        }
    }

    private void TryInitBooster()
    {
        if (BoosterAvailable)
        {
            Booster = new Booster(_carAudioHandler,
                new BoosterFuelTank(CarConfiguration.BoosterCountFuelQuantity),
                new BoosterScrew(_screw, _blade1, _blade2, _rotationSpeed)/*,
                _customizeCar.BoosterSprite.GetComponentInChildren<ParticleSystem>()*/
                );
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
            CarGun = new CarGun(_defaultPointAiming, _carAudioHandler,
                new CarGunDetector(_gunRef.transform, _targetsGunLayerMask, _distanceDetectionValue, _deadZoneDetectionValue),
                new GunGuidance(_gunTransformRotate, _speedLook),
                CarConfiguration.GunCountAmmo, _rateFire, _forceGun);
            CarGun.OnGunDestruct += GunDestruct;
        }
    }

    private void InitCarFSM()
    {
        Dictionary<Type, CarState> dictionaryStates = new Dictionary<Type, CarState>
        {
            {typeof(GasState), new GasState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, Booster)},
            {typeof(StopState), new StopState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, _brakes, Booster)},
            {typeof(RollState), new RollState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, Booster)},
            {typeof(FlyState), new FlyState(FrontWheelJoint, BackWheelJoint, _propulsionUnit, Booster, _bodyRigidbody2D, _force)}
        };
        _carFsm = new CarFSM(dictionaryStates);
        _carFsm.SetState<StopState>();
    }

    private void TryInitDestructionCar(Transform debrisParent)
    {
        if (_destructionActive == true)
        {
            _destructionCar.Construct(CarGun, _hotWheel ,CarMass, Booster, Speedometer, _coupAnalyzer, _hotWheelRef,
                _boosterRef, _gunRef,  debrisParent);
        }
    }

    private void InitCarMass()
    {
        CarMass.Construct(_boosterRef.transform, _gunRef.transform, _hotWheelRef.transform, Booster, FuelTank);
    }

    private void TryInitHotWheel()
    {
        if (_hotWheelRef.gameObject.activeSelf == true)
        {
            _hotWheel = new HotWheel(_hotWheelRef, _hotWheelRotationSpeed);
        } 
    }

    private void GunDestruct()
    {
        CarGun.OnGunDestruct -= GunDestruct;
        ControlCar.TryTurnOffCheckGun();
        CarGun = null;
    }

    private void BoosterDisable()
    {
        Booster.OnBoosterDisable -= BoosterDisable;
        Booster.BoosterFuelTank.OnTankEmpty -= Booster.StopBoosterOnOutFuel;
        Booster.BoosterFuelTank.OnTankEmpty -= _notificationsProvider.BoosterEmpty;
        ControlCar.TryTurnOffCheckBooster();
        Booster = null;
    }

    private void OnDisable()
    {
        FuelTank.OnTankEmpty -= _notificationsProvider.FuelTankEmpty;
        FuelTank.OnTankEmpty -= StopCar;
        _levelProgressCounter.OnGotPointDestination -= StopCar;
        _coupAnalyzer.Dispose();
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
    }
}
