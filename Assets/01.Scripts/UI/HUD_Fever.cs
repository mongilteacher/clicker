using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Fever : MonoBehaviour
{
    [Header("Gauge")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Color _normalColor = new Color(1f, 0.6f, 0f);
    [SerializeField] private Color _feverColor = new Color(1f, 0.2f, 0.2f);

    [Header("Fever Text")]
    [SerializeField] private TextMeshProUGUI _feverText;
    [SerializeField] private float _punchScale = 0.3f;
    [SerializeField] private float _punchDuration = 0.4f;

    private void Start()
    {
        _slider.interactable = false;

        RefreshGauge();
        RefreshMode();

        FeverManager.OnFeverGaugeChanged += RefreshGauge;
        FeverManager.OnFeverModeChanged += RefreshMode;
    }

    private void OnDestroy()
    {
        FeverManager.OnFeverGaugeChanged -= RefreshGauge;
        FeverManager.OnFeverModeChanged -= RefreshMode;
    }

    private void RefreshGauge()
    {
        _slider.value = FeverManager.Instance.GaugeRatio;
    }

    private void RefreshMode()
    {
        bool isFever = FeverManager.Instance.IsFeverMode;

        _fillImage.color = isFever ? _feverColor : _normalColor;

        if (_feverText != null)
        {
            _feverText.gameObject.SetActive(isFever);

            if (isFever)
            {
                _feverText.transform.DOKill();
                _feverText.transform.localScale = Vector3.one;
                _feverText.transform.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 1, 0.5f);
            }
        }
    }
}
