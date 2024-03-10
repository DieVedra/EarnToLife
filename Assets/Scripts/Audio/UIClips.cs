using UnityEngine;

public class UIClips
{
     public AudioClip ClipClick { get; }
     public AudioClip ClipPaySuccess { get; }
     public AudioClip ClipFailPayAudio { get; }
     public UIClips(AudioClip clipClick, AudioClip clipPaySuccess, AudioClip clipFailPayAudio)
     {
         ClipClick = clipClick; ClipPaySuccess = clipPaySuccess; ClipFailPayAudio = clipFailPayAudio;
     }
}
