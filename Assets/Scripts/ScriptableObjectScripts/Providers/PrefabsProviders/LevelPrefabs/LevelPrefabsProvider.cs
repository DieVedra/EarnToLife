using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelPrefabsProvider", menuName = "LevelContent/LevelPrefabsProvider", order = 51)]
public class LevelPrefabsProvider : ScriptableObject
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private List<CarInLevel> _carsInLevelPrefabs;

    [SerializeField, HorizontalLine(color: EColor.White)] private ViewUILevel _viewUILevelPrefab;
    [SerializeField, HorizontalLine(color: EColor.Gray)] private TextMeshProUGUI _notificationsTextPrefab;
    [SerializeField, Expandable] private LevelParticlesProvider _levelParticlesProvider;
    public IReadOnlyList<CarInLevel> CarsInLevelPrefabs => _carsInLevelPrefabs;
    public ViewUILevel ViewUILevelPrefab => _viewUILevelPrefab;
    public LevelParticlesProvider LevelParticlesProvider => _levelParticlesProvider;
    public TextMeshProUGUI NotificationsTextPrefab => _notificationsTextPrefab;
}
