using UnityEngine;

public class FrontWingDestructionHandler : DestructionHandler
{
    private readonly float _strength1AfterHit;
    private readonly float _strength2AfterHit;
    private readonly ArmoredFrontFrameRef _armoredFrontFrameRef;
    private readonly GlassDestructionHandler _glassDestructionHandler;
    private readonly HotWheelDestructionHandler _hotWheelDestructionHandler;
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
    // private readonly Collider2D _armoredFrontNormalCollider;
    private readonly Collider2D _armoredFrontDamagedCollider;
    private readonly bool _isArmored;
    private Transform _damaged2Hood;

    public FrontWingDestructionHandler(FrontWingRef frontWingRef, ArmoredFrontFrameRef armoredFrontFrameRef, 
        GlassDestructionHandler glassDestructionHandler, HotWheelDestructionHandler hotWheelDestructionHandler,
        DestructionHandlerContent destructionHandlerContent, int strength, bool isArmored)
        :base(frontWingRef, destructionHandlerContent, strength)
    {
        // if (_wingNormal.gameObject.activeSelf == true)
        // {
        //     Debug.Log("                      08");
        //
        // }
        _glassDestructionHandler = glassDestructionHandler;
        _hotWheelDestructionHandler = hotWheelDestructionHandler;
        _strength1AfterHit = strength * HalfStrengthMultiplier;
        _strength2AfterHit = strength * MinStrengthMultiplier;
        _wingNormal = frontWingRef.WingNormal;

        _lighterWingNormal = frontWingRef.LighterWingNormal;
        _lighterWingNormal.gameObject.SetActive(false);
        _damaged2Hood = frontWingRef.WingFrontDamaged2TrunkCover;
        _wingDamaged1 = frontWingRef.WingDamaged1;
        _wingDamaged2 = frontWingRef.WingDamaged2;
        _bottomNormal = frontWingRef.BottomNormal;
        _bottomDamaged = frontWingRef.BottomDamaged;
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
            SubscribeCollider(_armoredFrontNormal.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
        }
        SubscribeCollider(_wingNormal.GetComponent<Collider2D>(), CheckCollision, TryDestruct);
    }
    protected override void TryDestruct()
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
        ThrowLighter();
        SwitchSprites1();
        _glassDestructionHandler.TryBreakGlass();
        if (_isArmored == true)
        {
            SubscribeCollider(_armoredFrontDamagedCollider, CheckCollision, TryDestruct);
        }
        SubscribeCollider(_wingDamaged1Collider, CheckCollision, TryDestruct);
        Debug.Log(32323332);

    }
    private void DestructionMode2()
    {
        CompositeDisposable.Clear();
        SwitchSprites2();
        if (_isArmored == true)
        {
            _armoredFrontDamaged.gameObject.AddComponent<Rigidbody2D>();
            SetParentDebris(_armoredFrontFrameRef.transform);
        }

        if (_hotWheelDestructionHandler != null)
        {
            _hotWheelDestructionHandler.Destruct();
            SetParentDebris(_hotWheelDestructionHandler.HotWheelRef.transform);
        }
        _glassDestructionHandler?.TryThrowGlass();
        ThrowHood();
        SubscribeCollider(_wingDamaged2Collider, CheckCollision, TryDestruct);
    }
    private void DestructionMode3()
    {
        DestructionMode2();
        //engine broken
    }
    private void SwitchSprites1()
    {
        Debug.Log(111111);
        _wingNormal.gameObject.SetActive(false);
        _wingDamaged1.gameObject.SetActive(true);
        TrySwitchSpriteArmoredFrame();
    }
    private void SwitchSprites2()
    {
        Debug.Log(5454544444444);

        _wingNormal.gameObject.SetActive(false);
        _wingDamaged1.gameObject.SetActive(false);
        _wingDamaged2.gameObject.SetActive(true);
        TrySwitchSpriteArmoredFrame();
        SwitchBottom();
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
        _lighterWingNormal.gameObject.SetActive(true);
        _lighterWingNormal.gameObject.AddComponent<Rigidbody2D>();
        SetParentDebris(_lighterWingNormal);
    }

    private void ThrowHood()
    {
        _damaged2Hood.gameObject.AddComponent<Rigidbody2D>();
        SetParentDebris(_damaged2Hood);
        SetCarDebrisLayer(_damaged2Hood);
    }
    private void SwitchBottom()
    {
        _bottomNormal.gameObject.SetActive(false);
        _bottomDamaged.gameObject.SetActive(true);
    }
}