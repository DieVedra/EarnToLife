using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
public class DebrisEffect : MonoBehaviour
{
    [SerializeField, MinMaxSlider(0f, 1)] private Vector2 _sectionLifeTime;
    [SerializeField, HorizontalLine(color: EColor.Green)] private AnimationCurve _curveAttenuation;
    [SerializeField] private AnimationCurve _curveDurationByDistance;
    [SerializeField, HorizontalLine(color: EColor.Blue), BoxGroup("PolygonColliderValuesCurves")] private AnimationCurve _curveSizePolygon;
    [SerializeField, BoxGroup("PolygonColliderValuesCurves")] private AnimationCurve _curveLifetimePolygon;
    [SerializeField, BoxGroup("CircleColliderValuesCurves")] private AnimationCurve _curveSizeCircle;
    [SerializeField, BoxGroup("CircleColliderValuesCurves")] private AnimationCurve _curveLifetimeCircle;
    [SerializeField, BoxGroup("BoxColliderValuesCurves")] private AnimationCurve _curveSizeBox;
    [SerializeField, BoxGroup("BoxColliderValuesCurves")] private AnimationCurve _curveLifetimeBox;
    [SerializeField, BoxGroup("CapsuleColliderValuesCurves")] private AnimationCurve _curveSizeCapsule;
    [SerializeField, BoxGroup("CapsuleColliderValuesCurves")] private AnimationCurve _curveLifetimeCapsule;
    private AnimationCurve _currentCurveSize;
    private AnimationCurve _currentCurveLifetime;
    private ParticleSystem _effectSmoke;
    private ParticleSystem _effectBurn;
    private readonly float _deltaTimeMultiplier = 0.33f;
    private readonly float _delayAfterPlay = 0.5f;
    private readonly float _stopBurnValue = 0.45f;
    private readonly float _startSmokeValue = 0.7f;
    private readonly float _endValue = 0f;
    private float _currentTimeSmoke;
    private float _currentTimeBurn;
    private float _lifeTimeFire;
    private float _sizeFire;
    private float _lifeTimeSmoke;
    private float _sizeSmoke;
    private float _intensityByDistance;
    private float _durationTimeSmoke;
    private float _durationTimeBurn;
    private DebrisFragment _debrisFragment;
    private Transform _transform;
    private CompositeDisposable _compositeDisposableSmoke = new CompositeDisposable();
    private CompositeDisposable _compositeDisposableBurn = new CompositeDisposable();
    private Action<DebrisEffect> _callBack;
    private ParticleSystem.MainModule _mainModuleSmoke;
    private ParticleSystem.MainModule _mainModuleFire;
    private bool _isSmoked;
    private bool _isBurned;

    public void Construct(ParticleSystem effectSmoke, ParticleSystem effectBurn, Action<DebrisEffect> callBack)
    {
        _effectSmoke = effectSmoke;
        _effectBurn = effectBurn;
        _mainModuleSmoke = _effectSmoke.main;
        _mainModuleFire = _effectBurn.main;
        _callBack = callBack;
        _transform = transform;
        
    }
    public void PlayEffectTo(DebrisFragment debrisFragment, float intensityByDistance, bool isBurn)
    {
        _transform.position = debrisFragment.FragmentTransform.position;
        _transform.SetParent(debrisFragment.FragmentTransform);
        _intensityByDistance = intensityByDistance;
        _debrisFragment = debrisFragment;
        SetCurveByTypeCollider();
        if (isBurn == true)
        {
            _effectBurn.gameObject.SetActive(true);
            CustomizationFire();
            CustomizationSmoke();
            BurnAttenuationSubscribeAndSmokeAttenuationAfter();
        }
        else
        {
            _effectBurn.gameObject.SetActive(false);
            CustomizationSmoke();
            SmokeAttenuationSubscribe();
        }
    }

    private void StopEffects()
    {
        _effectBurn?.Stop();
        _effectSmoke?.Stop();
        _compositeDisposableSmoke?.Clear();
        _compositeDisposableBurn?.Clear();
    }
    private void SmokeAttenuationSubscribe()
    {
        if (_isSmoked == false)
        {
            _isSmoked = true;
            _currentTimeSmoke = _durationTimeSmoke;
            _effectSmoke.Play();
            Observable.EveryUpdate().Delay(TimeSpan.FromSeconds(_delayAfterPlay)).Subscribe(x =>
            {
                if (_currentTimeSmoke <= 0f)
                {
                    _effectSmoke.Stop();
                    _compositeDisposableSmoke.Clear();
                    _callBack?.Invoke(this);
                    return;
                }

                _currentTimeSmoke -= Time.deltaTime;
                float value = Mathf.InverseLerp(_endValue, _durationTimeSmoke, _currentTimeSmoke);
                _mainModuleSmoke.startSize = Mathf.Lerp( _endValue, _sizeSmoke, _curveAttenuation.Evaluate(value));
                _mainModuleSmoke.startLifetime = Mathf.Lerp(_endValue, _lifeTimeSmoke, _curveAttenuation.Evaluate(value));
            }).AddTo(_compositeDisposableSmoke);
        }
    }
    private void BurnAttenuationSubscribeAndSmokeAttenuationAfter()
    {
        if (_isBurned == false)
        {
            _isBurned = true;
            _currentTimeBurn = _durationTimeBurn;
            _effectBurn.Play();
            Observable.EveryUpdate().Delay(TimeSpan.FromSeconds(_delayAfterPlay)).Subscribe(x =>
            {
                float value = Mathf.InverseLerp(_endValue, _durationTimeBurn, _currentTimeBurn);
                if (_currentTimeBurn <= _stopBurnValue)
                {
                    _effectBurn.Stop();
                    _compositeDisposableBurn.Clear();
                    return;
                }

                if (value <= _startSmokeValue)
                {
                    SmokeAttenuationSubscribe();
                }
                _currentTimeBurn -= Time.deltaTime * _deltaTimeMultiplier;
                _mainModuleFire.startSize = Mathf.Lerp( _endValue, _sizeFire, _curveAttenuation.Evaluate(value));
                _mainModuleFire.startLifetime = Mathf.Lerp(_endValue, _lifeTimeFire, _curveAttenuation.Evaluate(value));
            }).AddTo(_compositeDisposableBurn);
        }
    }

    private void CustomizationSmoke()
    {
        _durationTimeSmoke = _curveDurationByDistance.Evaluate(_intensityByDistance);
        _sizeSmoke = _currentCurveSize.Evaluate(_debrisFragment.SizeFragment);
        _lifeTimeSmoke = _currentCurveLifetime.Evaluate(_debrisFragment.SizeFragment);
        _mainModuleSmoke.startLifetime = _lifeTimeSmoke;
        _mainModuleSmoke.startSize = _sizeSmoke;
    }

    private void CustomizationFire()
    {
        _durationTimeBurn = _curveDurationByDistance.Evaluate(_intensityByDistance);
        _sizeFire = _currentCurveSize.Evaluate(_debrisFragment.SizeFragment);
        _lifeTimeFire = _currentCurveLifetime.Evaluate(_debrisFragment.SizeFragment);
        _mainModuleFire.startLifetime = _lifeTimeFire;
        _mainModuleFire.startSize = _sizeFire;
    }

    private void SetCurveByTypeCollider()
    {
        switch (_debrisFragment.TypeCollider)
        {
            case TypeCollider.Polygon:
                SetCurrentCurve(_curveSizePolygon, _curveLifetimePolygon);
                break;
            case TypeCollider.Circle:
                SetCurrentCurve(_curveSizeCircle, _curveLifetimeCircle);
                break;
            case TypeCollider.Capsule:
                SetCurrentCurve(_curveSizeCapsule, _curveLifetimeCapsule);
                break;
            case TypeCollider.Box:
                SetCurrentCurve(_curveSizeBox, _curveLifetimeBox);
                break;
            case TypeCollider.Other:
                SetCurrentCurve(_curveSizeBox, _curveLifetimeBox);
                break;
        }
    }

    private void SetCurrentCurve(AnimationCurve curveSize, AnimationCurve curveLifeTime)
    {
        _currentCurveSize = curveSize;
        _currentCurveLifetime = curveLifeTime;
    }
    private void OnDisable()
    {
        StopEffects();
    }
}