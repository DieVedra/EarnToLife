
using UnityEngine;

public class LevelPool
{
    public readonly BarrelPool BarrelPool;
    public readonly ZombiePool ZombiePool;
    public readonly DebrisPool DebrisPool;
    public LevelPool(BarrelPool barrelPool, ZombiePool zombiePool, DebrisPool debrisPool)
    {
        BarrelPool = barrelPool;
        ZombiePool = zombiePool;
        DebrisPool = debrisPool;
    }
    public void Dispose()
    {
        BarrelPool.Dispose();
        ZombiePool.Dispose();
    }
}