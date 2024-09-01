using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField] private SegmentMap[] _segmentsMaps;
    [SerializeField] private int level;
    [Button("Init")]
    public void Init(/*int level*/)
    {
        Debug.Log($"Level: {level}");
        Spawner spawner = new Spawner();
        if (_segmentsMaps.Length > 0)
        {
            for (int i = 0; i < _segmentsMaps.Length; i++)
            {
                _segmentsMaps[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < level; i++)
            {
                if (_segmentsMaps.Length >= level)
                {
                    if (i == level - 1)
                    {
                        _segmentsMaps[i].Init(spawner,true);
                    }
                    else
                    {
                        _segmentsMaps[i].Init(spawner);
                    }
                }
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
