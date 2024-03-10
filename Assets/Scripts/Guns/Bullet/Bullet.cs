using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)]private  float _lenghtRay = 0.05f;
    [SerializeField] private ParticleSystem _particleSystem;
    private readonly Vector3 _rotateMultiplier = new Vector3(0f, 0f, 180f);
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _lifetime = 1f;
    private float _currentLifetime;
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private bool _isActive;
    private bool _directionIsLeft;
    private RaycastHit2D _hit;
    public event Action<Bullet> OnHit;
    public void Init()
    {
        gameObject.SetActive(false);
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Activate(bool shooterIsFlip, Transform firePoint)
    {
        _directionIsLeft = shooterIsFlip;
        SetStartPosition(firePoint);
        gameObject.SetActive(true);
        _currentLifetime = _lifetime;
        //_particleSystem.Play();
        _isActive = true;
    }
    public void UpdateBullet()
    {
        if (_isActive == true)
        {
            CheckLifetime();
            CheckHit();
            Move();
        }
    }
    private void Move()
    {
        _rigidbody.MovePosition(_transform.position + -_transform.right * Time.deltaTime * _speed);
    }
    private void CheckHit()
    {
        _hit = Physics2D.Raycast(_transform.position, -_transform.right, _lenghtRay);
        Debug.DrawRay(_transform.position, -_transform.right * _lenghtRay, Color.white);

        if (_hit.collider != null)
        {
            if (_hit.collider.TryGetComponent(out IDamageble damageble))
            {
                damageble.Damage();
                OnHit?.Invoke(this);
                Diactivate();
            }
        }
    }
    private void CheckLifetime()
    {
        if (_currentLifetime <= 0f)
        {
            Diactivate();
            return;
        }
        _currentLifetime -= Time.deltaTime;
    }
    private void SetStartPosition(Transform startPosition)
    {
        _transform.position = startPosition.position;

        if (_directionIsLeft == true)
        {
            _transform.rotation = Quaternion.Euler(startPosition.rotation.eulerAngles + _rotateMultiplier);
        }
        else
        {
            _transform.rotation = startPosition.rotation;
        }
    }
    private void Diactivate()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }
}
