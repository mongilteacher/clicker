using System.Collections;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageFloater : MonoBehaviour, IPoolable
{
    // ─────────────────────────────────────────────────────────────
    // 컴포넌트 참조
    // ─────────────────────────────────────────────────────────────
    [SerializeField] private TMP_Text _damageText;

    // ─────────────────────────────────────────────────────────────
    // 일반 설정
    // ─────────────────────────────────────────────────────────────
    [Header("Animation")] [SerializeField] private float _floatDistance = 1.0f;
    [SerializeField] private float _duration = 0.8f;

    [Header("Normal")] [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private float _normalScale = 1.0f;

    [Header("Critical")] [SerializeField] private Color _criticalColor = new Color(1f, 0.9f, 0.2f); // 노란색
    [SerializeField] private float _criticalScale = 1.4f;

    // ─────────────────────────────────────────────────────────────
    // 내부 변수
    // ─────────────────────────────────────────────────────────────
    private Sequence _currentSequence;
    private Renderer _renderer;
    private static int _sortingOrderCounter;

    private void Awake()
    {
        _renderer = _damageText.GetComponent<Renderer>();
    }

    // ═════════════════════════════════════════════════════════════
    // 공개 API
    // ═════════════════════════════════════════════════════════════

    /// <summary>
    /// 데미지 표시를 시작한다
    /// DamageFloaterManager에서 호출
    /// </summary>
    public void Show(ClickInfo clickInfo)
    {
        // 0. sortingOrder 증가
        _renderer.sortingOrder = ++_sortingOrderCounter;

        // 1. 텍스트 설정
        _damageText.text = $"<size=8><sprite=0></size>{clickInfo.Damage.ToFormattedString()}";
        //_damageText.text = clickInfo.Damage.ToFormattedString();


        // 2. 직접 클릭 / 오토클릭에 따라 색깔 변경
        if (clickInfo.Type == EClickType.Auto)
        {
            _damageText.color = _criticalColor;
            transform.localScale = Vector3.one * _criticalScale;
        }
        else
        {
            _damageText.color = _normalColor;
            transform.localScale = Vector3.one * _normalScale;
        }

        // 3. 애니메이션 시작
        PlayAnimation(clickInfo.Type);
    }
    


    // ═════════════════════════════════════════════════════════════
    // 애니메이션
    // ═════════════════════════════════════════════════════════════

    private void PlayAnimation(EClickType clickType)
    {
        // 기존 애니메이션이 남아있으면 정리
        _currentSequence?.Kill();

        // 초기 상태 설정
        _damageText.alpha = 1f;

        // DOTween Sequence: 여러 애니메이션을 순서/동시에 실행
        _currentSequence = DOTween.Sequence();

        // 수동 클릭이면 팝핑 효과 추가 (작게 → 크게 → 원래 크기)
        if (clickType == EClickType.Auto)
        {
            float targetScale = _criticalScale;
            transform.localScale = Vector3.one * 0.5f;
            _currentSequence.Append(
                transform.DOScale(targetScale * 1.3f, 0.1f).SetEase(Ease.OutBack)
            );
            _currentSequence.Append(
                transform.DOScale(targetScale, 0.1f).SetEase(Ease.InOutSine)
            );
        }

        // 위로 떠오르기
        _currentSequence.Join(
            transform.DOMoveY(transform.position.y + _floatDistance, _duration)
                .SetEase(Ease.OutCubic)
        );

        // 후반부에 페이드 아웃 (전체 시간의 마지막 40%에서 시작)
        float fadeDelay = _duration * 0.6f;
        float fadeDuration = _duration * 0.4f;
        _currentSequence.Insert(fadeDelay,
            DOTween.To(
                () => _damageText.alpha,
                x => _damageText.alpha = x,
                0f,
                fadeDuration
            )
        );

        // 애니메이션 완료 → 풀로 반환
        _currentSequence.OnComplete(() => { LeanPool.Despawn(gameObject); });
    }

    // ═════════════════════════════════════════════════════════════
    // IPoolable 구현
    // ═════════════════════════════════════════════════════════════

    /// <summary>
    /// 풀에서 꺼내질 때 호출 (Spawn 시)
    /// </summary>
    public void OnSpawn()
    {
        // Spawn 시 초기화가 필요하면 여기에 작성
    }

    /// <summary>
    /// 풀로 돌아갈 때 호출 (Despawn 시)
    /// </summary>
    public void OnDespawn()
    {
        // 진행 중인 애니메이션 정리
        _currentSequence?.Kill();
        _currentSequence = null;
    }
}