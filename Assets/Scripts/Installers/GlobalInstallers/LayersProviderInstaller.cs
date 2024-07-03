using UnityEngine;
using Zenject;

public class LayersProviderInstaller : MonoInstaller
{
    [SerializeField] private LayersProvider _layersProvider;

    public override void InstallBindings()
    {
        Container.Bind<LayersProvider>().FromInstance(_layersProvider).AsSingle();
    }
}