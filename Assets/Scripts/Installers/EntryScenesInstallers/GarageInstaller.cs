using UnityEngine;
using Zenject;

public class GarageInstaller : MonoInstaller
{
    [SerializeField] private Garage _garage;

    public override void InstallBindings()
    {
        Container.Bind<Garage>().FromInstance(_garage).AsSingle();
    }
}
