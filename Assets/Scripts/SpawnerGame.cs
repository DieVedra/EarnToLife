using UnityEngine;

public class SpawnerGame : Spawner
{
    [Header("Points Spawn")]
    [SerializeField] private Transform _positionSpawnCar;
    [SerializeField] private Transform _parentPositionsSpawnBots;
    // private PrefabStorage _prefabStorage;
    private CarInLevel _carInLevelPrefab;
    // private Bot _botWitgAK;
    private Transform[] _positionsSpawnBots;
    // public void InitFromEntryScene(PrefabStorage prefabStorage, out Car car, out Bot[] bots)
    // {
    //     InitPrefabs(prefabStorage);
    //     car = Spawn(_carPrefab, _positionSpawnCar, _positionSpawnCar);
    //
    //     InitPointArray();
    //     bots = new Bot[_positionsSpawnBots.Length];
    //     for (int i = 0; i < _positionsSpawnBots.Length; i++)
    //     {
    //         bots[i] = Spawn(_botWitgAK, _positionsSpawnBots[i], _positionsSpawnBots[i]);
    //     }
    // }
    // private void InitPrefabs(PrefabStorage prefabStorage)
    // {
    //     _prefabStorage = prefabStorage;
    //     _carPrefab = _prefabStorage.Car1Prefab;
    //     // _botWitgAK = _prefabStorage.BotWithAKPrefab;
    // }
    private void InitPointArray()
    {
        Transform[] points = _parentPositionsSpawnBots.GetComponentsInChildren<Transform>();
        _positionsSpawnBots = new Transform[points.Length - 1];
        for (int i = 0; i < _positionsSpawnBots.Length; i++)
        {
            _positionsSpawnBots[i] = points[i + 1];
        }
    }
}
