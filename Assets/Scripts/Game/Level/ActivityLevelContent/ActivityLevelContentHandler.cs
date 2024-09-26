using NaughtyAttributes;
using UniRx;
using UnityEngine;

public class ActivityLevelContentHandler : MonoBehaviour
{
    [Header("X Axis")]
    [Tooltip("XRangeDestructibleObjects")]
    [SerializeField] private float _addXRangeDestructibleObjects = 30f;
    [Tooltip("XRangeZombies")]
    [SerializeField] private float _addXRangeZombies = 20f;
    [Tooltip("XRangeLevelDecorations")]
    [SerializeField] private float _addXRangeLevelDecorations = 40f;
    
    [Header("Y Axis")]
    [Tooltip("YRangeForAll")]
    [SerializeField] private float _addYRangeForAll = 30f;

    [SerializeField, HorizontalLine(color:EColor.Orange)] private Transform _cameraTransform;

    
    [SerializeField, HorizontalLine(color:EColor.Green)] private LevelBlock[] _levelBlocks;

    private DebrisKeeper _debrisKeeper;
    private LevelBlocksHandler _levelBlocksHandler;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();


    public void Init(DebrisKeeper debrisKeeper)
    {
        _levelBlocksHandler = new LevelBlocksHandler(
            _levelBlocks,
            new LevelBlockContentGrouper(_addXRangeLevelDecorations, _addXRangeDestructibleObjects, _addXRangeZombies, _addYRangeForAll),
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
        _compositeDisposable.Clear();
        _levelBlocksHandler.DeactivateAllBlocks();
        _debrisKeeper.Dispose();
    }
}