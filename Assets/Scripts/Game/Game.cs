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
    // [SerializeField] private Pool _pool;
    [SerializeField] private GameObject _buildingsParent;
    [Space]
    [SerializeField, BoxGroup("Level Settings"), HorizontalLine(color:EColor.White)] private Level _level;
    [SerializeField, BoxGroup("Level Settings")] private Transform _startLevelPoint;
    [SerializeField, BoxGroup("Level Settings")] private Transform _endLevelPoint;
    [SerializeField, BoxGroup("Level Settings"), HorizontalLine(color:EColor.White)] private float _timeWaitingOnEndLevel = 1f;
    [SerializeField, BoxGroup("Level Settings"), HorizontalLine(color:EColor.White)] private SliderSectionValues _sliderSectionValue;
    // private Bot[] _botsWithAK;
    // private Tower[] _towers;
    [Inject] private SaveService _saveService;
    [Inject] private PlayerDataProvider _playerDataProvider;
    [Inject] private GlobalAudio _globalAudio;
    [Inject] private GameData _gameData;
    [Inject] private GarageData _garageData;
    [Inject] private IInstantiator _instantiator;
    [Inject] private LevelPrefabsProvider _levelPrefabsProvider;
    [Inject] private CarAudioHandler _carAudioHandler;

    private Factory _factory;
    private CarInLevel _carInLevel;
    private PresenterUILevel _presenterUILevel;
    private GamePause _gamePause;
    private KillsCount _killsCount;
    private ResultsLevelProvider _resultsLevelProvider;
    private LevelProgressCounter _levelProgressCounter;
    private PlayerDataHandler _playerDataHandler;
    private NotificationsProvider _notificationsProvider;
    private CarConfigurationProvider _carConfigurationProvider;
    private ViewUILevel _viewUILevel;
    private CarConfiguration _carConfiguration;
    
    private void Construct()
    {
        // _cinemachineVirtualCamera.
        // _pool.Init(_spawner);
        // for (int i = 0; i < _botsWithAK.Length; i++)
        // {
        //     _botsWithAK[i].Init(_car.transform, _pool.BulletAKReservoir, _killsCounter);
        // }
        // _towers = _buildingsParent.GetComponentsInChildren<Tower>();
        // for (int i = 0; i < _towers.Length; i++)
        // {
        //     _towers[i].Init(_car.transform, _pool.BulletAKReservoir, _spawner, _killsCounter);
        // }
    }

    private void Awake()
    {
        _playerDataHandler = _playerDataProvider.PlayerDataHandler;
        _factory = new Factory(_instantiator);
        _notificationsProvider = new NotificationsProvider();
        InitCar();
        InitViewUILevel();
        _gamePause = new GamePause(_globalAudio);
        _killsCount = new KillsCount();
        _resultsLevelProvider = new ResultsLevelProvider(_playerDataHandler, _carConfiguration, _killsCount,
            _levelProgressCounter, _notificationsProvider, _timeWaitingOnEndLevel);
        _presenterUILevel = new PresenterUILevel(_viewUILevel, _carInLevel, _gamePause, _resultsLevelProvider,
            new SceneSwitch(_playerDataHandler, _gameData), _globalAudio, _gameData.CarControlMethod);
        _notificationsProvider.ShowDayInfo(_playerDataHandler.PlayerData.Days.ToString());
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
        _carInLevel.Construct(_carConfiguration, _notificationsProvider, _carAudioHandler, _levelProgressCounter, _level.DebrisParent, _gameData.CarControlMethod);
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
        if (_gamePause.CheckPause())
        {

            // for (int i = 0; i < _botsWithAK.Length; i++)
            // {
            //     _botsWithAK[i].UpdateBot();
            // }
            // _pool.UpdatePool();
            // if (_towers.Length != 0)
            // {
            //     for (int i = 0; i < _towers.Length; i++)
            //     {
            //         _towers[i]?.UpdateTower();
            //     }
            // }
            _resultsLevelProvider.LevelProgressCounter.CalculateProgress();
        }
    }
    private void FixedUpdate()
    {
        if (_gamePause.CheckPause())
        {
            _carInLevel.UpdateCar();
        }
    }
    private void OnApplicationQuit()
    {
        _saveService.SetPlayerDataToSaving(_playerDataHandler.PlayerData);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause is true)
        {
            _saveService.SetPlayerDataToSaving(_playerDataHandler.PlayerData);
        }
    }
}