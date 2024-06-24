using System;
using UnityEngine;

public class StopCauseHandler
{
    private readonly FuelTank _fuelTank;
    private readonly LevelProgressCounter _levelProgressCounter;
    private readonly DestructionCar _destructionCar;
    private readonly NotificationsProvider _notificationsProvider;
    private readonly MoveAnalyzer _moveAnalyzer;
    private readonly CoupAnalyzer _coupAnalyzer;
    private readonly GroundAnalyzer _groundAnalyzer;
    private readonly Action _stopCarOtherOperation;
    private readonly Action _stopCarOperation;
    private readonly Action _softStopCarOnDestinationPointOperation;
    private readonly Action _stopCarOnFuelTankEmptyOperation;

    // public event Action OnStopCar;

    public StopCauseHandler(FuelTank fuelTank, LevelProgressCounter levelProgressCounter, DestructionCar destructionCar, NotificationsProvider notificationsProvider,
        MoveAnalyzer moveAnalyzer, CoupAnalyzer coupAnalyzer, GroundAnalyzer groundAnalyzer,
        Action stopCarOtherOperation, Action softStopCarOnDestinationPointOperation, Action stopCarOnFuelTankEmptyOperation)
    {
        _fuelTank = fuelTank;
        _levelProgressCounter = levelProgressCounter;
        _destructionCar = destructionCar;
        _notificationsProvider = notificationsProvider;
        _moveAnalyzer = moveAnalyzer;
        _coupAnalyzer = coupAnalyzer;
        _groundAnalyzer = groundAnalyzer;
        _stopCarOtherOperation = stopCarOtherOperation;
        _softStopCarOnDestinationPointOperation = softStopCarOnDestinationPointOperation;
        _stopCarOnFuelTankEmptyOperation = stopCarOnFuelTankEmptyOperation;
        Init();
    }
    public void Dispose()
    {
        _fuelTank.OnTankEmpty -= FuelTankEmpty;
        _levelProgressCounter.OnGotPointDestination -= PointDestinationGot;
        _moveAnalyzer.OnCarStands -= MoveAnalyzeHandle;
        if (_destructionCar.FrontWingDestructionHandler != null)
        {
            _destructionCar.FrontWingDestructionHandler.OnEngineBroken -= EngineBroken;
        }
        if (_destructionCar.CabineDestructionHandler != null)
        {
            _destructionCar.CabineDestructionHandler.OnDriverCrushed -= DriverCrushed;
        }
    }
    private void Init()
    {
        _fuelTank.OnTankEmpty += FuelTankEmpty;
        _levelProgressCounter.OnGotPointDestination += PointDestinationGot;
        _moveAnalyzer.OnCarStands += MoveAnalyzeHandle;
        if (_destructionCar.FrontWingDestructionHandler != null)
        {
            _destructionCar.FrontWingDestructionHandler.OnEngineBroken += EngineBroken;
        }
        if (_destructionCar.CabineDestructionHandler != null)
        {
            _destructionCar.CabineDestructionHandler.OnDriverCrushed += DriverCrushed;
        }
    }
    private void FuelTankEmpty()
    {
        _notificationsProvider.FuelTankEmpty();
        _stopCarOnFuelTankEmptyOperation?.Invoke();
    }
    private void PointDestinationGot()
    {
        _softStopCarOnDestinationPointOperation?.Invoke();
    }
    private void MoveAnalyzeHandle()
    {
        if (_coupAnalyzer.CarIsCoup == true)
        {
            _notificationsProvider.CarTurnOver();
        }
        else if (_groundAnalyzer.FrontWheelContact == false && _groundAnalyzer.BackWheelContact == false)
        {
            _notificationsProvider.CarStuck();
        }
        else
        {
            _notificationsProvider.DriverAsleep();
        }
        _stopCarOtherOperation?.Invoke();
    }

    private void EngineBroken()
    {
        _notificationsProvider.EngineBroken();
        _stopCarOtherOperation?.Invoke();
    }

    private void DriverCrushed()
    {
        _notificationsProvider.DriverCrushed();
        _stopCarOtherOperation?.Invoke();
    }
}