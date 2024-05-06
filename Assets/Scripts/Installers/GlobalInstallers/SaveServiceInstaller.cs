using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveServiceInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.Bind<SaveService>().FromNew().AsSingle();
    }
}
