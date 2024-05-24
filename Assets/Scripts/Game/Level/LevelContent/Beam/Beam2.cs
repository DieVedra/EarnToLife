using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Beam2 : Beam, IHitable, ICutable
{
        [SerializeField, BoxGroup("Debris")] private SpriteRenderer _spriteFragment3;

        private readonly float _oneThirdMultiplier = 0.33f;

        private float _oneThirdLength;
        private Transform _transform;
        public Vector2 Position => _transform.position;
        public bool IsBroken => ObjectIsBroken;
        public IReadOnlyList<DebrisFragment> DebrisFragments => base.FragmentsDebris;

        [Inject]
        private void Construct(ILevel level)
        {
                DebrisParentForDestroy = level.DebrisParent;
                WoodDestructibleAudioHandler = level.LevelAudio.WoodDestructibleAudioHandler;
                Rigidbody2D = GetComponent<Rigidbody2D>();
                _transform = transform;
                SetPositionsFragments();
                SetSizeToFragments();
        }

        public void DestructFromCut(Vector2 cutPos)
        {
                if (IsBroken == false)
                {
                        WoodDestructibleAudioHandler.PlayWoodBreakingSound();
                        Destruct();
                }
        }

        public bool TryBreakOnImpact(float forceHit)
        {
                bool result;
                if (IsBroken == false)
                {
                        if (forceHit > Hardness)
                        {
                                WoodDestructibleAudioHandler.PlayWoodBreakingSound();
                                Destruct();
                                result = true;
                        }
                        else
                        {
                                WoodDestructibleAudioHandler.PlayWoodNotBreakingSound(forceHit);
                                result = false;
                        }
                }
                else
                {
                        result = false;
                }
                return result;
        }

        // public void AddForce(Vector2 force)
        // {
        //         Rigidbody2D.AddForce(force * ForceMultiplierWholeObject);
        // }

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
                return CalculatePosition(_sprite.size.x * HalfWidthMultiplier );
        }

        private new void OnEnable()
        {
                OnDebrisHit += WoodDestructibleAudioHandler.PlayHitWoodSound;
                base.OnEnable();
        }

        private new void OnDisable()
        {
                OnDebrisHit -= WoodDestructibleAudioHandler.PlayHitWoodSound;
                base.OnDisable();
        }
}
