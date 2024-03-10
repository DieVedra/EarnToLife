using System.Collections.Generic;
using UnityEngine;

public class BackWingDestructionHandler : DestructionHandler
{
    private readonly float _strength1AfterHit;
    private readonly float _strength2AfterHit;
    private GlassDestructionHandler _glassDestructionHandler;
    private ArmoredBackFrameDestructionHandler _armoredBackFrameHandler;
    private readonly ArmoredBackFrameRef _armoredBackFrameRef;
    private Transform _wingNormal;
    private Transform _wingDamaged1;
    private Transform _wingDamaged2;
    private Transform _TrunkCoverWingDamaged2;
    
    private Transform _armoredBackNormal;
    private Transform _armoredBackDamaged;

    
    private Collider2D _wingDamaged1Collider;
    private Collider2D _wingDamaged2Collider;
    private IReadOnlyList<Transform> _wingContent;
    private bool _boosterActive;
    private bool _isArmored = false;
    public BackWingDestructionHandler(BackWingRef backWingRef, GlassDestructionHandler glassDestructionHandler, ArmoredBackFrameDestructionHandler armoredBackFrameHandler,
        DestructionHandlerContent destructionHandlerContent, int totalStrength, bool isArmored, bool boosterActive)
        :base(backWingRef, destructionHandlerContent, totalStrength)
    {
        _glassDestructionHandler = glassDestructionHandler;
        _armoredBackFrameHandler = armoredBackFrameHandler;
        _strength1AfterHit = backWingRef.StrengthWing * HalfStrengthMultiplier;
        _strength2AfterHit = backWingRef.StrengthWing * MinStrengthMultiplier;
        _wingNormal = backWingRef.WingNormal;
        _wingDamaged1 = backWingRef.WingDamaged1;
        _wingDamaged2 = backWingRef.WingDamaged2;
        _wingDamaged1Collider = _wingDamaged1.GetComponent<Collider2D>();
        _wingDamaged2Collider = _wingDamaged2.GetComponent<Collider2D>();
        _wingContent = backWingRef.WingContent;
        _boosterActive = boosterActive;
        TryOffTrunkCover(_wingNormal.GetComponentInChildren<Transform>());
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _armoredBackFrameRef = armoredBackFrameHandler.ArmoredBackFrameRef;
            _armoredBackNormal = _armoredBackFrameRef.ArmoredBackNormal;
            _armoredBackDamaged = _armoredBackFrameRef.ArmoredBackDamagedRoofDamaged;
            SubscribeCollider(_armoredBackNormal.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
        }
        SubscribeCollider(_wingNormal.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
    protected override void TrySwitchMode()
    {
        ApplyDamage();
        if (MaxStrength <= StrengthForDestruct)
        {
            DestructionMode3();
        }
        else if (MaxStrength <= _strength2AfterHit)
        {
            DestructionMode2();
        }
        else if (MaxStrength <= _strength1AfterHit)
        {
            DestructionMode1();
        }
    }
    private void DestructionMode1()
    {
        CompositeDisposable.Clear();
        ThrowContent();
        SwitchSprites1();
        TryOffTrunkCover(_wingDamaged1.GetComponentInChildren<Transform>());
        _glassDestructionHandler?.TryBreakGlass();
        if (_isArmored == true)
        {
            _armoredBackFrameHandler.TryTakeDamageFromBack();
            SubscribeCollider(_armoredBackFrameHandler.CurrentCollider, CheckCollision, TrySwitchMode);
        }
        SubscribeCollider(_wingDamaged1Collider, CheckCollision, TrySwitchMode);
    }
    private void DestructionMode2()
    {
        CompositeDisposable.Clear();
        SwitchSprites2();
        _glassDestructionHandler?.TryThrowGlass();
        _TrunkCoverWingDamaged2 = _wingDamaged2.GetComponentInChildren<Transform>();
        if (_boosterActive == true)
        {
            TryOffTrunkCover(_TrunkCoverWingDamaged2);
        }
        else
        {
            _TrunkCoverWingDamaged2.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_TrunkCoverWingDamaged2);
            SetCarDebrisLayer(_TrunkCoverWingDamaged2);
        }
        if (_isArmored == true)
        {
            _armoredBackFrameHandler.TryTakeDamageFromBack();
            if (_armoredBackFrameHandler.TryThrow() == true)
            {
                SetParentDebris(_armoredBackFrameRef.transform);
            }
        }
    }
    private void DestructionMode3()
    {
        DestructionMode2();
        //???? LostFuelTank
    }
    private void SwitchSprites1()
    {
        _wingNormal.gameObject.SetActive(false);
        _wingDamaged1.gameObject.SetActive(true);
    }
    private void SwitchSprites2()
    {
        _wingNormal.gameObject.SetActive(false);
        _wingDamaged1.gameObject.SetActive(false);
        _wingDamaged2.gameObject.SetActive(true);
    }
    private void ThrowContent()
    {
        for (int i = 0; i < _wingContent.Count; i++)
        {
            _wingContent[i].gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_wingContent[i]);
            SetCarDebrisLayer(_wingContent[i]);
        }
    }
    private void TryOffTrunkCover(Transform transform)
    {
        if (_boosterActive == true)
        {
            transform.gameObject.SetActive(false);
        }
    }
}