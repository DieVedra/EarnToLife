using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class DebrisBarrelEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField, MinMaxSlider(0f, 1)] private Vector2 _sectionLifeTime; 
    [SerializeField, MinMaxSlider(0f,1f)] private Vector2 _sectionSize;
    [SerializeField] private AnimationCurve _curve;
    private readonly float _deltaTimeMultiplier = 0.33f;
    private readonly float _delayAfterPlay = 0.5f;
    private Action<DebrisBarrelEffect> _callBack;
    private readonly float _endValue = 0f;
    private float _currentTime;
    private float _lifeTime;
    private float _size;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private ParticleSystem.MainModule _mainModule;
    private float Duration => _effect.main.duration;

    public void Init(Transform parent, Action<DebrisBarrelEffect> callBack)
    {
        transform.position = parent.position;
        transform.SetParent(parent);
        _callBack = callBack;
    }
    public void PlayEffect(float valueIntensity)
    {
        _mainModule = _effect.main;
        _lifeTime = Mathf.Lerp(_sectionLifeTime.x, _sectionLifeTime.y, valueIntensity);
        _size = Mathf.Lerp( _sectionSize.x, _sectionSize.y, valueIntensity);
        
        _mainModule.startLifetime = _lifeTime;
        _mainModule.startSize = _size;
        _effect.Play();
        SubscribeUpdate();
    }

    private void StopEffect()
    {
        _effect.Stop();
        _compositeDisposable.Clear();
    }
    private void SubscribeUpdate()
    {
        _currentTime = Duration;
        Observable.EveryUpdate().Delay(TimeSpan.FromSeconds(_delayAfterPlay)).Subscribe(x =>
        {
            if (_currentTime <= 0f)
            {
                StopEffect();
                _callBack.Invoke(this);
                return;
            }
            _currentTime -= Time.deltaTime * _deltaTimeMultiplier;
            float value = Mathf.InverseLerp(_endValue, Duration, _currentTime);
            _mainModule.startSize = Mathf.Lerp( _endValue, _size, _curve.Evaluate(value));
            _mainModule.startLifetime = Mathf.Lerp(_endValue, _lifeTime, _curve.Evaluate(value));
        }).AddTo(_compositeDisposable);
    }

    private void OnDisable()
    {
        StopEffect();
    }
}