using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeButtonsSprites", menuName = "GarageContent/UpgradeButtonsSprites", order = 51)]
public class UpgradeButtonsGarageContent : ScriptableObject
{
    [SerializeField, ShowAssetPreview] private Sprite _engineSprite;
    [SerializeField, ShowAssetPreview, HorizontalLine(color: EColor.Red)] private Sprite _gearboxSprite;
    [SerializeField, ShowAssetPreview, HorizontalLine(color: EColor.Blue)] private Sprite _wheelSprite;
    [SerializeField, ShowAssetPreview, HorizontalLine(color: EColor.Green)] private Sprite _gunSprite;
    [SerializeField, ShowAssetPreview, HorizontalLine(color: EColor.Yellow)] private Sprite _corpusSprite;
    [SerializeField, ShowAssetPreview, HorizontalLine(color: EColor.Orange)] private Sprite _boosterSprite;
    [SerializeField, ShowAssetPreview, HorizontalLine(color: EColor.Indigo)] private Sprite _tankSprite;
    private List<Sprite> _sprites;
    public IReadOnlyList<Sprite> GetSpriteList()
    {
        _sprites = new List<Sprite>
        {
            _engineSprite,
            _gearboxSprite,
            _wheelSprite,
            _gunSprite,
            _corpusSprite,
            _boosterSprite,
            _tankSprite
        };
        return _sprites;
    }
}
