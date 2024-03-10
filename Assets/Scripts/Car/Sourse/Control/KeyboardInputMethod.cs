using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputMethod : InputMethod
{
    private bool _boosterAvailable;
    private bool _gunAvailable;
    public KeyboardInputMethod(bool boosterAvailable, bool gunAvailable)
    {
        _boosterAvailable = boosterAvailable;
        _gunAvailable = gunAvailable;
    }
    public override bool CheckPressGas()
    {
        if (Input.GetKey(KeyCode.A))
        {
            TrySmoothGas(DIRECTION_BACKWARD);
            return true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TrySmoothGas(DIRECTION_FORWARD);
            return true;
        }
        else
        {
            StopSmoothTimerGas();
            return false;
        }
    }
    public override bool CheckPressRotation()
    {
        if (Input.GetKey(KeyCode.E))
        {
            TrySmoothRotation(ROTATION_COUNTER_CLOCKWISE);
            return true;
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            TrySmoothRotation(ROTATION_CLOCKWISE);
            return true;
        }
        else
        {
            StopSmoothTimerRotation();
            return false;
        }
    }
    public override bool CheckPressBreak()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Space))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool CheckPressBoost()
    {
        if (_boosterAvailable == true && Input.GetKey(KeyCode.LeftShift))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckPressFire()
    {
        if (_gunAvailable == true && Input.GetKey(KeyCode.F))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckPressNextTarget()
    {
        if (_gunAvailable == true && Input.GetKey(KeyCode.C))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void TurnOffCheckBooster()
    {
        _boosterAvailable = false;
    }
    public void TurnOffCheckGun()
    {
        _gunAvailable = false;
    }
    private void TrySmoothRotation(float direction)
    {
        TrySmooth(SmoothnessTimerForRotation, direction);
    }
    private void TrySmoothGas(float direction)
    {
        TrySmooth(SmoothnessTimerForGas, direction);
    }
    private void TrySmooth(SmoothnessTimer smoothnessTimer, float direction)
    {
        if (smoothnessTimer.TimerOn == false)
        {
            smoothnessTimer.StartTimer(direction);
        }
    }
    private void StopSmoothTimerGas()
    {
        SmoothnessTimerForGas.StopTimer();
    }
    private void StopSmoothTimerRotation()
    {
        SmoothnessTimerForRotation.StopTimer();
    }
}