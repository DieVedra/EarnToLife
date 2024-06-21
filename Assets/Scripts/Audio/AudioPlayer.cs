using UniRx;
using UnityEngine;

public class AudioPlayer
{
    private int _previousRandomValue = 0;
    public readonly AudioSource AudioSource;
    public bool SoundOn { get; private set; }

    public AudioPlayer(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty)
    {
        AudioSource = audioSource;
        soundReactiveProperty.Subscribe(SetSoundStatus);
        audioPauseReactiveProperty.Subscribe(SetPauseStatus);
        SoundOn = soundReactiveProperty.Value;
    }
    public void TryPlayClip(AudioClip audioClip, bool loop = true)
    {
        AudioSource.clip = audioClip;
        if (SoundOn == true)
        {
            AudioSource.loop = loop;
            AudioSource.Play();
        }
    }

    public void TryPlayClip(bool loop = true)
    {
        if (SoundOn == true)
        {
            if (AudioSource.clip != null)
            {
                AudioSource.loop = loop;
                AudioSource.Play();
            }
        }
    }

    public void SetClip(AudioClip audioClip)
    {
        AudioSource.clip = audioClip;
    }

    private void Play()
    {
        if (SoundOn == true && AudioSource.isPlaying != true)
        {
            AudioSource.loop = true;
            AudioSource.Play();
        }
    }

    private void Pause()
    {
        AudioSource.Pause();
    }
    public void TryPlayOneShotClip(AudioClip audioClip)
    {
        if (SoundOn == true)
        {
            AudioSource.PlayOneShot(audioClip);
        }
    }
    public void TryPlayOneShotClipWithRandomSectionVolumeAndPitch(AudioClip audioClip, Vector2 volumeSection, Vector2 pitchSection)
    {
        if (SoundOn == true)
        {
            SetPitch(GetRandomFloatValue(pitchSection.x, pitchSection.y));
            SetVolume(GetRandomFloatValue(volumeSection.x, volumeSection.y));
            AudioSource.PlayOneShot(audioClip);
        }
    }
    public void SetVolume(float volume)
    {
        AudioSource.volume = volume;
    }
    public void SetPitch(float pitch)
    {
        AudioSource.pitch = pitch;
    }
    public void StopPlay()
    {
        AudioSource.Stop();
    }
    public void StopPlayAndSetNull()
    {
        AudioSource.Stop();
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