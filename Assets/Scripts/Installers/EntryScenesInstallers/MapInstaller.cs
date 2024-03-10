using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class MapInstaller : MonoInstaller
{
    [SerializeField] private Map _map;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _parentMapTransforms;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform[] _pointsMapTransforms;


    public override void InstallBindings()
    {
        Container.Bind<Map>().FromInstance(_map).AsSingle();
        Container.Bind<Transform[]>().FromInstance(_pointsMapTransforms).AsSingle();
        Container.Bind<Transform>().FromInstance(_parentMapTransforms).AsSingle();
    }
}