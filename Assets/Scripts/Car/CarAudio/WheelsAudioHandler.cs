using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class WheelsAudioHandler : AudioPlayer
{
    private readonly float _volumeDefault = 0.01f;
    private readonly float _valueEndTimer = 1f;
    private readonly float _duration = 1.5f;
    private readonly float _deltaTimeMultiplier = 0.33f;
    private readonly AudioClip _wheelHitAudioClip;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private GroundAnalyzer _groundAnalyzer;
    private float _wheelsVolume;
    private bool _previousFrontWheelContactValue;
    private bool _previousBackWheelContactValue;
    private bool _timerOn = false;
    private ReactiveProperty<bool> _frontWheelContactReactiveProperty => _groundAnalyzer.FrontWheelContactReactiveProperty;
    private ReactiveProperty<bool> _backWheelContactReactiveProperty => _groundAnalyzer.BackWheelContactReactiveProperty;
    public BrakeAudioHandler BrakeAudioHandler { get; private set; }
    public WheelsAudioHandler(AudioSource forWheelsFriction, AudioSource forWheelsHit, ReactiveProperty<bool> soundReactiveProperty,
        AudioClip brakeAudioClip, AudioClip brake2AudioClip, AudioClip wheelHitAudioClip, AnimationCurve brakeVolumeCurve)
        : base(forWheelsHit, soundReactiveProperty)
    {
        _wheelHitAudioClip = wheelHitAudioClip;
        BrakeAudioHandler = new BrakeAudioHandler(forWheelsFriction, soundReactiveProperty, brakeAudioClip, brake2AudioClip, brakeVolumeCurve);
        SetVolume(_volumeDefault);
    }

    public void Dispose()
    {
        TryStopTimer();
    }
    public void Init(GroundAnalyzer groundAnalyzer)
    {
        _groundAnalyzer = groundAnalyzer;
        _frontWheelContactReactiveProperty.Subscribe(_ =>{ FrontWheelContactValueChanged(); });
        _backWheelContactReactiveProperty.Subscribe(_ => { BackWheelContactValueChanged(); });
    }

    private void FrontWheelContactValueChanged()
    {
        WheelContactValueChanged(_frontWheelContactReactiveProperty, ref _previousFrontWheelContactValue);
        _previousFrontWheelContactValue = _frontWheelContactReactiveProperty.Value;
    }
    private void BackWheelContactValueChanged()
    {
        WheelContactValueChanged(_backWheelContactReactiveProperty, ref _previousBackWheelContactValue);
        _previousBackWheelContactValue = _backWheelContactReactiveProperty.Value;
    }
    
    private void WheelContactValueChanged(ReactiveProperty<bool> property, ref bool previousValue)
    {
        if (property.Value == true && previousValue == false)
        {
            TryStopTimer();
            SetVolume(_wheelsVolume);
            TryPlayOneShotClip(_wheelHitAudioClip);
        }
        else if(property.Value == false)
        {
            _wheelsVolume = _volumeDefault;
            TryStartTimer();
        }
    }

    private void TryStartTimer()
    {
        if (_timerOn == false)
        {
            _timerOn = true;
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (_wheelsVolume < 1f)
                {
                    _wheelsVolume += Time.deltaTime * _deltaTimeMultiplier;
                }
                else
                {
                    TryStopTimer();
                }
            }).AddTo(_compositeDisposable);
        }
    }

    // private async UniTaskVoid TryStartTimer()
    // {
    //     if (_timerOn == false)
    //     {
    //         _timerOn = true;
    //         _wheelsVolume = _volumeDefault;
    //         await DOTween.To(() => _wheelsVolume, x => _wheelsVolume = x, _valueEndTimer, _duration).WithCancellation(_cancellationTokenSource.Token);
    //         SetVolume(_wheelsVolume);
    //         _timerOn = false;
    //     }
    // }

    private void TryStopTimer()
    {
        _compositeDisposable.Clear();
        _timerOn = false;
    }
}