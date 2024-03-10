using System;
using UniRx;

public class Booster
{
    private BoosterScrew _boosterScrew;
    private CarAudioHandler _carAudioHandler;
    private CompositeDisposable _compositeDisposable => _boosterScrew.CompositeDisposable;
    // private ParticleSystem _particleSystemAccelerator;
    private bool _isRun = false;
    public BoosterFuelTank BoosterFuelTank { get; private set; }
    public bool FuelAvailability => BoosterFuelTank.CheckFuel();
    public event Action OnBoosterDisable;
    public Booster(CarAudioHandler carAudioHandler, BoosterFuelTank boosterFuelTank, BoosterScrew boosterScrew/*, ParticleSystem particleSystemAccelerator*/)
    {
        _carAudioHandler = carAudioHandler;
        BoosterFuelTank = boosterFuelTank;
        _boosterScrew = boosterScrew;
        /*_particleSystemAccelerator = particleSystemAccelerator;*/
    }
    public void TryStopBooster()
    {
        if (_isRun == true)
        {
            StopBooster();
        }
    }
    public void StopBoosterOnOutFuel()
    {
        if (_isRun == true)
        {
            StopBooster();
            _carAudioHandler.PlayBoosterEndFuel();
            // _particleSystemAccelerator.Stop();
        }
    }
    public void RunBooster()
    {
        _isRun = true;
        _carAudioHandler.PlayBoosterRun();
        _compositeDisposable.Clear();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _boosterScrew.RotateScrew();
        }).AddTo(_compositeDisposable);
        /*_particleSystemAccelerator.Play();*/
    }
    public void BoosterDisable()
    {
        _isRun = false;
        _compositeDisposable.Clear();
        OnBoosterDisable?.Invoke();
    }

    private void StopBooster()
    {
        _isRun = false;
        _carAudioHandler.StopPlayRunBooster();
        _compositeDisposable.Clear();
        _boosterScrew.SetDefaultRotationSpeed();
        // _particleSystemAccelerator.Stop();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _boosterScrew.SmoothStopScrew();
        }).AddTo(_compositeDisposable);
    }
}