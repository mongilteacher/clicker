using Lean.Pool;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _manualPool;
    [SerializeField] private LeanGameObjectPool _autoPool;
    [SerializeField] private Vector2 _randomOffset;

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn(ClickInfo clickInfo)
    {
        LeanGameObjectPool pool = clickInfo.Type == EClickType.Manual ? _manualPool : _autoPool;

        Vector2 offset = new Vector2(
            Random.Range(-_randomOffset.x, _randomOffset.x),
            Random.Range(-_randomOffset.y, _randomOffset.y)
        );
        Vector2 spawnPosition = clickInfo.Position + offset;

        GameObject effectObject = pool.Spawn(spawnPosition, Quaternion.identity);
        Effect effect = effectObject.GetComponent<Effect>();

        effect.Play();
    }
}
