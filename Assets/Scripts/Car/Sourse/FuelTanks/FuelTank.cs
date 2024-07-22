using UnityEngine;
using System;

public class FuelTank : Tank
{
    private readonly float _burnIdlingMultiplier = 0.1f;
    private readonly float _combustionEfficiencyFuelMultiplier;
    private readonly GameOverSignal _gameOverSignal;
    private bool _gameOver;
    public FuelTank(GameOverSignal gameOverSignal, float fuelQuantity, float combustionEfficiencyFuelMultiplier) : base(fuelQuantity)
    {
        _gameOverSignal = gameOverSignal;
        _combustionEfficiencyFuelMultiplier = combustionEfficiencyFuelMultiplier;
        _gameOver = false;
        _gameOverSignal.OnGameOver += SetGameKey;
    }

    public void Dispose()
    {
        _gameOverSignal.OnGameOver -= SetGameKey;
    }
    public void BurnFuelOnMoving()
    {
        BurnFuel(_combustionEfficiencyFuelMultiplier);
    }
    public void BurnFuelOnIdling()
    {
        if (_gameOver == false)
        {
            BurnFuel(_burnIdlingMultiplier);
        }
    }

    private void SetGameKey()
    {
        _gameOver = true;
    }
}
