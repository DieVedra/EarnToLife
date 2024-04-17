using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private Level _level;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Level>().FromInstance(_level).AsSingle();
        Container.Bind<Factory>().FromNew().AsSingle();
        
        
        
        // var 
        // Container.Bind<LevelPool>().FromInstance(_level).AsSingle();
    }
}