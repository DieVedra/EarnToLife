using System.Threading;
using UnityEngine;

public class BoosterStateIncreaseRun : BoosterState
{
    private readonly AnimationCurve _increaseBoosterSoundCurve;
    public BoosterStateIncreaseRun(BoosterValues boosterValues, BoosterScrew boosterScrew, BoosterAudioHandler boosterAudioHandler,
        AnimationCurve increaseBoosterSoundCurve, ParticleSystem particleSystemBooster)
        :base(boosterScrew, boosterAudioHandler, boosterValues, particleSystemBooster)
    {
        _increaseBoosterSoundCurve = increaseBoosterSoundCurve;
    }

    public override void Enter()
    {
        BoosterAudioHandler.PlayRunBooster();
        ParticleSystemBooster.Play();

    }

    public override void Update()
    {
        if (BoosterValues.CurrentValue < BoosterValues.StartDecreaseValue)
        {
            BoosterValues.CurrentValue += Time.deltaTime * Time.timeScale;
            CalculateIncreaseValue();
        }
        BoosterScrew.RotateScrew(ref BoosterValues.CurrentValue);
        BoosterAudioHandler.PitchControl(ref BoosterValues.CurrentCurveValue);
    }
    private void CalculateIncreaseValue()
    {
        BoosterValues.CurrentCurveValue = _increaseBoosterSoundCurve.Evaluate(BoosterValues.CurrentValue) * Time.timeScale;
    }
}