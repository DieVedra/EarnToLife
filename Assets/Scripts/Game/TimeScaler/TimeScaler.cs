using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class TimeScaler : MonoBehaviour
{
    [SerializeField, HorizontalLine(color:EColor.White)] private AnimationCurve _downTimeCurve;
    [SerializeField, MinMaxSlider(0.0f, 1f)] private Vector2 _downDuration;
    [SerializeField, HorizontalLine(color:EColor.White)] private AnimationCurve _upTimeCurve;
    [SerializeField, MinMaxSlider(0.0f, 2f)] private Vector2 _upDuration;
    [SerializeField, HorizontalLine(color:EColor.White), MinMaxSlider(0.0f, 1f)] private Vector2 _downTargerTime;
    private TimeScalerState _currentState;
    private TimeScalerFSM _timeScalerFsm;
    private TimeScalerValues _timeScalerValues;

    [Inject]
    private void Construct(TimeScaleSignal timeScaleSignal)
    {
        _timeScalerValues = new TimeScalerValues(_downDuration, _upDuration, _downTargerTime);
        Dictionary<Type, TimeScalerState> dictionaryStates = new Dictionary<Type, TimeScalerState>
        {
            {typeof(TimeScalerRunState), new TimeScalerRunState(_timeScalerValues, timeScaleSignal)},
            {typeof(TimeScalerStopState), new TimeScalerStopState(_timeScalerValues, timeScaleSignal)},
            {typeof(TimeScalerWarpState), new TimeScalerWarpState(_downTimeCurve, _upTimeCurve, _timeScalerValues, timeScaleSignal,
                ()=>{_timeScalerFsm.SetState<TimeScalerRunState>();})}
        };
        _timeScalerFsm = new TimeScalerFSM(dictionaryStates);
    }
    public void TryStartTimeWarp()
    {
        _timeScalerFsm.SetState<TimeScalerWarpState>();
    }
    public void SetStopTime()
    {
        _timeScalerFsm.SetState<TimeScalerStopState>();
    }
    public void SetRunTime()
    {
        if (_timeScalerFsm.PreviousStateIsWarp == true)
        {
            _timeScalerFsm.SetState<TimeScalerWarpState>();
        }
        else
        {
            _timeScalerFsm.SetState<TimeScalerRunState>();
        }
    }

    private void OnDestroy()
    {
        _timeScalerFsm?.Dispose();
    }
}