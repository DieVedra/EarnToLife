using UnityEngine;

public class Engine
{
    private readonly float _overclockingMultiplier;
    private AnimationCurve _engineAccelerationCurve;
    private FuelTank _fuelTank;
    private CarAudioHandler _carAudioHandler;
    private float _currentTime = 0f;
    public float CurrentEngineSpeed { get; private set; }
    public Engine(AnimationCurve engineAccelerationCurve, CarAudioHandler carAudioHandler, float overclockingMultiplier)
    {
        _overclockingMultiplier = overclockingMultiplier;
        _engineAccelerationCurve = engineAccelerationCurve;
        _carAudioHandler = carAudioHandler;
    }
    public void AccelerationEngine()
    {
        if (_currentTime < 1f)
        {
            _currentTime += Time.deltaTime * _overclockingMultiplier;
        }
        SetCurrentEngineSpeed();
        SetPitch();
    }

    public void BreakingEngine()
    {
        if (_currentTime > 0f)
        {
            _currentTime -= Time.deltaTime * 3f;
        }
        SetCurrentEngineSpeed();
        SetPitch();
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
