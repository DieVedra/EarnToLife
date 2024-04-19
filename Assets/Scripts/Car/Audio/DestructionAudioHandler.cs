using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class DestructionAudioHandler : AudioPlayer, IDispose
{
    private readonly float _defaultVolume = 1f;
    private readonly AudioClip _carBurnAudioClip;
    private readonly AudioClip _engineClapAudioClip;
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

    public DestructionAudioHandler(AudioSource audioSource, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip carBurnAudioClip, AudioClip carHardHitAudioClip, AudioClip carSoftHitAudioClip, AudioClip glassBreakingAudioClip,
        AudioClip metalBendsAudioClip, AudioClip engineClapAudioClip, AudioClip driverNeckBrokeAudioClip)
        : base(audioSource, soundReactiveProperty)
    {
        _carBurnAudioClip = carBurnAudioClip;
        _carHardHitAudioClip = carHardHitAudioClip;
        _carSoftHitAudioClip = carSoftHitAudioClip;
        _glassBreakingAudioClip = glassBreakingAudioClip;
        _metalBendsAudioClip = metalBendsAudioClip;
        _engineClapAudioClip = engineClapAudioClip;
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
            // Debug.Log($"PlayHardHit");
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
            // Debug.Log($"PlaySoftHit   {a}");

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
        TryPlayOneShotClip(_engineClapAudioClip);
        Observable.Timer(TimeSpan.FromSeconds(_engineClapAudioClip.length)).Subscribe(_ =>
        {
            TryPlayClip(_carBurnAudioClip, true);
            _compositeDisposableBurn.Clear();
        }).AddTo(_compositeDisposableBurn);
    }

    public void PlayDriverNeckBroke()
    {
        TryPlayOneShotClip(_driverNeckBrokeAudioClip);
    }

    public void PlayRoofBends(float force)
    {
        SetVolume(_defaultVolume);
        Debug.Log($"                         PlayRoofBends:                     {AudioSource.volume}");
        TryPlayClip(_metalBendsAudioClip);
    }

    public void StopPlayEngineBurn()
    {
        StopPlay();
    }
}