
using System;
using System.Collections.Generic;

public class TimeScalerFSM
{
    private Dictionary<Type, TimeScalerState> _dictionaryStates;
    public TimeScalerState CurrentState { get; private set; }
    public bool PreviousStateIsWarp { get; private set; } = false;

    public TimeScalerFSM(Dictionary<Type, TimeScalerState> dictionaryStates)
    {
        _dictionaryStates = dictionaryStates;
    }
    public void Dispose()
    {
        CurrentState.Exit();
    }
    public bool SetState<T>() where T : TimeScalerState
    {
        var nextType = typeof(T);
        if (CurrentState?.GetType() == nextType)
        {
            return false;
        }
        else
        {
            if (CurrentState?.GetType() == typeof(TimeScalerWarpState) && nextType == typeof(TimeScalerStopState))
            {
                PreviousStateIsWarp = true;
            }
            else
            {
                PreviousStateIsWarp = false;
            }

            CurrentState?.Exit();
            if (_dictionaryStates.TryGetValue(nextType, out var extractState))
            {
                CurrentState = extractState;
                CurrentState?.Enter();
            }
            return true;
        }
    }
}