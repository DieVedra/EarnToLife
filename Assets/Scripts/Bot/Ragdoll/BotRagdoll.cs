using UnityEngine.U2D.Animation;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
using System.Threading.Tasks;

public class BotRagdoll : Ragdoll
{
    //[SerializeField] private Rigidbody2D _rigidbodyHead;
    //[SerializeField] private Rigidbody2D _rigidbodyBody;
    private ComponentRagdoll[] _ragdollComponents;
    private SpriteSkin[] _spriteSkins;
    private SpriteRenderer[] _spriteRenderersSkeleton;
    //private Rigidbody2D[] _rigidbodysForShot;
    private Gun _gun;
    private GameObject _skeleton;
    //private int _indexForRigidBodyForceble;
    public void Init(Gun gun, GameObject skeleton, float timeRagdollActive)
    {
        _skeleton = skeleton;
        _gun = gun;
        _timeRagdollActive = timeRagdollActive;
        //_rigidbodysForShot = new Rigidbody2D[] { _rigidbodyHead, _rigidbodyBody };
        //_indexForRigidBodyForceble = index;
        SkeletonBodyEnabled();

        _ragdollComponents = gameObject.GetComponentsInChildren<ComponentRagdoll>();
        _spriteSkins = _skeleton.GetComponentsInChildren<SpriteSkin>();
        _spriteRenderersSkeleton = new SpriteRenderer[_spriteSkins.Length];

        for (int i = 0; i < _ragdollComponents.Length; i++)
        {
            _ragdollComponents[i].Init();
            SetSkeletonSpriteRenderer(out _spriteRenderersSkeleton[i], _spriteSkins[i]);
            SetSprite(_ragdollComponents[i], _spriteRenderersSkeleton[i]);
            SetPosition(_ragdollComponents[i], _spriteSkins[i]);
            SetRotation(_ragdollComponents[i], _spriteSkins[i]);
        }
        InitSpritesToFade();
    }
    //public void ActiveRagdoll(Vector2 force, Vector2 position)
    //{
    //    _gun.DropWeapon();
    //    RagdollBodyEnabled();
    //    AddForceOnShot(force, position);
    //    StartCoroutine(FadeCorutine());
    //}
    public override void ActiveRagdoll()
    {
        _gun.DropWeapon();
        RagdollBodyEnabled();
        StartCoroutine(FadeCorutine());
    }
    protected override void InitSpritesToFade()
    {
        _spriteRenderersToFade = new List<SpriteRenderer>();
        for (int i = 0; i < _ragdollComponents.Length; i++)
        {
            _spriteRenderersToFade.Add(_ragdollComponents[i].SpriteRenderer);
        }
        _spriteRenderersToFade.Add(_gun.GetComponentInChildren<SpriteRenderer>());
    }
    private void SetPosition(ComponentRagdoll componentRagdoll, SpriteSkin bone)
    {
        //componentRagdoll.Rigidbody.position = bone.rootBone.position;
        componentRagdoll.TransformRagdoll.position = bone.rootBone.position;
    }
    private void SetRotation(ComponentRagdoll componentRagdoll, SpriteSkin bone)
    {
        //componentRagdoll.Rigidbody.rotation = bone.rootBone.rotation.z;
        componentRagdoll.TransformRagdoll.rotation = bone.rootBone.rotation;
    }
    private void SetSprite(ComponentRagdoll componentRagdoll, SpriteRenderer spriteRenderer)
    {
        componentRagdoll.SpriteRenderer.sprite = spriteRenderer.sprite;
        componentRagdoll.SpriteRenderer.color = Color.red;
        componentRagdoll.SpriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
    }
    private void RagdollBodyEnabled()
    {
        _skeleton.SetActive(false);
        gameObject.SetActive(true);
        //for (int i = 0; i < _ragdollComponents.Length; i++)
        //{
        //    _ragdollComponents[i].Rigidbody.isKinematic = false;
        //}
    }
    private void SkeletonBodyEnabled()
    {
        _skeleton.SetActive(true);
        gameObject.SetActive(false);
    }
    private void SetSkeletonSpriteRenderer(out SpriteRenderer spriteSkeleton, SpriteSkin spiteSkin)
    {
        spriteSkeleton = spiteSkin.GetComponent<SpriteRenderer>();
    }
    //private void AddForceOnShot(Vector2 force, Vector2 position)
    //{
    //    _rigidbodysForShot[_indexForRigidBodyForceble].AddForceAtPosition(force, position);
    //}
}
