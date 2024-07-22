using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Booster
{
    private readonly BoosterValues _boosterValues;
    private readonly Dictionary<Type, BoosterState> _dictionaryStates;
    private BoosterState _currentState;
    private bool _isBroken = false;

    public BoosterFuelTank BoosterFuelTank { get; private set; }
    public bool FuelAvailability => BoosterFuelTank.CheckFuel();
    public event Action OnBoosterDisable;
    public Booster(BoosterAudioHandler boosterAudioHandler, BoosterFuelTank boosterFuelTank,
        BoosterScrew boosterScrew, ParticleSystem particleSystemBooster,
        AnimationCurve increaseBoosterSoundCurve, AnimationCurve decreaseBoosterSoundCurve)
    {
        BoosterFuelTank = boosterFuelTank;
        _boosterValues = new BoosterValues();
        _dictionaryStates = new Dictionary<Type, BoosterState>()
        {
            {typeof(BoosterStateIncreaseRun), new BoosterStateIncreaseRun(_boosterValues, boosterScrew, boosterAudioHandler, increaseBoosterSoundCurve, particleSystemBooster)},
            {typeof(BoosterStateDecreaseStop), new BoosterStateDecreaseStop(_boosterValues, boosterScrew, boosterAudioHandler, decreaseBoosterSoundCurve, particleSystemBooster) },
            {typeof(BoosterStateOutFuelStop), new BoosterStateOutFuelStop(_boosterValues, boosterScrew, boosterAudioHandler, particleSystemBooster) },
            {typeof(BoosterStateOnBrokenStop), new BoosterStateOnBrokenStop(_boosterValues, boosterScrew, boosterAudioHandler, particleSystemBooster) },
        };
    }

    public void Dispose()
    {
        _currentState?.Exit();
    }
    public void RunBooster()
    {
        if (_isBroken == false)
        {
            // Debug.Log($"            TryRunBooster");

            SetStateIncrease();
        }
    }
    public void TryStopBooster()
    {
        if (_isBroken == false)
        {
            // Debug.Log($"            TryStopBooster");

            SetStateDecrease();
        }
    }
    public void StopBoosterOnOutFuel()
    {
        if (_isBroken == false)
        {
            SetStateStopOnOutFuel();
        }
    }
    public void StopBoosterOnBroken()
    {
        if (_isBroken == false)
        {
            SetStateStopOnBroken();
        }
    }
    public void Update()
    {
        _currentState?.Update();
    }
    private void SetStateIncrease()
    {
        SetState<BoosterStateIncreaseRun>();
    }
    private void SetStateDecrease()
    {
        SetState<BoosterStateDecreaseStop>();
    }
    private void SetStateStopOnOutFuel()
    {
        _isBroken = true;
        SetState<BoosterStateOutFuelStop>();
    }
    private void SetStateStopOnBroken()
    {
        _isBroken = true;
        SetState<BoosterStateOnBrokenStop>();
        OnBoosterDisable?.Invoke();
    }

    private void SetState<T>() where T : BoosterState
    {
        var type = typeof(T);
        if (_currentState?.GetType() == type)
        {
            return;
        }

        if (_dictionaryStates.TryGetValue(type, out var extractState))
        {
            _currentState?.Exit();
            Debug.Log($"            prev                     {_currentState?.GetType()}");

            _currentState = extractState;
            Debug.Log($"          curr                       {_currentState?.GetType()}");
            _currentState.Enter();
        }
    }
}