using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FireAnimation
{
    private Sprite[] _shootSprites;
    private ParticleSystem _particleSystem;
    //private TextureSheetAnimationModule _textureSheetAnimationModule;
    public FireAnimation(Sprite[] shootSprites, ParticleSystem particleSystem)
    {
        _shootSprites = shootSprites;
        _particleSystem = particleSystem;
        //_textureSheetAnimationModule = _particleSystem.textureSheetAnimation;
        //_textureSheetAnimationModule.AddSprite(GetRandomSprite());

    }
    public void FireAnimationPlay()
    {
        //_textureSheetAnimationModule.RemoveSprite(0);
        //_textureSheetAnimationModule.AddSprite(GetRandomSprite());
        _particleSystem.Play();
    }
    private Sprite GetRandomSprite()
    {
        return _shootSprites[Random.Range(0, _shootSprites.Length)];
    }
}
