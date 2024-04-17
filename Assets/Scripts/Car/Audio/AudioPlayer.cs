using UniRx;
using UnityEngine;

public class AudioPlayer
{
    private readonly AudioSource _audioSource;
    private bool _soundOn;
    public bool SoundOn => _soundOn;
    public AudioSource AudioSource => _audioSource;
    public AudioPlayer(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty)
    {
        _audioSource = audioSource;
        soundReactiveProperty.Subscribe(SetSoundStatus);
        _soundOn = soundReactiveProperty.Value;
    }
    public void TryPlayClip(AudioClip audioClip, bool loop = false)
    {
        if (_soundOn == true)
        {
            _audioSource.clip = audioClip;
            _audioSource.loop = loop;
            _audioSource.Play();
        }
    }
    public void TryPlayOneShotClip(AudioClip audioClip)
    {
        if (_soundOn == true)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
    public void TryPlayOneShotClipWithRandomSectionVolumeAndPitch(AudioClip audioClip, Vector2 volumeSection, Vector2 pitchSection)
    {
        if (_soundOn == true)
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
        _audioSource.loop = false;
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