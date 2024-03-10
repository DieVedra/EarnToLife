public interface IPlayerData
{
    public string Name { get;}
    public int AllKills { get;}
    public int CurrentKills { get;}
    public int LastKills { get;}
    public Wallet Wallet { get;}
    public GarageConfig GarageConfig { get;}
    public ResultsLevel ResultsLevel { get;}
    public int Days { get;}
    public int Level { get;}
    public bool NewLevelHasBeenOpen { get;}
    public bool SoundOn { get; }
    public bool MusicOn { get; }
}