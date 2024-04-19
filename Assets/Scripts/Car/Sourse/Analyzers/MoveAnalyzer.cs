using System;
using UniRx;
using UnityEngine;

public class MoveAnalyzer
{
    private readonly float _analyzeDelay = 3f;
    private readonly Speedometer _speedometer;
    private readonly TimerAsync _timerAsync;
    private int _speedInPreviousFrame = 0;
    private bool _isAnalyze = false;
    public event Action OnCarStands;
    public MoveAnalyzer(Speedometer speedometer, ReactiveProperty<bool> driveStarted)
    {
        _speedometer = speedometer;
        _timerAsync = new TimerAsync();
        driveStarted.Subscribe(_ =>{ SetStartAnalyze();});
    }

    public void Update()
    {
        if (_isAnalyze == true)
        {
            if (_speedInPreviousFrame == _speedometer.CurrentSpeedInt)
            {
                _timerAsync.TryStartTimer(CarStand, _analyzeDelay).Forget();
            }
            else
            {
                _timerAsync.TryStopTimer();
            }

            _speedInPreviousFrame = _speedometer.CurrentSpeedInt;
            // Debug.Log(_speedometer.CurrentSpeedFloat);
        }
    }

    private void CarStand()
    {
        OnCarStands?.Invoke();
    }

    private void SetStartAnalyze()
    {
        _isAnalyze = true;
    }
}