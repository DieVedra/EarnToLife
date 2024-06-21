using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class CoupAnalyzer
{
    public readonly ReactiveProperty<bool> IsCoup = new ReactiveProperty<bool>();
    private readonly float _maxDotProduct = 0.7f;
    private readonly float _maxDotProductToTilted = -0.1f;
    private readonly Transform _carTransform;
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    public bool CarIsCoup => IsCoup.Value;
    public bool CarIsTilted { get; private set; }
    private float _dot;
    public CoupAnalyzer(Transform carTransform)
    {
        _carTransform = carTransform;
    }
    public void Dispose()
    {
        _compositeDisposable.Clear();
    }
    public void Update()
    {
        _dot = Vector3.Dot(_carTransform.up, Vector3.down);
        SetCoup(_dot);
        SetTilt(_dot);
    }
    private void SetCoup(float dot)
    {
        if (dot > _maxDotProduct)
        {
            IsCoup.Value = true;
        }
        else
        {
            IsCoup.Value = false;
        }
    }
    private void SetTilt(float dot)
    {
        if (dot > _maxDotProductToTilted)
        {
            CarIsTilted = true;
        }
        else
        {
            CarIsTilted = false;
        }
    }
}