using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Text;

public class ResultsLevelHandler
{
    private const string DAY = "Day: ";
    private const string DISTANCE = "Distance: ";
    private const string KILLS = "Kills: ";
    private const string MONEY = "Total money: ";
    private PanelScore _panelScore;
    private Slider _levelProgressResults;
    private Slider _levelProgressLastResults;
    private TextMeshProUGUI _textDay;
    private TextMeshProUGUI _textReason;
    private TextMeshProUGUI _textDistance;
    private TextMeshProUGUI _textKills;
    private TextMeshProUGUI _textCash;
    public ResultsLevelHandler(ViewUILevel viewUILevel)
    {
        _panelScore = viewUILevel.PanelScore;
        _levelProgressResults = _panelScore.LevelProgressResults;
        _levelProgressLastResults = _panelScore.LevelProgressLastResults;
        _textDay = _panelScore.TextDay;
        _textReason = _panelScore.TextReason;
        _textDistance = _panelScore.TextDistance;
        _textKills = _panelScore.TextKills;
        _textCash = _panelScore.TextCash;
    }
    public void DisplayOutResultsLevel(ResultsLevel results, ResultsLevel lastResults)
    {
        _textDay.text = BuildString.GetString( new string[] { DAY, results.Day.ToString() });
        _textReason.text = results.ReasonGameOver;
        _textDistance.text = BuildString.GetString(new string[] { DISTANCE, results.Distance.ToString() });
        _textKills.text = BuildString.GetString(new string[] { KILLS, results.Kills.ToString() });
        _textCash.text = BuildString.GetString(new string[] { MONEY, results.TotalMoney.ToString() });

        if (lastResults != null)
        {
            _levelProgressLastResults.value = lastResults.DistanceToDisplayOnSliderInScorePanel;
        }
        else
        {
            _levelProgressLastResults.gameObject.SetActive(false);
        }

        _levelProgressResults.value = 0f;
        _levelProgressResults.DOValue(results.DistanceToDisplayOnSliderInScorePanel, 1f);
    }
}
