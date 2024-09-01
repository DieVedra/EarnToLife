using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GamePause : IGamePause
{
    private readonly TimeScaler _timeScaler;
    private readonly ISoundPause _soundPause;
    public bool IsPause { get; private set; }
    public GamePause(TimeScaler timeScaler, ISoundPause soundPause)
    {
        _timeScaler = timeScaler;
        _soundPause = soundPause;
        IsPause = false;
        AbortPause();
    }

    public void SetPause()
    {
        IsPause = true;
        _soundPause.SoundOnPause(IsPause);
        _timeScaler.SetStopTime();
    }

    public void AbortPause()
    {
        IsPause = false;
        _soundPause.SoundOnPause(IsPause);
        _timeScaler.SetRunTime();
    }

    public void AbortPauseForLoad()
    {
        _timeScaler.SetRunTime();
    }
}
