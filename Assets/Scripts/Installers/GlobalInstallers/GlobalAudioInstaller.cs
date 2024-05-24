using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GlobalAudioInstaller : MonoInstaller
{
    [SerializeField] private GlobalAudio _globalAudio;
    [SerializeField, Expandable] private AudioClipProvider _audioClipProvider;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GlobalAudio>().FromInstance(_globalAudio).AsSingle().NonLazy();
        Container.Bind<AudioClipProvider>().FromInstance(_audioClipProvider).AsSingle();
    }
}
