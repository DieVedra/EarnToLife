using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

public class GamePause /*: IGamePause*/
{
    private const float MIN_VALUE_TIME = 0f;
    private const float NORMAL_VALUE_TIME = 1f;
    private AudioMixerGroup _levelGroup;
    private ISoundPause _soundPause;
    public ReactiveProperty<bool> PauseReactiveProperty { get; private set; }

    public bool IsPause { get; private set; }

    public GamePause(ISoundPause soundPause)
    {
        _soundPause = soundPause;
        PauseReactiveProperty = new ReactiveProperty<bool>();
        IsPause = false;
        AbortPause();
    }
    public void SetPause()
    {
        IsPause = true;
        _soundPause.SoundOnPause(IsPause);
        Time.timeScale = MIN_VALUE_TIME;
    }
    public void AbortPause()
    {
        IsPause = false;
        _soundPause.SoundOnPause(IsPause);
        Time.timeScale = NORMAL_VALUE_TIME;
    }
    public void AbortPauseForLoad()
    {
        Time.timeScale = NORMAL_VALUE_TIME;
    }
}
