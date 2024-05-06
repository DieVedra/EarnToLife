using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GlobalAudio : MonoBehaviour, ISoundPause, IAudioSettingSwitch, IGlobalAudio, IUIAudio
{
    [SerializeField, BoxGroup("AudioSources"), HorizontalLine(color:EColor.White)] private AudioSource _audioSourceBackground;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _uI;
    private readonly GlobalAudioValues _globalAudioValues = new GlobalAudioValues();
    private PlayerDataHandler _playerDataHandler;
    public UIAudioClipProvider UIAudioClipProvider { get; private set; }
    public AudioSource UI => _uI;
    public bool SoundOn => SoundReactiveProperty.Value;
    public bool MusicOn { get; private set; }
    public ReactiveProperty<bool> SoundReactiveProperty { get; private set; }
    public ReactiveProperty<bool> AudioPauseReactiveProperty { get; private set; }
    
    [Inject]
    private void Construct(AudioClipProvider audioClipProvider)
    {
        UIAudioClipProvider ??= audioClipProvider.UIAudioClipProvider;
        _audioSourceBackground.clip ??= audioClipProvider.ClipBackground;
    }
    public void InitFromEntryScene(PlayerDataHandler playerDataHandler)
    {
        _playerDataHandler ??= playerDataHandler;
        InitReactiveProperties();
        MusicOn = playerDataHandler.PlayerData.MusicOn;
        if (MusicOn == true)
        {
            PlayBackground();
        }
    }

    private void InitReactiveProperties()
    {
        SoundReactiveProperty = new ReactiveProperty<bool> { Value = _playerDataHandler.PlayerData.SoundOn };
        AudioPauseReactiveProperty = new ReactiveProperty<bool> { Value = false };
    }
    public void DisposeAndReInit()
    {
        SoundReactiveProperty.Dispose();
        AudioPauseReactiveProperty.Dispose();
        InitReactiveProperties();
    }
    public void SetMusicOff()
    {
        SetMusic(false);
        StopBackground();
    }
    public void SetMusicOn()
    {
        SetMusic(true);
        PlayBackground();
    }
    public void SetSoundsOff()
    {
        SetSounds(false);
    }
    public void SetSoundsOn()
    {
        SetSounds(true);
    }
    private void SetSounds(bool key)
    {
        SoundReactiveProperty.Value = key;
        _playerDataHandler.SetSoundKey(SoundReactiveProperty.Value);
    }
    private void SetMusic(bool key)
    {
        MusicOn = key;
        _playerDataHandler.SetMusicKey(MusicOn);
    }
    private void SetPauseVolumeMusic()
    {
        _audioSourceBackground.volume = _globalAudioValues.VolumeBackgroundMusicLow;
    }
    private void SetNormalVolumeMusic()
    {
        _audioSourceBackground.volume = _globalAudioValues.VolumeBackgroundMusicNormal;
    }
    private void PlayBackground()
    {
        _audioSourceBackground.Play();
    }
    private void StopBackground()
    {
        _audioSourceBackground.Stop();
    }
    public void SoundOnPause(bool pause)
    {
        AudioPauseReactiveProperty.Value = pause;

        if (pause == true)
        {
            SetPauseVolumeMusic();
        }
        else
        {
            SetNormalVolumeMusic();
        }
    }
}