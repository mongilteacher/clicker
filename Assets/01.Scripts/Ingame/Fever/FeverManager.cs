using System;
using UnityEngine;

public class FeverManager : MonoBehaviour
{
    public static FeverManager Instance { get; private set; }

    public static event Action OnFeverGaugeChanged;
    public static event Action OnFeverModeChanged;

    [Header("Gauge")]
    [SerializeField] private float _maxGauge = 100f;
    [SerializeField] private float _manualClickFactor = 10f;
    [SerializeField] private float _autoClickFactor = 2f;
    [SerializeField] private float _decayRate = 8f;

    [Header("Fever")]
    [SerializeField] private float _feverDuration = 10f;
    [SerializeField] private float _feverDamageMultiplier = 3f;

    private float _currentGauge;
    private bool _isFeverMode;
    private float _feverTimer;

    public bool IsFeverMode => _isFeverMode;
    public float FeverDamageMultiplier => _isFeverMode ? _feverDamageMultiplier : 1f;
    public float FeverTimeRemaining => _feverTimer;
    public float FeverDuration => _feverDuration;
    public float GaugeRatio => _isFeverMode
        ? _feverTimer / _feverDuration
        : _currentGauge / _maxGauge;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_isFeverMode)
        {
            _feverTimer -= Time.deltaTime;
            if (_feverTimer <= 0f)
            {
                EndFever();
                return;
            }

            OnFeverGaugeChanged?.Invoke();
        }
        else
        {
            if (_currentGauge > 0f)
            {
                _currentGauge -= _decayRate * Time.deltaTime;
                _currentGauge = Mathf.Max(0f, _currentGauge);
                OnFeverGaugeChanged?.Invoke();
            }
        }
    }

    public void AddClick(ClickInfo clickInfo)
    {
        if (_isFeverMode) return;

        float factor = clickInfo.Type == EClickType.Manual
            ? _manualClickFactor
            : _autoClickFactor;

        _currentGauge += factor;

        if (_currentGauge >= _maxGauge)
        {
            _currentGauge = _maxGauge;
            StartFever();
            return;
        }

        OnFeverGaugeChanged?.Invoke();
    }

    private void StartFever()
    {
        _isFeverMode = true;
        _feverTimer = _feverDuration;
        OnFeverModeChanged?.Invoke();
        OnFeverGaugeChanged?.Invoke();
    }

    private void EndFever()
    {
        _isFeverMode = false;
        _currentGauge = 0f;
        _feverTimer = 0f;
        OnFeverModeChanged?.Invoke();
        OnFeverGaugeChanged?.Invoke();
    }
}
