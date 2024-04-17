using UnityEngine;

public class Spawner
{
    public T Spawn<T>(T prefab, Transform transformPar, Transform parent = null) where T : Object
    {
        return Object.Instantiate(prefab, transformPar.position, transformPar.rotation, parent);
    }
    // public T Spawn<T>(T prefab) where T : MonoBehaviour
    // {
    //     return Object.Instantiate(prefab);
    // }
    public T Spawn<T>(T prefab) where T : Object
    {
        return Object.Instantiate(prefab);
    }
    public void KillObject(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }
}