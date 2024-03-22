using UnityEngine;

public class Engine
{
    private readonly float _overclockingMultiplier;
    private readonly float _breakingEngineMultiplier = 3f;
    private readonly AnimationCurve _engineAccelerationCurve;
    private readonly CarAudioHandler _carAudioHandler;
    private readonly Exhaust _exhaust;
    private float _currentTime = 0f;
    public float CurrentEngineSpeed { get; private set; }
    public Engine(AnimationCurve engineAccelerationCurve, CarAudioHandler carAudioHandler, Exhaust exhaust, float overclockingMultiplier)
    {
        _overclockingMultiplier = overclockingMultiplier;
        _engineAccelerationCurve = engineAccelerationCurve;
        _carAudioHandler = carAudioHandler;
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
        _carAudioHandler.PitchControl(CurrentEngineSpeed);
    }
    private void SetCurrentEngineSpeed()
    {
        CurrentEngineSpeed = _engineAccelerationCurve.Evaluate(_currentTime);
    }
}
