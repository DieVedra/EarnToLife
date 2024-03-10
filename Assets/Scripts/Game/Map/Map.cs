using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private SegmentMap[] _segmentsMaps;
    private GarageConfig _garageConfig;
    public void Init(SegmentMap[] segmentsMaps, GameData gameData)
    {
        _segmentsMaps = segmentsMaps;
        // _garageConfig = gameData.PlayerData.GarageConfig;
    }
    public void InitSegments()
    {
        // for (int i = 0; i < _segmentsMaps.Length; i++)
        // {
        //     _segmentsMaps[i].gameObject.SetActive(false);
        // }
        // for (int i = 0; i <= _garageConfig.AvailableLotCarIndex; i++)
        // {
        //     _segmentsMaps[i].gameObject.SetActive(true);
        //     if (i < _garageConfig.AvailableLotCarIndex)
        //     {
        //         _segmentsMaps[i].Init(false);
        //     }
        //     else
        //     {
        //         _segmentsMaps[i].Init();
        //     }
        // }
    }
}
