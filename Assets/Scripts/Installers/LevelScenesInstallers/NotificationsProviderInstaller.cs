using UnityEngine;
using Zenject;

public class NotificationsProviderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<NotificationsProvider>().FromNew().AsSingle();
    }
}