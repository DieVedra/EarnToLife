
using System.Collections.Generic;
using UnityEngine;

public class CarConfigurationProvider
{
    public CarConfiguration GetCarConfiguration(ParkingLotConfiguration parkingLotConfiguration)
     {
         return new CarConfiguration(
             parkingLotConfiguration.GetEnginePowerValue(),
             parkingLotConfiguration.GetGearRatioValue(),
             parkingLotConfiguration.GetWheelsValue(),
             parkingLotConfiguration.GetGunAmmoValue(),
             parkingLotConfiguration.GetCorpusUpgradeValue(),
             parkingLotConfiguration.GetBoosterFuelValue(),
             parkingLotConfiguration.GetMaxBoosterFuelQuantity(),
             parkingLotConfiguration.GetFuelQuantityValue(),
             parkingLotConfiguration.GetMaxFuelQuantity(),
             parkingLotConfiguration.PriceTagForTheMurder,
             parkingLotConfiguration.PriceTagForTheDestruction,
             parkingLotConfiguration.PriceTagForTheWayMeter
         ) ;
     }
    public CarConfiguration GetCarConfiguration(IReadOnlyList<ParkingLotConfiguration> parkingLotConfigurations, CarConfigurationInParkingLot carConfigurationInParkingLot, int currentSelectLotCarIndex)
    {
        ParkingLotConfiguration parkingLotConfiguration = parkingLotConfigurations[currentSelectLotCarIndex];
        parkingLotConfiguration.Construct(carConfigurationInParkingLot);
        return GetCarConfiguration(parkingLotConfiguration);
    }
}
