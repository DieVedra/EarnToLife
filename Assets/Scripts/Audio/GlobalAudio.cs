using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

public class GlobalAudio : MonoBehaviour, ISoundPause, IAudioSettingSwitch, IGlobalAudio, IUIAudio
{
    [SerializeField, BoxGroup("AudioSources"), HorizontalLine(color:EColor.White)] private AudioSource _audioSourceBackground;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _uI;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forLevel;
    
    [SerializeField, BoxGroup("AudioGroups"), HorizontalLine(color:EColor.Yellow)] private AudioMixerGroup _masterMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _levelMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _backgroundMixer;
    private GlobalAudioValues _globalAudioValues = new GlobalAudioValues();
    private AudioClip _clipBackground;
    public UIAudioClipProvider UIAudioClipProvider { get; private set; }
    public AudioSource UI => _uI;
    private AudioSource[] _audioSourcesAll;

    // public event Action OnSoundChange; 
    public bool SoundOn => SoundReactiveProperty.Value;
    public bool MusicOn => MusicReactiveProperty.Value;
    public ReactiveProperty<bool> SoundReactiveProperty { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> MusicReactiveProperty { get; private set; } = new ReactiveProperty<bool>();

    public void Construct(UIAudioClipProvider uIAudioClipProvider, AudioClip clipBackground, bool keySound, bool keyMusic)
    {
        UIAudioClipProvider = uIAudioClipProvider;
        _clipBackground = clipBackground;
        SoundReactiveProperty.Value = keySound;
        MusicReactiveProperty.Value = keyMusic;
        if (MusicOn == true)
        {
            SetAndPlayBackground();
        }
        _audioSourcesAll = new[]
        {
            _uI
        };
    }
    private void StopBackground()
    {
        _audioSourceBackground.Stop();
        _audioSourceBackground.clip = null;
    }
    public void SetMusicOff()
    {
        MusicReactiveProperty.Value = false;
        // OnMusicChange?.Invoke(MusicOn);
        StopBackground();
    }
    public void SetMusicOn()
    {
        MusicReactiveProperty.Value = true;
        // OnMusicChange?.Invoke(MusicOn);
        SetAndPlayBackground();
    }
    public void SetSoundsOff()
    {
        SoundReactiveProperty.Value = false;
        StopAllSources();
        MuteMaster();
    }
    public void SetSoundsOn()
    {
        SoundReactiveProperty.Value = true;
        UnmuteMaster();
    }
    private void SetAndPlayBackground()
    {
        _audioSourceBackground.clip = _clipBackground;
        _audioSourceBackground.Play();
    }

    private void MuteMaster()
    {
        StopAllSources();
        _masterMixer.audioMixer.SetFloat(_globalAudioValues.NameVolumeMaster, _globalAudioValues.VolumeLevelLow);
    }
    private void UnmuteMaster()
    {
        PlayAllSources();
        _masterMixer.audioMixer.SetFloat(_globalAudioValues.NameVolumeMaster, _globalAudioValues.VolumeNormal);
    }

    private void PauseAllSources()
    {
        SelectionSources(PauseSource);
    }
    private void StopAllSources()
    {
        Debug.Log($"StopAllSources");
        SelectionSources(StopSource);
    }
    private void PlayAllSources()
    {
        SelectionSources(PlaySource);
    }

    private void SelectionSources(Action<AudioSource> operation)
    {
        for (int i = 0; i < _audioSourcesAll.Length; i++)
        {
            operation.Invoke(_audioSourcesAll[i]);
        }
    }

    private void PauseSource(AudioSource audioSource)
    {
        audioSource.Pause();
    }
    private void StopSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }
    private void PlaySource(AudioSource audioSource)
    {
        audioSource.Play();
    }
    public void SoundOnPause(bool pause)
    {
        if (pause == true)
        {
            PauseAllSources();
            _levelMixer.audioMixer.SetFloat(_globalAudioValues.NameVolumeLevel, _globalAudioValues.VolumeLevelLow);
            _backgroundMixer.audioMixer.SetFloat(_globalAudioValues.NameVolumeBackgroundMusic, _globalAudioValues.VolumeBackgroundMusicLow);
        }
        else
        {
            PlayAllSources();
            _levelMixer.audioMixer.SetFloat(_globalAudioValues.NameVolumeLevel, _globalAudioValues.VolumeNormal);
            _backgroundMixer.audioMixer.SetFloat(_globalAudioValues.NameVolumeBackgroundMusic, _globalAudioValues.VolumeBackgroundNormal);
        }
    }
}