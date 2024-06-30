using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GamePause : IGamePause, IDisposable
{
    private const float MIN_VALUE_TIME = 0f;
    private const float NORMAL_VALUE_TIME = 1f;
    // private AudioMixerGroup _levelGroup;
    private readonly TimeScaler _timeScaler;
    private ISoundPause _soundPause;
    private ReactiveProperty<bool> _pauseReactiveProperty = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> PauseReactiveProperty => _pauseReactiveProperty;

    public bool IsPause => PauseReactiveProperty.Value;

    // [Inject]
    public GamePause(TimeScaler timeScaler, ISoundPause soundPause)
    {
        _timeScaler = timeScaler;
        _soundPause = soundPause;
        _pauseReactiveProperty.Value = false;
        AbortPause();
    }

    public void Dispose()
    {
        _pauseReactiveProperty.Dispose();
    }

    public void SetPause()
    {
        _pauseReactiveProperty.Value = true;
        _soundPause.SoundOnPause(IsPause);
        _timeScaler.SetStopTime();
    }

    public void AbortPause()
    {
        _pauseReactiveProperty.Value = false;
        _soundPause.SoundOnPause(IsPause);
        _timeScaler.SetRunTime();
    }

    public void AbortPauseForLoad()
    {
        _timeScaler.SetRunTime();
    }
}
