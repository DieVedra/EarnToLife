using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSound
{
    private AudioSource _botAudioSource;
    private AudioClip _botSaluteAudioClip;
    private AudioClip _botBueAudioClip;
    private AudioClip _botDieAudioClip;

    public BotSound(AudioSource audioSource, AudioClip saluteAudioClip, AudioClip bueAudioClip, AudioClip dieAudioClip)
    {
        _botAudioSource = audioSource;
        _botSaluteAudioClip = saluteAudioClip;
        _botBueAudioClip = bueAudioClip;
        _botDieAudioClip = dieAudioClip;
    }
    public void PlaySalute()
    {
        _botAudioSource.PlayOneShot(_botSaluteAudioClip);
    }
    public void PlayBue()
    {
        _botAudioSource.PlayOneShot(_botBueAudioClip);
    }
    public void PlayDie()
    {
        _botAudioSource.PlayOneShot(_botDieAudioClip);
    }
    public void PlaySomething(AudioClip audioClip)
    {
        _botAudioSource.PlayOneShot(audioClip);
    }
}
