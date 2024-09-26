using UnityEngine;

public interface ILevel
{
    public DebrisKeeper DebrisKeeper { get; }
    public LevelPool LevelPool { get; }
    public LevelAudio LevelAudio { get; }
    public Transform StartLevelPoint { get; }
    public Transform EndLevelPoint { get; }
}