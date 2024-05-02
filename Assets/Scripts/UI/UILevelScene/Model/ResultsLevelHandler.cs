using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public class ResultsLevelHandler
{
    private const string DAY = "Day ";
    private const string COMPLETE = " Complete";
    private const string METERS = " Meters";
    private PanelScore _panelScore;
    private Slider _levelProgressResults;
    private Slider _levelProgressLastResults;
    private TextMeshProUGUI _textDay;
    private TextMeshProUGUI _textReason;
    private TextMeshProUGUI _textDistance;
    private TextMeshProUGUI _textMoneyForDistance;
    private TextMeshProUGUI _textKills;
    private TextMeshProUGUI _textMoneyForKills;
    private TextMeshProUGUI _textCash;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    public ResultsLevelHandler(PanelScore panelScore, ReactiveCommand disposeCommand)
    {
        _panelScore = panelScore;
        _levelProgressResults = _panelScore.LevelProgressResults;
        _levelProgressLastResults = _panelScore.LevelProgressLastResults;
        _textDay = _panelScore.TextDay;
        _textReason = _panelScore.TextReason;
        _textDistance = _panelScore.TextDistance;
        _textKills = _panelScore.TextKills;
        _textCash = _panelScore.TextCash;
        _textMoneyForDistance = _panelScore.TextMoneyDistance;
        _textMoneyForKills = _panelScore.TextMoneyKills;
        disposeCommand.Subscribe(_ => { Dispose();});
    }
    public void DisplayOutResultsLevel(ResultsLevel results, ResultsLevel lastResults)
    {
        _textDay.text = BuildString.GetString( new string[] { DAY, results.Day.ToString(), COMPLETE});
        _textReason.text = results.ReasonGameOver;
        _textKills.text = results.Kills.ToString();
        _textDistance.text = BuildString.GetString( new string[] { results.Distance.ToString(), METERS});
        _textMoneyForDistance.text = results.Distance.ToString();   
        _textCash.text = results.TotalMoney.ToString();
        _textMoneyForKills.text = results.MoneyForKills.ToString();

        if (lastResults != null)
        {
            _levelProgressLastResults.value = lastResults.DistanceToDisplayOnSliderInScorePanel;
        }
        else
        {
            _levelProgressLastResults.gameObject.SetActive(false);
        }

        _levelProgressResults.value = 0f;
        _levelProgressResults.DOValue(results.DistanceToDisplayOnSliderInScorePanel, 1f).WithCancellation(_cancellationTokenSource.Token);
    }

    private void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
}
