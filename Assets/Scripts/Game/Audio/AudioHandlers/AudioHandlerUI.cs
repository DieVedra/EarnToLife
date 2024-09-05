using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioHandlerUI: IClickAudio
{
    private IUIAudio _uIAudio;
    private AudioSource _audioSource => _uIAudio.UI;
    private bool SoundOn => _uIAudio.SoundOn;
    public AudioHandlerUI(IUIAudio uIAudio)
    {
        _uIAudio = uIAudio;
    }
    public void PlayClick()
    {
        TryPlayOneShot(_audioSource, _uIAudio.UIAudioClipProvider.ClipClick);
    }
    public void PlayBuySuccess()
    {
        TryPlayOneShot(_audioSource, _uIAudio.UIAudioClipProvider.ClipPaySuccess);
    }
    public void PlayBuyFail()
    {
        TryPlayOneShot(_audioSource, _uIAudio.UIAudioClipProvider.ClipFailPayAudio);
    }
    private void TryPlayOneShot(AudioSource audioSource, AudioClip clip)
    {
        if (SoundOn == true)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}