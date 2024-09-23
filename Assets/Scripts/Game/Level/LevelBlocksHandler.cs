using UniRx;
using UnityEngine;

public class LevelBlocksHandler
{
    private readonly Transform _cameraTransform;
    private readonly LevelBlock[] _levelBlocks;
    private int _currentBlockActiveIndex = 0;
    private ActivityHandler _activityHandler;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    public LevelBlocksHandler(LevelBlock[] levelBlocks,
        LevelBlockContentGrouper levelBlockContentGrouper, Transform cameraTransform)
    {
        _levelBlocks = levelBlocks;
        _cameraTransform = cameraTransform;
        _activityHandler = new ActivityHandler(cameraTransform);
        InitBlocks(levelBlockContentGrouper);
        DefineCurrentBlockActive();
        _levelBlocks[_currentBlockActiveIndex].Activate();
    }

    public void Dispose()
    {
        _compositeDisposable.Clear();
    }

    public void TrySwitchToNextBlock()
    {
        // Debug.Log($"TrySwitchToNextBlock");
        if (_cameraTransform.position.x > _levelBlocks[_currentBlockActiveIndex].PointEnableNextBlockX)
        {
            _levelBlocks[_currentBlockActiveIndex + 1].Activate();
            if (_cameraTransform.position.x > _levelBlocks[_currentBlockActiveIndex + 1].PointDisablePreviousBlockX)
            {
                _levelBlocks[_currentBlockActiveIndex].Deactivate();
                _currentBlockActiveIndex++;
            }
        }
    }

    private void InitBlocks(LevelBlockContentGrouper levelBlockContentGrouper)
    {
        foreach (LevelBlock levelBlock in _levelBlocks)
        {
            levelBlock.Init( _activityHandler, levelBlockContentGrouper);
        }
    }
    private void DefineCurrentBlockActive()
    {
        for (int i = 0; i < _levelBlocks.Length; i++)
        {
            _currentBlockActiveIndex = i;
            if (i == _levelBlocks.Length -1)
            {
                break;
            }
            else if (_levelBlocks[i+1].transform.position.x > _cameraTransform.position.x)
            {
                break;
            }
        }
    }
}