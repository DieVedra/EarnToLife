using UnityEngine;

public class Vision
{
    private Transform _visionPointTransform;
    private Transform _targetTransform;
    private Transform _targetCalm;
    private float _rangeVision;
    private float _radiusDetection;
    private LayerMask _layerMask;
    private Transform _targetSelected;
    private BotSound _botSound;
    private bool _targetDetected;
    public Vector2 Target => _targetSelected.position;
    public bool TargetDetected => _targetDetected;
    private Collider2D _hitCollider;

    public float RangeVision { set { _rangeVision = value; } } 

    public Vision(LayerMask layerMask, Transform viewPoint, Transform target, Transform targetCalm, float distanceDetection, BotSound botSound)
    {
        _targetTransform  = target;
        _targetCalm = targetCalm;
        _layerMask = layerMask;
        _visionPointTransform = viewPoint;
        _radiusDetection = distanceDetection;
        _targetDetected = false;
        _botSound = botSound;
    }
    public void Update()
    {
        Detector();
    }
    private void Detector()
    {
        if (CheckDistance())
        {
            if (_targetDetected == false)
            {
                CastSphere();
            }
        }
        else
        {
            if (_targetDetected == true)
            {
                _botSound.PlayBue();
            }
            _targetDetected = false;
            SetTargetCalm();
        }
    }
    private void CastSphere()
    {
        _hitCollider = Physics2D.OverlapCircle(_visionPointTransform.position, _radiusDetection, _layerMask.value);
        if (_hitCollider != null)
        {
            if (_hitCollider.gameObject.TryGetComponent(out IDamageble damageble))
            {
                _botSound.PlaySalute();
                _targetDetected = true;
                SetTargetToAttack();
            }
        }
        else
        {
            SetTargetCalm();
        }
    }
    private bool CheckDistance()
    {
        if (Vector2.Distance(_visionPointTransform.position, _targetTransform.position) < _rangeVision)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private void SetTargetCalm()
    {
        _targetSelected = _targetCalm;
    }
    private void SetTargetToAttack()
    {
        _targetSelected = _targetTransform;
    }
}
