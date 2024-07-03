public class KillsCount
{
    private readonly KillsSignal _killsSignal;

    public int Kills { get; private set; } = 0;

    public KillsCount(KillsSignal killsSignal)
    {
        _killsSignal = killsSignal;
        _killsSignal.OnKill += AddKill;
    }

    private void AddKill()
    {
        Kills++;
    }

    public void Dispose()
    {
        _killsSignal.OnKill -= AddKill;
    }
}
