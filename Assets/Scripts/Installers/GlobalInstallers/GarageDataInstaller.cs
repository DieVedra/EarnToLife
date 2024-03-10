using UnityEngine;
using Zenject;

public class GarageDataInstaller : MonoInstaller
{
    [SerializeField] private GarageData _garageData;

    public override void InstallBindings()
    {
        Container.Bind<GarageData>().FromInstance(_garageData).AsSingle();
    }
}
