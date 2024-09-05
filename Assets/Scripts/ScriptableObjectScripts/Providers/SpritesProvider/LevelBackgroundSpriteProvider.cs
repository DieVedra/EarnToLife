using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBackgroundSpriteProvider", menuName = "Providers/LevelBackgroundSpriteProvider", order = 51)]
public class LevelBackgroundSpriteProvider : ScriptableObject
{
    [SerializeField, BoxGroup("Cloud Color Settings")] private Color _upColor1;
    [SerializeField, BoxGroup("Cloud Color Settings")] private Color _upColor2;
    [SerializeField, BoxGroup("Cloud Color Settings")] private Color _downColor1;
    [SerializeField, BoxGroup("Cloud Color Settings")] private Color _downColor2;
    
    [SerializeField, ShowAssetPreview, HorizontalLine(color: EColor.White)] private Sprite _cloudSprite1;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite2;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite3;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite4;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite5;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite6;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite7;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite8;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite9;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite10;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite11;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite12;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite13;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite14;
    [SerializeField, ShowAssetPreview] private Sprite _cloudSprite15;
    [SerializeField, HorizontalLine(color: EColor.White)] private Sprite _skyGradient;
    public IReadOnlyList<Sprite> GetSpriteList()
    {
        List<Sprite> sprites = new List<Sprite>
        {
            _cloudSprite1,
            _cloudSprite2,
            _cloudSprite3,
            _cloudSprite4,
            _cloudSprite5,
            _cloudSprite6,
            _cloudSprite7,
            _cloudSprite8,
            _cloudSprite9,
            _cloudSprite10,
            _cloudSprite11,
            _cloudSprite12,
            _cloudSprite13,
            _cloudSprite14,
            _cloudSprite15
        };
        return sprites;
    }

    public Sprite SkyGradient => _skyGradient;
    public Color UpColor1 => _upColor1;
    public Color UpColor2 => _upColor2;
    public Color DownColor1 => _downColor1;
    public Color DownColor2 => _downColor2;
}
