using UnityEngine;
using Zenject;

public class ViewEntrySceneInstaller : MonoInstaller
{
    [SerializeField] private ViewEntryScene _viewEntryScene;
    public override void InstallBindings()
    {
        Container.Bind<ViewEntryScene>().FromInstance(_viewEntryScene).AsSingle();
    }
}
