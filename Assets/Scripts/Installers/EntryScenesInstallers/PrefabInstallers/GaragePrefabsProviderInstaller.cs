using UnityEngine;
using Zenject;

public class GaragePrefabsProviderInstaller : MonoInstaller
{
        [SerializeField] private GaragePrefabsProvider garagePrefabsProvider;
        public override void InstallBindings()
        {
                Container.Bind<GaragePrefabsProvider>().FromInstance(garagePrefabsProvider).AsSingle();
        }
}