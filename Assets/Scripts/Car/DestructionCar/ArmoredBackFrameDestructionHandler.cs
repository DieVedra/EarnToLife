using System;
using UnityEngine;

public class ArmoredBackFrameDestructionHandler : DestructionHandler
{
    public readonly ArmoredBackFrameRef ArmoredBackFrameRef;
    private readonly Transform _armoredBackDamagedRoofDamaged;
    private readonly Transform _armoredBackNormalRoofDamaged;
    private readonly Transform _armoredBackDamagedRoofNotDamaged;
    private readonly Collider2D _colliderBackDamagedRoofDamaged;
    private readonly Collider2D _colliderBackNormalRoofDamaged;
    private readonly Collider2D _colliderBackDamagedRoofNotDamaged;
    private Transform _currentActive;
    private bool _backIsDamaged = false;
    private bool _roofIsDamaged = false;
    private bool _isBroken = false;
    public Collider2D CurrentCollider { get; private set; }
    public ArmoredBackFrameDestructionHandler(ArmoredBackFrameRef armoredBackFrameRef, DestructionHandlerContent destructionHandlerContent, Action<float> soundSoftHit)
        : base(armoredBackFrameRef, destructionHandlerContent, soundSoftHit)
    {
        ArmoredBackFrameRef = armoredBackFrameRef;
        _armoredBackDamagedRoofDamaged = armoredBackFrameRef.ArmoredBackDamagedRoofDamaged;
        _armoredBackNormalRoofDamaged = armoredBackFrameRef.ArmoredBackNormalRoofDamaged;
        _armoredBackDamagedRoofNotDamaged = armoredBackFrameRef.ArmoredBackDamagedRoofNotDamaged;
        _colliderBackDamagedRoofDamaged = _armoredBackDamagedRoofDamaged.GetComponent<Collider2D>();
        _colliderBackNormalRoofDamaged = _armoredBackNormalRoofDamaged.GetComponent<Collider2D>();
        _colliderBackDamagedRoofNotDamaged = _armoredBackDamagedRoofNotDamaged.GetComponent<Collider2D>();
        SetActivePart(armoredBackFrameRef.ArmoredBackNormal, armoredBackFrameRef.ArmoredBackNormal.GetComponent<Collider2D>());
    }
    public void TryTakeDamageFromBack()
    {
        if (_backIsDamaged == false)
        {
            _currentActive.gameObject.SetActive(false);
            _backIsDamaged = true;
            if (_roofIsDamaged == true)
            {
                SetActivePart(_armoredBackDamagedRoofDamaged, _colliderBackDamagedRoofDamaged);
            }
            else
            {
                SetActivePart(_armoredBackDamagedRoofNotDamaged, _colliderBackDamagedRoofNotDamaged);
            }
        }
    }
    public void TryTakeDamageFromRoof()
    {
        if (_roofIsDamaged == false)
        {
            _currentActive.gameObject.SetActive(false);
            _roofIsDamaged = true;
            if (_backIsDamaged == true)
            {
                SetActivePart(_armoredBackDamagedRoofDamaged, _colliderBackDamagedRoofDamaged);
            }
            else
            {
                SetActivePart(_armoredBackNormalRoofDamaged, _colliderBackNormalRoofDamaged);
            }
        }
    }

    public bool TryThrow()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            _currentActive.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris();
            SetCarDebrisLayerNonInteractableWithCar();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetActivePart(Transform part, Collider2D collider)
    {
        part.gameObject.SetActive(true);
        _currentActive = part;
        CurrentCollider = collider;
    }
}