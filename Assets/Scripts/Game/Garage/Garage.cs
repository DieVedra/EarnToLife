using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Garage : MonoBehaviour
{
    [SerializeField] private Transform _lotsParent;
    [SerializeField] private List<ParkingLot> _parkingLots;
    [SerializeField, HorizontalLine(color:EColor.Pink)] private Light2D _lamp1;
    [SerializeField] private Light2D _lamp2;
    [SerializeField] private Light2D _globalLight2D;
    [SerializeField] private Color _colorForGarage;
    
    private GarageData _garageData;
    private GarageConfig _garageConfig;
    private Spawner _spawner;
    private IReadOnlyList<CarInGarage> _carsInGaragePrefabs;
    private int _availableLotCarIndex;
    
    public int CurrentSelectLotCarIndex { get; private set; }
    public SwitchGarageLot SwitchGarageLot { get; private set; }
    public GarageLight GarageLight { get; private set; }
    public Wallet Wallet { get; private set; }
    public UpgradeParkingLot UpgradeParkingLot { get; private set; }
    public IReadOnlyList<ParkingLot> ParkingLots => _parkingLots;
    
    public void Init(IPlayerData playerData, GarageData garageData)
    {
        _garageData = garageData;
        _carsInGaragePrefabs = garageData.GaragePrefabsProvider.CarsInGaragePrefabs;
        Wallet = playerData.Wallet;
        _spawner = new Spawner();
        GarageLight = new GarageLight(_lamp1, _lamp2, _globalLight2D, _colorForGarage);
        _garageConfig = playerData.GarageConfig;
        CurrentSelectLotCarIndex = _garageConfig.CurrentSelectLotCarIndex;
        _availableLotCarIndex = _garageConfig.AvailableLotCarIndex;
        InitParkingLots();
        SwitchGarageLot = new SwitchGarageLot(ParkingLots, _lotsParent);
        UpgradeParkingLot = new UpgradeParkingLot(Wallet);
    }

    public void Activate()
    {
        GarageLight.SetColorForGarage();
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
        GarageLight.SetDefaultColor();
        gameObject.SetActive(false);
        SwitchGarageLot.OnSwitch -= SetCurrentSelectLotCar;
        for (int i = 0; i < _parkingLots.Count; i++)
        {
            _parkingLots[i].RemoveCar();
        }
        _garageConfig.SetCurrentSelectLotCarIndex(CurrentSelectLotCarIndex);
        _garageConfig.SetAvailableCarLotIndex(_availableLotCarIndex);
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

    private void OnDestroy()
    {
        GarageLight.Dispose();
    }
}
