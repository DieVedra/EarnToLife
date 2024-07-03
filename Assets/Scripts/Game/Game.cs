using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using NaughtyAttributes;
using Zenject;

public class Game : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private LimitRideBack _limitRideBack;
    [Space]
    [SerializeField, BoxGroup("Level Settings"), HorizontalLine(color:EColor.White)] private Transform _startLevelPoint;
    [SerializeField, BoxGroup("Level Settings")] private Transform _endLevelPoint;
    [SerializeField, BoxGroup("Level Settings"), HorizontalLine(color:EColor.White)] private float _timeWaitingOnEndLevel = 1f;
    [SerializeField, BoxGroup("Level Settings"), HorizontalLine(color:EColor.White)] private SliderSectionValues _sliderSectionValue;
    [SerializeField, BoxGroup("Level Settings"), HorizontalLine(color:EColor.White)] private bool _limitRideBackIsOn;
    [Inject] private SaveService _saveService;
    [Inject] private PlayerDataProvider _playerDataProvider;
    [Inject] private GlobalAudio _globalAudio;
    [Inject] private IGlobalAudio _globalAudioForCar;
    [Inject] private GameData _gameData;
    [Inject] private GarageData _garageData;
    // [Inject] private IInstantiator _instantiator;
    [Inject] private LevelPrefabsProvider _levelPrefabsProvider;
    [Inject] private AudioClipProvider _audioClipProvider;
    [Inject] private GamePause _gamePause;
    [Inject] private GameOverSignal _gameOverSignal;
    [Inject] private ILevel _level;
    [Inject] private NotificationsProvider _notificationsProvider;
    [Inject] private TimeScaler _timeScaler;
    [Inject] private ExplodeSignal _explodeSignal;
    [Inject] private DestructionsSignal _destructionsSignal;
    [Inject] private KillsSignal _killsSignal;
    [Inject] private TimeScaleSignal _timeScaleSignal;
    

    private Factory _factory;
    private CarInLevel _carInLevel;
    private PresenterUILevel _presenterUILevel;
    private KillsCount _killsCount;
    private DestructionCount _destructionCount;
    private ResultsLevelProvider _resultsLevelProvider;
    private LevelProgressCounter _levelProgressCounter;
    private PlayerDataHandler _playerDataHandler;
    private CarConfigurationProvider _carConfigurationProvider;
    private ViewUILevel _viewUILevel;
    private CarConfiguration _carConfiguration;
    private ActionAnalyzer _actionAnalyzer;
    private void Awake()
    {
        _playerDataHandler = _playerDataProvider.PlayerDataHandler;
        _factory = new Factory();
        _actionAnalyzer = new ActionAnalyzer(_timeScaler, _notificationsProvider, _explodeSignal, _destructionsSignal, _killsSignal);
        InitCar();
        _limitRideBack.Init(_limitRideBackIsOn);
        InitViewUILevel();
        _killsCount = new KillsCount(_killsSignal);
        _destructionCount = new DestructionCount(_destructionsSignal);
        _resultsLevelProvider = new ResultsLevelProvider(_playerDataHandler, _carConfiguration, _killsCount, _destructionCount,
            _levelProgressCounter, _notificationsProvider, _gameOverSignal, _timeWaitingOnEndLevel);
        _presenterUILevel = new PresenterUILevel(_viewUILevel, _carInLevel, _gamePause, _resultsLevelProvider,
            new SceneSwitch(_playerDataHandler, _gameData), _globalAudio, _gameData.CarControlMethod);
        _notificationsProvider.ShowDayInfo(_playerDataHandler.PlayerData.Days.ToString(), true);
    }
    private void InitCar()
    {
        int currentSelectLotCarIndex = _playerDataHandler.GetCurrentSelectLotCarIndex();
        _carConfigurationProvider = new CarConfigurationProvider();
        InitCarConfiguration(currentSelectLotCarIndex);
        _carInLevel = _factory.CreateCar(
            _levelPrefabsProvider.CarsInLevelPrefabs[currentSelectLotCarIndex],
            _startLevelPoint
            );
        _cinemachineVirtualCamera.Follow = _carInLevel.transform;
        InitProgressCounter();
        _carInLevel.Construct(_carConfiguration, _notificationsProvider, _levelProgressCounter,
            _level.DebrisParent, _globalAudioForCar, _audioClipProvider.CarsAudioClipsProvider.GetCarAudioClipProvider(currentSelectLotCarIndex),
            _timeScaleSignal, _gameData.CarControlMethod);
    }

    private void InitProgressCounter()
    {
        _levelProgressCounter = new LevelProgressCounter(
            _startLevelPoint, _endLevelPoint, _carInLevel.transform,
            _notificationsProvider,
            _sliderSectionValue,
            _playerDataHandler.TryGetResultLevelForSlider()
        );
    }

    private void InitCarConfiguration(int currentSelectLotCarIndex)
    {
        _carConfiguration = _carConfigurationProvider.GetCarConfiguration( _garageData.ParkingLotsConfigurations,
            _playerDataHandler.GetCurrentCarConfigurationInParkingLot(),
            currentSelectLotCarIndex);
    }

    private void InitViewUILevel()
    {
        _viewUILevel = _factory.CreateCanvas(_levelPrefabsProvider.ViewUILevelPrefab);
        _viewUILevel.GetComponent<Canvas>().worldCamera = Camera.main;
    }
    private void Update()
    {
        if (_gamePause.IsPause == false)
        {
            _carInLevel.UpdateCar();

            
            _resultsLevelProvider.LevelProgressCounter.CalculateProgress();
        }
    }
    private void FixedUpdate()
    {
        if (_gamePause.IsPause == false)
        {
            _carInLevel.FixedUpdateCar();
        }
    }
    private void OnApplicationQuit()
    {
        // Debug.Log($"OnApplicationQuit Game    SaveOn: {_gameData.SaveOn}");
        _saveService.SetPlayerDataToSaving(_playerDataHandler.PlayerData, _gameData.SaveOn);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause is true)
        {
            _saveService.SetPlayerDataToSaving(_playerDataHandler.PlayerData, _gameData.SaveOn);
        }
    }

    private void OnDestroy()
    {
        _resultsLevelProvider.Dispose();
        _globalAudio.DisposeAndReInit();
        _killsCount.Dispose();
        _destructionCount.Dispose();
        _actionAnalyzer.Dispose();
    }
}