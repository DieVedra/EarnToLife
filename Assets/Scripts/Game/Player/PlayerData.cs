using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerData : IPlayerData
{
    public string Name { get;  set; } = "User";
    public int AllKills { get;  set; }
    public int CurrentKills { get;  set; }
    public int LastKills { get;  set; }
    public Wallet Wallet { get;}
    public GarageConfig GarageConfig { get;}
    public ResultsLevel ResultsLevel { get;  set; } = null;

    public int Days { get;  set; }
    public int Level { get;  set; }
    public bool NewLevelHasBeenOpen { get;  set; }
    
    public bool SoundOn { get; set; }
    public bool MusicOn { get; set; }
    public PlayerData(Wallet wallet, GarageConfig garageConfig, int level = 1, int days = 1, bool newLevelHasBeenOpen = true, bool soundOn = true, bool musicOn = false)
    {
        Wallet = wallet;
        GarageConfig = garageConfig;
        Level = level;
        Days = days;
        NewLevelHasBeenOpen = newLevelHasBeenOpen;
        SoundOn = soundOn;
        MusicOn = musicOn;
    }
}
