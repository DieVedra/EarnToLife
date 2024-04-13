using UniRx;
using UnityEngine;

public class AudioPlayer
{
    protected readonly AudioSource AudioSource;
    private bool _soundOn;
    protected bool SoundOn => _soundOn;

    protected AudioPlayer(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty)
    {
        AudioSource = audioSource;
        soundReactiveProperty.Subscribe(SetSoundStatus);
        _soundOn = soundReactiveProperty.Value;
    }
    protected void TryPlayClip(AudioClip audioClip, bool loop = false)
    {
        if (_soundOn == true)
        {
            AudioSource.clip = audioClip;
            AudioSource.loop = loop;
            AudioSource.Play();
        }
    }
    protected void TryPlayOneShotClip(AudioClip audioClip)
    {
        if (_soundOn == true)
        {
            AudioSource.PlayOneShot(audioClip);
        }
    }
    protected void TryPlayOneShotClipWithRandomSectionVolumeAndPitch(AudioClip audioClip, Vector2 volumeSection, Vector2 pitchSection)
    {
        if (_soundOn == true)
        {
            SetPitch(GetRandomFloatValue(pitchSection.x, pitchSection.y));
            SetVolume(GetRandomFloatValue(volumeSection.x, volumeSection.y));
            AudioSource.PlayOneShot(audioClip);
        }
    }
    protected void SetVolume(float volume)
    {
        AudioSource.volume = volume;
    }
    protected void SetPitch(float pitch)
    {
        AudioSource.pitch = pitch;
    }
    protected void StopPlay()
    {
        AudioSource.Stop();
        AudioSource.loop = false;
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
        _soundOn = value;
    }

    private int GetRandomIntValue(int min, int max)
    {
        return Random.Range(min, max);
    }
    private float GetRandomFloatValue(float min, float max)
    {
        return Random.Range(min, max);
    }
}