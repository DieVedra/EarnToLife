using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _intensity = 5f;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _noise;
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private float _currentDuration = 0f;
    [Inject] private ExplodeSignal _explodeSignal;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _explodeSignal.OnExplosion += Shake;
        SetDefault();

    }

    private void Shake(Vector2 position)
    {
        _currentDuration = _duration;
        _noise.m_FrequencyGain = _duration;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_currentDuration > 0f)
            {
                _currentDuration -= Time.deltaTime * Time.timeScale;
                _noise.m_AmplitudeGain = Mathf.Lerp( 0f, _intensity,_currentDuration / _duration);
            }
            else
            {
                _compositeDisposable.Clear();
                SetDefault();
            }
        }).AddTo(_compositeDisposable);
    }

    private void SetDefault()
    {
        _noise.m_AmplitudeGain = 0f;
        _noise.m_FrequencyGain = 0f;
    }
    private void OnDestroy()
    {
        _explodeSignal.OnExplosion -= Shake;
        _compositeDisposable.Clear();
    }
}