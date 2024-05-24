
using UnityEngine;

public class LevelPool
{
    public readonly BarrelPool BarrelPool;
    public readonly BloodPool BloodPool;
    public LevelPool(BarrelPool barrelPool, BloodPool bloodPool)
    {
        BarrelPool = barrelPool;
        BloodPool = bloodPool;
    }
}