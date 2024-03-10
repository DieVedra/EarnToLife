using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelPrefabsProvider", menuName = "LevelContent/LevelPrefabsProvider", order = 51)]
public class LevelPrefabsProvider : ScriptableObject
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private List<CarInLevel> _carsInLevelPrefabs;

    [SerializeField, HorizontalLine(color: EColor.White)] private ViewUILevel _viewUILevelPrefab;
    // [SerializeField, HorizontalLine(color: EColor.Green)] private Image _segmentIconUpgradeButtonPrefab;
    // public Image SegmentIconUpgradeButtonIconPrefab => _segmentIconUpgradeButtonPrefab;
    public IReadOnlyList<CarInLevel> CarsInLevelPrefabs => _carsInLevelPrefabs;
    public ViewUILevel ViewUILevelPrefab => _viewUILevelPrefab;
}
