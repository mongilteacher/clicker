using System.Collections;
using Lean.Pool;
using UnityEngine;

public class JellyDrop : MonoBehaviour, IPoolable
{
    // ─────────────────────────────────────────────────────────────
    // 컴포넌트 참조
    // ─────────────────────────────────────────────────────────────
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Sprite[] _jellySprites;

    // ─────────────────────────────────────────────────────────────
    // 설정
    // ─────────────────────────────────────────────────────────────
    [Header("Launch")]
    [SerializeField] private float _launchForceY = 10f;
    [SerializeField] private float _launchForceX = 3f;

    [Header("Scale")]
    [SerializeField] private float _scaleMin = 0.5f;
    [SerializeField] private float _scaleMax = 1.2f;

    [Header("Fade")]
    [SerializeField] private float _fadeDurationMin = 0.3f;
    [SerializeField] private float _fadeDurationMax = 0.7f;

    // ─────────────────────────────────────────────────────────────
    // 내부 변수
    // ─────────────────────────────────────────────────────────────
    private Coroutine _fadeCoroutine;
    private bool _isFading;
    private float _fadeDuration;

    // ═════════════════════════════════════════════════════════════
    // 공개 API
    // ═════════════════════════════════════════════════════════════

    public void Play()
    {
        // 랜덤 방향으로 위쪽 힘 적용
        Vector2 force = new Vector2(
            Random.Range(-_launchForceX, _launchForceX),
            _launchForceY + Random.Range(0f, _launchForceY * 0.3f)
        );
        _rigidbody.AddForce(force, ForceMode2D.Impulse);

        // 랜덤 회전
        _rigidbody.angularVelocity = Random.Range(-360f, 360f);

        // 랜덤 페이드 시간
        _fadeDuration = Random.Range(_fadeDurationMin, _fadeDurationMax);

        _isFading = false;
    }

    // ═════════════════════════════════════════════════════════════
    // 라이프사이클
    // ═════════════════════════════════════════════════════════════

    private void Update()
    {
        // 떨어지기 시작하면 페이드 시작
        if (!_isFading && _rigidbody.linearVelocity.y < 0f)
        {
            _isFading = true;
            _fadeCoroutine = StartCoroutine(FadeAndDespawn());
        }
    }

    private IEnumerator FadeAndDespawn()
    {
        Color color = _spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeDuration);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        _spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        LeanPool.Despawn(gameObject);
    }

    // ═════════════════════════════════════════════════════════════
    // IPoolable 구현
    // ═════════════════════════════════════════════════════════════

    public void OnSpawn()
    {
        // 알파값 1로 리셋
        Color color = _spriteRenderer.color;
        _spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);

        // 랜덤 스프라이트 선택
        if (_jellySprites != null && _jellySprites.Length > 0)
            _spriteRenderer.sprite = _jellySprites[Random.Range(0, _jellySprites.Length)];

        // 랜덤 크기
        float scale = Random.Range(_scaleMin, _scaleMax);
        transform.localScale = Vector3.one * scale;

        transform.rotation = Quaternion.identity;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _isFading = false;
    }

    public void OnDespawn()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }
}
