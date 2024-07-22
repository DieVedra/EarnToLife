using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ActionAnalyzer
{
    private readonly float _delayPeriod = 2f;
    private readonly int _maxDestructionValue = 10;
    private readonly TimeScaler _timeScaler;
    private readonly INotificationForActionAnalyzer _notificationsProvider;
    private readonly ExplodeSignal _explodeSignal;
    private readonly DestructionsSignal _destructionsSignal;
    private readonly KillsSignal _killsSignal;
    private readonly GameOverSignal _gameOverSignal;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private int _currentDestructionValue;
    private bool _analyzeKey;

    public ActionAnalyzer(TimeScaler timeScaler, INotificationForActionAnalyzer notificationsProvider,
        ExplodeSignal explodeSignal, DestructionsSignal destructionsSignal, KillsSignal killsSignal, GameOverSignal gameOverSignal)
    {
        _timeScaler = timeScaler;
        _notificationsProvider = notificationsProvider;
        _explodeSignal = explodeSignal;
        _destructionsSignal = destructionsSignal;
        _killsSignal = killsSignal;
        _gameOverSignal = gameOverSignal;
        _gameOverSignal.OnGameOver += OnGameOver;
        _explodeSignal.OnExplosion += OnExplode;
        _destructionsSignal.OnDestruction += OnDestruction;
        _killsSignal.OnKill += OnDestruction;
        Analyze();
    }
    private void Analyze()
    {
        _analyzeKey = true;
        AnalyzeDestructionValue().Forget();
        AnalyzeDelayPeriod().Forget();
    }
    private async UniTaskVoid AnalyzeDestructionValue()
    {
        while (_analyzeKey == true)
        {
            await UniTask.NextFrame(_cancellationTokenSource.Token);
            if (_currentDestructionValue >= _maxDestructionValue)
            {
                _timeScaler.TryStartTimeWarp();
                // Debug.Log($"                                                           StartTimeWarp {_currentDestructionValue} ");
            }
        }
    }
    private async UniTaskVoid AnalyzeDelayPeriod()
    {
        while (_analyzeKey == true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayPeriod),cancellationToken: _cancellationTokenSource.Token);
            Debug.Log($"                 AnalyzeDelayPeriod {_currentDestructionValue} ");

            _currentDestructionValue = 0;
        }
    }
    private void OnExplode(Vector2 position)
    {
        _notificationsProvider.ShowExplosion(position);
    }

    private void OnGameOver()
    {
        _analyzeKey = false;
    }

    private void OnDestruction()
    {
        _currentDestructionValue++;
    }
    public void Dispose()
    {
        _gameOverSignal.OnGameOver -= OnGameOver;
        _explodeSignal.OnExplosion -= OnExplode;
        _destructionsSignal.OnDestruction -= OnDestruction;
        _killsSignal.OnKill -= OnDestruction;
        _cancellationTokenSource.Cancel();
        _compositeDisposable.Clear();
    }
}