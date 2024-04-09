using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipProvider", menuName = "Providers/AudioClipProvider", order = 51)]
public class AudioClipProvider : ScriptableObject
{
    [SerializeField, BoxGroup("BackgroundClip")] private AudioClip _clipBackground;

    [SerializeField, Expandable, HorizontalLine(color: EColor.White)]
    private UIAudioClipProvider _uiAudioClipProvider;

    [SerializeField, Expandable, HorizontalLine(color: EColor.White)]
    private CarAudioClipProvider _carAudioClipProvider;

    [SerializeField, Expandable, HorizontalLine(color: EColor.White)]
    private LevelAudioClipProvider _levelAudioClipProvider;
    public AudioClip ClipBackground => _clipBackground;
    public UIAudioClipProvider UIAudioClipProvider => _uiAudioClipProvider;
    public CarAudioClipProvider CarAudioClipProvider => _carAudioClipProvider;
    public LevelAudioClipProvider LevelAudioClipProvider => _levelAudioClipProvider;
}