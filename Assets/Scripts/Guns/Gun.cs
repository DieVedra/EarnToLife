using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _handlePoint;
    [SerializeField] private Transform _forearmPoint;
    [SerializeField] private Transform _buttPoint;
    [SerializeField] private Transform _firePoint;
    [Space]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _rateFireCount = 1f;
    [Space]
    [SerializeField] private AudioClip _shootAudioClip;
    [SerializeField] private Sprite[] _shootSprites;
    [SerializeField] private ParticleSystem _particleSystem;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private BulletReservoir _bulletReservoir;
    private PersecuteHands _persecuteHands;
    private RateFire _rateFire;
    private BotSound _botSound;
    private FireAnimation _fireAnimation;
    private bool _shooterIsFlip;

    public void Init(Transform shoulderSupport, Transform leftHand, Transform rightHand, BulletReservoir bulletReservoir, BotSound botSound)
    {
        _persecuteHands = new PersecuteHands(shoulderSupport, leftHand, rightHand, _handlePoint, _forearmPoint, _buttPoint);
        _bulletReservoir = bulletReservoir;
        _bulletReservoir.FillReservoirBulletsAK(_bulletPrefab);
        _rateFire = new RateFire(_rateFireCount);
        // _rateFire.DoFire += Shoot;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = true;
        _collider = GetComponentInChildren<Collider2D>();
        _collider.enabled = false;
        _botSound = botSound;
        _fireAnimation = new FireAnimation(_shootSprites, _particleSystem);
    }
    public void DropWeapon()
    {
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
    }
    public void UpdateGun(bool shooterIsLive, bool targetDetected, bool shooterIsFlip)
    {
        _shooterIsFlip = shooterIsFlip;
        if (shooterIsLive == true)
        {
            _persecuteHands.Update();
        }
        // _rateFire.Update(targetDetected);
    }
    private void Shoot()
    {
        _bulletReservoir.LaunchFromQueue(_shooterIsFlip, _bulletPrefab, _firePoint);
        _fireAnimation.FireAnimationPlay();
        _botSound.PlaySomething(_shootAudioClip);
    }
    private void OnDestroy()
    {
        // _rateFire.DoFire -= Shoot;
    }
}
