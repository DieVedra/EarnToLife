using System;
using System.Collections.Generic;
using UnityEngine;

public class CarFSM : IStateSetter
{
    private Dictionary<Type, CarState> _dictionaryStates;
    public CarState CurrentState { get; private set; }
    public CarFSM(Dictionary<Type, CarState> dictionaryStates)
    {
        _dictionaryStates = dictionaryStates;
    }

    public void Dispose()
    {
        CurrentState.Exit();
    }
    public void SetState<T>() where T : CarState
    {
        var type = typeof(T);
        if (CurrentState != null)
        {
            if (CurrentState?.GetType() == type)
            {
                return;
            }
            CurrentState.Exit();
        }
        if (_dictionaryStates.TryGetValue(type, out var extractState))
        {
            CurrentState = extractState;
            CurrentState.Enter();
        }
    }
    public void Update()
    {
        CurrentState?.Update();
    }
}
