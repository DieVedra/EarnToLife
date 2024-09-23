using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class DestructionAudioHandler : AudioPlayer, IDispose
{
    private readonly float _defaultVolume = 1f;
    private readonly TimeScalePitchHandler _timeScalePitchHandler;
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
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private AnimationCurve _destructionAudioCurve;
    private bool _hitSoundIsPlay = false;

    public DestructionAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty, ReactiveProperty<bool> audioPauseReactiveProperty,
        TimeScalePitchHandler timeScalePitchHandler, AudioClip carBurnAudioClip, AudioClip carHardHitAudioClip, AudioClip carSoftHitAudioClip, AudioClip glassBreakingAudioClip,
        AudioClip metalBendsAudioClip, AudioClip engineBrokenAudioClip, AudioClip driverNeckBrokeAudioClip)
        : base(audioSource, soundReactiveProperty, audioPauseReactiveProperty)
    {
        _timeScalePitchHandler = timeScalePitchHandler;
        _carBurnAudioClip = carBurnAudioClip;
        _carHardHitAudioClip = carHardHitAudioClip;
        _carSoftHitAudioClip = carSoftHitAudioClip;
        _glassBreakingAudioClip = glassBreakingAudioClip;
        _metalBendsAudioClip = metalBendsAudioClip;
        _engineBrokenAudioClip = engineBrokenAudioClip;
        _driverNeckBrokeAudioClip = driverNeckBrokeAudioClip;
        _timeScalePitchHandler.OnPitchTimeWarped += SetPitch;
        _timeScalePitchHandler.IsTimeWarpedRP.Subscribe(_ =>
        {
            if (_timeScalePitchHandler.IsTimeWarpedRP.Value == true)
            {
                _timeScalePitchHandler.SetPitchValueNormalTimeScale(AudioSource.pitch);
            }
        });
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
        _timeScalePitchHandler.OnPitchTimeWarped -= SetPitch;
        _timeScalePitchHandler.Dispose();
        _cancellationTokenSource.Cancel();
    }

    public void SoftStopSound()
    {
        AudioSource.DOFade(0f, 1.5f).OnComplete(()=>
        {
            StopPlay();
            Debug.Log($"    AudioSourceDestr    {AudioSource.volume} ");
        }).WithCancellation(_cancellationTokenSource.Token);
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

    public void PlaySoftHit(float force)
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

    public void PlayEngineBurnSound()
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
        TryPlayOneShotClip(_metalBendsAudioClip);
    }
}