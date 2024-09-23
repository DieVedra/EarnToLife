using System.Collections.Generic;
using UnityEngine;

public class LevelBlockContentGrouper
{
    private readonly float _addXRangeLevelDecorations;
    private readonly float _addXRangeDestructibleObjects;
    private readonly float _addXRangeZombies;

    public LevelBlockContentGrouper(float addXRangeLevelDecorations, float addXRangeDestructibleObjects, float addXRangeZombies)
    {
        _addXRangeLevelDecorations = addXRangeLevelDecorations;
        _addXRangeDestructibleObjects = addXRangeDestructibleObjects;
        _addXRangeZombies = addXRangeZombies;
    }

    public List<ActivityObject> GetActivityContent(Transform content)
    {
        DestructibleObject[] destructibleObjects = content.GetComponentsInChildren<DestructibleObject>();
        Zombie[] zombies = content.GetComponentsInChildren<Zombie>();
        LevelDecoration[] levelDecorations = content.GetComponentsInChildren<LevelDecoration>();
        List<ActivityObject> activityObjects = new List<ActivityObject>();
        
        CreateAndAdd(ref activityObjects, destructibleObjects, _addXRangeDestructibleObjects);
        CreateAndAdd(ref activityObjects, zombies, _addXRangeZombies);
        CreateAndAdd(ref activityObjects, levelDecorations, _addXRangeLevelDecorations);
        
        return activityObjects;
    }

    private void CreateAndAdd(ref List<ActivityObject> activityObjectsPar, MonoBehaviour[] objectsContent, float addXRange)
    {
        foreach (var objectContent in objectsContent)
        {
            activityObjectsPar.Add(new ActivityObject(objectContent.transform, addXRange));
        }
    }
}