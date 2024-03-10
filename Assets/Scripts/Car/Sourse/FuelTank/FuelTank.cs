using UnityEngine;
using System;

public class FuelTank : Tank
{
    private readonly float _burnIdlingMultiplier = 0.2f;
    private readonly float _combustionEfficiencyFuelMultiplier;

    public FuelTank(float fuelQuantity, float combustionEfficiencyFuelMultiplier) : base(fuelQuantity)
    {
        _combustionEfficiencyFuelMultiplier = combustionEfficiencyFuelMultiplier;
    }
    public void BurnFuelOnMoving()
    {
        BurnFuel(Time.deltaTime * _combustionEfficiencyFuelMultiplier);
    }
    public void BurnFuelOnIdling()
    {
        BurnFuel(Time.deltaTime * _burnIdlingMultiplier);
    }
}
