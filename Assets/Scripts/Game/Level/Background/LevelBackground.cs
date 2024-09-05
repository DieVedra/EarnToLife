
using System;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class LevelBackground : MonoBehaviour
{
    [SerializeField, HorizontalLine(color: EColor.Green)] private Transform _cameraTransform;

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
    [SerializeField, BoxGroup("Cloud Generator Settings")] private int _cloudsCount = 60;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Transform _pointRightUpBorder;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Transform _pointLeftDownBorder;
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(0f,1f)] private Vector2 _cloudColorAlphaRange = new Vector2(0.16f, 0.22f);
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(5f,20f)] private Vector2 _cloudScaleRange = new Vector2(8f, 15f);
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(0f,1f)] private Vector2 _speedAddedRange = new Vector2(0.002f, 0.01f);
    [SerializeField, BoxGroup("Cloud Generator Settings"), Range(0f,0.5f)] private float _speed = 0.015f;
    [SerializeField, BoxGroup("Cloud Generator Settings"), Range(0f,20f)] private float _speedAdd = 1f;

    [SerializeField, Expandable, BoxGroup("Cloud Generator Settings")] private LevelBackgroundSpriteProvider _levelBackgroundSpriteProvider;
    
    private readonly float _halfMultiplier = 0.5f;
    private GameOverSignal _gameOverSignal;
    private Transform _transform;
    private CloudsGenerator _cloudsGenerator;
    private Cloud[] _clouds;
    private Vector2 _previousPos;
    private Vector2 _skyPos = new Vector2();
    private Vector2 _cloudsPosY = new Vector2();
    private GamePause _gamePause;
    private CompositeDisposable _compositeDisposable;
    private float _differenceBetweenPreviousAndCurrentPositions;
    private float _tBordersLevel;
    private float _calculatedSpeed;
    private bool _isFrameSplitted;
    private float _negativeDeltaTime => Time.deltaTime * -1f;
    [Inject]
    private void Construct( GamePause gamePause, GameOverSignal gameOverSignal)
    {
        _gameOverSignal = gameOverSignal;
        _transform = transform;
        _gamePause = gamePause;
        _previousPos = _transform.position;
        _transform.SetParent(_cameraTransform);
        _skyTransform.GetComponent<SpriteRenderer>().sprite = _levelBackgroundSpriteProvider.SkyGradient;
        _cloudsGenerator = new CloudsGenerator(_levelBackgroundSpriteProvider.GetSpriteList(), _cloudPrefab, _cloudParent, _pointRightUpBorder, _pointLeftDownBorder,
            _levelBackgroundSpriteProvider.UpColor1, _levelBackgroundSpriteProvider.UpColor2,
            _levelBackgroundSpriteProvider.DownColor1, _levelBackgroundSpriteProvider.DownColor2,
            _cloudScaleRange, _cloudColorAlphaRange, _speedAddedRange,
            _cloudsCount);
        _cloudsGenerator.Generate();
        _gameOverSignal.OnGameOver += GameOver;
        SubscribeFixedUpdate();
    }

    // [Button("SetDefault")]
    // private void SetDefault()
    // {
    //     _transform = transform;
    //     CalculateParallaxY();
    //     _skyTransform.localPosition = _skyPos;
    //     _cloudsTransform.localPosition = _cloudsPosY;
    // }
    // private void OnValidate()
    // {
    //     _skyPos.x = _skyTransform.localPosition.x;
    //     _skyPos.y = Mathf.Lerp(_skyTopBorderPoint.localPosition.y, _skyDownBorderPoint.localPosition.y, b);
    //     _cloudsPosY.x = _cloudsTransform.localPosition.x;
    //     _cloudsPosY.y = Mathf.Lerp(_cloudTopBorderPoint.localPosition.y, _cloudDownBorderPoint.localPosition.y, b);
    //     _skyTransform.localPosition = _skyPos;
    //     _cloudsTransform.localPosition = _cloudsPosY;
    //     
    // }

    private void SubscribeFixedUpdate()
    {
        _compositeDisposable = new CompositeDisposable();
        Observable.EveryFixedUpdate().Where(x => _gamePause.IsPause == false).Subscribe(_ =>
        {
            if (_isFrameSplitted == false)
            {
                _differenceBetweenPreviousAndCurrentPositions = _cameraTransform.PositionVector2().x - _previousPos.x;
                _previousPos = _cameraTransform.position;
                _calculatedSpeed = 
                    (_speed * Time.deltaTime + _differenceBetweenPreviousAndCurrentPositions * _speedAdd * _negativeDeltaTime) * Time.timeScale * _halfMultiplier;
                CalculateParallaxY();
                _isFrameSplitted = true;
            }
            else
            {
                _isFrameSplitted = false;
            }
            foreach (var cloud in _cloudsGenerator.Clouds)
            {
                cloud.Move(_calculatedSpeed);
            }
            _skyTransform.localPosition = _skyPos;
            _cloudsTransform.localPosition = _cloudsPosY;
        }).AddTo(_compositeDisposable);
    }
    private void CalculateParallaxY()
    {
        _tBordersLevel = Mathf.InverseLerp(_pointBorderLevelDown.position.y,_pointBorderLevelTop.position.y, _transform.position.y);
        CalculateSkyParallax();
        CalculateCloudsParallax();
    }
    private void CalculateSkyParallax()
    {
        _skyPos.x = _skyTransform.localPosition.x;
        _skyPos.y = Mathf.Lerp(_skyTopBorderPoint.localPosition.y, _skyDownBorderPoint.localPosition.y, _tBordersLevel);
    }
    private void CalculateCloudsParallax()
    {
        _cloudsPosY.x = _cloudsTransform.localPosition.x;
        _cloudsPosY.y = Mathf.Lerp(_cloudTopBorderPoint.localPosition.y, _cloudDownBorderPoint.localPosition.y, _tBordersLevel);
    }

    private void GameOver()
    {
        _compositeDisposable.Clear();
    }
    private void OnDestroy()
    {
        if (_gameOverSignal != null)
        {
            _gameOverSignal.OnGameOver -= GameOver;
        }

        _compositeDisposable?.Clear();
    }
}