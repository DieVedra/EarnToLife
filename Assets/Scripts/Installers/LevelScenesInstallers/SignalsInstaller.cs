using Zenject;

public class SignalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<KillsSignal>().FromNew().AsSingle().NonLazy();
        Container.Bind<DestructionsSignal>().FromNew().AsSingle().NonLazy();
        Container.Bind<GameOverSignal>().FromNew().AsSingle().NonLazy();
        Container.Bind<ExplodeSignal>().FromNew().AsSingle().NonLazy();
        Container.Bind<TimeScaleSignal>().FromNew().AsSingle().NonLazy();
    }
}