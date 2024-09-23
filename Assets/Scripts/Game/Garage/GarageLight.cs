using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GarageLight
{
    private readonly float _intensityMax = 1.1f;
    private readonly float _intensityMin = 0.5f;
    private readonly float _duration = 0.8f;
    private readonly Light2D _lamp1;
    private readonly Light2D _lamp2;
    private readonly Light2D _globalLight2D;
    private readonly Color _colorForGarage;
    
    private CancellationTokenSource _cancellationTokenSource;
    private float _currentValueIntensity;
    public GarageLight(Light2D lamp1, Light2D lamp2, Light2D globalLight2D, Color colorForGarage)
    {
        _lamp1 = lamp1;
        _lamp2 = lamp2;
        _globalLight2D = globalLight2D;
        _colorForGarage = colorForGarage;
        _lamp1.intensity = _intensityMin;
        _lamp2.intensity = _intensityMin;
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
    }

    public void SetColorForGarage()
    {
        _globalLight2D.color = _colorForGarage;
    }

    public void SetDefaultColor()
    {
        _globalLight2D.color = Color.white;
    }
    public void LampOn()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _lamp1.intensity = _intensityMin;
        _lamp2.intensity = _intensityMin;
        DOTween.To(() => _intensityMin, x =>
            {
                _lamp1.intensity = x;
                _lamp2.intensity = x;
            }, _intensityMax, _duration)
            .SetEase(Ease.OutCubic).WithCancellation(_cancellationTokenSource.Token);
    }
    public async UniTask LampOff()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        await DOTween.To(() => _lamp1.intensity, x =>
            {
                _lamp1.intensity = x;
                _lamp2.intensity = x;
            }, _intensityMin, _duration)
            .SetEase(Ease.OutCubic).WithCancellation(_cancellationTokenSource.Token);
    }
}