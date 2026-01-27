using UnityEngine;

public class FloatingAbility : MonoBehaviour
{
    [Header("Vertical")]
    [SerializeField] private float _bobSpeed = 1.5f;
    [SerializeField] private float _bobAmount = 0.3f;

    [Header("Horizontal")]
    [SerializeField] private float _swaySpeed = 0.8f;
    [SerializeField] private float _swayAmount = 0.15f;

    private Vector3 _origin;
    private float _randomOffset;

    private void Awake()
    {
        _origin = transform.localPosition;
        _randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float time = Time.time + _randomOffset;

        float offsetY = Mathf.Sin(time * _bobSpeed) * _bobAmount;
        float offsetX = Mathf.Sin(time * _swaySpeed) * _swayAmount;

        transform.localPosition = _origin + new Vector3(offsetX, offsetY, 0f);
    }
}
