using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CustomizeCar : MonoBehaviour
{
    [SerializeField, BoxGroup("CorpusUpgrades"), HorizontalLine(color: EColor.Red)] protected List<CombinedImprovement> CorpusUpgrades;
    [SerializeField, BoxGroup("WheelsUpgrades"), HorizontalLine(color: EColor.Green)] protected List<CombinedImprovement> WheelsUpgrades;
    [SerializeField, BoxGroup("Gun"), HorizontalLine(color: EColor.Yellow)] protected GameObject Gun;
    [SerializeField, BoxGroup("Booster"), HorizontalLine(color: EColor.Violet)] protected CombinedImprovement BoosterImprovement;
    private CarConfiguration _carConfiguration;
    public event Action<IReadOnlyList<GameObject>> OnSetWheels;
    public void Construct(CarConfiguration carConfiguration)
    {
        _carConfiguration = carConfiguration;
        TryOpenGun();
        TryOpenBooster();
        SetCorpusParts();
        SetWheels();
    }
    private void SetWheels()
    {
        WheelsUpgrades[_carConfiguration.WheelsIndex].ImprovementSwitch();
        OnSetWheels?.Invoke(WheelsUpgrades[_carConfiguration.WheelsIndex].PartsToON);
    }
    private void SetCorpusParts()
    {
        for (int i = 0; i <= _carConfiguration.CorpusIndex; i++)
        {
            CorpusUpgrades[i].ImprovementSwitch();
        }
    }
    private void TryOpenGun()
    {
        if (CheckQuantity(_carConfiguration.GunCountAmmo))
        {
            Gun.gameObject.SetActive(true);
        }
        else
        {
            Gun.gameObject.SetActive(false);
        }
    }
    private void TryOpenBooster()
    {
        if (CheckQuantity(_carConfiguration.BoosterCountFuelQuantity))
        {
            
            BoosterImprovement.ImprovementSwitch();
        }
        else
        {
            BoosterImprovement.ResetImprovement();
        }
    }
    private bool CheckQuantity(float quantity)
    {
        if (quantity > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    [Button("Corpus 0")]
    private void a()
    {
        CorpusUpgrades[0].ImprovementSwitch();
    }
    
    [Button("Corpus 1")]
    private void b()
    {
        a();
        for (int i = 0; i < CorpusUpgrades.Count; i++)
        {
            CorpusUpgrades[i].ImprovementSwitch();
            if (i == 1)
            {
                break;
            }
        }
    }
    
    [Button("Corpus 2")]
    private void c()
    {
        a();
        for (int i = 0; i < CorpusUpgrades.Count; i++)
        {
            CorpusUpgrades[i].ImprovementSwitch();
            if (i == 2)
            {
                break;
            }
        }
    }
    
    [Button("Corpus 3")]
    private void e()
    {
        a();
        for (int i = 0; i < CorpusUpgrades.Count; i++)
        {
            CorpusUpgrades[i].ImprovementSwitch();
            if (i == 3)
            {
                break;
            }
        }
    }
    [Button("Corpus 4")]
    private void d()
    {
        a();
        for (int i = 0; i < CorpusUpgrades.Count; i++)
        {
            CorpusUpgrades[i].ImprovementSwitch();
            if (i == 4)
            {
                break;
            }
        }
    }
}
