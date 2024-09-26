using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class Trap : MonoBehaviour
{
    [SerializeField] private Collider2D _trigger;
    [SerializeField] private Rigidbody2D[] _rigidbodies2D;
    [SerializeField, Layer] private int _targetLayerTriggering;
    [SerializeField] private LayerMask _targetLayerMaskHitSound;

    private AudioClipProvider _audioClipProvider;
    private IGlobalAudio _globalAudio;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private TrapAudioHandler _trapAudioHandler;
    
    private List<Collider2D> _collider2Ds = new List<Collider2D>();
    private List<CompositeDisposable> _compositeDisposables = new List<CompositeDisposable>();

    [Inject]
    private void Construct(AudioClipProvider audioClipProvider, IGlobalAudio globalAudio)
    {
        _audioClipProvider = audioClipProvider;
        _globalAudio = globalAudio;
        _trapAudioHandler = new TrapAudioHandler(GetComponent<AudioSource>(),
            _globalAudio.SoundReactiveProperty, _globalAudio.AudioPauseReactiveProperty, _audioClipProvider.LevelAudioClipProvider);
        for (int i = 0; i < _rigidbodies2D.Length; i++)
        {
            _collider2Ds.Add(_rigidbodies2D[i].GetComponent<Collider2D>());
            _compositeDisposables.Add(new CompositeDisposable());
        }
        SetKinematics(true);

    }
    

    private void OnEnable()
    {
        _compositeDisposable = new CompositeDisposable();
        _trigger.OnTriggerEnter2DAsObservable().Subscribe(collider2D =>
        {
            if (collider2D.gameObject.layer == _targetLayerTriggering)
            {
                SetKinematics(false);
                _trapAudioHandler.PlayFall();
                SubscribeToHit();
                _compositeDisposable.Clear();
            }
        }).AddTo(_compositeDisposable);
    }

    private void SubscribeToHit()
    {
        for (int i = 0; i < _collider2Ds.Count; i++)
        {
            _collider2Ds[i].OnCollisionEnter2DAsObservable().First().Where(CheckHitGround).Subscribe(_ =>
            {
                _trapAudioHandler.PlayHit();
            }).AddTo(_compositeDisposables[i]);
        }
    }
    private bool CheckHitGround(Collision2D collision2D)
    {
        if ((1 << collision2D.gameObject.layer & _targetLayerMaskHitSound.value) == 1 << collision2D.gameObject.layer)
        {
            return true;
        }
        else return false;
    }
    private void SetKinematics(bool key)
    {
        for (int i = 0; i < _rigidbodies2D.Length; i++)
        {
            _rigidbodies2D[i].isKinematic = key;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _compositeDisposables.Count; i++)
        {
            _compositeDisposables[i].Clear();
        }
        _compositeDisposable.Clear();
    }
}
