
using UniRx;
using UnityEngine;

public class DebrisAudioHandler : AudioPlayer
{
    private readonly Vector2 _volumeSection = new Vector2(0.7f, 1f);
    private readonly Vector2 _pitchSection = new Vector2(0.9f, 1.1f);
    private readonly TimeScalePitchHandler _timeScalePitchHandler;
    private readonly AudioClip _hit1DebrisAudioClip;
    private readonly AudioClip _hit2DebrisAudioClip;
    private readonly AudioClip _burnAudioClip;

    public DebrisAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        TimeScalePitchHandler timeScalePitchHandler, AudioClip hit1DebrisAudioClip, AudioClip hit2DebrisAudioClip, AudioClip burnAudioClip)
        : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _timeScalePitchHandler = timeScalePitchHandler;
        _hit1DebrisAudioClip = hit1DebrisAudioClip;
        _hit2DebrisAudioClip = hit2DebrisAudioClip;
        _burnAudioClip = burnAudioClip;
        _timeScalePitchHandler.OnPitchTimeWarped += SetPitch;
        _timeScalePitchHandler.IsTimeWarpedRP.Subscribe(_ =>
        {
            if (_timeScalePitchHandler.IsTimeWarpedRP.Value == true)
            {
                _timeScalePitchHandler.SetPitchValueNormalTimeScale(AudioSource.pitch);
            }
        });
    }
    public void Dispose()
    {
        _timeScalePitchHandler.OnPitchTimeWarped -= SetPitch;
        _timeScalePitchHandler.Dispose();
    }
    public void PlayDebrisHitSound()
    {
        TryPlayOneShotClipWithRandomSectionVolumeAndPitch(GetRandomAudioClip(new []{_hit1DebrisAudioClip, _hit2DebrisAudioClip}),
            _volumeSection, _pitchSection);
    }
    public void PlayBurnSound()
    {
        TryPlayOneShotClip(_burnAudioClip);
    }
}