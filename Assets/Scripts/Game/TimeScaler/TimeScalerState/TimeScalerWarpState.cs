using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimeScalerWarpState : TimeScalerState
{
    private readonly AnimationCurve _downTimeCurve;
    private readonly AnimationCurve _upTimeCurve;
    private readonly Action _setRunState;
    private readonly CompositeDisposable _compositeDisposableUpdate = new CompositeDisposable();
    
    private float _currentDownDuration;
    private float _currentUpDuration;
    private float _currentDownTargetTime;
    private float _duration;
    private float _time;
    private float _currentTimeScaleValue;
    private bool _timeDown = false;
    private bool _timeUp = false;

    public TimeScalerWarpState(AnimationCurve downTimeCurve, AnimationCurve upTimeCurve, TimeScalerValues timeScalerValues, TimeScaleSignal timeScaleSignal, Action setRunState)
        : base(timeScalerValues, timeScaleSignal)
    {
        _downTimeCurve = downTimeCurve;
        _upTimeCurve = upTimeCurve;
        _setRunState = setRunState;
    }

    public override void Enter()
    {
        Debug.Log($"                            Warp State");
        TimeScaleSignal.OnTimeWarpedOn?.Invoke();
        if (_timeDown == false && _timeUp == false)
        {
            CalculateRandomValues();
            
            SubscribeUpdate(_currentDownDuration, TimeDown);
        }
        else if (_timeDown == false && _timeUp == true)
        {
            SubscribeUpdate(_duration, TimeUp);
        }
        else if (_timeDown == true && _timeUp == false)
        {
            SubscribeUpdate(_duration, TimeDown);
        }
    }

    public override void Exit()
    {
        _compositeDisposableUpdate.Clear();
    }
    private void SubscribeUpdate(float duration, Action operation)
    {
        _duration = duration;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            operation?.Invoke();
        }).AddTo(_compositeDisposableUpdate);
    }

    private void TimeDown()
    {
        _timeDown = true;
        SetAndCalculateCurrentTimeScale(GetTimeDown());
        if (_duration <= 0f)
        {
            _compositeDisposableUpdate.Clear();
            _timeDown = false;
            SubscribeUpdate(_currentUpDuration, TimeUp);
        }
    }

    private void TimeUp()
    {
        _timeUp = true;
        SetAndCalculateCurrentTimeScale(GetTimeUp());
        if (_duration <= 0f)
        {
            _compositeDisposableUpdate.Clear();
            _timeUp = false;
            TimeScaleSignal.OnTimeWarpedOff?.Invoke();
            _setRunState?.Invoke();
            Debug.Log($"             Warp state End       ");
        }
    }

    private void SetAndCalculateCurrentTimeScale(float time)
    {
        _duration -= Time.deltaTime;
        _currentTimeScaleValue = Mathf.Lerp(_currentDownTargetTime,TimeScalerValues.NORMAL_VALUE_TIME, time);
        Time.timeScale = _currentTimeScaleValue;
        TimeScaleSignal.OnTimeScaleChange?.Invoke(_currentTimeScaleValue);
    }

    private float GetTimeDown()
    {
        return _downTimeCurve.Evaluate(Mathf.InverseLerp(0f, _currentDownTargetTime, _duration));
    }
    private float GetTimeUp()
    {
        return _upTimeCurve.Evaluate(Mathf.InverseLerp(_currentDownTargetTime,0f, _duration));
    }

    private void CalculateRandomValues()
    {
        _currentDownDuration = Random.Range(TimeScalerValues.DownDurationRange.x, TimeScalerValues.DownDurationRange.y);
        _currentUpDuration = Random.Range(TimeScalerValues.UpDurationRange.x, TimeScalerValues.UpDurationRange.y);
        _currentDownTargetTime = Random.Range(TimeScalerValues.DownTargetTimeRange.x, TimeScalerValues.DownTargetTimeRange.y);
    }
}