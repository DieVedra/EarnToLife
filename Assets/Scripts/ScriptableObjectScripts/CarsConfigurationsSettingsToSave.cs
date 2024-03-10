using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarsConfigurationsSettingsToSave", menuName = "CarsConfigurations/CarsConfigurationsSettingsToSave", order = 51)]
public class CarsConfigurationsSettingsToSave : ScriptableObject
{
    private ParkingLotConfiguration _car1Comfiguration;
    private ParkingLotConfiguration _car2Comfiguration;
    private ParkingLotConfiguration _car3Comfiguration;
    private ParkingLotConfiguration _car4Comfiguration;
    public IReadOnlyList<ParkingLotConfiguration> GetCarConfigurations =>
        new List<ParkingLotConfiguration>()
        {
            _car1Comfiguration,
            _car2Comfiguration,
            _car3Comfiguration,
            _car4Comfiguration
        };
}
