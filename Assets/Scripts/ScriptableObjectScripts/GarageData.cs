using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GarageData", menuName = "Data/GarageData", order = 51)]
public class GarageData : ScriptableObject
{
    [SerializeField, Expandable, HorizontalLine(color: EColor.Red), BoxGroup("ParkingLots")]
    private ParkingLotConfiguration[] _parkingLots;
    [SerializeField, HorizontalLine(color: EColor.Red), BoxGroup("LevelConfigs")]
    private LevelConfig[] _levelConfigs;
    [SerializeField, Expandable, HorizontalLine(color: EColor.Red), BoxGroup("UpgradeButtonsGarageContent")]
    private UpgradeButtonsGarageContent[] _upgradeButtonsGarageContent;
    [SerializeField, Expandable, HorizontalLine(color: EColor.Red), BoxGroup("DescriptionsButtonsUpgrades")]
    private DescriptionsButtonsUpgrades _descriptionsButtonsUpgrades;
    [SerializeField, Expandable, HorizontalLine(color: EColor.Red), BoxGroup("GaragePrefabsProvider")]
    private GaragePrefabsProvider _garagePrefabsProvider;
    public IReadOnlyList<ParkingLotConfiguration> ParkingLotsConfigurations => _parkingLots;
    public IReadOnlyList<LevelConfig> LevelConfigs => _levelConfigs;
    public IReadOnlyList<UpgradeButtonsGarageContent> UpgradeButtonsGarageContent => _upgradeButtonsGarageContent;
    public DescriptionsButtonsUpgrades DescriptionsButtonsUpgrades => _descriptionsButtonsUpgrades;
    public GaragePrefabsProvider GaragePrefabsProvider => _garagePrefabsProvider;
}
