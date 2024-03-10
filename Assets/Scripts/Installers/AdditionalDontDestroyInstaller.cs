using UnityEngine;
using Zenject;

public class AdditionalDontDestroyInstaller : MonoInstaller
{
    [SerializeField] private GameObject[] _toInstall;
    public override void InstallBindings()
    {
        foreach (var installable in _toInstall)
        {
            DontDestroyOnLoad(Instantiate(installable, transform));
        }
    }
}
