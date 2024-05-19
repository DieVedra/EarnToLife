using System.Threading;
using UniRx;
using UnityEngine;

public class SuspensionAudioHandler
{
    private readonly float _minSoundSpeed = 5f;
    private readonly AudioPlayer _audioPlayerFrontSuspension;
    private readonly AudioPlayer _audioPlayerBackSuspension;
    private readonly AudioClip _suspensionAudioClip;
    private GroundAnalyzer _groundAnalyzer;
    private Speedometer _speedometer;
    private float _speedValueToPitch;
    private float _arithmeticMean;
    private float _pitchValue;
    public SuspensionAudioHandler(AudioSource carAudioSourceFrontSuspension, AudioSource carAudioSourceBackSuspension,
        ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip suspensionAudioClip)
    {
        _audioPlayerFrontSuspension = new AudioPlayer(carAudioSourceFrontSuspension, soundReactiveProperty, audioPauseReactiveProperty);
        _audioPlayerBackSuspension = new AudioPlayer(carAudioSourceBackSuspension, soundReactiveProperty, audioPauseReactiveProperty);
        _suspensionAudioClip = suspensionAudioClip;
    }
    public void CalculateVolumeFrontSuspension(float value)
    {
        SetVolumeSuspension(_audioPlayerFrontSuspension, value);
    }
    public void CalculateVolumeBackSuspension(float value)
    {
        SetVolumeSuspension(_audioPlayerBackSuspension, value);
    }
    public void Init(GroundAnalyzer groundAnalyzer, Speedometer speedometer)
    {
        _groundAnalyzer = groundAnalyzer;
        _speedometer = speedometer;
        _groundAnalyzer.FrontWheelContactReactiveProperty.Subscribe(_ =>
        {
            SetFrontSourceStatus(_groundAnalyzer.FrontWheelContactReactiveProperty.Value);
        });
        _groundAnalyzer.BackWheelContactReactiveProperty.Subscribe(_ =>
        {
            SetBackSourceStatus(_groundAnalyzer.BackWheelContactReactiveProperty.Value);
        });
        _audioPlayerFrontSuspension.TryPlayClip(_suspensionAudioClip);
        _audioPlayerFrontSuspension.SetVolume(0f);
        _audioPlayerBackSuspension.SetVolume(0f);
        _audioPlayerBackSuspension.TryPlayClip(_suspensionAudioClip);
    }

    private void SetVolumeSuspension(AudioPlayer audioPlayer, float value)
    {
        if (_speedometer.CurrentSpeedFloat > _minSoundSpeed)
        {
            audioPlayer.SetVolume(value);
        }
        else
        {
            audioPlayer.SetVolume(0f);
        }
    }
    private void SetFrontSourceStatus(bool key)
    {
        SetSourceStatus(_audioPlayerFrontSuspension, ref key);
    }
    private void SetBackSourceStatus(bool key)
    {
        SetSourceStatus(_audioPlayerBackSuspension, ref key);
    }

    private void SetSourceStatus(AudioPlayer audioPlayerBackSuspension, ref bool key)
    {
        if (key == true)
        {
            audioPlayerBackSuspension.SetPauseStatus(false);
        }
        else
        {
            audioPlayerBackSuspension.SetPauseStatus(true);
        }
    }
}