using UnityEngine;

public class PropulsionUnit
{
    public Engine Engine { get; }
    public Transmission Transmission { get;}
    public FuelTank FuelTank { get; }
    public bool FuelAvailability => FuelTank.CheckFuel();
    public PropulsionUnit(Engine engine, FuelTank fuelTank, float gearRatioTransmission)
    {
        Engine = engine;
        Transmission = new Transmission(gearRatioTransmission);
        FuelTank = fuelTank;
    }
    public float GetCarSpeed()
    {
        return Engine.CurrentEngineSpeed * Transmission.GearRatio;
    }
}