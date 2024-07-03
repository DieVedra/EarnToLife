using UnityEngine;

public interface ILevel
{
    public DebrisParent DebrisParent { get; }
    public LevelPool LevelPool { get; }
    public LevelAudio LevelAudio { get; }
    public Transform CameraTransform { get; }
}