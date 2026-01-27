using UnityEngine;

public class FeverFlameEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _flameRenderer;

    [Header("Shader")]
    [SerializeField] private float _flameWidth = 0.08f;
    [SerializeField] private float _intensity = 2f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _noiseScale = 10f;
    [SerializeField] private float _rainbowSpeed = 1f;

    private Material _material;

    private static readonly int PropFlameWidth = Shader.PropertyToID("_FlameWidth");
    private static readonly int PropIntensity = Shader.PropertyToID("_Intensity");
    private static readonly int PropSpeed = Shader.PropertyToID("_Speed");
    private static readonly int PropNoiseScale = Shader.PropertyToID("_NoiseScale");
    private static readonly int PropRainbowSpeed = Shader.PropertyToID("_RainbowSpeed");

    private void Start()
    {
        _material = new Material(Shader.Find("Custom/FeverFlame"));
        _flameRenderer.material = _material;

        ApplySettings();

        _flameRenderer.enabled = false;

        FeverManager.OnFeverModeChanged += Refresh;
    }

    private void OnDestroy()
    {
        FeverManager.OnFeverModeChanged -= Refresh;
        if (_material != null) Destroy(_material);
    }

    private void Refresh()
    {
        _flameRenderer.enabled = FeverManager.Instance.IsFeverMode;
    }

    private void ApplySettings()
    {
        _material.SetFloat(PropFlameWidth, _flameWidth);
        _material.SetFloat(PropIntensity, _intensity);
        _material.SetFloat(PropSpeed, _speed);
        _material.SetFloat(PropNoiseScale, _noiseScale);
        _material.SetFloat(PropRainbowSpeed, _rainbowSpeed);
    }
}
