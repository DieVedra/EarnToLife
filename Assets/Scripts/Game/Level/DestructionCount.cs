
public class DestructionCount
{
    private readonly DestructionsSignal _destructionsSignal;
    public int Count { get; private set; }

    public DestructionCount(DestructionsSignal destructionsSignal)
    {
        _destructionsSignal = destructionsSignal;
        _destructionsSignal.OnDestruction += Add;
    }

    public void Dispose()
    {
        _destructionsSignal.OnDestruction -= Add;
    }
    private void Add()
    {
        Count++;
    }
}