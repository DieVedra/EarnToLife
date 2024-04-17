
using UnityEngine;

public class LevelPool
{
    private readonly BarrelPool _barrelPool;
    public BarrelPool BarrelPool => _barrelPool;
    public LevelPool(BarrelPool barrelPool)
    {
        _barrelPool = barrelPool;
    }
}