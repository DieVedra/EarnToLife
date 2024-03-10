using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class MapPanelHandler
{
    // private readonly string _day;
    private TextMeshProUGUI _textDate;
    private PlayerDataHandler _playerDataHandler;
    private Map _map;
    public MapPanelHandler(MapPanel mapPanel, Map map)
    {
        _map = map;
        // _textDate = mapPanel.TextDate;
        // _day = $"{_textDate.text} ";
        // _playerDataHandler = playerDataHandler;
    }
    public void MapEnable()
    {
        _map.gameObject.SetActive(true);
        _map.InitSegments();
        SetDay();
    }
    public void MapDisable()
    {
        _map.gameObject.SetActive(false);
    }
    private void SetDay()
    {
        // StringBuilder stringBuilder = new StringBuilder();
        // stringBuilder.Append(_day);
        // stringBuilder.Append(_playerConfig.Days.ToString());
        //
        // _textDate.text = stringBuilder.ToString();
        
        //!!!!!!
        // _textDate.text = _playerConfig.Days.ToString();
    }
}
