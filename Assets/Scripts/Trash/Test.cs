using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
// [ExecuteInEditMode]
public class Test : MonoBehaviour
{
    public AudioSource _audioSource;
    [Range(0f, 1f)] public float Pitch;

    private void Start()
    {
    }

    private void OnValidate()
    {
        if (_audioSource != null)
        {
            _audioSource.pitch = Mathf.Lerp(0f,1f,Pitch);
        }
    }

    private void Update()
    {
        
    }

    [Button("Play")]
    private void a()
    {
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        
    }
}
