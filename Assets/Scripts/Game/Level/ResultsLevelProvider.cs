using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class ResultsLevelProvider
{
    //private const string TANK_EMPTY_REASON = "Out of fuel";
    //private const string GOT_POINT_DESTINATION_REASON = "Have arrived!";
    private const float MILISECONDS_MULTIPLIER = 1000f;
    //private GameData _gameData;
    private Wallet _wallet;
    private PlayerDataHandler _playerDataHandler;
    private ResultsLevel _resultsLevel;
    private float _priceTagForTheMurder;
    private float _priceTagForTheWayMeter;
    private int _totalCash = 0;
    private int _timePauseOnEndLevel;
    private bool _gameEnd = false;
    public LevelProgressCounter LevelProgressCounter { get; private set; }
    public KillsCount KillsCount { get; private set; }
    public NotificationsProvider NotificationsProvider { get; private set; }

    public event Action<ResultsLevel, ResultsLevel> OnOutCalculateResultsLevel;
    public ResultsLevelProvider(PlayerDataHandler playerDataHandler, CarConfiguration carConfiguration, KillsCount killsCount, 
        LevelProgressCounter levelProgressCounter, NotificationsProvider notificationsProvider,
        float timePauseOnEndLevel)
    {
        _priceTagForTheMurder = carConfiguration.PriceTagForTheMurder;
        _priceTagForTheWayMeter = carConfiguration.PriceTagForTheWayMeter;
        _wallet = playerDataHandler.PlayerData.Wallet;
        KillsCount = killsCount;
        _playerDataHandler = playerDataHandler;
        NotificationsProvider = notificationsProvider;
        LevelProgressCounter = levelProgressCounter;
        _timePauseOnEndLevel = (int)(timePauseOnEndLevel * MILISECONDS_MULTIPLIER);
        NotificationsProvider.OnFueltankEmpty += GameOverBecauseTankEmpty;
        NotificationsProvider.OnGotPointDestination += GameOverBecauseGotPointDestination;
    }

    private async void GameOverBecauseTankEmpty(string reason)
    {
        if (CheckGameStatus())
        {
            CalculateResults();
            await UniTask.Delay(_timePauseOnEndLevel);
            CreateAndSend(reason);
            _playerDataHandler.SetResultsLevel(_resultsLevel);
            _playerDataHandler.AddDay();
            _playerDataHandler.LevelWasNotPassed();
        }
    }

    private async void GameOverBecauseGotPointDestination(string reason)
    {
        if (CheckGameStatus())
        {
            CalculateResults();
            await UniTask.Delay(_timePauseOnEndLevel);
            CreateAndSend(reason);
            _playerDataHandler.SetResultsLevel(null);
            _playerDataHandler.AddDay();
            _playerDataHandler.OpenNextLevel();
        }
    }
    private void CalculateResults()
    {
        CalculateDistance(LevelProgressCounter.GetFinalDistance());
        CalculateKills(KillsCount.Kills);
        _wallet.AddCash(_totalCash);
    }
    private void CalculateDistance(float distance)
    {
        _totalCash += (int)(distance * _priceTagForTheWayMeter);
    }
    private void CalculateKills(float kills)
    {
        if (kills > 0)
        {
            _totalCash += (int)(kills * _priceTagForTheMurder);
        }
    }
    private bool CheckGameStatus()
    {
        if (_gameEnd == false)
        {
            _gameEnd = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void CreateAndSend(string reason)
    {
        _resultsLevel = CreateResults(reason);
        OnOutCalculateResultsLevel?.Invoke(_resultsLevel, _playerDataHandler.TryGetResultLevel());
    }
    private ResultsLevel CreateResults(string reason)
    {
        return new ResultsLevel(
            KillsCount.Kills, _playerDataHandler.PlayerData.Days,
            LevelProgressCounter.GetFinalDistance(),
            LevelProgressCounter.GetFinalDistanceToDisplayOnSliderInScorePanel(),
            _totalCash, reason, 
            LevelProgressCounter.GetCarDistanceForDisplaySlider());
    }
    private void OpenNextLevel()
    {
        _playerDataHandler.OpenNextLevel();

    }
    private void AddDay()
    {
        _playerDataHandler.AddDay();
    }
    //private void OnDestroy()
    //{
    //    _car.FuelTank.OnTankEmpty -= GameOverOnTankEmpty;
    //    LevelProgressCounter.OnGotPointDestination -= GameOverOnGotPointDestination;
    //}
}

