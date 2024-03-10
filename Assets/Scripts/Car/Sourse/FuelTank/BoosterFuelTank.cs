using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoosterFuelTank : Tank
{
    public BoosterFuelTank(float fuelQuantity) : base(fuelQuantity) { }
    public void BurnBoosterFuelOnFly()
    {
        BurnFuel(Time.deltaTime);
    }
}
