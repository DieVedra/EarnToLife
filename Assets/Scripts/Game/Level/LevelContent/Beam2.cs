using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Beam2 : Beam1
{
        [SerializeField, BoxGroup("Debris")] private SpriteRenderer _spriteFragment3;
        [SerializeField, BoxGroup("Settings"), Range(-1f, 1f)] protected float _offsetPositionMediumFragment = 1f;

        private readonly float _oneThirdMultiplier = 0.33f;

        private float _oneThirdLength;

        protected override void SetSizeToFragments()
        {
                CalculateOneThirdBeamLength();
                SetSizeToFragment(_spriteFragment1, _oneThirdLength);
                SetSizeToFragment(_spriteFragment2, _oneThirdLength);
                SetSizeToFragment(_spriteFragment3, _oneThirdLength);
        }
        protected override void SetPositionsFragments()
        {
                SetPositionFragment(_spriteFragment1, GetFirstPositionFragment());
                SetPositionFragment(_spriteFragment2, GetMediumPositionFragment());
                SetPositionFragment(_spriteFragment3, GetEndPositionFragment());
        }
        private void CalculateOneThirdBeamLength()
        {
                _oneThirdLength = _sprite.size.x * _oneThirdMultiplier  + _offsetSizeFragment;
        }
        private Vector3 GetMediumPositionFragment()
        {
                Vector2 newPos = _sprite.transform.position;
                newPos.x += _sprite.size.x * HalfWidthMultiplier + _offsetPositionMediumFragment;
                return newPos;
        }
        [Button("SetChanges")]
        private void Init()
        {
                SetPositionsFragments();
                SetSizeToFragments();
        }
        // 0.33
        // -0.03
}
