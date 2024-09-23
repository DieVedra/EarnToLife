using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoosterFuelTank : Tank
{
    private readonly float _burnValue = 0.5f;
    public BoosterFuelTank(float fuelQuantity) : base(fuelQuantity) { }
    public void BurnBoosterFuelOnFly()
    {
        BurnFuel(_burnValue);
    }
}
