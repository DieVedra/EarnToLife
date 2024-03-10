using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet
{
    public int Money { get; private set; }
    public Action<int> OnTakeCashSuccess;
    public Action<int> OnAddCashSuccess;
    public Action OnTakeCashFailed;

    public Wallet(int money = 0)
    {
        if (money >= 0)
        {
            Money = money;
        }
    }

    public void AddCash(int value)
    {
        if (value > 0)
        {
            Money += value;
            OnAddCashSuccess?.Invoke(Money);
        }
    }
    public bool TakeCash(int value)
    {
        if (value > 0 && (Money - value) >= 0)
        {
            Money -= value;
            OnTakeCashSuccess?.Invoke(Money);
            return true;
        }
        else
        {
            OnTakeCashFailed?.Invoke();
            return false;
        }
    }
    public bool CheckAvailableMoney(int value)
    {
        if (value >= 0 && (Money - value) >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
