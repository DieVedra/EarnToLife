using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class LevelContentInstaller : MonoInstaller
{
    [SerializeField, Expandable] private LevelPrefabsProvider _levelPrefabsProvider;
    
    public override void InstallBindings()
    {
        Container.Bind<LevelPrefabsProvider>().FromInstance(_levelPrefabsProvider).AsSingle();
        // Container.BindInterfacesAndSelfTo<CarAudioInitializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelAudioHandler>().AsSingle();
    }
}