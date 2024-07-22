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
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private int _currentDestructionValue;

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
        Observable.EveryUpdate().Where(x=>_currentDestructionValue >= _maxDestructionValue).Subscribe(_=>
        {
            _currentDestructionValue = 0;
            _timeScaler.TryStartTimeWarp();

            Debug.Log(111);
        }).AddTo(_compositeDisposable);
        Observable.Interval(TimeSpan.FromSeconds(_delayPeriod)).Subscribe(_ =>
        {
            _currentDestructionValue = 0;
        }).AddTo(_compositeDisposable);
    }
    private void OnExplode(Vector2 position)
    {
        _notificationsProvider.ShowExplosion(position);
    }

    private void OnGameOver()
    {
        _compositeDisposable.Clear();
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
        _compositeDisposable.Clear();
    }
}