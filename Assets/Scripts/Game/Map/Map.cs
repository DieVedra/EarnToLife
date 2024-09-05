using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField] private SegmentMap[] _segmentsMaps;
    private int _level;
    
    public void Init(int level)
    {
        _level = level;
        for (int i = 0; i < _segmentsMaps.Length; i++)
        {
            _segmentsMaps[i].gameObject.SetActive(false);
            _segmentsMaps[i].Init(new Spawner());
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (_segmentsMaps.Length > 0)
        {
            for (int i = 0; i < _level; i++)
            {
                if (_segmentsMaps.Length - 1 >= i)
                {
                    if (i == _level - 1)
                    {
                        _segmentsMaps[i].CreateSegment(true);
                    }
                    else
                    {
                        _segmentsMaps[i].CreateSegment();
                    }
                    _segmentsMaps[i].gameObject.SetActive(true);
                }
            }
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < _segmentsMaps.Length; i++)
        {
            _segmentsMaps[i].Dispose();
            _segmentsMaps[i].gameObject.SetActive(false);
        }
    }
}
