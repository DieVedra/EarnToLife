using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Factory
{
    private readonly Spawner _spawner;
    // private readonly DiContainer _container;
    // private readonly IInstantiator _instantiator;
    public Factory(/*IInstantiator instantiator*/)
    {
        _spawner = new Spawner();
        // _instantiator = instantiator;
    }

    public CarInLevel CreateCar(CarInLevel prefab, Transform position)
    {
        return _spawner.Spawn(prefab, position);
    }

    public ViewUILevel CreateCanvas(ViewUILevel prefab)
    {
        return _spawner.Spawn(prefab);
        // return _instantiator.InstantiatePrefabForComponent<ViewUILevel>(prefab);
    }

    public ParticleSystem CreateEffect(ParticleSystem prefab, Transform transform)
    {
        return _spawner.Spawn(prefab, transform, transform);
    }
    public DebrisEffect CreateEffect(DebrisEffect prefab, Transform transform)
    {
        return _spawner.Spawn(prefab, transform, transform);
    }
    // public class UnitFactory
    // {
    //     readonly DiContainer _container;
    //     readonly List<UnityEngine.Object> _prefabs;
    //
    //     public UnitFactory(
    //         List<UnityEngine.Object> prefabs,
    //         DiContainer container)
    //     {
    //         _container = container;
    //         _prefabs = prefabs;
    //     }
    //
    //     public BaseUnit Create<T>()
    //         where T : BaseUnit
    //     {
    //         var prefab = _prefabs.OfType<T>().Single();
    //         return _container.InstantiatePrefabForComponent<T>(prefab);
    //     }
    // }
    //
    // public class TestInstaller : MonoInstaller<TestInstaller>
    // {
    //     public FooUnit FooPrefab;
    //     public BarUnit BarPrefab;
    //
    //     public override void InstallBindings()
    //     {
    //         Container.Bind<UnitFactory>().AsSingle();
    //         Container.Bind<UnityEngine.Object>().FromInstance(FooPrefab).WhenInjectedInto<UnitFactory>();
    //         Container.Bind<UnityEngine.Object>().FromInstance(BarPrefab).WhenInjectedInto<UnitFactory>();
    //     }
    // }
    
    
    
    
    
    
}
