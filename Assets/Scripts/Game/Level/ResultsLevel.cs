using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsLevel
{
    public int Kills { get; }
    public int MoneyForKills { get; }
    public int Day { get; }
    public int Distance { get; }
    public int MoneyForDistance { get; }
    public float DistanceToDisplayOnSliderInScorePanel { get; }
    public int TotalMoney { get; }
    public string ReasonGameOver { get; }
    public ResultLevelForSlider ResultLevelForSlider { get; }
    public ResultsLevel(int kills, int day, float distance, float distanceToSlider,
        int totalMoney, int moneyForKills, int moneyForDistance, string reason,
        ResultLevelForSlider resultLevelForSlider = null)
    {
        Kills = kills;
        Day = day;
        Distance = (int)distance;
        DistanceToDisplayOnSliderInScorePanel = distanceToSlider;
        TotalMoney = totalMoney;
        ReasonGameOver = reason;
        ResultLevelForSlider = resultLevelForSlider;
        MoneyForKills = moneyForKills;
        MoneyForDistance = moneyForDistance;
    }
}
