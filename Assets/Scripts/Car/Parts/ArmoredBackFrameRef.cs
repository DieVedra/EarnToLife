using NaughtyAttributes;
using UnityEngine;

public class ArmoredBackFrameRef : MonoBehaviour
{
    [SerializeField, BoxGroup("ArmoredBack")] private Transform _armoredBackNormal;
    [SerializeField, BoxGroup("ArmoredBack")] private Transform _armoredBackDamagedRoofDamaged;
    [SerializeField, BoxGroup("ArmoredBack")] private Transform _armoredBackNormalRoofDamaged;
    [SerializeField, BoxGroup("ArmoredBack")] private Transform _armoredBackDamagedRoofNotDamaged;
 
    [SerializeField, BoxGroup("Settings"), Range(1,50)] private int _strengthArmoredBack;

    public Transform ArmoredBackNormal => _armoredBackNormal;
    public Transform ArmoredBackDamagedRoofDamaged => _armoredBackDamagedRoofDamaged;
    public Transform ArmoredBackNormalRoofDamaged => _armoredBackNormalRoofDamaged;
    public Transform ArmoredBackDamagedRoofNotDamaged => _armoredBackDamagedRoofNotDamaged;
    public int StrengthArmoredBack => _strengthArmoredBack;
}