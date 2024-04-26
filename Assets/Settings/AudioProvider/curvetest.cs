using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class curvetest : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;

    public AudioSource AudioSource;

    [Button("set")]
    private void a()
    {
        // _animationCurve = new AnimationCurve();
        _animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        // _animationCurve = AnimationCurve.EaseInOut(0f, 0f, 0.2f, 0.8f);
        // _animationCurve.AddKey(new Keyframe(0f,0f));
        // _animationCurve.AddKey(0.2f,0.8f);
        // _animationCurve.AddKey(1f,1f);
        //
        // _animationCurve.SmoothTangents(1, -1);
    }

    [Button("play")]
    private void b()
    {
        AudioSource.Play();
    }
}
