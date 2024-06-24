using System;
using UniRx;
using UnityEngine;

public class MoveAnalyzer
{
    private readonly float _analyzeDelayTime = 7f;
    private readonly Speedometer _speedometer;
    private float _currentTime;
    private bool _isAnalyze = false;
    public event Action OnCarStands;
    public MoveAnalyzer(Speedometer speedometer, ReactiveCommand driveStarted)
    {
        _speedometer = speedometer;
        _currentTime = _analyzeDelayTime;
        driveStarted.Subscribe(_ =>
        {
            _isAnalyze = true;
        });
    }

    public void Update()
    {
        if (_isAnalyze == true)
        {
            if (_speedometer.CurrentSpeedInt == 0)
            {
                _currentTime -= Time.deltaTime;
                if (_currentTime <= 0f)
                {
                    OnCarStands?.Invoke();
                    _isAnalyze = false;
                }
            }
            else if(_speedometer.CurrentSpeedInt > 0)
            {
                _currentTime = _analyzeDelayTime;
            }
        }
    }
}