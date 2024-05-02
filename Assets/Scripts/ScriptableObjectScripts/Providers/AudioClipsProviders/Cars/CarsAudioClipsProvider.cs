using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CarsAudioClipsProvider", menuName = "Providers/CarsAudioClipsProvider", order = 51)]
public class CarsAudioClipsProvider : ScriptableObject
{
    [SerializeField, Expandable] private CarAudioClipProvider _car1AudioClipProvider;

    [SerializeField, Expandable] private CarAudioClipProvider _car2AudioClipProvider;

    [SerializeField, Expandable] private CarAudioClipProvider _car3AudioClipProvider;

    [SerializeField, Expandable] private CarAudioClipProvider _car4AudioClipProvider;
    
    [SerializeField, Expandable] private CarDefaultAudioClipProvider _carDefaultAudioClipProvider;

    private CarAudioClipProvider[] _carAudioClipProvider;
    public CarAudioClipProvider Car1AudioClipProvider
    {
        get
        {
            _car1AudioClipProvider.Init(_carDefaultAudioClipProvider);
            return _car1AudioClipProvider;
        }
    }
    public CarAudioClipProvider Car2AudioClipProvider
    {
        get
        {
            _car2AudioClipProvider.Init(_carDefaultAudioClipProvider);
            return _car1AudioClipProvider;
        }
    }

    public CarAudioClipProvider Car3AudioClipProvider
    {
        get
        {
            _car3AudioClipProvider.Init(_carDefaultAudioClipProvider);
            return _car1AudioClipProvider;
        }
    }

    public CarAudioClipProvider Car4AudioClipProvider
    {
        get
        {
            _car4AudioClipProvider.Init(_carDefaultAudioClipProvider);
            return _car1AudioClipProvider;
        }
    }

    public CarAudioClipProvider GetCarAudioClipProvider(int index)
    {
        _carAudioClipProvider = new[]
        {
            Car1AudioClipProvider,
            Car2AudioClipProvider,
            Car3AudioClipProvider,
            Car4AudioClipProvider
        };
        return _carAudioClipProvider[index];
    }
}
