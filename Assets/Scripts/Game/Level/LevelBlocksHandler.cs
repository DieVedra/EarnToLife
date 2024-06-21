
using UniRx;
using UnityEngine;

public class LevelBlocksHandler
{
    private readonly LevelBlock[] _levelBlocks;
    private readonly Transform _cameraTransform;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private int _currentBlockActiveIndex = 0;
    public LevelBlocksHandler(LevelBlock[] levelBlocks, Transform cameraTransform)
    {
        _levelBlocks = levelBlocks;
        _cameraTransform = cameraTransform;
        for (int i = 1; i < _levelBlocks.Length; i++)
        {
            levelBlocks[i].SetInactive();
        }

        Observable.EveryUpdate().Subscribe(x =>
        {
            TrySwitchBlocks();
        }).AddTo(_compositeDisposable);
    }

    private void TrySwitchBlocks()
    {
        if (_cameraTransform.position.x > _levelBlocks[_currentBlockActiveIndex].PointEnableNextBlockX)
        {
            _levelBlocks[_currentBlockActiveIndex + 1].SetActive();
            if (_cameraTransform.position.x > _levelBlocks[_currentBlockActiveIndex + 1].PointDisablePreviousBlockX)
            {
                _levelBlocks[_currentBlockActiveIndex].SetInactive();
                _currentBlockActiveIndex++;
            }
        }
    }

    public void Dispose()
    {
        _compositeDisposable.Clear();
    }
    
}