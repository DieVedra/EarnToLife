
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
    [SerializeField, BoxGroup("Cloud Generator Settings")] private int _cloudsCount = 60;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Transform _pointRightUpBorder;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Transform _pointLeftDownBorder;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _upColor1;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _upColor2;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _downColor1;
    [SerializeField, BoxGroup("Cloud Generator Settings")] private Color _downColor2;
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(0f,1f)] private Vector2 _cloudColorAlphaRange = new Vector2(0.16f, 0.22f);
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(5f,20f)] private Vector2 _cloudScaleRange = new Vector2(8f, 15f);
    [SerializeField, BoxGroup("Cloud Generator Settings"), MinMaxSlider(0f,1f)] private Vector2 _speedAddedRange = new Vector2(0.002f, 0.01f);
    [SerializeField, BoxGroup("Cloud Generator Settings"), Range(0f,0.5f)] private float _speed = 0.015f;
    [SerializeField, BoxGroup("Cloud Generator Settings"), Range(1f,20f)] private float _speedAdd = 5f;


    private GameOverSignal _gameOverSignal;
    private Transform _transform;
    private CloudsGenerator _cloudsGenerator;
    private Cloud[] _clouds;
    private Vector2 _previousPos;
    private Vector2 _skyPos = new Vector2();
    private Vector2 _cloudsPosY = new Vector2();
    private GamePause _gamePause;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _gamePlay;
    private float _differenceBetweenPreviousAndCurrentPositions;
    private float _tBordersLevel;
    private float _calculatedSpeed;

    private float _negativeDeltaTime => Time.deltaTime * -1f;
    [Inject]
    private void Construct(Factory factory, GamePause gamePause, GameOverSignal gameOverSignal)
    {
        _gameOverSignal = gameOverSignal;
        _transform = transform;
        _gamePause = gamePause;
        _previousPos = _transform.position;
        _transform.SetParent(_cameraTransform);
        _cloudsGenerator = new CloudsGenerator(_sprites, _cloudPrefab, factory, _cloudParent, _pointRightUpBorder, _pointLeftDownBorder,
            _upColor1, _upColor2, _downColor1, _downColor2,
            _cloudScaleRange, _cloudColorAlphaRange, _speedAddedRange,
            _cloudsCount);
        _gameOverSignal.OnGameOver += GameOver;
        _gamePlay = true;
        _cancellationTokenSource = new CancellationTokenSource();
        DoUpdate().Forget();
    }

    private async UniTaskVoid DoUpdate()
    {
        while (_gamePlay == true)
        {
            if (_gamePause.IsPause == false)
            {
                CalculateAddedSpeedFromCameraMoveToClouds();
                _calculatedSpeed = _speed * Time.deltaTime +
                                   _differenceBetweenPreviousAndCurrentPositions * _speedAdd * _negativeDeltaTime;
                _calculatedSpeed *= Time.timeScale;
                await UniTask.NextFrame(_cancellationTokenSource.Token);
                CalculateParallaxY();
                for (int i = 0; i < _cloudsGenerator.Clouds.Count; i++)
                {
                    if (i % 20 == 0)
                    {
                        await UniTask.NextFrame(_cancellationTokenSource.Token);
                    }
                    _cloudsGenerator.Clouds[i].Move(_calculatedSpeed);
                }
            }
        }
    }
    // private void FixedUpdate()
    // {
    //     if (_gamePause.IsPause == false)
    //     {
    //         CalculateAddedSpeedFromCameraMoveToClouds();
    //         _calculatedSpeed = _speed * Time.deltaTime +
    //                            _differenceBetweenPreviousAndCurrentPositions * _speedAdd * _negativeDeltaTime;
    //         _calculatedSpeed *= Time.timeScale;
    //         foreach (var cloud in _cloudsGenerator.Clouds)
    //         {
    //             cloud.Move(_calculatedSpeed);
    //         }
    //
    //         CalculateParallaxY();
    //     }
    // }

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
        _skyPos.x = _skyTransform.localPosition.x;
        _skyPos.y = Mathf.Lerp(_skyTopBorderPoint.localPosition.y, _skyDownBorderPoint.localPosition.y, _tBordersLevel);
        _skyTransform.localPosition = _skyPos;
    }
    private void CalculateCloudsParallax()
    {
        _cloudsPosY.x = _cloudsTransform.localPosition.x;
        _cloudsPosY.y = Mathf.Lerp(_cloudTopBorderPoint.localPosition.y, _cloudDownBorderPoint.localPosition.y, _tBordersLevel);
        _cloudsTransform.localPosition = _cloudsPosY;
    }

    private void GameOver()
    {
        _gamePlay = false;
    }
    private void OnDestroy()
    {
        _gameOverSignal.OnGameOver -= GameOver;
        _gamePlay = false;
        _cancellationTokenSource.Cancel();
    }
}