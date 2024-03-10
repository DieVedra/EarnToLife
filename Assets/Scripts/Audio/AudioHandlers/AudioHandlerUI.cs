using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioHandlerUI: IClickAudio
{
    private GlobalAudio _globalAudio;
    private AudioSource _audioSource => _globalAudio.AudioSourceUI;
    private bool SoundOn => _globalAudio.SoundOn;
    public AudioHandlerUI(GlobalAudio globalAudio)
    {
        _globalAudio = globalAudio;
    }
    public void PlayClick()
    {
        TryPlayOneShot(_audioSource, _globalAudio.UiClips.ClipClick);
    }
    public void PlayBuySuccess()
    {
        TryPlayOneShot(_audioSource, _globalAudio.UiClips.ClipPaySuccess);
    }
    public void PlayBuyFail()
    {
        TryPlayOneShot(_audioSource, _globalAudio.UiClips.ClipFailPayAudio);
    }
    private void TryPlayOneShot(AudioSource audioSource, AudioClip clip)
    {
        if (SoundOn == true)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}