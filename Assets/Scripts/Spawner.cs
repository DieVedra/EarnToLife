using UnityEngine;

public class Spawner
{
    public T Spawn<T>(T prefab, Transform transformPar, Transform parent = null) where T : Object
    {
        return Object.Instantiate(prefab, transformPar.position, transformPar.rotation, parent);
    }
    public T Spawn<T>(T prefab, Vector3 position, Transform parent = null) where T : Object
    {
        return Object.Instantiate(prefab, position, Quaternion.identity, parent);
    }
    public T Spawn<T>(T prefab) where T : Object
    {
        return Object.Instantiate(prefab);
    }
    public void KillObject(GameObject gameObject)
    {
        if (Application.isEditor)
        {
            Object.DestroyImmediate(gameObject);
        }
        else if (Application.isPlaying)
        {
            Object.Destroy(gameObject);
        }
    }
}