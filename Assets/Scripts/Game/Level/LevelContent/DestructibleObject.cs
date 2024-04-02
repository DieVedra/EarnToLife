using UnityEngine;
using Zenject;

public class DestructibleObject : MonoBehaviour, IKnockable
{
    [SerializeField] private Transform _debris;
    [SerializeField] private Transform _wholeObjectTransform;
    [SerializeField] private float _hardness;
    
    [Inject(Id = "DebrisParent")] private Transform _debrisParentForDestroy;
    public void Destruct(float bodyImpulse)
    {
        if (bodyImpulse > _hardness)
        {
            _wholeObjectTransform.gameObject.SetActive(false);
            AddRigidBodiesToDebris();
        }
    }
    private void AddRigidBodiesToDebris()
    {
        EnumerateChilds(_debris);
    }

    private void EnumerateChilds(Transform debris)
    {
        for (int i = 0; i < debris.childCount; i++)
        {
            if (debris.GetChild(i).childCount > 0)
            {
                EnumerateChilds(debris.GetChild(i));
            }
            AddRigidBodyTo(debris.GetChild(i));
            SetDebrisParent(debris.GetChild(i));
        }
    }
    private void AddRigidBodyTo(Transform chip)
    {
        chip.gameObject.AddComponent<Rigidbody2D>();
    }

    private void SetDebrisParent(Transform chip)
    {
        chip.SetParent(_debrisParentForDestroy);
    }
}