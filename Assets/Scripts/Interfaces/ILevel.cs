using UnityEngine;

public interface ILevel
{
    public Transform DebrisParent { get; }
    public LevelPool LevelPool { get; }
    public LevelAudio LevelAudio { get; }
}