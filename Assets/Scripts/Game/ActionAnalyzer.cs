using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ActionAnalyzer
{
    private readonly float _delayPeriod = 0.4f;
    private readonly int _maxDestructionValue = 15;
    private readonly TimeScaler _timeScaler;
    private readonly INotificationForActionAnalyzer _notificationsProvider;
    private readonly ExplodeSignal _explodeSignal;
    private readonly DestructionsSignal _destructionsSignal;
    private readonly KillsSignal _killsSignal;
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private int _currentDestructionValue;

    public ActionAnalyzer(TimeScaler timeScaler, INotificationForActionAnalyzer notificationsProvider,
        ExplodeSignal explodeSignal, DestructionsSignal destructionsSignal, KillsSignal killsSignal)
    {
        _timeScaler = timeScaler;
        _notificationsProvider = notificationsProvider;
        _explodeSignal = explodeSignal;
        _destructionsSignal = destructionsSignal;
        _killsSignal = killsSignal;
        _explodeSignal.OnExplosion += OnExplode;
        _destructionsSignal.OnDestruction += OnDestruction;
        _killsSignal.OnKill += OnDestruction;
        Analyze().Forget();
    }
    private async UniTaskVoid Analyze()
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayPeriod), ignoreTimeScale: true, cancellationToken: _cancellationTokenSource.Token);
            if (_currentDestructionValue >= _maxDestructionValue)
            {
                TryTimeWarpAndShowCompliment();
            }
        }
    }

    private void OnExplode()
    {
        TryTimeWarpAndShowCompliment();
    }

    private void OnDestruction()
    {
        _currentDestructionValue++;
    }

    private void TryTimeWarpAndShowCompliment()
    {
        if (_timeScaler.TryStartTimeWarp())
        {
            _notificationsProvider.ShowCompliment(_currentDestructionValue);
        }
        _currentDestructionValue = 0;
    }
    public void Dispose()
    {
        _explodeSignal.OnExplosion -= OnExplode;
        _destructionsSignal.OnDestruction -= OnDestruction;
        _killsSignal.OnKill -= OnDestruction;
        _cancellationTokenSource.Cancel();
        _compositeDisposable.Clear();
    }
}