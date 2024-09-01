using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPrefabsProvider", menuName = " MapPrefabsProvider", order = 51)]
public class MapPrefabsProvider : ScriptableObject
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _currentPointFlagPrefab;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _previousPointFlagPrefab;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _nextPointFlagPrefab;
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _lineSpritePrefab;
    
    public Transform LineSpritePrefab => _lineSpritePrefab;
    public Transform CurrentPointFlagPrefab => _currentPointFlagPrefab;
    public Transform PreviousPointFlagPrefab => _previousPointFlagPrefab;
    public Transform NextPointFlagPrefab => _nextPointFlagPrefab;
}