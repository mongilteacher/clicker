using UnityEngine;

public class Godray : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Rotation")]
    [SerializeField] private float _rotateSpeed = 15f;

    [Header("Scale Pulse")]
    [SerializeField] private float _pulseSpeed = 1.5f;
    [SerializeField] private float _pulseAmount = 0.08f;

    [Header("Alpha Pulse")]
    [SerializeField] private float _alphaSpeed = 1f;
    [SerializeField] private float _alphaMin = 0.3f;
    [SerializeField] private float _alphaMax = 0.5f;

    private Vector3 _baseScale;
    private float _randomOffset;

    private void Awake()
    {
        _baseScale = transform.localScale;
        _randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float time = Time.time + _randomOffset;

        // 느린 회전
        transform.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);

        // 스케일 펄스
        float scaleFactor = 1f + Mathf.Sin(time * _pulseSpeed) * _pulseAmount;
        transform.localScale = _baseScale * scaleFactor;

        // 알파 펄스
        float alpha = Mathf.Lerp(_alphaMin, _alphaMax, (Mathf.Sin(time * _alphaSpeed) + 1f) * 0.5f);
        Color color = _spriteRenderer.color;
        _spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
    }
}
