using UnityEngine;

public interface ILevel
{
    public Transform DebrisParent { get; }
    public BarrelPool BarrelPool { get; }
    public LevelAudio LevelAudio { get; }
}