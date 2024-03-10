using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombinedImprovement
{
    [SerializeField] private List<GameObject> _partsToON;
    [SerializeField] private List<GameObject> _partsToOFF;

    public IReadOnlyList<GameObject> PartsToON => _partsToON;
    public IReadOnlyList<GameObject> PartsToOFF => _partsToOFF;
    public void ImprovementSwitch()
    {
        if (_partsToOFF != null)
        {
            for (int i = 0; i < _partsToOFF.Count; i++)
            {
                if (_partsToOFF[i] != null)
                {
                    _partsToOFF[i].SetActive(false);
                }
            }
        }

        if (_partsToON != null)
        {
            for (int i = 0; i < _partsToON.Count; i++)
            {
                if (_partsToON[i] != null)
                {
                    _partsToON[i].SetActive(true);
                }
            }
        }
    }

    public void ResetImprovement()
    {
        if (_partsToOFF != null)
        {
            for (int i = 0; i < _partsToOFF.Count; i++)
            {
                if (_partsToOFF[i] != null)
                {
                    _partsToOFF[i].SetActive(true);
                }
            }
        }

        if (_partsToON != null)
        {
            for (int i = 0; i < _partsToON.Count; i++)
            {
                if (_partsToON[i] != null)
                {
                    _partsToON[i].SetActive(false);
                }
            }
        }
    }
}