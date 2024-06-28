using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

public class GamePause : IGamePause, IDisposable
{
    private const float MIN_VALUE_TIME = 0f;
    private const float NORMAL_VALUE_TIME = 1f;
    private AudioMixerGroup _levelGroup;
    private ISoundPause _soundPause;
    private ReactiveProperty<bool> _pauseReactiveProperty = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> PauseReactiveProperty => _pauseReactiveProperty;

    public bool IsPause => PauseReactiveProperty.Value;

    public GamePause(ISoundPause soundPause)
    {
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
        Time.timeScale = MIN_VALUE_TIME;
    }

    public void AbortPause()
    {
        _pauseReactiveProperty.Value = false;
        _soundPause.SoundOnPause(IsPause);
        Time.timeScale = NORMAL_VALUE_TIME;
    }

    public void AbortPauseForLoad()
    {
        Time.timeScale = NORMAL_VALUE_TIME;
    }
}
