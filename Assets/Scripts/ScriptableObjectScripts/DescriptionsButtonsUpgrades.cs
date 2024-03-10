using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
[CreateAssetMenu(fileName = "DescriptionsButtonsUpgrades", menuName = "GarageContent/DescriptionsButtonsUpgrades", order = 51)]
public class DescriptionsButtonsUpgrades : ScriptableObject
{
    [SerializeField] private string _nameEngineUpgrade;
    [SerializeField, ResizableTextArea] private string _descriptionEngineUpgrade;
    
    [SerializeField, HorizontalLine(color: EColor.Red)] private string _nameGearboxUpgrade;
    [SerializeField, ResizableTextArea] private string _descriptionGearboxUpgrade;

    [SerializeField, HorizontalLine(color: EColor.Blue)] private string _nameWheelUpgrade;
    [SerializeField, ResizableTextArea] private string _descriptionWheelUpgrade;

    [SerializeField, HorizontalLine(color: EColor.Green)] private string _nameGunUpgrade;
    [SerializeField, ResizableTextArea] private string _descriptionGunUpgrade;

    [SerializeField, HorizontalLine(color: EColor.Yellow)] private string _nameCorpusUpgrade;
    [SerializeField, ResizableTextArea] private string _descriptionCorpusUpgrade;

    [SerializeField, HorizontalLine(color: EColor.Orange)] private string _nameBoosterUpgrade;
    [SerializeField, ResizableTextArea] private string _descriptionBoosterUpgrade;

    [SerializeField, HorizontalLine(color: EColor.Indigo)] private string _nameTankUpgrade;
    [SerializeField, ResizableTextArea] private string _descriptionTankUpgrade;

    public string GetNameEngineUpgrade => _nameEngineUpgrade;
    public string GetDescriptionEngineUpgrade => _descriptionEngineUpgrade;
    
    public string GetNameGearboxUpgrade => _nameGearboxUpgrade;
    public string GetDescriptionGearboxUpgrade => _descriptionGearboxUpgrade;
    
    public string GetNameWheelUpgrade => _nameWheelUpgrade;
    public string GetDescriptionWheelUpgrade => _descriptionWheelUpgrade;
    
    public string GetNameGunUpgrade => _nameGunUpgrade;
    public string GetDescriptionGunUpgrade => _descriptionGunUpgrade;
    
    public string GetNameCorpusUpgrade => _nameCorpusUpgrade;
    public string GetDescriptionCorpusUpgrade => _descriptionCorpusUpgrade;
    
    public string GetNameBoosterUpgrade => _nameBoosterUpgrade;
    public string GetDescriptionBoosterUpgrade => _descriptionBoosterUpgrade;
    
    public string GetNameTankUpgrade => _nameTankUpgrade;
    public string GetDescriptionTankUpgrade => _descriptionTankUpgrade;
}
