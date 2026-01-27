using Lean.Pool;
using UnityEngine;

public class FeverMeteor : MonoBehaviour, IPoolable
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
    [Header("Movement")]
    [SerializeField] private float _fallSpeed = 12f;
    [SerializeField] private float _horizontalSpeedRange = 3f;
    [SerializeField] private float _gravityScale = 1.5f;

    [Header("Rotation")]
    [SerializeField] private float _angularVelocityMin = 90f;
    [SerializeField] private float _angularVelocityMax = 360f;

    [Header("Scale")]
    [SerializeField] private float _scaleMin = 0.6f;
    [SerializeField] private float _scaleMax = 1.3f;

    [Header("Despawn")]
    [SerializeField] private float _despawnY = -7f;

    // ═════════════════════════════════════════════════════════════
    // 공개 API
    // ═════════════════════════════════════════════════════════════

    public void Play()
    {
        // 왼쪽 아래 방향으로 힘 적용
        Vector2 force = new Vector2(
            -Random.Range(_horizontalSpeedRange * 0.5f, _horizontalSpeedRange),
            -_fallSpeed
        );
        _rigidbody.AddForce(force, ForceMode2D.Impulse);

        // 랜덤 회전
        _rigidbody.angularVelocity = Random.Range(_angularVelocityMin, _angularVelocityMax)
                                     * (Random.value > 0.5f ? 1f : -1f);
    }

    // ═════════════════════════════════════════════════════════════
    // 라이프사이클
    // ═════════════════════════════════════════════════════════════

    private void Update()
    {
        // 화면 아래로 벗어나면 디스폰
        if (transform.position.y < _despawnY)
        {
            LeanPool.Despawn(gameObject);
        }
    }

    // ═════════════════════════════════════════════════════════════
    // IPoolable 구현
    // ═════════════════════════════════════════════════════════════

    public void OnSpawn()
    {
        // 알파값 리셋
        Color color = _spriteRenderer.color;
        _spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);

        // 랜덤 스프라이트 선택
        if (_jellySprites != null && _jellySprites.Length > 0)
            _spriteRenderer.sprite = _jellySprites[Random.Range(0, _jellySprites.Length)];

        // 랜덤 크기
        float scale = Random.Range(_scaleMin, _scaleMax);
        transform.localScale = Vector3.one * scale;

        // 물리 리셋
        transform.rotation = Quaternion.identity;
        _rigidbody.gravityScale = _gravityScale;
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }

    public void OnDespawn()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }
}
