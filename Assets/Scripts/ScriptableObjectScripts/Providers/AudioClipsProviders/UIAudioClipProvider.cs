using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "UIAudioClipProvider", menuName = "Providers/UIAudioClipProvider", order = 51)]
public class UIAudioClipProvider : ScriptableObject
{
    [SerializeField, BoxGroup("UI Clips"), HorizontalLine( color:EColor.White)] private AudioClip _clipClick;
    [SerializeField, BoxGroup("UI Clips") ] private AudioClip _clipPaySuccess;
    [SerializeField, BoxGroup("UI Clips")] private AudioClip _clipFailPayAudio;
    
    public AudioClip ClipClick => _clipClick;
    public AudioClip ClipPaySuccess => _clipPaySuccess;
    public AudioClip ClipFailPayAudio => _clipFailPayAudio;
}
