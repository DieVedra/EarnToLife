using UniRx;

public class IGamePause
{
    public ReactiveProperty<bool> PauseReactiveProperty { get; }
    public bool IsPause { get; }

}