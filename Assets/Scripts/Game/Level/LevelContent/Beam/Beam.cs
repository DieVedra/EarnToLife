using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public abstract class Beam : DestructibleObject
{
    protected readonly float HalfWidthMultiplier = 0.5f;
    [SerializeField, BoxGroup("Whole object")] protected SpriteRenderer _sprite;
    [SerializeField, BoxGroup("Debris"), HorizontalLine(color: EColor.Green)] protected SpriteRenderer _spriteFragment1;
    [SerializeField, BoxGroup("Debris")] protected SpriteRenderer _spriteFragment2;
    [SerializeField, BoxGroup("Settings"), Range(0f, 1f), HorizontalLine(color: EColor.Green)] protected float _offsetSizeFragment;
    [SerializeField, BoxGroup("Settings")] protected bool NotDestructible;
    protected WoodDestructibleAudioHandler WoodDestructibleAudioHandler;
    protected Transform BeamTransform;


    [Inject]
    private void Construct(IGlobalAudio globalAudio, AudioClipProvider audioClipProvider, TimeScaleSignal timeScaleSignal)
    {
        BeamTransform = transform;
        WoodDestructibleAudioHandler = new WoodDestructibleAudioHandler(GetComponent<AudioSource>(),
            globalAudio.SoundReactiveProperty, globalAudio.AudioPauseReactiveProperty, new TimeScalePitchHandler(timeScaleSignal),
            audioClipProvider.LevelAudioClipProvider.WoodBreaking1AudioClip,
            audioClipProvider.LevelAudioClipProvider.WoodBreaking2AudioClip,
            audioClipProvider.LevelAudioClipProvider.HitWood1AudioClip,
            audioClipProvider.LevelAudioClipProvider.HitWood2AudioClip,
            audioClipProvider.LevelAudioClipProvider.HitWood3AudioClip);
        DebrisHitSound = WoodDestructibleAudioHandler.PlayHitWoodSound;
    
    }
    protected abstract void SetSizeToFragments();
    protected abstract void SetPositionsFragments();
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
        return CalculatePosition(_sprite.size.x);
    }
    protected Vector2 CalculatePosition(float size)
    {
        float angle = _sprite.transform.rotation.eulerAngles.z;
        float resultX = _sprite.transform.position.x + size * Mathf.Cos(Mathf.Deg2Rad * angle);
        float resultY = _sprite.transform.position.y + size * Mathf.Sin(Mathf.Deg2Rad * angle);
        return new Vector2(resultX, resultY);
        // x = x₀ + L * cos(α)
        // y = y₀ + L * sin(α)
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        WoodDestructibleAudioHandler?.Dispose();
    }
}