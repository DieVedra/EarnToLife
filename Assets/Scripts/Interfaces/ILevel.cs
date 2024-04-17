using UnityEngine;

public interface ILevel
{
    public Transform DebrisParent { get; }
    public BarrelPool BarrelPool { get; }
}