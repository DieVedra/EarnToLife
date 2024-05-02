using UniRx;
using UnityEngine;

public interface IUIAudio
{
    public AudioSource UI { get; }
    public UIAudioClipProvider UIAudioClipProvider { get; }
    public bool SoundOn { get; }
}