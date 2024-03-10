using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GlobalAudio : MonoBehaviour, ISoundPause, ICarAudio, IAudioSettingSwitch
{
    [SerializeField, BoxGroup("AudioSources"), HorizontalLine(color:EColor.White)] private AudioSource _audioSourceBackground;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _audioSourceUI;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _carAudioSource1;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _carAudioSource2;
    
    [SerializeField, BoxGroup("AudioGroups"), HorizontalLine(color:EColor.Yellow)] private AudioMixerGroup _masterMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _levelMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _backgroundMixer;
    [Inject] private AudioClipProvider _audioClipProvider;
    private GlobalAudioValues _globalAudioValues = new GlobalAudioValues();

    public UIClips UiClips => _audioClipProvider.UiClips;
    public CarClips CarClips => _audioClipProvider.ClipsCar;
    public AudioSource AudioSourceUI => _audioSourceUI;
    public AudioSource CarAudioSource1 => _carAudioSource1;
    public AudioSource CarAudioSource2 => _carAudioSource2;

    public event Action OnSoundChange; 
    public event Action<bool> OnMusicChange; 
    public bool SoundOn { get; private set; }
    public bool MusicOn { get; private set;}

    public void Construct(bool keySound, bool keyMusic)
    {
        SoundOn = keySound;
        MusicOn = keyMusic;
        if (MusicOn == true)
        {
            PlayBackground();
        }
    }
    private void PlayBackground()
    {
        SetAndPlay();
    }
    private void StopBackground()
    {
        _audioSourceBackground.Stop();
        _audioSourceBackground.clip = null;
    }
    public void SetMusicOff()
    {
        MusicOn = false;
        OnMusicChange?.Invoke(MusicOn);
        StopBackground();
    }
    public void SetMusicOn()
    {
        MusicOn = true;
        OnMusicChange?.Invoke(MusicOn);
        PlayBackground();
    }
    public void SetSoundsOff()
    {
        SoundOn = false;
        OnSoundChange?.Invoke();
        _carAudioSource1.Stop();
        _carAudioSource2.Stop();
    }
    public void SetSoundsOn()
    {
        SoundOn = true;
        OnSoundChange?.Invoke();
    }
    private void SetAndPlay()
    {
        _audioSourceBackground.clip = _audioClipProvider.ClipBackground;
        _audioSourceBackground.Play();
    }

    public void SoundOnPause(bool pause)
    {
        if (pause == true)
        {
            _levelMixer.audioMixer.SetFloat(_globalAudioValues.VolumeLevel, _globalAudioValues.VolumeLevelLow);
            _backgroundMixer.audioMixer.SetFloat(_globalAudioValues.VolumeBackgroundMusic, _globalAudioValues.VolumeBackgroundMusicLow);
        }
        else
        {
            _levelMixer.audioMixer.SetFloat(_globalAudioValues.VolumeLevel, _globalAudioValues.VolumeNormal);
            _backgroundMixer.audioMixer.SetFloat(_globalAudioValues.VolumeBackgroundMusic, _globalAudioValues.VolumeNormal);
        }
    }
}