
using System;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class LevelBackground : MonoBehaviour
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _cameraTransform;
    [SerializeField] private Sprite[] _sprites;

    [SerializeField, HorizontalLine(color: EColor.White), BoxGroup("ParallaxY Settings")] private Transform _skyTransform;
    [SerializeField, BoxGroup("ParallaxY Settings")] private Transform _skyTopBorderPoint;
    [SerializeField, BoxGroup("ParallaxY Settings")] private Transform _skyDownBorderPoint;
    [Space]
    [SerializeField, BoxGroup("ParallaxY Settings")] private Transform _cloudsTransform;
    [SerializeField, BoxGroup("ParallaxY Settings")] private Transform _cloudTopBorderPoint;
    [SerializeField, BoxGroup("ParallaxY Settings")] private Transform _cloudDownBorderPoint;
    [Space]
    [SerializeField, BoxGroup("ParallaxY Settings")] private Transform _pointBorderLevelTop;
    [SerializeField, BoxGroup("ParallaxY Settings")] private Transform _pointBorderLevelDown;

    [SerializeField, HorizontalLine(color: EColor.Blue), BoxGroup("Cloud Generator Settings")] private SpriteRenderer _cloudPrefab;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Transform _cloudParent;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private int _cloudsCount;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Transform _pointRightUpBorder;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Transform _pointLeftDownBorder;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _upColor1;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _upColor2;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _downColor1;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _downColor2;
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(0f,1f)] private Vector2 _cloudColorAlphaRange;
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(5f,20f)] private Vector2 _cloudScaleRange;
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(0f,1f)] private Vector2 _speedAddedRange;
    [SerializeField, BoxGroup("Cloud Generator Settings"), Range(0f,0.5f)] private float _speed;
    [SerializeField, BoxGroup("Cloud Generator Settings"), Range(1f,20f)] private float _speedAdd;



    private Transform _transform;
    private CloudsGenerator _cloudsGenerator;
    private Cloud[] _clouds;
    private Vector2 _previousPos;
    private GamePause _gamePause;
    private CompositeDisposable _compositeDisposablePauseProperty = new CompositeDisposable();
    private CompositeDisposable _compositeDisposableUpdate = new CompositeDisposable();
    private float _differenceBetweenPreviousAndCurrentPositions;
    private float _tBordersLevel;
    private float _tSkyYPos;
    private float _tCloudsYPos;
    private float _calculatedSpeed;

    private float _negativeDeltaTime => Time.deltaTime * -1f;
    [Inject]
    private void Construct(Factory factory, GamePause gamePause)
    {
        _transform = transform;
        _gamePause = gamePause;
        _previousPos = _transform.position;
        _transform.SetParent(_cameraTransform);
        _cloudsGenerator = new CloudsGenerator(_sprites, _cloudPrefab, factory, _cloudParent, _pointRightUpBorder, _pointLeftDownBorder,
            _upColor1, _upColor2, _downColor1, _downColor2,
            _cloudScaleRange, _cloudColorAlphaRange, _speedAddedRange,
            _cloudsCount);
    }
    private void FixedUpdate()
    {
        if (_gamePause.IsPause == false)
        {
            CalculateAddedSpeedFromCameraMoveToClouds();
            _calculatedSpeed = _speed * Time.deltaTime +
                               _differenceBetweenPreviousAndCurrentPositions * _speedAdd * _negativeDeltaTime;
            foreach (var cloud in _cloudsGenerator.Clouds)
            {
                cloud.Move(_calculatedSpeed);
            }

            CalculateParallaxY();
        }
    }

    private void CalculateAddedSpeedFromCameraMoveToClouds()
    {
        _differenceBetweenPreviousAndCurrentPositions = _cameraTransform.PositionVector2().x - _previousPos.x;
        _previousPos = _cameraTransform.position;
    }
    private void CalculateParallaxY()
    {
        _tBordersLevel = Mathf.InverseLerp(_pointBorderLevelDown.position.y,_pointBorderLevelTop.position.y, _transform.position.y);
        CalculateSkyParallax();
        CalculateCloudsParallax();
    }
    private void CalculateSkyParallax()
    {
        _tSkyYPos = Mathf.Lerp(_skyTopBorderPoint.localPosition.y, _skyDownBorderPoint.localPosition.y, _tBordersLevel);
        _skyTransform.localPosition = new Vector2(_skyTransform.localPosition.x, _tSkyYPos);
    }
    private void CalculateCloudsParallax()
    {
        _tCloudsYPos = Mathf.Lerp(_cloudTopBorderPoint.localPosition.y, _cloudDownBorderPoint.localPosition.y, _tBordersLevel);
        _cloudsTransform.localPosition = new Vector2(_cloudsTransform.localPosition.x, _tCloudsYPos);
    }
}