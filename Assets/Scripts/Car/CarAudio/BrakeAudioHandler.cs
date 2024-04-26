using UniRx;
using UnityEngine;

public class BrakeAudioHandler : AudioPlayer
{
    private readonly AudioClip _brakeAudioClip;
    private readonly AnimationCurve _brakeVolumeCurve;

    public BrakeAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip brakeAudioClip, AnimationCurve brakeVolumeCurve)
        : base(audioSource, soundReactiveProperty)
    {
        _brakeAudioClip = brakeAudioClip;
        _brakeVolumeCurve = brakeVolumeCurve;
    }
    public void PlayBrake()
    {
        TryPlayClip(_brakeAudioClip, true);
    }
    public void SetVolumeBrake(float volume)
    {
        SetVolume(_brakeVolumeCurve.Evaluate(volume));
    }
    public void SetMuteVolumeBrake()
    {
        SetVolumeBrake(0f);
    }
    public void StopPlayBrake()
    {
        StopPlay();
    }
}