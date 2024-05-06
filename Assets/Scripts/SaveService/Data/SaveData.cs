[System.Serializable]
public struct SaveData
{
    public int CurrentSelectLotCarIndex;
    public int AvailableLotCarIndex;
    public int Money;
    public int Days;
    public int Level;
    public bool NewLevelHasBeenOpen;
    public bool SoundOn;
    public bool MusicOn;
    public SaveParkingIndexes[] SavesParkingsIndexes;
}
