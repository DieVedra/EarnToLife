using System.Threading;
using UnityEngine;

public class BoosterStateIncreaseRun : BoosterState
{
    private readonly Rigidbody2D _rigidbody;
    private readonly Transform _transform;
    private readonly AnimationCurve _increaseBoosterSoundCurve;
    private readonly float _force;
    private float _currentForceValue;

    public BoosterStateIncreaseRun(BoosterValues boosterValues, BoosterScrew boosterScrew, BoosterAudioHandler boosterAudioHandler, Rigidbody2D rigidbody,
        AnimationCurve increaseBoosterSoundCurve, ParticleSystem particleSystemBooster, float force)
        :base(boosterScrew, boosterAudioHandler, boosterValues, particleSystemBooster)
    {
        _rigidbody = rigidbody;
        _transform = rigidbody.transform;
        _increaseBoosterSoundCurve = increaseBoosterSoundCurve;
        _force = force;
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
        _currentForceValue = _force * BoosterValues.CurrentCurveValue;
        _rigidbody.AddForce(_transform.right * _currentForceValue);
    }
    private void CalculateIncreaseValue()
    {
        BoosterValues.CurrentCurveValue = _increaseBoosterSoundCurve.Evaluate(BoosterValues.CurrentValue) * Time.timeScale;
    }
}