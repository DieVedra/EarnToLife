
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class BoosterStateDecreaseStop : BoosterState
{
    private readonly float _addToSmoothRotate = 0.2f;
    private readonly float _smoothRotateMultiplier = 0.002f;
    private readonly IGamePause _gamePause;
    private readonly AnimationCurve _decreaseBoosterSoundCurve;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private bool _isStopped;
    public BoosterStateDecreaseStop(IGamePause gamePause, BoosterValues boosterValues, BoosterScrew boosterScrew, BoosterAudioHandler boosterAudioHandler,
        AnimationCurve decreaseBoosterSoundCurve, ParticleSystem particleSystemBooster)
        :base(boosterScrew, boosterAudioHandler, boosterValues, particleSystemBooster)
    {
        _gamePause = gamePause;
        _decreaseBoosterSoundCurve = decreaseBoosterSoundCurve;
    }

    public override void Enter()
    {
        ParticleSystemBooster.Stop();
        SubscribePitchDecrease();
    }

    private void SubscribePitchDecrease()
    {
        Observable.EveryUpdate().Where(_=>_gamePause.IsPause == false).Subscribe(_ =>
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
                _compositeDisposable.Clear();
                BoosterAudioHandler.StopPlayRunBoosterImmediately();
                SubscribeScrewSmoothStop();
            } 
        }).AddTo(_compositeDisposable);
    }
    private void SubscribeScrewSmoothStop()
    {
        BoosterValues.CurrentValue = _addToSmoothRotate;
        Observable.EveryUpdate().Where(_=>_gamePause.IsPause == false).Subscribe(_ =>
        {
            if (BoosterValues.CurrentValue > BoosterValues.StartIncreaseValue)
            {
                BoosterValues.CurrentValue -= _smoothRotateMultiplier * Time.timeScale;
                BoosterScrew.RotateScrew(ref BoosterValues.CurrentValue);
            }
            else
            {
                _compositeDisposable.Clear();
            }
        }).AddTo(_compositeDisposable);
    }
    public override void Exit()
    {
        _compositeDisposable.Clear();
    }

    private void CalculateDecreaseValue()
    {
        BoosterValues.CurrentCurveValue = _decreaseBoosterSoundCurve.Evaluate(BoosterValues.CurrentValue) * Time.timeScale;
    }
}