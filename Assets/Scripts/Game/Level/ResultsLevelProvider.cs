using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class ResultsLevelProvider
{
    private readonly float _timePauseOnEndLevel;
    private readonly float _timeDelayOnEndLevelOnDriverCrushed = 3f;
    private readonly float _timeDelayOnEndLevelOnEngineBurn = 3f;
    private readonly float _timeDelayOnEndLevelOnDriverAsleep = 1f;
    private readonly float _timeDelayOnEndLevelDefault = 3f;
    private readonly float _priceTagForTheWayMeter;
    private readonly float _priceTagForTheMurder;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly Wallet _wallet;
    private readonly PlayerDataHandler _playerDataHandler;
    private ResultsLevel _resultsLevel;
    private int _totalCash = 0;
    private int _distanceMoney = 0;
    private int _killsMoney = 0;
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
        NotificationsProvider.OnCarTurnOver += GameOverBecauseTurnOver;
        NotificationsProvider.OnCarStuck += GameOverBecauseCarStuck;
        NotificationsProvider.OnDriverAsleep += GameOverBecauseDriverAsleep;
    }

    public void Dispose()
    {
        NotificationsProvider.OnFueltankEmpty -= GameOverBecauseTankEmpty;
        NotificationsProvider.OnGotPointDestination -= GameOverBecauseGotPointDestination;
        NotificationsProvider.OnEngineBroken -= GameOverBecauseEngineBroken;
        NotificationsProvider.OnDriverCrushed -= GameOverBecauseDriverCrushed;
        NotificationsProvider.OnCarTurnOver -= GameOverBecauseTurnOver;
        NotificationsProvider.OnCarStuck -= GameOverBecauseCarStuck;
        NotificationsProvider.OnDriverAsleep -= GameOverBecauseDriverAsleep;
        _cancellationTokenSource.Cancel();
    }
    private void GameOverBecauseTankEmpty(string reason)
    {
        GameOver(false, true, reason, _timePauseOnEndLevel).Forget();
    }

    private void GameOverBecauseGotPointDestination(string reason)
    {
        GameOver(true, false, reason, _timePauseOnEndLevel).Forget();
    }

    private void GameOverBecauseEngineBroken(string reason)
    {
        GameOver(false, true, reason, _timeDelayOnEndLevelOnEngineBurn).Forget();
    }
    private void GameOverBecauseDriverCrushed(string reason)
    {
        GameOver(false, true, reason, _timeDelayOnEndLevelOnDriverCrushed).Forget();
    }
    private void GameOverBecauseTurnOver(string reason)
    {
        GameOver(false, true, reason, _timeDelayOnEndLevelDefault).Forget();
    }
    private void GameOverBecauseCarStuck(string reason)
    {
        GameOver(false, true, reason, _timeDelayOnEndLevelDefault).Forget();
    }
    private void GameOverBecauseDriverAsleep(string reason)
    {
        GameOver(false, true, reason, _timeDelayOnEndLevelOnDriverAsleep).Forget();
    }
    private async UniTaskVoid GameOver(bool openNextLevel, bool availabilityResultLevel, string reason, float delay)
    {
        if (CheckGameStatus())
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken:  _cancellationTokenSource.Token);
            CalculateResultsAndSend(reason);
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
        _distanceMoney = CalculateDistanceMoney();
        _killsMoney = CalculateKillsMoney();
        _totalCash += _distanceMoney + _killsMoney;
        _wallet.AddCash(_totalCash);
    }
    private int CalculateDistanceMoney()
    {
        return (int)(LevelProgressCounter.GetFinalDistance() * _priceTagForTheWayMeter);
    }
    private int CalculateKillsMoney()
    {
        if (KillsCount.Kills > 0)
        {
            return (int)(KillsCount.Kills * _priceTagForTheMurder);
        }
        else
        {
            return 0;
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
    private void CalculateResultsAndSend(string reason)
    {
        _resultsLevel = CreateResults(reason);
        OnOutCalculateResultsLevel?.Invoke(_resultsLevel, _playerDataHandler.TryGetResultLevel());
    }
    private ResultsLevel CreateResults(string reason)
    {
        CalculateResults();
        return new ResultsLevel(
            KillsCount.Kills, _playerDataHandler.PlayerData.Days,
            LevelProgressCounter.GetFinalDistance(),
            LevelProgressCounter.GetFinalDistanceToDisplayOnSliderInScorePanel(),
            _totalCash, _killsMoney, _distanceMoney,
            reason, 
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

