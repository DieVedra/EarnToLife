using System;
using NaughtyAttributes;
using UnityEngine;

public class Beam1 : MonoBehaviour
{
    [SerializeField, BoxGroup("Whole object")] protected SpriteRenderer _sprite;
    [SerializeField, BoxGroup("Debris"), HorizontalLine(color: EColor.Green)] protected SpriteRenderer _spriteFragment1;
    [SerializeField, BoxGroup("Debris")] protected SpriteRenderer _spriteFragment2;
    [SerializeField, BoxGroup("Settings"), Range(0f, 1f), HorizontalLine(color: EColor.Green)] protected float _offsetSizeFragment;

    protected readonly float HalfWidthMultiplier = 0.5f;
    private float _halfBeamLength;
    private void Start()
    {
        Init();
    }
    [Button("SetChanges")]
    private void Init()
    {
        SetPositionsFragments();
        SetSizeToFragments();
    }

    protected virtual void SetPositionsFragments()
    {
        SetPositionFragment(_spriteFragment1, GetFirstPositionFragment());
        SetPositionFragment(_spriteFragment2, GetEndPositionFragment());
    }

    protected virtual void SetSizeToFragments()
    {
        CalculateHalfBeamLength();
        SetSizeToFragment(_spriteFragment1, _halfBeamLength);
        SetSizeToFragment(_spriteFragment2, _halfBeamLength);
    }
    protected void SetSizeToFragment(SpriteRenderer sprite, float beamLength)
    {
        sprite.size = new Vector2(beamLength, sprite.size.y);
    }

    protected void SetPositionFragment(SpriteRenderer sprite, Vector2 position)
    {
        sprite.transform.position = position;
    }

    protected Vector3 GetFirstPositionFragment()
    {
        return _sprite.transform.position;
    }
    protected Vector3 GetEndPositionFragment()
    {
        Vector2 newPos = _sprite.transform.position;
        newPos.x += _sprite.size.x;
        return newPos;
    }
    private void CalculateHalfBeamLength()
    {
        _halfBeamLength = _sprite.size.x * HalfWidthMultiplier  + _offsetSizeFragment;
    }
}