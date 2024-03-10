using UnityEngine;
using Zenject;

public class PlayerDataProviderInstaller : MonoInstaller
{
    [SerializeField] private PlayerDataProvider _playerDataProvider;
    public override void InstallBindings()
    {
        Container.Bind<PlayerDataProvider>().FromInstance(_playerDataProvider).AsSingle();
    }
}