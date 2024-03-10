using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GamePause
{
    private const float MIN_VALUE_TIME = 0f;
    private const float NORMAL_VALUE_TIME = 1f;
    private bool _isPause;
    private AudioMixerGroup _levelGroup;
    private ISoundPause _soundPause;
    public GamePause(ISoundPause soundPause)
    {
        _soundPause = soundPause;
        AbortPause();
    }
    public void SetPause()
    {
        _isPause = true;
        _soundPause.SoundOnPause(_isPause);
        Time.timeScale = MIN_VALUE_TIME;
    }
    public void AbortPause()
    {
        _isPause = false;
        _soundPause.SoundOnPause(_isPause);
        Time.timeScale = NORMAL_VALUE_TIME;
    }
    public void AbortPauseForLoad()
    {
        Time.timeScale = NORMAL_VALUE_TIME;
    }
    public bool CheckPause()
    {
        if (_isPause == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
