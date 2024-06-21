using System.Collections.Generic;
using UnityEngine;

public class CloudsGenerator
{
    private readonly int _cloudsCount;
    private readonly Vector2 _cloudScaleRange;
    private readonly Vector2 _cloudColorAlphaRange;
    private readonly Vector2 _speedAddedRange;
    private readonly Color _upColor1;
    private readonly Color _upColor2;
    private readonly Color _downColor1;
    private readonly Color _downColor2;
    private readonly Factory _factory;
    private readonly SpriteRenderer _cloudPrefab;
    private readonly Transform _cloudParent;
    private readonly Transform _pointRightUpBorder;
    private readonly Transform _pointLeftDownBorder;

    private Sprite[] _sprites;
    private List<Cloud> _clouds;
    public IReadOnlyList<Cloud> Clouds => _clouds;
    
    public CloudsGenerator(Sprite[] sprites, SpriteRenderer cloudPrefab, Factory factory,
        Transform cloudParent, Transform pointRightUpBorder, Transform pointLeftDownBorder,
        Color upColor1, Color upColor2, Color downColor1, Color downColor2,
        Vector2 cloudScaleRange, Vector2 cloudColorAlphaRange, Vector2 speedAddedRange,
        int cloudsCount)
    {
        _sprites = sprites;
        _clouds = new List<Cloud>(_cloudsCount);
        _cloudsCount = cloudsCount;
        _factory = factory;
        _cloudPrefab = cloudPrefab;
        _cloudParent = cloudParent;
        _cloudScaleRange = cloudScaleRange;
        _cloudColorAlphaRange = cloudColorAlphaRange;
        _speedAddedRange = speedAddedRange;
        _pointRightUpBorder = pointRightUpBorder;
        _pointLeftDownBorder = pointLeftDownBorder;
        _upColor1 = upColor1;
        _upColor2 = upColor2;
        _downColor1 = downColor1;
        _downColor2 = downColor2;
        Generate();
    }
    public void Generate()
    {
        for (int i = 0; i < _cloudsCount; i++)
        {
            SpriteRenderer cloud = _factory.CreateCloud(_cloudPrefab, _cloudParent);
            Transform cloudTransform = cloud.transform;
            float posY = UnityEngine.Random.Range(_pointLeftDownBorder.position.y, _pointRightUpBorder.position.y);
            SetPosition(cloudTransform, posY);
            SetScale(cloudTransform);
            SetSprite(cloud);
            SetColor(cloud, posY);
            _clouds.Add(new Cloud(cloudTransform, _pointLeftDownBorder, _pointRightUpBorder, _speedAddedRange));
        }
    }
    private void SetColor(SpriteRenderer cloud, float posY)
    {
        Color colorUp = Color.Lerp(_upColor1, _upColor2, UnityEngine.Random.Range(0f,1f));
        Color colorDown = Color.Lerp(_downColor1, _downColor2, UnityEngine.Random.Range(0f, 1f));
        Color colorWithoutAlpha = Color.Lerp(colorUp,colorDown,
            Mathf.InverseLerp(_pointRightUpBorder.position.y, _pointLeftDownBorder.position.y, posY));
        Color colorWithAlpha = new Color(colorWithoutAlpha.r, colorWithoutAlpha.g, colorWithoutAlpha.b,
            UnityEngine.Random.Range(_cloudColorAlphaRange.x, _cloudColorAlphaRange.y));
        cloud.color = colorWithAlpha;
    }
    private void SetSprite(SpriteRenderer cloud)
    {
        cloud.sprite = _sprites[UnityEngine.Random.Range(0,_sprites.Length)];
    }
    private void SetScale(Transform cloudTransform)
    {
        float valueScale = UnityEngine.Random.Range(_cloudScaleRange.x, _cloudScaleRange.y);
        cloudTransform.localScale = new Vector2(valueScale,valueScale);
    }
    private void SetPosition(Transform cloudTransform, float posY)
    {
        cloudTransform.position = new Vector2(UnityEngine.Random.Range(_pointLeftDownBorder.position.x, _pointRightUpBorder.position.x), posY);
    }
}