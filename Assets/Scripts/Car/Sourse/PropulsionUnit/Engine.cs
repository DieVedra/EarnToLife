using UnityEngine;

public class Engine
{
    private readonly float _overclockingMultiplier;
    private readonly float _breakingEngineMultiplier = 3f;
    private readonly AnimationCurve _engineAccelerationCurve;
    private readonly EngineAudioHandler _engineAudioHandler;
    private readonly Exhaust _exhaust;
    private float _currentTime = 0f;
    public float CurrentEngineSpeed { get; private set; }
    public Engine(AnimationCurve engineAccelerationCurve, EngineAudioHandler engineAudioHandler, Exhaust exhaust, float overclockingMultiplier)
    {
        _overclockingMultiplier = overclockingMultiplier;
        _engineAccelerationCurve = engineAccelerationCurve;
        _engineAudioHandler = engineAudioHandler;
        _exhaust = exhaust;
    }
    public void AccelerationEngine()
    {
        if (_currentTime < 1f)
        {
            _currentTime += Time.deltaTime * _overclockingMultiplier;
        }
        SetCurrentEngineSpeed();
        SetPitch();
        _exhaust.SetSmokeSpeed(CurrentEngineSpeed);
    }

    public void BreakingEngine()
    {
        if (_currentTime > 0f)
        {
            _currentTime -= Time.deltaTime * _breakingEngineMultiplier;
        }
        SetCurrentEngineSpeed();
        SetPitch();
        _exhaust.SetSmokeSpeed(CurrentEngineSpeed);

    }
    private void SetPitch() 
    {
        _engineAudioHandler.PitchControl(CurrentEngineSpeed);
    }
    private void SetCurrentEngineSpeed()
    {
        CurrentEngineSpeed = _engineAccelerationCurve.Evaluate(_currentTime);
    }
}
