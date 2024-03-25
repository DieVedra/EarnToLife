using UniRx;
using UnityEngine;

public class AudioPlayer
{
    protected readonly AudioSource AudioSource;
    private bool _soundOn;

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
    protected void StopPlay()
    {
        AudioSource.Stop();
        AudioSource.loop = false;
    }
    private void SetSoundStatus(bool value)
    {
        _soundOn = value;
    }
}