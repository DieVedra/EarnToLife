using UniRx;

public interface ITimeScalePitch
{
    public ReactiveProperty<float> ChangePitchReactiveProperty { get; }
}