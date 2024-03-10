public class KillsCount
{
    public int Kills { get; private set; } = 0;
    public void AddKill()
    {
        Kills++;
    }
}
