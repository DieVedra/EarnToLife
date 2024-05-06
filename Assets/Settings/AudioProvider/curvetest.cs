using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class curvetest : MonoBehaviour
{
    // [SerializeField] private AnimationCurve _animationCurve;

    public AudioSource AudioSource;
    public AudioClip Clip1;
    public AudioClip Clip2;

    [Button("set1")]
    private void a()
    {
        // _animationCurve = new AnimationCurve();
        // _animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        // _animationCurve = AnimationCurve.EaseInOut(0f, 0f, 0.2f, 0.8f);
        // _animationCurve.AddKey(new Keyframe(0f,0f));
        // _animationCurve.AddKey(0.2f,0.8f);
        // _animationCurve.AddKey(1f,1f);
        //
        // _animationCurve.SmoothTangents(1, -1);

        AudioSource.clip = Clip1;
    }
    [Button("playOneShot")]
    private void b()
    {
        
        AudioSource.PlayOneShot(Clip2);
    }

    [Button("play")]
    private void c()
    {
        AudioSource.Play();
    }
    [Button("pause")]
    private void d()
    {
        AudioSource.Pause();
    }
    [Button("stop")]
    private void e()
    {
        AudioSource.Stop();
    }
}
