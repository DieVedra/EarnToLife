using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ActivityLevelContentHandler : MonoBehaviour
{
    [SerializeField] private float _addXRangeDestructibleObjects = 30f;
    [SerializeField] private float _addXRangeZombies = 20f;
    [SerializeField] private float _addXRangeLevelDecorations = 40f;

    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private LevelBlock[] _levelBlocks;

    private DebrisKeeper _debrisKeeper;
    private LevelBlocksHandler _levelBlocksHandler;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();


    public void Init(DebrisKeeper debrisKeeper)
    {
        _levelBlocksHandler = new LevelBlocksHandler(
            _levelBlocks,
            new LevelBlockContentGrouper(_addXRangeLevelDecorations, _addXRangeDestructibleObjects, _addXRangeZombies),
            _cameraTransform);
        _debrisKeeper = debrisKeeper;
        _debrisKeeper.Init(_cameraTransform);

        Observable.EveryUpdate().Subscribe(x =>
        {
            _levelBlocksHandler.TrySwitchToNextBlock();
            _debrisKeeper.DebrisCheckActivity();

        }).AddTo(_compositeDisposable);
    }
    private void OnDisable()
    {
        _levelBlocksHandler?.Dispose();
        _compositeDisposable.Clear();
        _debrisKeeper.Dispose();
    }
}