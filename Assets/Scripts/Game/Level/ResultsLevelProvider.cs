using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class ResultsLevelProvider
{
    private readonly float _timePauseOnEndLevel;
    private readonly float _timeDelayOnEndLevelOnDriverCrushed = 3f;
    private readonly float _timeDelayOnEndLevelOnEngineBurn = 3f;
    private readonly Wallet _wallet;
    private readonly PlayerDataHandler _playerDataHandler;
    private readonly float _priceTagForTheWayMeter;
    private readonly float _priceTagForTheMurder;
    private ResultsLevel _resultsLevel;
    private int _totalCash = 0;
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
        _timePauseOnEndLevel = timePauseOnEndLevel;
        NotificationsProvider.OnFueltankEmpty += GameOverBecauseTankEmpty;
        NotificationsProvider.OnGotPointDestination += GameOverBecauseGotPointDestination;
        NotificationsProvider.OnEngineBroken += GameOverBecauseEngineBroken;
        NotificationsProvider.OnDriverCrushed += GameOverBecauseDriverCrushed;
    }

    public void Dispose()
    {
        NotificationsProvider.OnFueltankEmpty -= GameOverBecauseTankEmpty;
        NotificationsProvider.OnGotPointDestination -= GameOverBecauseGotPointDestination;
        NotificationsProvider.OnEngineBroken -= GameOverBecauseEngineBroken;
        NotificationsProvider.OnDriverCrushed -= GameOverBecauseDriverCrushed;
    }
    private void GameOverBecauseTankEmpty(string reason)
    {
        GameOver(false, true, reason, _timePauseOnEndLevel);
    }

    private void GameOverBecauseGotPointDestination(string reason)
    {
        GameOver(true, false, reason, _timePauseOnEndLevel);
    }

    private void GameOverBecauseEngineBroken(string reason)
    {
        GameOver(false, true, reason, _timeDelayOnEndLevelOnEngineBurn);
    }
    private void GameOverBecauseDriverCrushed(string reason)
    {
        GameOver(false, true, reason, _timeDelayOnEndLevelOnDriverCrushed);
    }

    private async void GameOver(bool openNextLevel, bool availabilityResultLevel, string reason, float delay)
    {
        if (CheckGameStatus())
        {
            CalculateResults();
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            CreateAndSend(reason);
            _playerDataHandler.AddDay();
            if (openNextLevel == true)
            {
                _playerDataHandler.OpenNextLevel();
            }
            else
            {
                _playerDataHandler.LevelWasNotPassed();
            }

            if (availabilityResultLevel == true)
            {
                _playerDataHandler.SetResultsLevel(_resultsLevel);
            }
            else
            {
                _playerDataHandler.SetResultsLevel();
            }
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

