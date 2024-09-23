using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public class SwitchGarageLot
{
    private const float LOT_OFFSET = -20f;
    private const float DURATION_IN_SEC = 1f;
    private Transform _transformLotsParent;
    private int _currentSelectLotCarIndex;
    private IReadOnlyList<ParkingLot> _parkingLots;

    private CancellationTokenSource _cancellationTokenSource;
    private CompositeDisposable _compositeDisposable;
    public event Action<int> OnSwitch;
    public event Action<ParkingLot, int> OnEndLotSwitch;
    public SwitchGarageLot(IReadOnlyList<ParkingLot> lots, Transform lotsParentTransform)
    {
        _parkingLots = lots;
        _transformLotsParent = lotsParentTransform;
        _currentSelectLotCarIndex = 0;
        _cancellationTokenSource = new CancellationTokenSource();
        _compositeDisposable = new CompositeDisposable();
        Observable.OnceApplicationQuit().Subscribe(_ =>
        {
            _cancellationTokenSource.Cancel();
            _compositeDisposable.Clear();
        }).AddTo(_compositeDisposable);
    }
    public void SwitchLotRight()
    {
        MoveLot(LOT_OFFSET);
        _currentSelectLotCarIndex++;
        OnSwitch?.Invoke(_currentSelectLotCarIndex);
    }
    public void SwitchLotLeft()
    {
        MoveLot(-LOT_OFFSET);
        _currentSelectLotCarIndex--;
        OnSwitch?.Invoke(_currentSelectLotCarIndex);
    }
    public void SwitchToLotDirectly(int indexLot)
    {
        int iterationsCount;
        if (_currentSelectLotCarIndex == 0)
        {
            iterationsCount = indexLot;
            MoveCycle(iterationsCount, LOT_OFFSET);
            _currentSelectLotCarIndex = indexLot;
        }
        else if (_currentSelectLotCarIndex < indexLot)
        {
            iterationsCount = indexLot - _currentSelectLotCarIndex;
            MoveCycle(iterationsCount, LOT_OFFSET);
            _currentSelectLotCarIndex = indexLot;

        }
        else if (_currentSelectLotCarIndex > indexLot)
        {
            iterationsCount = _currentSelectLotCarIndex - indexLot;
            MoveCycle(iterationsCount, -LOT_OFFSET);
            _currentSelectLotCarIndex = indexLot;
        }
        OnSwitch?.Invoke(_currentSelectLotCarIndex);
        InvokeEventEndSwitch();
    }
    private async void MoveLot(float offset)
    {
        await _transformLotsParent.DOLocalMoveX(CalculateOffset(offset), DURATION_IN_SEC).SetEase(Ease.OutCubic)
            .WithCancellation(_cancellationTokenSource.Token);
        InvokeEventEndSwitch();
    }
    private void InvokeEventEndSwitch()
    {
        OnEndLotSwitch?.Invoke(_parkingLots[_currentSelectLotCarIndex], _currentSelectLotCarIndex);
    }
    private float CalculateOffset(float offset)
    {
        return _transformLotsParent.localPosition.x + offset;
    }
    private void MoveCycle(int iterationCount, float offset)
    {
        Vector3 position;
        for (int i = 0; i < iterationCount; i++)
        {
            position = _transformLotsParent.localPosition;
            position = new Vector3(position.x + CalculateOffset(offset), position.y, position.z);
            _transformLotsParent.localPosition = position;
        }
    }
}
