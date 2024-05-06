using System;
using UniRx;
using UnityEngine;

public class DestructionAudioHandler : AudioPlayer, IDispose
{
    private readonly float _defaultVolume = 1f;
    private readonly AudioClip _carBurnAudioClip;
    private readonly AudioClip _engineBrokenAudioClip;
    private readonly AudioClip _carHardHitAudioClip;
    private readonly AudioClip _carSoftHitAudioClip;
    private readonly AudioClip _glassBreakingAudioClip;
    private readonly AudioClip _metalBendsAudioClip;
    private readonly AudioClip _driverNeckBrokeAudioClip;
    private readonly CompositeDisposable _compositeDisposableBurn = new CompositeDisposable();
    private readonly CompositeDisposable _compositeDisposableSoftHit = new CompositeDisposable();
    private readonly CompositeDisposable _compositeDisposableHardHit = new CompositeDisposable();
    private AnimationCurve _destructionAudioCurve;
    private bool _hitSoundIsPlay = false;

    public DestructionAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        AudioClip carBurnAudioClip, AudioClip carHardHitAudioClip, AudioClip carSoftHitAudioClip, AudioClip glassBreakingAudioClip,
        AudioClip metalBendsAudioClip, AudioClip engineBrokenAudioClip, AudioClip driverNeckBrokeAudioClip)
        : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _carBurnAudioClip = carBurnAudioClip;
        _carHardHitAudioClip = carHardHitAudioClip;
        _carSoftHitAudioClip = carSoftHitAudioClip;
        _glassBreakingAudioClip = glassBreakingAudioClip;
        _metalBendsAudioClip = metalBendsAudioClip;
        _engineBrokenAudioClip = engineBrokenAudioClip;
        _driverNeckBrokeAudioClip = driverNeckBrokeAudioClip;
    }
    public void Init(AnimationCurve destructionAudioCurve)
    {
        _destructionAudioCurve = destructionAudioCurve;
    }

    public void Dispose()
    {
        _compositeDisposableBurn.Clear();
        _compositeDisposableSoftHit.Clear();
        _compositeDisposableHardHit.Clear();
    }

    public void PlayHardHit(float force)
    {
        if (_hitSoundIsPlay == false)
        {
            _hitSoundIsPlay = true;
            SetVolume(_destructionAudioCurve.Evaluate(force));
            TryPlayOneShotClip(_carHardHitAudioClip);
            Observable.Timer(TimeSpan.FromSeconds(_carHardHitAudioClip.length)).Subscribe(_ =>
            {
                _hitSoundIsPlay = false;
                _compositeDisposableHardHit.Clear();
            }).AddTo(_compositeDisposableHardHit);
        }
    }

    public void PlaySoftHit(float force, string a)
    {
        if (_hitSoundIsPlay == false)
        {
            _hitSoundIsPlay = true;
            SetVolume(_destructionAudioCurve.Evaluate(force));
            TryPlayOneShotClip(_carSoftHitAudioClip);
            Observable.Timer(TimeSpan.FromSeconds(_carSoftHitAudioClip.length)).Subscribe(_ =>
            {
                _hitSoundIsPlay = false;
                _compositeDisposableSoftHit.Clear();
            }).AddTo(_compositeDisposableSoftHit);
        }
    }

    public void PlayGlassBreak()
    {
        TryPlayOneShotClip(_glassBreakingAudioClip);

    }

    public void PlayEngineBurn()
    {
        SetVolume(_defaultVolume);
        TryPlayOneShotClip(_engineBrokenAudioClip);
        Observable.Timer(TimeSpan.FromSeconds(_engineBrokenAudioClip.length)).Subscribe(_ =>
        {
            TryPlayClip(_carBurnAudioClip);
            _compositeDisposableBurn.Clear();
        }).AddTo(_compositeDisposableBurn);
    }

    public void PlayDriverNeckBroke()
    {
        TryPlayOneShotClip(_driverNeckBrokeAudioClip);
    }

    public void PlayRoofBends()
    {
        SetVolume(_defaultVolume);
        // Debug.Log($"                         PlayRoofBends:                     {AudioSource.volume}");
        TryPlayOneShotClip(_metalBendsAudioClip);
    }

    public void StopPlayEngineBurn()
    {
        StopPlay();
    }
}