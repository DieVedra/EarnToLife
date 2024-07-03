using UnityEngine;
using Zenject;

public class SignalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<KillsSignal>().FromNew().AsSingle();
        Container.Bind<DestructionsSignal>().FromNew().AsSingle();
        Container.Bind<GameOverSignal>().FromNew().AsSingle();
        Container.Bind<ExplodeSignal>().FromNew().AsSingle();
        Container.Bind<TimeScaleSignal>().FromNew().AsSingle();
    }
}