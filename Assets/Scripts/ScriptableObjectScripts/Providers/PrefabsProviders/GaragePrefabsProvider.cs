using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GaragePrefabsProvider", menuName = "GarageContent/GaragePrefabsProvider", order = 51)]
public class GaragePrefabsProvider : ScriptableObject
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private List<CarInGarage> _carsInGaragePrefabs;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Image _segmentIconUpgradeButtonPrefab;
    public Image SegmentIconUpgradeButtonIconPrefab => _segmentIconUpgradeButtonPrefab;
    public IReadOnlyList<CarInGarage> CarsInGaragePrefabs => _carsInGaragePrefabs;
}
