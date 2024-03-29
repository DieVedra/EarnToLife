﻿using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class GroundAnalyzer
{
    private readonly LayerMask _ground;
    private readonly LayerMask _asphalt;
    private readonly Collider2D _frontWheelCollider;
    private readonly Collider2D _backWheelCollider;
    private readonly CompositeDisposable _frontWheelCompositeDisposable = new CompositeDisposable();
    private readonly CompositeDisposable _backWheelCompositeDisposable = new CompositeDisposable();
    public Vector2 FrontWheelPointContact { get; private set; }
    public Vector2 BackWheelPointContact { get; private set; }

    public ReactiveProperty<bool> FrontWheelOnGroundReactiveProperty = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelOnGroundReactiveProperty = new ReactiveProperty<bool>();
    
    public ReactiveProperty<bool> FrontWheelOnAsphaltReactiveProperty = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> BackWheelOnAsphaltReactiveProperty = new ReactiveProperty<bool>();
    public bool FrontWheelContact { get; private set; } = false;
    public bool BackWheelContact { get; private set; } = false;
    public GroundAnalyzer(CarWheel frontWheel, CarWheel backWheel, ReactiveCommand onCarBrokenIntoTwoParts, LayerMask ground, LayerMask asphalt)
    {
        _ground = ground;
        _asphalt = asphalt;
        _frontWheelCollider = frontWheel.Collider2D;
        _backWheelCollider = backWheel.Collider2D;
        onCarBrokenIntoTwoParts.Subscribe(_ => { CarBrokenIntoTwoParts();});
        SubscribeCollider(_frontWheelCollider, CheckCircleFrontWheel, _frontWheelCompositeDisposable);
        SubscribeCollider(_backWheelCollider, CheckCircleBackWheel, _backWheelCompositeDisposable);
    }
    public void Dispose()
    {
        _frontWheelCompositeDisposable.Clear();
        _backWheelCompositeDisposable.Clear();
    }
    private void CheckCircleBackWheel(Collision2D collision2D)
    {
        if (CheckGround(collision2D) == true)
        {
            BackWheelOnGroundReactiveProperty.Value = true;
            BackWheelOnAsphaltReactiveProperty.Value = false;
            SetContactPointBackWheel(collision2D);
        }
        else if(CheckAsphalt(collision2D) == true)
        {
            BackWheelOnGroundReactiveProperty.Value = false;
            BackWheelOnAsphaltReactiveProperty.Value = true;
            SetContactPointBackWheel(collision2D);
        }
        else
        {
            BackWheelOnGroundReactiveProperty.Value = false;
            BackWheelOnAsphaltReactiveProperty.Value = false;
            BackWheelContact = false;
        }
    }
    private void CheckCircleFrontWheel(Collision2D collision2D)
    {
        if (CheckGround(collision2D) == true)
        {
            FrontWheelOnGroundReactiveProperty.Value = true;
            FrontWheelOnAsphaltReactiveProperty.Value = false;
            SetContactPointFrontWheel(collision2D);
        }
        else if(CheckAsphalt(collision2D) == true)
        {
            FrontWheelOnGroundReactiveProperty.Value = false;
            FrontWheelOnAsphaltReactiveProperty.Value = true;
            SetContactPointFrontWheel(collision2D);
        }
        else
        {
            FrontWheelOnGroundReactiveProperty.Value = false;
            FrontWheelOnAsphaltReactiveProperty.Value = false;
            FrontWheelContact = false;
        }
        
        // Debug.Log($"FrontWheelOnAsphalt: {FrontWheelOnAsphaltReactiveProperty.Value}    " +
        //           $"FrontWheelOnGround: {FrontWheelOnGroundReactiveProperty.Value}  " +
        //           $"GroundContact: {FrontWheelContact}");
    }
    private bool CheckGround(Collision2D collision2D)
    {
        if ((1 << collision2D.gameObject.layer & _ground.value) == 1 << collision2D.gameObject.layer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckAsphalt(Collision2D collision2D)
    {
        if((1 << collision2D.gameObject.layer & _asphalt.value) == 1 << collision2D.gameObject.layer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SubscribeCollider(Collider2D collider2D, Action<Collision2D> operation, CompositeDisposable compositeDisposable)
    {
        collider2D.OnCollisionStay2DAsObservable().Do(operation.Invoke).Subscribe().AddTo(compositeDisposable);
        collider2D.OnCollisionExit2DAsObservable().Do(operation.Invoke).Subscribe().AddTo(compositeDisposable);
    }
    private void SetContactPointFrontWheel(Collision2D collision2D)
    {
        FrontWheelContact = true;
        FrontWheelPointContact = GetContactPoint(collision2D);
    }
    private void SetContactPointBackWheel(Collision2D collision2D)
    {
        BackWheelContact = true;
        BackWheelPointContact = GetContactPoint(collision2D);
    }
    private Vector2 GetContactPoint(Collision2D collision2D)
    {
        if (collision2D.contacts.Length > 0)
        {
            return collision2D.contacts[0].point;
        }
        else
        {
            return Vector2.zero;
        }
    }
    private void CarBrokenIntoTwoParts()
    {
        _backWheelCompositeDisposable.Clear();
    }
}