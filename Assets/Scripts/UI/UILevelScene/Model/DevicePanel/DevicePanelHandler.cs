using UniRx;

public class DevicePanelHandler
{
    private readonly DeviceForFuel _deviceFuelIndicate;
    private readonly DeviceForSpeed _deviceSpeedIndicate;
    private readonly DeviceForFuel _deviceBoosterFuelIndicate;
    private readonly CarBoosterHandler _carBoosterHandler;
    private readonly DevicesOnPanelValues _devicesOnPanelValues = new DevicesOnPanelValues();
    private readonly CompositeDisposable _compositeDisposableForBoosterFuelTank = new CompositeDisposable();
    private readonly CompositeDisposable _compositeDisposableForFuelTankAndSpeedDevice = new CompositeDisposable();
    public DevicePanelHandler(DevicePanel devicePanel, CarInLevel carInLevel, CarBoosterHandler carBoosterHandler, ReactiveCommand disposeCommand)
    {
        _carBoosterHandler = carBoosterHandler;
        
        _deviceFuelIndicate = new DeviceForFuel(devicePanel.ArrowFuelTransform, carInLevel.FuelTank,
            new BlinkIndicator(devicePanel.IndicatorFuel, devicePanel.ColorActive, devicePanel.ColorDisactive),
            _compositeDisposableForFuelTankAndSpeedDevice,
            _devicesOnPanelValues.FuelAngleMin, _devicesOnPanelValues.FuelAngleMax, carInLevel.CarConfiguration.MaxFuelQuantity);
        _deviceSpeedIndicate = new DeviceForSpeed(devicePanel.ArrowSpeedTransform, carInLevel.Speedometer,
            _compositeDisposableForFuelTankAndSpeedDevice,
            _devicesOnPanelValues.SpeedAngleMin,
            _devicesOnPanelValues.SpeedAngleMax,
            _devicesOnPanelValues.MaxSpeed);
        if (_carBoosterHandler != null)
        {
            _deviceBoosterFuelIndicate = new DeviceForFuel(devicePanel.ArrowAcceleratorTransform, carBoosterHandler.Booster.BoosterFuelTank,
                new BlinkIndicator(devicePanel.IndicatorFuelBooster, devicePanel.ColorActive, devicePanel.ColorDisactive),
                _compositeDisposableForBoosterFuelTank,
                _devicesOnPanelValues.FuelAngleMin, _devicesOnPanelValues.FuelAngleMax, carInLevel.CarConfiguration.MaxBoosterFuelQuantity);
            _carBoosterHandler.Booster.OnBoosterDisable += BoosterDisableOnDestruct;
        }
        disposeCommand.Subscribe(_ => { Dispose();});
    }
    private void Dispose()
    {
        _compositeDisposableForFuelTankAndSpeedDevice.Clear();
        if (_carBoosterHandler != null)
        {
            BoosterDisableOnDestruct();
        }
    }
    private void BoosterDisableOnDestruct()
    {
        _carBoosterHandler.Booster.OnBoosterDisable -= BoosterDisableOnDestruct;
        _compositeDisposableForBoosterFuelTank.Clear();
        _carBoosterHandler.SetBoosterButtonOffInteractable();
    }
}