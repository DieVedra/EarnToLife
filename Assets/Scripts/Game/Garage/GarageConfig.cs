using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageConfig
{
    public int CurrentSelectLotCarIndex { get; private set; }
    public int AvailableLotCarIndex { get; private set; }

    public IReadOnlyList<CarConfigurationInParkingLot> CarConfigurationsInParkingLotsIndexes { get; }

    public GarageConfig(CarConfigurationInParkingLot[] carConfigurationsInParkingLotsIndexes = null, int selectedCarLotIndex = 0, int availableCarLotIndex = 0)
    {
        CurrentSelectLotCarIndex = selectedCarLotIndex;
        AvailableLotCarIndex = availableCarLotIndex;
        CarConfigurationsInParkingLotsIndexes = carConfigurationsInParkingLotsIndexes;
    }
    public void SetAvailableCarLotIndex(int value)
    {
        AvailableLotCarIndex = value;
    }
    public void SetCurrentSelectLotCarIndex(int value)
    {
        CurrentSelectLotCarIndex = value;
    }
}
