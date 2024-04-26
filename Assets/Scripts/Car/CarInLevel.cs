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

    [SerializeField, Range(0f, 1f), BoxGroup("Gyroscope"), HorizontalLine(color:EColor.Green)] private float _gyroscopePower;

    [SerializeField, BoxGroup("WheelGroundInteraction"), HorizontalLine(color:EColor.Yellow)] private LayerMask _groundsLayerMask;
    [SerializeField, BoxGroup("WheelGroundInteraction"), Layer] private int _asphaltLayer;
    [SerializeField, BoxGroup("WheelGroundInteraction"), Layer] private int _groundLayer;
    [SerializeField, BoxGroup("WheelGroundInteraction")] private AnimationCurve _brakeVolumeCurve;
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
    [SerializeField, BoxGroup("Booster")] private AnimationCurve _increaseBoosterSoundCurve;
    [SerializeField, BoxGroup("Booster")] private AnimationCurve _decreaseBoosterSoundCurve;

    [SerializeField, BoxGroup("HotWheel"), HorizontalLine(color:EColor.Red)] private HotWheelRef _hotWheelRef;
    [SerializeField, BoxGroup("HotWheel"), Range(1f,50f)] private float _hotWheelRotationSpeed;
    [SerializeField, BoxGroup("HotWheel"), Range(0f,1f)] private float _radiusWheel1;
    [SerializeField, BoxGroup("HotWheel"), Range(0f,1f)] private float _radiusWheel2;
    [SerializeField, BoxGroup("HotWheel")] private LayerMask _contactMask;
    [SerializeField, BoxGroup("HotWheel"), Layer] private int _layerAfterBreaking;

    [HorizontalLine(color:EColor.Orange)]
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

    private DestructionCar _destructionCar;
    private CarFSM _carFsm;
    private Engine _engine;
    private Transmission _transmission;
    private Exhaust _exhaust;
    // private CarAudioInitializer _carAudioInitializer;
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
    public CarConfiguration CarConfiguration { get; private set; }
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
    private ReactiveCommand _onCarBrokenIntoTwoPartsReactiveCommand = new ReactiveCommand();
    public void Construct(CarConfiguration carConfiguration, NotificationsProvider notificationsProvider,
        LevelProgressCounter levelProgressCounter, Transform debrisParent,
        IGlobalAudio globalAudio, CarAudioClipProvider carAudioClipProvider,
        
        CarControlMethod carControlMethod)
    {
        CarConfiguration = carConfiguration;
        _carAudio.Construct(globalAudio, carAudioClipProvider);
        InitCustomizeCar();
        _bodyRigidbody2D = GetComponent<Rigidbody2D>();
        _destructionCar = GetComponent<DestructionCar>();
        CarMass = GetComponent<CarMass>();
        InitBase(_carAudio.SuspensionAudioHandler);
        _notificationsProvider = notificationsProvider;
        
        _levelProgressCounter = levelProgressCounter;
        _transmission = new Transmission(carConfiguration.GearRatio);
        Speedometer = new Speedometer(_transmission, _bodyRigidbody2D);
        FuelTank = new FuelTank(carConfiguration.FuelQuantity, CarConfiguration.EngineOverclockingMultiplier);
        InitExhaust();

        _engine = new Engine(_engineAccelerationCurve, _carAudio.EngineAudioHandler, _exhaust, CarConfiguration.EngineOverclockingMultiplier);
        _propulsionUnit = new PropulsionUnit(_engine, _transmission, FuelTank);
        InitWheels();
        _groundAnalyzer = new GroundAnalyzer(_frontWheel, _backWheel, _onCarBrokenIntoTwoPartsReactiveCommand, _groundsLayerMask, _asphaltLayer, _groundLayer);
        _brakes = new Brakes(_carAudio.BrakeAudioHandler, Speedometer, _groundAnalyzer, _brakeVolumeCurve);
        _coupAnalyzer = new CoupAnalyzer(transform);
        _carAudio.SuspensionAudioHandler.Init(_groundAnalyzer, Speedometer).Forget();
        _controlActive = true;
        TryInitBooster();
        TryInitGun();
        TryInitHotWheel(_carAudio.HotWheelAudioHandler, debrisParent);
        InitCarFSM();
        Gyroscope = new Gyroscope(_groundAnalyzer, _bodyRigidbody2D, CarMass, _gyroscopePower);
        InitControlCar(carControlMethod);
        InitCarMass();
        TryInitDestructionCar(debrisParent);
        SubscribeActions();
        _moveAnalyzer = new MoveAnalyzer(Speedometer, ControlCar.DriveStarted);
        TryInitStopCauseHandler();
        _bodyRigidbody2D.centerOfMass += _centerMassOffset;
        _carAudio.EngineAudioHandler.PlayStartEngine();

    }

    public void UpdateCar()
    {
        if (_controlActive == true)
        {
            ControlCar.Update();
            _coupAnalyzer.Update();
            _moveAnalyzer.Update();
            _hotWheel?.Update();
            CarGun?.Update();
        }
        _carFsm.Update();
        if (Booster != null)
        {
            _currentBoosterFuelQuantity = Booster.BoosterFuelTank.FuelQuantity;
        }
        _groundAnalyzer.Update();
        FrontSuspension.Calculate();
        BackSuspension.Calculate();
        _currentFuelQuantity = FuelTank.FuelQuantity;
        _currentSpeed = Speedometer.CurrentSpeedInt;
        Speedometer.Update();
    }

    private void StopCarOther()
    {
        EndOfRide();
        _carFsm.SetState<StopState>();
    }

    private void SoftStopCarOnDestinationPoint()
    {
        EndOfRide();
        _carFsm.SetState<SoftStopState>();
    }

    private void StopCarOnFuelTankEmpty()
    {
        EndOfRide();
        _carFsm.SetState<RollState>();
    }

    private void EndOfRide()
    {
        _controlActive = false;
        _carAudio.EngineAudioHandler.StopPlayEngine();
        _carAudio.EngineAudioHandler.PlaySoundStopEngine();
        _carAudio.HotWheelAudioHandler.StopPlaySoundRotateWheels().Forget();
        _exhaust.StopPlayEffect();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryKnock(collision);
    }

    private void TryKnock(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IHitable hitable))
        {
            var points = collision.contacts;
            float normalImpulse = 0f;
            for (int i = 0; i < points.Length; i++)
            {
                if (normalImpulse < points[i].normalImpulse)
                {
                    normalImpulse = points[i].normalImpulse;
                }
            }
            hitable.TryBreakOnImpact(normalImpulse);
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
                Gizmos.DrawWireSphere(_hotWheelRef.Wheel1.position, _radiusWheel1);
                Gizmos.DrawWireSphere(_hotWheelRef.Wheel2.position, _radiusWheel2);
            }
        }
    }

    private void SubscribeActions()
    {
        // FuelTank.OnTankEmpty += _notificationsProvider.FuelTankEmpty;
        // FuelTank.OnTankEmpty += EndOfRide;
        // _levelProgressCounter.OnGotPointDestination += EndOfRide;
        // _coupAnalyzer.OnCarCouped += _notificationsProvider.CarTurnOver;
        // _coupAnalyzer.OnCarCouped +=EndOfRide;
        // if (_destructionCar.FrontWingDestructionHandler != null)
        // {
        //     _destructionCar.FrontWingDestructionHandler.OnEngineBroken += EndOfRide;
        //     _destructionCar.FrontWingDestructionHandler.OnEngineBroken += _notificationsProvider.EngineBroken;
        // }
        // if (_destructionCar.CabineDestructionHandler != null)
        // {
        //     _destructionCar.CabineDestructionHandler.OnDriverCrushed += EndOfRide;
        //     _destructionCar.CabineDestructionHandler.OnDriverCrushed += _notificationsProvider.DriverCrushed;
        // }
        
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
    private void TryInitDestructionCar(Transform debrisParent)
    {
        if (_destructionActive == true)
        {
            _destructionCar.Construct(_carAudio.DestructionAudioHandler, _exhaust, CarGun, _hotWheel ,CarMass, Booster, Speedometer, _coupAnalyzer, _hotWheelRef,
                _boosterRef, _gunRef,  debrisParent);
            if (_destructionCar.BottomDestructionOn == true)
            {
                _destructionCar.OnCarBrokenIntoTwoParts += CarBrokenIntoTwoParts;
            }
        }
    }

    private void InitCarMass()
    {
        CarMass.Construct(_boosterRef.transform, _gunRef.transform, _hotWheelRef.transform, Booster, FuelTank);
    }

    private void InitWheels()
    {
        _frontWheel = new CarWheel(FrontWheelJoint, FrontWheelCarValues.WheelRigidbody2D, _corpusTransform, 
            FrontWheelCarValues.SmokeWheelParticleSystem, FrontWheelCarValues.DirtWheelParticleSystem);
        _backWheel = new CarWheel(BackWheelJoint, BackWheelCarValues.WheelRigidbody2D, _corpusTransform,
            BackWheelCarValues.SmokeWheelParticleSystem, BackWheelCarValues.DirtWheelParticleSystem);
    }

    private void InitExhaust()
    {
        _exhaust = new Exhaust(_exhaustParticleSystem);
        FuelTank.OnTankEmpty += _exhaust.StopPlayEffect;
    }
    private void TryInitHotWheel(HotWheelAudioHandler hotWheelAudioHandler, Transform debrisParent)
    {
        if (_hotWheelRef.gameObject.activeSelf == true)
        {
            _hotWheel = new HotWheel(_hotWheelRef, hotWheelAudioHandler, debrisParent, _contactMask, _layerAfterBreaking, _hotWheelRotationSpeed, _radiusWheel1, _radiusWheel2);
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
    private void CarBrokenIntoTwoParts(WheelJoint2D joint2D, WheelCarValues wheelCarValues)
    {
        ReinitBackWheelAndSuspensionOnCarBrokenIntoTwoParts(joint2D, wheelCarValues);
        ControlCar.TryTurnOffCheckBooster();
        _onCarBrokenIntoTwoPartsReactiveCommand.Execute();
        _bodyRigidbody2D.centerOfMass += _centerMassAfterOnCarBrokenOffset;
    }
    private void OnDisable()
    {
        FuelTank.OnTankEmpty -= _exhaust.StopPlayEffect;
        _stopCauseHandler.Dispose();
        _coupAnalyzer.Dispose();
        _onCarBrokenIntoTwoPartsReactiveCommand.Dispose();
        ControlCar.DriveStarted.Dispose();
        _carFsm.Dispose();
        // base.Dispose();
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
