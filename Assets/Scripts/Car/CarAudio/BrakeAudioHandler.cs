using UniRx;
using UnityEngine;

public class BrakeAudioHandler : AudioPlayer
{
    private readonly AudioClip _brakeAudioClip;
    private readonly AudioClip _brake2AudioClip;
    private readonly AnimationCurve _brakeVolumeCurve;

    public BrakeAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip brakeAudioClip, AudioClip brake2AudioClip, AnimationCurve brakeVolumeCurve)
        : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _brakeAudioClip = brakeAudioClip;
        _brake2AudioClip = brake2AudioClip;
        _brakeVolumeCurve = brakeVolumeCurve;
    }

    public void PlayBrake()
    {
        TryPlayClip();
    }
    public void TrySetGroundClip()
    {
        if (AudioSource.clip != _brake2AudioClip)
        {
            SetClip(_brake2AudioClip);
        }
    }
    public void TrySetAsphaltClip()
    {
        if (AudioSource.clip != _brakeAudioClip)
        {
            SetClip(_brakeAudioClip);
        }
    }
    public void SetVolumeBrake(float volume)
    {
        SetVolume(_brakeVolumeCurve.Evaluate(volume));
    }
    public void SetMuteVolumeBrake()
    {
        SetVolume(0f);
    }
    public void StopPlayBrake()
    {
        StopPlay();
    }
}