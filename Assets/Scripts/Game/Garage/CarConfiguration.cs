using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarConfiguration
{
    public float EngineOverclockingMultiplier { get; }
    public float GearRatio { get; }
    public int WheelsIndex { get; }
    public int GunCountAmmo { get; }
    public int CorpusIndex { get; }
    public float BoosterCountFuelQuantity { get; }
    public float MaxBoosterFuelQuantity { get; }
    public float FuelQuantity { get; }
    public float MaxFuelQuantity { get; }
    public float PriceTagForTheMurder { get; }
    public float PriceTagForTheWayMeter { get; }
    public float PriceTagForTheDestruction { get; }
    
    public CarConfiguration(
        float engineOverclockingMultiplier, float gearRatio, int wheelsIndex,
        int gunCountAmmo, int corpusIndex, float boosterCountFuelQuantity, float maxBoosterFuelQuantity,
        float fuelQuantity, float maxFuelQuantity, float priceTagForTheMurder, float priceTagForTheDestruction, float priceTagForTheWayMeter)
    {
        EngineOverclockingMultiplier = engineOverclockingMultiplier;
        GearRatio = gearRatio;
        WheelsIndex = wheelsIndex;
        GunCountAmmo = gunCountAmmo;
        CorpusIndex = corpusIndex;
        BoosterCountFuelQuantity = boosterCountFuelQuantity;
        MaxBoosterFuelQuantity = maxBoosterFuelQuantity;
        FuelQuantity = fuelQuantity;
        MaxFuelQuantity = maxFuelQuantity;
        PriceTagForTheMurder = priceTagForTheMurder;
        PriceTagForTheDestruction = priceTagForTheDestruction;
        PriceTagForTheWayMeter = priceTagForTheWayMeter;
    }
}