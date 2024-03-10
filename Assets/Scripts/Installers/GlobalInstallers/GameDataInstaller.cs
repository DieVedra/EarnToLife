using UnityEngine;
using Zenject;

public class GameDataInstaller : MonoInstaller
{
    [SerializeField] private GameData _gameData; 
    public override void InstallBindings()
    {
        Container.Bind<GameData>().FromInstance(_gameData).AsSingle();
    }
}

