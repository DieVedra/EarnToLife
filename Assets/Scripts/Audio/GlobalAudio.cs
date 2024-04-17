using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class GlobalAudio : MonoBehaviour, ISoundPause, ICarAudio, ILevelAudio, IAudioSettingSwitch
{
    [SerializeField, BoxGroup("AudioSources"), HorizontalLine(color:EColor.White)] private AudioSource _audioSourceBackground;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _uI;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forEngine;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forBooster;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forDestruction;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forHotWheels1;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forHotWheels2;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forBrakes;
    [SerializeField, BoxGroup("AudioSources")] private AudioSource _forLevel;
    
    [SerializeField, BoxGroup("AudioGroups"), HorizontalLine(color:EColor.Yellow)] private AudioMixerGroup _masterMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _levelMixer;
    [SerializeField, BoxGroup("AudioGroups")] private AudioMixerGroup _backgroundMixer;
    [Inject] private AudioClipProvider _audioClipProvider;
    private GlobalAudioValues _globalAudioValues = new GlobalAudioValues();

    public UIAudioClipProvider UIAudioClipProvider => _audioClipProvider.UIAudioClipProvider;
    public CarAudioClipProvider CarAudioClipProvider => _audioClipProvider.CarAudioClipProvider;
    public LevelAudioClipProvider LevelAudioClipProvider => _audioClipProvider.LevelAudioClipProvider;
    public AudioSource UI => _uI;
    public AudioSource CarAudioSourceForEngine => _forEngine;
    public AudioSource CarAudioSourceForBooster => _forBooster;
    public AudioSource CarAudioSourceForDestruction => _forDestruction;
    public AudioSource CarAudioSourceForHotWheels1 => _forHotWheels1;
    public AudioSource CarAudioSourceForHotWheels2 => _forHotWheels2;
    public AudioSource CarAudioSourceForBrakes => _forBrakes;
    public AudioSource LevelAudioSource => _forLevel;
    private AudioSource[] _audioSources;
    private ILevelAudio _levelAudioImplementation;

    public event Action OnSoundChange; 
    // public event Action<bool> OnMusicChange;
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
            SetAndPlayBackground();
        }
        _audioSources = new[]
        {
            _uI, _forEngine, _forBooster, _forDestruction, _forHotWheels1, _forHotWheels2,
            _forBrakes, _forLevel
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
        OnSoundChange?.Invoke();
        StopAllSources();
        MuteMaster();
    }
    public void SetSoundsOn()
    {
        SoundReactiveProperty.Value = true;
        OnSoundChange?.Invoke();
        UnmuteMaster();
    }
    private void SetAndPlayBackground()
    {
        _audioSourceBackground.clip = _audioClipProvider.ClipBackground;
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
        for (int i = 0; i < _audioSources.Length; i++)
        {
            operation.Invoke(_audioSources[i]);
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
            _backgroundMixer.audioMixer.SetFloat(_globalAudioValues.NameVolumeBackgroundMusic, _globalAudioValues.VolumeNormal);
        }
    }
}