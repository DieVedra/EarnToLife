using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveServiceInstaller : MonoInstaller
{
    [SerializeField] private bool _saveOn;

    public override void InstallBindings()
    {
        Container.Bind<SaveService>().FromNew().AsSingle().WithArguments(_saveOn);
    }
}
