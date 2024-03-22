using UnityEngine;

public class FrontWingDestructionHandler : DestructionHandler, IDispose
{
    private readonly ArmoredFrontFrameRef _armoredFrontFrameRef;
    private readonly GlassDestructionHandler _glassDestructionHandler;
    private readonly HotWheelDestructionHandler _hotWheelDestructionHandler;
    private readonly BumperDestructionHandler _bumperDestructionFrontHandler;
    private readonly ParticleSystem _fireParticleSystem;
    private readonly ParticleSystem _smokeParticleSystem;
    private readonly Transform _wingNormal;
    private readonly Transform _lighterWingNormal;
    private readonly Transform _wingDamaged1;
    private readonly Transform _wingDamaged2;
    private readonly Transform _bottomNormal;
    private readonly Transform _bottomDamaged;
    private readonly Transform _armoredFrontNormal;
    private readonly Transform _armoredFrontDamaged;
    private readonly Collider2D _wingDamaged1Collider;
    private readonly Collider2D _wingDamaged2Collider;
    private readonly Collider2D _armoredFrontDamagedCollider;
    private readonly bool _isArmored;
    private Transform _damaged2Hood;
    private DestructionMode _destructionMode = DestructionMode.ModeDefault;
    private bool _lighterBroken;

    public FrontWingDestructionHandler(FrontWingRef frontWingRef, ArmoredFrontFrameRef armoredFrontFrameRef, 
        GlassDestructionHandler glassDestructionHandler, HotWheelDestructionHandler hotWheelDestructionHandler,
        BumperDestructionHandler bumperDestructionFrontHandler, DestructionHandlerContent destructionHandlerContent,
        int strength, bool isArmored)
        :base(frontWingRef, destructionHandlerContent, strength)
    {
        _glassDestructionHandler = glassDestructionHandler;
        _hotWheelDestructionHandler = hotWheelDestructionHandler;
        _bumperDestructionFrontHandler = bumperDestructionFrontHandler;
        _wingNormal = frontWingRef.WingNormal;
        _lighterWingNormal = frontWingRef.LighterWingNormal;
        _lighterWingNormal.gameObject.SetActive(false);
        _damaged2Hood = frontWingRef.WingFrontDamaged2TrunkCover;
        _wingDamaged1 = frontWingRef.WingDamaged1;
        _wingDamaged2 = frontWingRef.WingDamaged2;
        _bottomNormal = frontWingRef.BottomNormal;
        _bottomDamaged = frontWingRef.BottomDamaged;
        _fireParticleSystem = frontWingRef.FireParticleSystem;
        _smokeParticleSystem = frontWingRef.SmokeParticleSystem;
        _wingDamaged1Collider = _wingDamaged1.GetComponent<Collider2D>();
        _wingDamaged2Collider = _wingDamaged2.GetComponent<Collider2D>();
        _bottomDamaged.gameObject.SetActive(false);
        _isArmored = isArmored;
        if (_isArmored == true)
        {
            _armoredFrontFrameRef = armoredFrontFrameRef;
            _armoredFrontNormal = armoredFrontFrameRef.ArmoredFrontNormal;
            _armoredFrontDamaged = armoredFrontFrameRef.ArmoredFrontDamaged;
            _armoredFrontDamagedCollider = _armoredFrontDamaged.GetComponent<Collider2D>();
            SubscribeCollider(_armoredFrontNormal.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
        }
        SubscribeCollider(_wingNormal.GetComponent<Collider2D>(), CheckCollision, TrySwitchMode);
    }

    public void Dispose()
    {
        CompositeDisposable.Clear();
    }
    protected override void TrySwitchMode()
    {
        if (ValueNormalImpulse > MaxStrength)
        {
            DestructionMode3AndSubscribe();
        }
        else if (ValueNormalImpulse > HalfStrength)
        {
            RecalculateStrength();
            DestructionMode2AndSubscribe();
        }
        else if (ValueNormalImpulse > MinStrength)
        {
            RecalculateStrength();
            if (_destructionMode == DestructionMode.Mode2)
            {
                DestructionMode2AndSubscribe();
            }
            else
            {
                DestructionMode1AndSubscribe();
            }
        }
    }

    private void DestructionMode1AndSubscribe()
    {
        DestructionMode1();
        if (_isArmored == true)
        {
            SubscribeCollider(_armoredFrontDamagedCollider, CheckCollision, TrySwitchMode);
        }
        SubscribeCollider(_wingDamaged1Collider, CheckCollision, TrySwitchMode);
    }
    private void DestructionMode1()
    {
        CompositeDisposable.Clear();
        _bumperDestructionFrontHandler.TryThrow().Forget();
        ThrowLighter();
        SwitchSprites1();
        TrySwitchSpriteArmoredFrame();
        _glassDestructionHandler?.TryBreakGlass();
        _destructionMode = DestructionMode.Mode1;
    }

    private void DestructionMode2AndSubscribe()
    {
        DestructionMode2();
        SubscribeCollider(_wingDamaged2Collider, CheckCollision, TrySwitchMode);
    }
    private void DestructionMode2()
    {
        if (_destructionMode == DestructionMode.ModeDefault)
        {
            DestructionMode1();
        }
        CompositeDisposable.Clear();
        SwitchSprites2();
        SwitchBottom();
        TrySwitchSpriteArmoredFrame();
        if (_isArmored == true)
        {
            TryAddRigidBody(_armoredFrontDamaged.gameObject);
            SetParentDebris(_armoredFrontFrameRef.transform);
        }
        if (_hotWheelDestructionHandler != null)
        {
            _hotWheelDestructionHandler.Destruct();
            SetParentDebris(_hotWheelDestructionHandler.HotWheelRef.transform);
        }
        _glassDestructionHandler?.TryThrowGlass();
        ThrowHood();
        StartSmoke();
        _destructionMode = DestructionMode.Mode2;
    }
    private void DestructionMode3AndSubscribe()
    {
        if (_destructionMode != DestructionMode.Mode2)
        {
            DestructionMode2();
        }
        _destructionMode = DestructionMode.Mode3;
        StartFire();
        Debug.Log("       engine broken");
        //engine broken
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

    private void TrySwitchSpriteArmoredFrame()
    {
        if (_isArmored == true)
        {
            _armoredFrontNormal.gameObject.SetActive(false);
            _armoredFrontDamaged.gameObject.SetActive(true);
        }
    }
    private void ThrowLighter()
    {
        if (_lighterBroken == false)
        {
            _lighterBroken = true;
            _lighterWingNormal.gameObject.SetActive(true);
            TryAddRigidBody(_lighterWingNormal.gameObject);
            SetParentDebris(_lighterWingNormal);
        }
    }

    private void ThrowHood()
    {
        TryAddRigidBody(_damaged2Hood.gameObject);
        SetParentDebris(_damaged2Hood);
        SetCarDebrisLayer(_damaged2Hood);
    }
    private void SwitchBottom()
    {
        _bottomNormal.gameObject.SetActive(false);
        _bottomDamaged.gameObject.SetActive(true);
    }

    private void StartSmoke()
    {
        _smokeParticleSystem.gameObject.SetActive(true);
        _smokeParticleSystem.Play();
    }
    private void StopSmoke()
    {
        _smokeParticleSystem.gameObject.SetActive(false);
        _smokeParticleSystem.Stop();
    }
    private void StartFire()
    {
        if (_smokeParticleSystem.gameObject.activeSelf == true)
        {
            StopSmoke();
        }
        _fireParticleSystem.gameObject.SetActive(true);
        _fireParticleSystem.Play();
    }
}