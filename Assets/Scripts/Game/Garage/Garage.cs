using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Garage : MonoBehaviour
{
    [SerializeField] private Transform _lotsParent;
    [SerializeField] private List<ParkingLot> _parkingLots;
    private GarageData _garageData;
    private GarageConfig _garageConfig;
    private Spawner _spawner;
    private IReadOnlyList<CarInGarage> _carsInGaragePrefabs;
    private int _availableLotCarIndex;
    public int CurrentSelectLotCarIndex { get; private set; }
    public SwitchGarageLot SwitchGarageLot { get; private set; }
    public Wallet Wallet { get; private set; }
    public UpgradeParkingLot UpgradeParkingLot { get; private set; }
    public IReadOnlyList<ParkingLot> ParkingLots => _parkingLots;

    public void Construct(IPlayerData playerData, GarageData garageData)
    {
        // _gameData = gameData;
        _garageData = garageData;
        _carsInGaragePrefabs = garageData.GaragePrefabsProvider.CarsInGaragePrefabs;
        Wallet = playerData.Wallet;
        _spawner = new Spawner();
        _garageConfig = playerData.GarageConfig;
        CurrentSelectLotCarIndex = _garageConfig.CurrentSelectLotCarIndex;
        _availableLotCarIndex = _garageConfig.AvailableLotCarIndex;
        InitParkingLots();
        SwitchGarageLot = new SwitchGarageLot(ParkingLots, _lotsParent);
        UpgradeParkingLot = new UpgradeParkingLot(Wallet);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        SwitchGarageLot.OnSwitch += SetCurrentSelectLotCar;
        for (int i = 0; i < _parkingLots.Count; i++)
        {
            _parkingLots[i].SetCar(_carsInGaragePrefabs[i]);
        }
        SetLastAvailableLot();
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
        SwitchGarageLot.OnSwitch -= SetCurrentSelectLotCar;
        for (int i = 0; i < _parkingLots.Count; i++)
        {
            _parkingLots[i].RemoveCar();
        }
        _garageConfig.SetCurrentSelectLotCarIndex(CurrentSelectLotCarIndex);
        _garageConfig.SetAvailableCarLotIndex(_availableLotCarIndex);
        // _gameData.CarConfigurationToSendInScene = _parkingLots[CurrentSelectLotCarIndex].CurrentCarConfiguration;
    }

    private void SetCurrentSelectLotCar(int index)
    {
        CurrentSelectLotCarIndex = index;
        UpgradeParkingLot.SetParkingLot(_parkingLots[CurrentSelectLotCarIndex]);
    }

    private void InitParkingLots()
    {
        for (int i = 0; i < _parkingLots.Count; i++)
        {
            _parkingLots[i].Construct(
                _spawner,
                _garageData.ParkingLotsConfigurations[i],
                _garageConfig.CarConfigurationsInParkingLotsIndexes[i],
                i == 0 ? false : true);
        }
    }

    private void SetLastAvailableLot()
    {
        for (int i = ParkingLots.Count - 1; i >= 0; i--)
        {
            if (ParkingLots[i].IsBlocked == false)
            {
                SwitchGarageLot.SwitchToLotDirectly(i);
                break;
            }
        }
    }

    // private bool SetFirstLotOpen(int index)
    // {
    //     bool result = index == 0 ? false : true;
    //     return result;
    // }
}
