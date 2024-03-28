using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipProvider", menuName = "Providers/AudioClipProvider", order = 51)]
public class AudioClipProvider : ScriptableObject
{
    [SerializeField, BoxGroup("BackgroundClip")] private AudioClip _clipBackground;

    [SerializeField, BoxGroup("UI Clips"), HorizontalLine( color:EColor.White)] private AudioClip _clipClick;
    [SerializeField, BoxGroup("UI Clips") ] private AudioClip _clipPaySuccess;
    [SerializeField, BoxGroup("UI Clips")] private AudioClip _clipFailPayAudio;
    
    [SerializeField, BoxGroup("SoundsCar"), HorizontalLine( color:EColor.White)] private AudioClip _engineRunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _brakeAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineStartAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _engineStopAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _boosterRunAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carHotweelAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carBurnAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carCarHardHitAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _carSoftHitAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _glassBreakingAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _metalBendsAudioClip;
    [SerializeField, BoxGroup("SoundsCar")] private AudioClip _shotGunAudioClip;
    
    public AudioClip ClipBackground => _clipBackground;
    public UIClips UiClips { get; private set; }
    public CarClips ClipsCar { get; private set;}
    private void OnEnable()
    {
        UiClips = new UIClips(_clipClick, _clipPaySuccess, _clipFailPayAudio);
        ClipsCar = new CarClips(_engineRunAudioClip, _brakeAudioClip, _engineStartAudioClip,
            _engineStopAudioClip, _boosterRunAudioClip, _shotGunAudioClip, _carHotweelAudioClip, _carBurnAudioClip,
            _carCarHardHitAudioClip, _carSoftHitAudioClip, _glassBreakingAudioClip, _metalBendsAudioClip);
    }
}