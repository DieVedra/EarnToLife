using UniRx;
using UnityEngine;

public class BoosterScrew
{
    private readonly int _sortingOrderForeground = 0;
    private readonly int _sortingOrderBackground = -2;
    private readonly float _rotationSpeed;
    private Transform _screw;
    private SpriteRenderer _blade1;
    private SpriteRenderer _blade2;
    private float _screwRotationX => _screw.rotation.eulerAngles.x;

    public BoosterScrew(Transform screw, SpriteRenderer blade1, SpriteRenderer blade2, float rotationSpeed)
    {
        _screw = screw;
        _blade1 = blade1;
        _blade2 = blade2;
        _rotationSpeed = rotationSpeed;
    }
    public void RotateScrew(ref float rotationSpeedMultiplier)
    {
        _screw.Rotate(Vector3.right, _rotationSpeed * rotationSpeedMultiplier);
        ChangesBladesOrderLayer();
    }
    public void RotateScrew(float rotationSpeedMultiplier)
    {
        _screw.Rotate(Vector3.right, _rotationSpeed * rotationSpeedMultiplier);
        ChangesBladesOrderLayer();
    }
    private void ChangesBladesOrderLayer()
    {
        if (_screwRotationX > 0 && _screwRotationX < 180)
        {
            _blade1.sortingOrder = _sortingOrderBackground;
            _blade2.sortingOrder = _sortingOrderForeground;
        }
        else
        {
            _blade1.sortingOrder = _sortingOrderForeground;
            _blade2.sortingOrder = _sortingOrderBackground;
        }
    }
}