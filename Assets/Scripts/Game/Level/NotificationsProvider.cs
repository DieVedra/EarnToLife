using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotificationsProvider
{
    private const string ACCELERATOR_TANK_EMPTY = "Accelerator Empty!";
    private const string FUEL_TANK_EMPTY = "Out of fuel";
    private const string GOT_POINT_DESTINATION = "Have arrived!";
    private const string DAY = "Day: ";

    public event Action<string> OnShowNotification;
    public event Action<string> OnGotPointDestination;
    public event Action<string> OnFueltankEmpty;
    public void ShowDayInfo(string day)
    {
        OnShowNotification?.Invoke(BuildString.GetString( new string[] { DAY, day}));
    }
    public void BoosterEmpty()
    {
        OnShowNotification?.Invoke(ACCELERATOR_TANK_EMPTY);
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
}
