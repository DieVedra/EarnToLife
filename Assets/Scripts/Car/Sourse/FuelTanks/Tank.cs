using System;
using System.Collections.Generic;
using UnityEngine;

public class Tank
{
    public float FuelQuantity { get; private set; }
    public bool IsEmpty { get; private set; } = false;
    public event Action OnTankEmpty;
    protected Tank(float fuelQuantity)
    {
        FuelQuantity = fuelQuantity;
    }
    public bool CheckFuel()
    {
        if (FuelQuantity > 0f)
        {
            return true;
        }
        else
        {
            if (IsEmpty == false)
            {
                IsEmpty = true;
                OnTankEmpty?.Invoke();
            }
            return false;
        }
    }
    protected void BurnFuel(float value)
    {
        if (CheckFuel())
        {
            FuelQuantity -= value * Time.deltaTime * Time.timeScale;
        }
    }

}
