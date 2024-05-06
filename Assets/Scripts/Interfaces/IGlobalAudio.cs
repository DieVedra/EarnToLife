using UniRx;

public interface IGlobalAudio
{
    public ReactiveProperty<bool> SoundReactiveProperty { get; }
    public ReactiveProperty<bool> AudioPauseReactiveProperty { get; }
}