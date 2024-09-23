using System;
using UniRx;

public class FrameByFrameDivider
{
    private int _operationPart = 0;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();

    public void Dispose()
    {
        _compositeDisposable.Clear();
    }
    public void FrameByFrameSeparatedOperation(bool isLoop , params Action[] operations)
    {
        _compositeDisposable.Clear();
        _compositeDisposable = new CompositeDisposable();
        _operationPart = 0;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            operations[_operationPart].Invoke();
            if (_operationPart == operations.Length - 1)
            {
                if (isLoop == false)
                {
                    _compositeDisposable.Clear();
                }
                _operationPart = 0;
            }
            else
            {
                _operationPart++;
            }
        }).AddTo(_compositeDisposable);
    }
}