using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ParkingLot : MonoBehaviour
{
    [SerializeField, Expandable] private ParkingLotConfiguration _parkingLotConfiguration;
    private Transform _transform;
    private CarInGarage _carInGarage;
    private Spawner _spawner;
    private CarConfigurationProvider _carConfigurationProvider;
    private CarConfigurationInParkingLot _carConfigurationInParkingLot;
    private bool _isBlocked;
    
    public ParkingLotConfiguration ParkingLotConfiguration => _parkingLotConfiguration;
    [ShowNativeProperty] public bool IsBlocked =>_isBlocked;
    public void Construct(Spawner spawner, ParkingLotConfiguration parkingLotConfiguration,
        CarConfigurationInParkingLot carConfigurationInParkingLot, bool block)
    {
        _transform = transform;
        _spawner = spawner;
        _carConfigurationProvider = new CarConfigurationProvider();
        _parkingLotConfiguration = parkingLotConfiguration;
        _carConfigurationInParkingLot = carConfigurationInParkingLot;
        ParkingLotConfiguration.Construct(_carConfigurationInParkingLot);
        _isBlocked = block;
    }
    public void SetCar(CarInGarage carPrefab)
    {
        _carInGarage = _spawner.Spawn(carPrefab, _transform, _transform);
        SetCarConfiguration();
    }
    public void SetCarConfiguration()
    {
        _carInGarage.Cunstruct(_carConfigurationProvider.GetCarConfiguration(_parkingLotConfiguration));
    }
    public void RemoveCar()
    {
        if (_carInGarage != null)
        {
            _spawner.KillObject(_carInGarage.gameObject);
        }
    }
    [Button("ResetLotConfigurations")]
    private void ResetLotConfigurations()
    {
        ParkingLotConfiguration.ResetIndexes();
    }
}