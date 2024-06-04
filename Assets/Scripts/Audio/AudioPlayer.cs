using UniRx;
using UnityEngine;

public class AudioPlayer
{
    private readonly AudioSource _audioSource;
    private int _previousRandomValue = 0;
    public bool SoundOn { get; private set; }
    public AudioSource AudioSource => _audioSource;
    public AudioPlayer(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty)
    {
        _audioSource = audioSource;
        soundReactiveProperty.Subscribe(SetSoundStatus);
        audioPauseReactiveProperty.Subscribe(SetPauseStatus);
        SoundOn = soundReactiveProperty.Value;
    }
    public void TryPlayClip(AudioClip audioClip, bool loop = true)
    {
        _audioSource.clip = audioClip;
        if (SoundOn == true)
        {
            _audioSource.loop = loop;
            _audioSource.Play();
        }
    }

    public void TryPlayClip(bool loop = true)
    {
        if (SoundOn == true)
        {
            if (_audioSource.clip != null)
            {
                _audioSource.loop = loop;
                _audioSource.Play();
            }
        }
    }

    public void SetClip(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
    }

    private void Play()
    {
        if (SoundOn == true && _audioSource.isPlaying != true)
        {
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    private void Pause()
    {
        _audioSource.Pause();
    }
    public void TryPlayOneShotClip(AudioClip audioClip)
    {
        if (SoundOn == true)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
    public void TryPlayOneShotClipWithRandomSectionVolumeAndPitch(AudioClip audioClip, Vector2 volumeSection, Vector2 pitchSection)
    {
        if (SoundOn == true)
        {
            SetPitch(GetRandomFloatValue(pitchSection.x, pitchSection.y));
            SetVolume(GetRandomFloatValue(volumeSection.x, volumeSection.y));
            _audioSource.PlayOneShot(audioClip);
        }
    }
    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }
    public void SetPitch(float pitch)
    {
        _audioSource.pitch = pitch;
    }
    public void StopPlay()
    {
        _audioSource.Stop();
    }
    public void StopPlayAndSetNull()
    {
        _audioSource.Stop();
        SetClip(null);
    }
    protected AudioClip GetRandomAudioClip(AudioClip[] clips)
    {
        int value = GetRandomIntValue(0, clips.Length);
        AudioClip result = null;
        for (int i = 0; i < clips.Length; i++)
        {
            if (value == i)
            {
                result = clips[i];
                break;
            }
        }
        return result;
    }
    private void SetSoundStatus(bool value)
    {
        SoundOn = value;
        if (value == false)
        {
            StopPlay();
        }
    }
    public void SetPauseStatus(bool value)
    {
        if (value == true)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }
    private int GetRandomIntValue(int min, int max)
    {
        int newValue = max;
        while (_previousRandomValue == newValue)
        {
            newValue = Random.Range(min, max);
        }
        return Random.Range(min, max);
    }
    private float GetRandomFloatValue(float min, float max)
    {
        return Random.Range(min, max);
    }
    private float GetRandomFloatValue(Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }
}