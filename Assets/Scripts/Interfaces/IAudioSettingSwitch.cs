public interface IAudioSettingSwitch
{
    public bool SoundOn { get; }
    public bool MusicOn { get; }

    public void SetSoundsOff();
    public void SetSoundsOn();
    public void SetMusicOff();
    public void SetMusicOn();
}