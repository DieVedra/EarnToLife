using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class BulletReservoir
{
    private const int BULLETSCOUNTBYDEFAULT = 30;
    private Queue<Bullet> _reservior;
    private List<Bullet> _reserviorForUpdate;
    private SpawnerGame _spawner;
    private Transform _transformParent;
    public bool IsEmpty => _reservior.Count == 0 ? true : false;
    public BulletReservoir(SpawnerGame spawner, Transform parent)
    {
        _reservior = new Queue<Bullet>();
        _reserviorForUpdate = new List<Bullet>();
        _spawner = spawner;
        _transformParent = parent;
    }
    public void FillReservoirBulletsAK(Bullet bulletPrefab)
    {
        if (IsEmpty == true)
        {
            for (int i = 0; i < BULLETSCOUNTBYDEFAULT; i++)
            {
                Create(bulletPrefab);
            }
        }
    }
    private void Create(Bullet bulletPrefab)
    {
        Bullet bullet = _spawner.Spawn(bulletPrefab, _transformParent, _transformParent);
        bullet.Init();
        AddToQueue(bullet);
        _reserviorForUpdate.Add(bullet);
    }
    private void AddToQueue(Bullet bullet)
    {
        _reservior.Enqueue(bullet);
        bullet.OnHit -= AddToQueue;
    }
    public void LaunchFromQueue(bool shooterIsFlip, Bullet bulletPrefab, Transform firePoint)
    {
        if (IsEmpty == true)
        {
            Create(bulletPrefab);
        }
        Bullet bullet = _reservior.Dequeue();
        bullet.OnHit += AddToQueue;
        bullet.Activate(shooterIsFlip, firePoint);
    }
    public void Update()
    {
        foreach (Bullet bullet in _reserviorForUpdate)
        {
            bullet.UpdateBullet();
        }
    }
}