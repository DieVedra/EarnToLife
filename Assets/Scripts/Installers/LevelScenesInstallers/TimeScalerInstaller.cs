using UnityEngine;
using Zenject;

public class TimeScalerInstaller : MonoInstaller
{
    [SerializeField] private TimeScaler _timeScaler;
    
    public override void InstallBindings()
    {
        Container.Bind<TimeScaler>().FromInstance(_timeScaler).AsSingle();

    }
}