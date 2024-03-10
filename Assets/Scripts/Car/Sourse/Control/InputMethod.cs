using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class InputMethod
{
    protected const float DIRECTION_FORWARD = -1f;
    protected const float DIRECTION_BACKWARD = 1f;
    protected const float ROTATION_CLOCKWISE = 1f;
    protected const float ROTATION_COUNTER_CLOCKWISE = -1f;
    public SmoothnessTimer SmoothnessTimerForGas = new SmoothnessTimer();
    public SmoothnessTimer SmoothnessTimerForRotation = new SmoothnessTimer();
    public abstract bool CheckPressGas();
    public abstract bool CheckPressBreak();
    public abstract bool CheckPressRotation();
    public abstract bool CheckPressBoost();
}
