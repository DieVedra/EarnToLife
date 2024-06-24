using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotificationsProvider
{
    private const string BOOSTER_TANK_EMPTY = "Accelerator Empty!";
    private const string FUEL_TANK_EMPTY = "Out of fuel";
    private const string GOT_POINT_DESTINATION = "Have arrived!";
    private const string ENGINE_BROKEN = "Engine Broken!";
    private const string DRIVER_CRUSHED = "Driver crushed!";
    private const string CAR_STUCK = "Сar is stuck...";
    private const string DRIVER_ASLEEP = "Are you sleeping?";
    private const string CAR_TURNED_OVER = "Turned over.";
    private const string DAY = "Day ";
    public event Action<string> OnShowNotification;
    public event Action<string> OnGotPointDestination;
    public event Action<string> OnFueltankEmpty;
    public event Action<string> OnEngineBroken;
    public event Action<string> OnDriverCrushed;
    public event Action<string> OnCarStuck;
    public event Action<string> OnDriverAsleep;
    public event Action<string> OnCarTurnOver;
    public void ShowDayInfo(string day)
    {
        OnShowNotification?.Invoke(BuildString.GetString( new string[] { DAY, day}));
    }
    public void BoosterEmpty()
    {
        OnShowNotification?.Invoke(BOOSTER_TANK_EMPTY);
    }
    public void GotPointDestination()
    {
        OnShowNotification?.Invoke(GOT_POINT_DESTINATION);
        OnGotPointDestination?.Invoke(GOT_POINT_DESTINATION);
    }
    public void FuelTankEmpty()
    {
        OnShowNotification?.Invoke(FUEL_TANK_EMPTY);
        OnFueltankEmpty?.Invoke(FUEL_TANK_EMPTY);
    }

    public void EngineBroken()
    {
        OnShowNotification?.Invoke(ENGINE_BROKEN);
        OnEngineBroken?.Invoke(ENGINE_BROKEN);
    }

    public void DriverCrushed()
    {
        OnShowNotification?.Invoke(DRIVER_CRUSHED);
        OnDriverCrushed?.Invoke(DRIVER_CRUSHED);
    }
    public void CarStuck()
    {
        OnShowNotification?.Invoke(CAR_STUCK);
        OnCarStuck?.Invoke(CAR_STUCK);
    }
    public void DriverAsleep()
    {
        OnShowNotification?.Invoke(DRIVER_ASLEEP);
        OnDriverAsleep?.Invoke(DRIVER_ASLEEP);
    }
    public void CarTurnOver()
    {
        OnShowNotification?.Invoke(CAR_TURNED_OVER);
        OnCarTurnOver?.Invoke(CAR_TURNED_OVER);
    }
}
