using System;
using NaughtyAttributes;
using UnityEngine;

public class ResolutionSpriteScaler : MonoBehaviour
{
    [SerializeField, HorizontalLine(color: EColor.Gray)] private SpriteRenderer _startMenu;
    [SerializeField] private float _startMenuScaleDefault = 0.95f;
    [SerializeField, HorizontalLine(color: EColor.Gray)] private SpriteRenderer _map;
    [SerializeField] private AnimationCurve _resolutionsCurve;

    [SerializeField] private float _mapScaleDefault = 0.88f;
    [SerializeField, HorizontalLine(color: EColor.Gray)] private SpriteRenderer _garage;
    [SerializeField] private float _garageScaleDefault = 0.95f;
    
    [SerializeField, HorizontalLine(color: EColor.Gray)] private SpriteRenderer _background;
    private readonly Vector2 _defaultTextureResolution = new Vector2(1920f, 1080f);
    private readonly float _sizeMultiplier = 0.01f;
    private void Start()
    {
        SetStartMenuResolution();
        SetMapResolution();
        SetGarageResolution();
    }
    private float GetDefaultTextureQuotient()
    {
        var result = CalculateQuotient(_defaultTextureResolution.x, _defaultTextureResolution.y);
        return result;
    }
    private float GetCurrentResolutionQuotient()
    {
        var result = CalculateQuotient(Screen.currentResolution.width, Screen.currentResolution.height);
        return result;
    }

    private float CalculateQuotient(float width, float height)
    {
        return width / height;
    }
    [Button("SetStartMenuResolution")]
    private void SetStartMenuResolution()
    {
        SetScale(_startMenu, _startMenuScaleDefault);
    }
    [Button("SetMapResolution")]
    private void SetMapResolution()
    {
        SetMapScale();
        SetMapBackgroundSize();
    }
    [Button("SetGarageResolution")]
    private void SetGarageResolution()
    {
        SetScale(_garage, _garageScaleDefault);
    }
    private void SetScale(SpriteRenderer spriteRenderer, float defaultScale)
    {
        float defaultTextureQuotient = GetDefaultTextureQuotient();
        float currentResolutionQuotient = GetCurrentResolutionQuotient();
        float newScale = GetProportionValue(defaultScale, currentResolutionQuotient, defaultTextureQuotient);
        spriteRenderer.transform.localScale = new Vector2(newScale, newScale);
    }

    private float GetProportionValue(float b, float c, float a)
    {
        return (b * c) / a;  //a/b = c/d
    }
    private void SetMapBackgroundSize()
    {
        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;
        float resolution = GetCurrentResolutionQuotient();
        Vector2 newSize = new Vector2(width, height);
        if (width < _defaultTextureResolution.x)
        {
            resolution = resolution * (_defaultTextureResolution.x / width);
        }
        newSize = newSize * resolution;
        _background.size = newSize * _sizeMultiplier;
    }

    private void SetMapScale()
    {
        float currentResolutionQuotient = GetCurrentResolutionQuotient();
        float valueScale = _resolutionsCurve.Evaluate(currentResolutionQuotient);
        _map.transform.localScale = new Vector2(valueScale, valueScale);
    }
    [Button("Reset")]
    private void Reset()
    {
        _startMenu.transform.localScale = new Vector2(_startMenuScaleDefault, _startMenuScaleDefault);
        _map.transform.localScale = new Vector2(_mapScaleDefault, _mapScaleDefault);
        _garage.transform.localScale = new Vector2(_garageScaleDefault, _garageScaleDefault);
        SetMapBackgroundSize();
    }
}