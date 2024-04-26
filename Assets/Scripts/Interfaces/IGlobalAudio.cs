using UniRx;

public interface IGlobalAudio
{
    public ReactiveProperty<bool> SoundReactiveProperty { get; }
}