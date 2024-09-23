using System;
using System.Collections.Generic;
using UnityEngine;

public class BackWingDestructionHandler : DestructionHandler, IDispose
{
    private readonly GlassDestructionHandler _glassDestructionHandler;
    private readonly ArmoredBackFrameDestructionHandler _armoredBackFrameHandler;
    private readonly ArmoredBackFrameRef _armoredBackFrameRef;
    private readonly BumperDestructionHandler _backBumperDestructionHandler;
    private readonly ExhaustHandler _exhaustHandler;
    private readonly DestructionEffectsHandler _destructionEffectsHandler;
    private readonly Transform _wingNormal;
    private readonly Transform _wingDamaged1;
    private readonly Transform _wingDamaged2;
    private readonly Transform _trunkCoverWingDamaged2;
    
    private readonly Transform _armoredBackNormal;
    private readonly Transform _armoredBackDamaged;

    
    private Collider2D _wingDamaged1Collider;
    private Collider2D _wingDamaged2Collider;
    private List<Collider2D> _contentColliders;
    private IReadOnlyList<Transform> _wingContent;
    private IReadOnlyList<Transform> _trunkCovers;
    private bool _boosterActive;
    private bool _isArmored = false;
    public BackWingDestructionHandler(BackWingRef backWingRef, GlassDestructionHandler glassDestructionHandler,
        ArmoredBackFrameDestructionHandler armoredBackFrameHandler, BumperDestructionHandler backBumperDestructionHandler, 
        ExhaustHandler exhaustHandler, DestructionEffectsHandler destructionEffectsHandler, DestructionAudioHandler destructionAudioHandler,
        DestructionHandlerContent destructionHandlerContent, int totalStrength, bool isArmored, bool boosterActive)
        :base(backWingRef, destructionHandlerContent, destructionAudioHandler, totalStrength)
    {
        _glassDestructionHandler = glassDestructionHandler;
        _armoredBackFrameHandler = armoredBackFrameHandler;
        _backBumperDestructionHandler = backBumperDestructionHandler;
        _exhaustHandler = exhaustHandler;
        _destructionEffectsHandler = destructionEffectsHandler;
        _wingNormal = backWingRef.WingNormal;
        _wingDamaged1 = backWingRef.WingDamaged1;
        _wingDamaged2 = backWingRef.WingDamaged2;
        _trunkCoverWingDamaged2 = backWingRef.TrunkCover2;
        _wingDamaged1Collider = _wingDamaged1.GetComponent<Collider2D>();
        _wingDamaged2Collider = _wingDamaged2.GetComponent<Collider2D>();
        _wingContent = backWingRef.WingContent;
        _trunkCovers = backWingRef.TrunkCovers;
        _boosterActive = boosterActive;
        TryOffTrunkCovers();
        InitContent();
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _armoredBackFrameRef = armoredBackFrameHandler.ArmoredBackFrameRef;
            _armoredBackNormal = _armoredBackFrameRef.ArmoredBackNormal;
            _armoredBackDamaged = _armoredBackFrameRef.ArmoredBackDamagedRoofDamaged;
            SubscribeCollider(_armoredBackNormal.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
        }
        SubscribeCollider(_wingNormal.GetComponent<Collider2D>(), CollisionHandling, TrySwitchMode);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
    protected override void TrySwitchMode()
    {
        if (ImpulseNormalValue > MaxStrength)
        {
            PlayEffect();
            DestructionMode3();
        }
        else if (ImpulseNormalValue > HalfStrength)
        {
            PlayEffect();
            RecalculateStrength();
            DestructionMode2AndSubscribe();
        }
        else if (ImpulseNormalValue > MinStrength)
        {
            PlayEffect();
            RecalculateStrength();
            DestructionMode1AndSubscribe();
        }
        else
        {
            PlaySoftHitSound();
        }
    }

    private void PlayEffect()
    {
        _destructionEffectsHandler.HitBrokenEffect(HitPosition, ImpulseNormalValue);
    }

    private void DestructionMode1AndSubscribe()
    {
        DestructionMode1();
        if (_isArmored == true)
        {
            SubscribeCollider(_armoredBackFrameHandler.CurrentCollider, CollisionHandling, TrySwitchMode);
        }
        SubscribeCollider(_wingDamaged1Collider, CollisionHandling, TrySwitchMode);
    }
    private void DestructionMode1()
    {
        CompositeDisposable.Clear();
        _backBumperDestructionHandler.TryThrow().Forget();
        ThrowContent();
        SwitchSprites1();
        _glassDestructionHandler?.TryBreakGlassFromWings();
        if (_isArmored == true)
        {
            _armoredBackFrameHandler.TryTakeDamageFromBack();
        }
        DestructionMode = DestructionMode.Mode1;
    }

    private void DestructionMode2AndSubscribe()
    {
        DestructionMode2();
        if (_isArmored == true)
        {
            SubscribeCollider(_armoredBackFrameHandler.CurrentCollider, CollisionHandling, TrySwitchMode);
        }
        SubscribeCollider(_wingDamaged2Collider, CollisionHandling, TrySwitchMode);
    }
    private void DestructionMode2()
    {
        CompositeDisposable.Clear();
        if (DestructionMode == DestructionMode.ModeDefault)
        {
            DestructionMode1();
        }
        SwitchSprites2();
        _glassDestructionHandler?.TryThrowGlass();
        if (_boosterActive == false)
        {
            _trunkCoverWingDamaged2.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_trunkCoverWingDamaged2);
            SetCarDebrisLayerInteractableWithCar(_trunkCoverWingDamaged2);
        }
        if (_isArmored == true)
        {
            _armoredBackFrameHandler.TryTakeDamageFromBack();
            if (_armoredBackFrameHandler.TryThrow() == true)
            {
                SetParentDebris(_armoredBackFrameRef.transform);
                SetCarDebrisLayerNonInteractableWithCar(_armoredBackFrameRef.transform);
            }
        }
        DestructionMode = DestructionMode.Mode2;
    }
    private void DestructionMode3()
    {
        CompositeDisposable.Clear();
        if (DestructionMode != DestructionMode.Mode2)
        {
            DestructionMode2();
        }
        Debug.Log("LostFuelTank");
        //???? LostFuelTank
    }
    private void SwitchSprites1()
    {
        _wingNormal.gameObject.SetActive(false);
        _wingDamaged1.gameObject.SetActive(true);
    }
    private void SwitchSprites2()
    {
        _wingDamaged1.gameObject.SetActive(false);
        _wingDamaged2.gameObject.SetActive(true);
    }
    private void ThrowContent()
    {
        for (int i = 0; i < _wingContent.Count; i++)
        {
            _contentColliders[i].enabled = true;
            TryAddRigidBody(_wingContent[i].gameObject);
            SetParentDebris(_wingContent[i]);
            SetCarDebrisLayerNonInteractableWithCar(_wingContent[i]);
        }
        _exhaustHandler.SetPoint1();
    }
    private void TryOffTrunkCovers()
    {
        if (_boosterActive == true)
        {
            for (int i = 0; i < _trunkCovers.Count; i++)
            {
                _trunkCovers[i].gameObject.SetActive(false);
            }
        }
    }

    private void InitContent()
    {
        _contentColliders = new List<Collider2D>(_trunkCovers.Count);
        for (int i = 0; i < _wingContent.Count; i++)
        {
            _contentColliders.Add(_wingContent[i].GetComponent<Collider2D>());
        }
    }
}