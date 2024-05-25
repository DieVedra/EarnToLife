using UnityEngine;

public class PoolMetods
{
    protected void GetAction(DebrisEffect effect)
    {
        effect.gameObject.SetActive(true);
    }
    protected  void GetAction(ParticleSystem effect)
    {
        effect.gameObject.SetActive(true);
    }
    protected  void ReturnAction(DebrisEffect effect)
    {
        // effect.transform.SetParent(_barrelPoolEffectsParent);
        effect.gameObject.SetActive(false);
    }
    protected  void ReturnAction(ParticleSystem effect)
    {
        effect.gameObject.SetActive(false);
    }
}