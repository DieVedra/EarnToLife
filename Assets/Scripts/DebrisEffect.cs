using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
public class DebrisEffect : MonoBehaviour
{
    [SerializeField, MinMaxSlider(0f, 1)] private Vector2 _sectionLifeTime;
    [SerializeField, MinMaxSlider(0f,1f)] private Vector2 _sectionSize;
    [SerializeField] private AnimationCurve _curve;
    private ParticleSystem _effectSmoke;
    private ParticleSystem _effectBurn;
    private readonly float _deltaTimeMultiplier = 0.33f;
    private readonly float _delayAfterPlay = 0.5f;
    private Action<DebrisEffect> _callBack;
    private readonly float _endValue = 0f;
    private float _currentTime;
    private float _lifeTime;
    private float _size;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private ParticleSystem.MainModule _mainModuleSmoke;
    private ParticleSystem.MainModule _mainModuleFire;
    // private float Duration => _effect.main.duration;
    private float Duration;

    public void Construct(ParticleSystem effectSmoke, ParticleSystem effectBurn, Action<DebrisEffect> callBack)
    {
        _effectSmoke = effectSmoke;
        _effectBurn = effectBurn;
        _callBack = callBack;
    }
    public void PlayEffectTo(DebrisFragment debrisFragment, float valueIntensityByDistance)
    {
        transform.position = debrisFragment.FragmentTransform.position;
        transform.SetParent(debrisFragment.FragmentTransform);
        _mainModuleSmoke = _effectSmoke.main;
        _mainModuleFire = _effectBurn.main;
        
        _lifeTime = Mathf.Lerp(_sectionLifeTime.x, _sectionLifeTime.y, valueIntensityByDistance);
        _size = Mathf.Lerp( _sectionSize.x, _sectionSize.y, valueIntensityByDistance);
        
        _mainModuleSmoke.startLifetime = _lifeTime;
        _mainModuleSmoke.startSize = _size;
        // _effect.Play();
        SubscribeUpdate();
    }

    private void StopEffect()
    {
        // _effect.Stop();
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
            _mainModuleSmoke.startSize = Mathf.Lerp( _endValue, _size, _curve.Evaluate(value));
            _mainModuleSmoke.startLifetime = Mathf.Lerp(_endValue, _lifeTime, _curve.Evaluate(value));
        }).AddTo(_compositeDisposable);
    }

    private void OnDisable()
    {
        StopEffect();
    }
}