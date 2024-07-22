
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoosterStateDecreaseStop : BoosterState
{
    private readonly float _addToSmoothRotate = 0.2f;
    private readonly float _smoothRotateMultiplier = 0.002f;
    private readonly AnimationCurve _decreaseBoosterSoundCurve;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isStopped;
    public BoosterStateDecreaseStop(BoosterValues boosterValues, BoosterScrew boosterScrew, BoosterAudioHandler boosterAudioHandler,
        AnimationCurve decreaseBoosterSoundCurve, ParticleSystem particleSystemBooster)
        :base(boosterScrew, boosterAudioHandler, boosterValues, particleSystemBooster)
    {
        _decreaseBoosterSoundCurve = decreaseBoosterSoundCurve;
    }

    public override void Enter()
    {
        _isStopped = false;
        _cancellationTokenSource = new CancellationTokenSource();
        Stop().Forget();
    }

    private async UniTask Stop()
    {
        ParticleSystemBooster.Stop();
        await PitchDecrease();
        _isStopped = false;
        // _cancellationTokenSource = new CancellationTokenSource();
        await ScrewSmoothStop();
    }

    private async UniTask PitchDecrease()
    {
        while (_isStopped == false)
        {
            if (BoosterValues.CurrentValue > BoosterValues.StartIncreaseValue)
            {
                BoosterValues.CurrentValue -= Time.deltaTime * Time.timeScale;
                if (BoosterValues.CurrentValue > _addToSmoothRotate)
                {
                    BoosterScrew.RotateScrew(ref BoosterValues.CurrentCurveValue);
                }
                else
                {
                    BoosterScrew.RotateScrew(_addToSmoothRotate);
                }
                CalculateDecreaseValue();
                BoosterAudioHandler.PitchControl(ref BoosterValues.CurrentCurveValue);
            }
            else
            {
                _isStopped = true;
            }
            await UniTask.NextFrame(_cancellationTokenSource.Token);
        }

        BoosterAudioHandler.StopPlayRunBoosterImmediately();
    }

    private async UniTask ScrewSmoothStop()
    {
        BoosterValues.CurrentValue = _addToSmoothRotate;
        while (_isStopped == false)
        {
            if (BoosterValues.CurrentValue > BoosterValues.StartIncreaseValue)
            {
                BoosterValues.CurrentValue -= _smoothRotateMultiplier * Time.timeScale;
                BoosterScrew.RotateScrew(ref BoosterValues.CurrentValue);
            }
            else
            {
                _isStopped = true;
            }
            await UniTask.NextFrame(_cancellationTokenSource.Token);
        }
    }
    public override void Exit()
    {
        _cancellationTokenSource.Cancel();
        _isStopped = true;
    }

    private void CalculateDecreaseValue()
    {
        BoosterValues.CurrentCurveValue = _decreaseBoosterSoundCurve.Evaluate(BoosterValues.CurrentValue) * Time.timeScale;
    }
}