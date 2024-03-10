using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class CoupAnalyzer
{
    private readonly float _minAngle = 140f;
    private readonly float _maxAngle = 220f;
    private readonly float _timeDelay = 10f;
    private readonly Transform _carTransform;
    public bool CarIsCoup => IsCoup.Value;
    public ReactiveProperty<bool> IsCoup = new ReactiveProperty<bool>();
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private bool _timerActive = false;
    private float _angleZ => _carTransform.eulerAngles.z;
    public CoupAnalyzer(Transform carTransform)
    {
        _carTransform = carTransform;
        IsCoup.Subscribe(_ =>
        {
            TryStartTimer();
            TryStopTimer();
        }).AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Clear();
        _timerActive = false;
        _cancellationTokenSource.Cancel();
    }
    public void Update()
    {
        if (_angleZ >= _minAngle && _angleZ <= _maxAngle)
        {
            // if (_isCoup.Value == false)
            // {
            //     _isCoup.Value = true;
            // }
            IsCoup.Value = true;
        }
        else
        {
            // if (_isCoup.Value == true)
            // {
            //     _isCoup.Value = false;
            // }
            IsCoup.Value = false;
        }
    }

    private async void TryStartTimer()
    {
        if (IsCoup.Value == true && _timerActive == false)
        {
            _timerActive = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_timeDelay), cancellationToken: _cancellationTokenSource.Token);
            if (_timerActive == true)
            {
                //invoke game over
            }
        }
    }
    private void TryStopTimer()
    {
        if (IsCoup.Value == false)
        {
            _timerActive = false;
            _cancellationTokenSource.Cancel();
        }
    }
}