using System;
using System.Collections.Generic;
using UnityEngine;

public class CarFSM : IStateSetter
{
    private Dictionary<Type, CarState> _dictionaryStates;
    public CarState CurrentState { get; private set; }
    private bool _isLastState = false;
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
        if (_isLastState == false)
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
                Debug.Log($"CurrentState: {CurrentState}");

                CurrentState.Enter();
            }
        }
    }

    public void SetKeyLastState()
    {
        _isLastState = true;
    }
    public void Update()
    {
        CurrentState?.Update();
        // Debug.Log($"CurrentState: {CurrentState}");
    }
    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}
