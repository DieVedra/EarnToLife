using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
public class CarAudioHandler : IDisposable
{
    private const float TIME_DELAY_PLAY_START_SOUND = 0.4f;
    private const float MINTIMEPITCH = 0.85f;
    private const float MAXTIMEPITCH = 1.45f;
    private ICarAudio _globalAudioToCar;
    private AudioSource _carAudioSource1 => _globalAudioToCar.CarAudioSource1;
    private AudioSource _carAudioSource2 => _globalAudioToCar.CarAudioSource2;
    private CarClips _carClips => _globalAudioToCar.CarClips;
    private bool SoundOn => _globalAudioToCar.SoundOn;
    
    public CarAudioHandler(ICarAudio globalAudioToCar)
    {
        _globalAudioToCar = globalAudioToCar;
        _globalAudioToCar.OnSoundChange += PlayRun;
    }
    public void Dispose()
    {
        _globalAudioToCar.OnSoundChange -= PlayRun;
    }
    public void PlayShotGun()
    {
        PlayOneShotClip(_carAudioSource1, _carClips.ShotGunAudioClip);
    }

    private void PlayRun()
    {
        PlayClip(_carAudioSource1, _carClips.EngineRunAudioClip, true);
    }
    public void PlayBoosterRun()
    {
        PlayClip(_carAudioSource2, _carClips.BoosterRunAudioClip, true);
    }
    public void StopPlayRunBooster()
    {
        StopPlay(_carAudioSource2);
    }
    public void PlayBoosterEndFuel()
    {
        PlayOneShotClip(_carAudioSource1, _carClips.BoosterStopAudioClip);
    }
    public async void PlayStartEngine()
    {
        PlayOneShotClip(_carAudioSource1, _carClips.EngineStartAudioClip);
        await UniTask.Delay(TimeSpan.FromSeconds(TIME_DELAY_PLAY_START_SOUND));
        PlayRun();
    }
    public void PlaySoundStopEngine()
    {
        PlayClip(_carAudioSource1, _carClips.EngineStopAudioClip, false);
    }
    public void StopPlayEngine()
    {
        StopPlay(_carAudioSource1);
    }
    public void PlayBrake()
    {
        PlayClip(_carAudioSource2, _carClips.BrakeAudioClip, true);
    }
    public void SetVolumeBrake(float volume)
    {
        _carAudioSource2.volume = volume;
    }
    public void StopPlayBrake()
    {
        StopPlay(_carAudioSource2);
    }
    public void PitchControl(float value)
    {
        _carAudioSource1.pitch = Mathf.LerpUnclamped(MINTIMEPITCH, MAXTIMEPITCH, value);
    }
    private void PlayClip(AudioSource audioSource, AudioClip audioClip, bool loop)
    {
        if (SoundOn == true)
        {
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }
    private void PlayOneShotClip(AudioSource audioSource, AudioClip audioClip)
    {
        if (SoundOn == true)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
    private void StopPlay(AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
}
