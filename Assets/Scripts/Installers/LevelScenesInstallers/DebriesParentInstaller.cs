using UnityEngine;
using Zenject;

public class DebriesParentInstaller : MonoInstaller
{
    [SerializeField] private Transform _debrisParent;
    public override void InstallBindings()
    {
        Container.Bind<Transform>().WithId("DebrisParent").FromInstance(_debrisParent).AsSingle();
    }
}