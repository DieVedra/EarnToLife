public class CarConfigurationInParkingLot
{
    public int EnginePowerIndex { get; private set; }
    public int GearRatioIndex { get; private set; }
    public int WheelsIndex { get; private set; }
    public int GunIndex { get; private set; }
    public int CorpusIndex { get; private set; }
    public int BoosterIndex { get; private set; }
    public int FuelQuantityIndex { get; private set; }
    public CarConfigurationInParkingLot(
        int enginePowerCurrentIndex = 0, int gearRatioCurrentIndex = 0,
        int wheelsCurrentIndex = 0, int gunCurrentIndex = 0,
        int corpusCurrentIndex = 0, int boosterCurrentIndex = 0, int fuelQuantityCurrentIndex = 0)
    {
        SetEnginePowerIndex(enginePowerCurrentIndex);
        SetGearRatioIndex(gearRatioCurrentIndex);
        SetWheelsIndex(wheelsCurrentIndex);
        SetGunIndex(gunCurrentIndex);
        SetCorpusIndex(corpusCurrentIndex);
        SetBoosterIndex(boosterCurrentIndex);
        SetFuelQuantityIndex(fuelQuantityCurrentIndex);
    }
    #region SetValues
    public void SetEnginePowerIndex(int value)
    {
        EnginePowerIndex = value;
    }
    public void SetGearRatioIndex(int value)
    {
        GearRatioIndex = value;
    }
    public void SetWheelsIndex(int value)
    {
        WheelsIndex = value;
    }
    public void SetGunIndex(int value)
    {
        GunIndex = value;
    }
    public void SetCorpusIndex(int value)
    {
        CorpusIndex = value;
    }
    public void SetBoosterIndex(int value)
    {
        BoosterIndex = value;
    }
    public void SetFuelQuantityIndex(int value)
    {
        FuelQuantityIndex = value;
    }
    #endregion
}