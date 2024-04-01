using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class CoupAnalyzer
{
    public readonly ReactiveProperty<bool> IsCoup = new ReactiveProperty<bool>();
    private readonly float _maxDotProduct = 0.7f;
    private readonly float _timeDelay = 4f;
    private readonly Transform _carTransform;
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    public bool CarIsCoupCurrentValue => IsCoup.Value;
    private float _time;
    private bool _timerActive = false;
    public event Action OnCarCouped;
    public CoupAnalyzer(Transform carTransform)
    {
        _carTransform = carTransform;
        IsCoup.Subscribe(_ =>
        {
            TryStartTimer();
            TryStopTimer();
        }).AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Clear();
        _timerActive = false;
    }
    public void Update()
    {
        if (Vector3.Dot(_carTransform.up, Vector3.down) > _maxDotProduct)
        {
            IsCoup.Value = true;
        }
        else
        {
            IsCoup.Value = false;
        }

        if (_timerActive == true)
        {
            _time -= Time.deltaTime;
            if (_time <= 0f)
            {
                OnCarCouped?.Invoke();
                StopTimer();
            }
        }
    }

    private void TryStartTimer()
    {
        if (IsCoup.Value == true && _timerActive == false)
        {
            _timerActive = true;
            _time = _timeDelay;
        }
    }
    private void TryStopTimer()
    {
        if (IsCoup.Value == false)
        {
            StopTimer();
        }
    }

    private void StopTimer()
    {
        _timerActive = false;
        _time = _timeDelay;
    }
}