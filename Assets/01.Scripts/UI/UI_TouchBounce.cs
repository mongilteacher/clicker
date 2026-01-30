using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UI_TouchBounce : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("타겟")]
    [Tooltip("스케일 애니메이션 대상. 비어있으면 자신의 RectTransform 사용")]
    [SerializeField] private RectTransform _targetRect;
    
    [Tooltip("연결된 버튼. 비어있으면 자동 탐색")]
    [SerializeField] private Button _button;
    
    [Header("애니메이션")]
    [SerializeField] private float _pressedScale = 0.96f;
    [SerializeField] private float _pressDuration = 0.1f;
    [SerializeField] private float _releaseDuration = 0.08f;
    
    private Tween _currentTween;
    private bool _isEnabled = true;

    
    public bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }
    
    private void Awake()
    {
        if (_targetRect == null)
        {
            _targetRect = GetComponent<RectTransform>();
        }

        if (_button == null)
        {
            _button = GetComponent<Button>();
        }
    }

    private void OnDisable()
    {
        KillTween();
        _targetRect.localScale = Vector3.one;
    }
    
    private void OnDestroy()
    {
        KillTween();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isEnabled) return;
        if (_button != null && !_button.interactable) return;

        KillTween();
        _currentTween = _targetRect
            .DOScale(_pressedScale, _pressDuration)
            .SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button != null && !_button.interactable) return;

        KillTween();
        _currentTween = _targetRect
            .DOScale(1f, _releaseDuration)
            .SetUpdate(true);
    }
    
    private void KillTween()
    {
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
            _currentTween = null;
        }
    }
}