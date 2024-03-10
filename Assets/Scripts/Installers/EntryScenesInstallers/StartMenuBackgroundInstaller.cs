using UnityEngine;
using Zenject;

public class StartMenuBackgroundInstaller : MonoInstaller
{
    [SerializeField] private SpriteRenderer _startMenuBackground;

    public override void InstallBindings()
    {
        Container.Bind<SpriteRenderer>().FromInstance(_startMenuBackground).AsSingle();
    }
}