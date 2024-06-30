
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TimeScaler  : IDisposable
{
    private const float MIN_VALUE_TIME = 0f;
    private const float NORMAL_VALUE_TIME = 1f;
    private const float TARGET_DOWN_VALUE_TIME = 0.3f;
    private const float DURATION_DOWN = 1f;
    private const float DURATION_UP = 2f;
    private float _currentTime;
    private bool _isWarped;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private Tween _timeDownTween;
    private Tween _timeUpTween;
    private float _currentSpeedValue;
    public bool IsWarped => _isWarped;
    public TimeScaler()
    {
        Debug.Log($"      Scaler");

        _timeDownTween = DOTween.To(() => NORMAL_VALUE_TIME, x => { Debug.Log(x); }, TARGET_DOWN_VALUE_TIME, DURATION_DOWN)
            .SetUpdate(true)
            .SetEase(Ease.InCubic);
        _timeUpTween = DOTween.To(() => TARGET_DOWN_VALUE_TIME, x => Time.timeScale = x, NORMAL_VALUE_TIME, DURATION_UP)
            .SetUpdate(true)
            .SetEase(Ease.OutCubic);
    }
    public async UniTaskVoid TryStartTimeWarp()
    {
        if (_isWarped == false)
        {
            _isWarped = true;
            await _timeDownTween.WithCancellation(_cancellationTokenSource.Token);
            await _timeUpTween.WithCancellation(_cancellationTokenSource.Token);
            _isWarped = false;
        }
    }
    public void SetStopTime()
    {
        Time.timeScale = MIN_VALUE_TIME;
    }
    public void SetRunTime()
    {
        Time.timeScale = NORMAL_VALUE_TIME;
    }
    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _timeDownTween.Kill();
        _timeUpTween.Kill();
    }
}