using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GlobalAudio : MonoBehaviour, ISoundPause, ICarAudio, ILevelAudio, IAudioSettingSwitch
{
    [SerializeField, BoxGroup("AudioSources"), HorizontalLine(color:EColor.White)] private AudioSource _audioSourceBackground;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _audioSourceUI;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _carAudioSourceForEngine;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _carAudioSourceForBooster;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _carAudioSourceForDestruction;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _carAudioSourceForHotWheels;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _carAudioSourceForOther;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _levelAudioSource;
    
    [SerializeField, BoxGroup("AudioGroups"), HorizontalLine(color:EColor.Yellow)] private AudioMixerGroup _masterMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _levelMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _backgroundMixer;
    [Inject] private AudioClipProvider _audioClipProvider;
    private GlobalAudioValues _globalAudioValues = new GlobalAudioValues();

    public UIAudioClipProvider UIAudioClipProvider => _audioClipProvider.UIAudioClipProvider;
    public CarAudioClipProvider CarAudioClipProvider => _audioClipProvider.CarAudioClipProvider;
    public LevelAudioClipProvider LevelAudioClipProvider => _audioClipProvider.LevelAudioClipProvider;
    public AudioSource AudioSourceUI => _audioSourceUI;
    public AudioSource CarAudioSourceForEngine => _carAudioSourceForEngine;
    public AudioSource CarAudioSourceForBooster => _carAudioSourceForBooster;
    public AudioSource CarAudioSourceForDestruction => _carAudioSourceForDestruction;
    public AudioSource CarAudioSourceForHotWheels => _carAudioSourceForHotWheels;
    public AudioSource CarAudioSourceForOther => _carAudioSourceForOther;
    public AudioSource LevelAudioSource => _levelAudioSource;
    private AudioSource[] _audioSources;
    private ILevelAudio _levelAudioImplementation;

    public event Action OnSoundChange; 
    public event Action<bool> OnMusicChange;
    public bool SoundOn => SoundReactiveProperty.Value;
    public bool MusicOn => MusicReactiveProperty.Value;
    public ReactiveProperty<bool> SoundReactiveProperty { get; private set; }
    public ReactiveProperty<bool> MusicReactiveProperty { get; private set; }

    public void Construct(bool keySound, bool keyMusic)
    {
        SoundReactiveProperty = new ReactiveProperty<bool>();
        MusicReactiveProperty = new ReactiveProperty<bool>();
        SoundReactiveProperty.Value = keySound;
        MusicReactiveProperty.Value = keyMusic;
        if (MusicOn == true)
        {
            PlayBackground();
        }
        _audioSources = new[]
        {
            _audioSourceUI, _carAudioSourceForEngine, _carAudioSourceForBooster,
            _carAudioSourceForOther, _levelAudioSource
        };
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
        MusicReactiveProperty.Value = false;
        OnMusicChange?.Invoke(MusicOn);
        StopBackground();
    }
    public void SetMusicOn()
    {
        MusicReactiveProperty.Value = true;
        OnMusicChange?.Invoke(MusicOn);
        PlayBackground();
    }
    public void SetSoundsOff()
    {
        SoundReactiveProperty.Value = false;
        OnSoundChange?.Invoke();
        _carAudioSourceForEngine.Stop();
        _carAudioSourceForBooster.Stop();
    }
    public void SetSoundsOn()
    {
        SoundReactiveProperty.Value = true;
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
            for (int i = 0; i < _audioSources.Length; i++)
            {
                _audioSources[i].Pause();
            }
            _levelMixer.audioMixer.SetFloat(_globalAudioValues.VolumeLevel, _globalAudioValues.VolumeLevelLow);
            _backgroundMixer.audioMixer.SetFloat(_globalAudioValues.VolumeBackgroundMusic, _globalAudioValues.VolumeBackgroundMusicLow);
        }
        else
        {
            for (int i = 0; i < _audioSources.Length; i++)
            {
                _audioSources[i].Play();
            }
            _levelMixer.audioMixer.SetFloat(_globalAudioValues.VolumeLevel, _globalAudioValues.VolumeNormal);
            _backgroundMixer.audioMixer.SetFloat(_globalAudioValues.VolumeBackgroundMusic, _globalAudioValues.VolumeNormal);
        }
    }
}