using Cinemachine;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using Zenject;

public class Game : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private LimitRideBack _limitRideBack;
    
    [SerializeField, BoxGroup("Game Settings"), HorizontalLine(color:EColor.White)] private float _timeWaitingOnEndLevel = 1f;

    [SerializeField, BoxGroup("Game Settings"), HorizontalLine(color:EColor.White)] private SliderSectionValues _sliderSectionValue;
    
    [SerializeField, BoxGroup("Game Settings"), HorizontalLine(color:EColor.White)] private bool _limitRideBackIsOn;
    [SerializeField, BoxGroup("Game Settings")] private bool _autoGameOverIsOn;
    [SerializeField, BoxGroup("Game Settings")]private bool _timeScaleIsOn;

    
    [Inject] private SaveService _saveService;
    [Inject] private PlayerDataProvider _playerDataProvider;
    [Inject] private GlobalAudio _globalAudio;
    [Inject] private IGlobalAudio _globalAudioForCar;
    [Inject] private GameData _gameData;
    [Inject] private GarageData _garageData;
    [Inject] private LevelPrefabsProvider _levelPrefabsProvider;
    [Inject] private AudioClipProvider _audioClipProvider;
    [Inject] private GamePause _gamePause;
    [Inject] private ILevel _level;
    [Inject] private NotificationsProvider _notificationsProvider;
    [Inject] private TimeScaler _timeScaler;
    [Inject] private ExplodeSignal _explodeSignal;
    [Inject] private DestructionsSignal _destructionsSignal;
    [Inject] private KillsSignal _killsSignal;
    [Inject] private TimeScaleSignal _timeScaleSignal;
    [Inject] private GameOverSignal _gameOverSignal;


    private Spawner _spawner;
    private CarInLevel _carInLevel;
    private LogicUILevel _logicUILevel;
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
        if (_playerDataProvider != null)
        {
            _playerDataHandler = _playerDataProvider.PlayerDataHandler;
        }
        _spawner = new Spawner();
        _timeScaler.Init(_timeScaleSignal, _timeScaleIsOn);
        _actionAnalyzer = new ActionAnalyzer(_timeScaler, _notificationsProvider, _explodeSignal, _destructionsSignal, _killsSignal, _gameOverSignal);
        InitCar();
        _limitRideBack.Init(_limitRideBackIsOn);
        InitViewUILevel();
        _killsCount = new KillsCount(_killsSignal);
        _destructionCount = new DestructionCount(_destructionsSignal);
        _resultsLevelProvider = new ResultsLevelProvider(_playerDataHandler, _carConfiguration, _killsCount, _destructionCount,
            _levelProgressCounter, _notificationsProvider, _gameOverSignal, _timeWaitingOnEndLevel);
        _logicUILevel = new LogicUILevel(_viewUILevel, _carInLevel, _gamePause, _resultsLevelProvider,
            _playerDataHandler, _globalAudio, _levelPrefabsProvider.NotificationsTextPrefab, _gameData);
        _notificationsProvider.ShowDayInfo(_playerDataHandler.PlayerData.Days.ToString());
    }
    private void InitCar()
    {
        int currentSelectLotCarIndex = _playerDataHandler.GetCurrentSelectLotCarIndex();
        _carConfigurationProvider = new CarConfigurationProvider();
        InitCarConfiguration(currentSelectLotCarIndex);
        _carInLevel = _spawner.Spawn(_levelPrefabsProvider.CarsInLevelPrefabs[currentSelectLotCarIndex],
            _level.StartLevelPoint);
        _cinemachineVirtualCamera.Follow = _carInLevel.transform;
        InitProgressCounter();
        _carInLevel.Init(_carConfiguration, _notificationsProvider, _levelProgressCounter,
            _level.DebrisKeeper, _globalAudioForCar, _audioClipProvider.CarsAudioClipsProvider.GetCarAudioClipProvider(currentSelectLotCarIndex),
            _timeScaleSignal, _gameOverSignal, _gamePause, _gameData.GetCarControlMethod(), _autoGameOverIsOn);
    }

    private void InitProgressCounter()
    {
        _levelProgressCounter = new LevelProgressCounter(
            _level.StartLevelPoint, _level.EndLevelPoint, _carInLevel.transform,
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
        _viewUILevel = _spawner.Spawn(_levelPrefabsProvider.ViewUILevelPrefab);
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
        _saveService?.SetPlayerDataToSaving(_playerDataHandler.PlayerData, _gameData.SaveOn);
        Time.timeScale = 1f;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause is true)
        {
            _saveService?.SetPlayerDataToSaving(_playerDataHandler.PlayerData, _gameData.SaveOn);
        }
    }

    private void OnDestroy()
    {
        _resultsLevelProvider?.Dispose();
        _globalAudio?.DisposeAndReInit();
        _killsCount?.Dispose();
        _destructionCount?.Dispose();
        _actionAnalyzer?.Dispose();
    }
}