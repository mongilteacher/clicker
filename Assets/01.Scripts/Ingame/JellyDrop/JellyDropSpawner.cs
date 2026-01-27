using Lean.Pool;
using UnityEngine;

public class JellyDropSpawner : MonoBehaviour
{
    public static JellyDropSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _pool;
    [SerializeField] private int _spawnCountMin = 2;
    [SerializeField] private int _spawnCountMax = 5;
    [SerializeField] private int _feverSpawnMultiplier = 3;
    [SerializeField] private Vector2 _randomOffset;

    private void Awake()
    {
        Instance = this;

        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void Spawn(Vector2 ownerPosition)
    {
        int count = Random.Range(_spawnCountMin, _spawnCountMax + 1);
        if (FeverManager.Instance.IsFeverMode)
            count *= _feverSpawnMultiplier;
        for (int i = 0; i < count; i++)
        {
            Vector2 offset = new Vector2(
                Random.Range(-_randomOffset.x, _randomOffset.x),
                Random.Range(-_randomOffset.y, _randomOffset.y)
            );
            Vector2 spawnPosition = ownerPosition + offset;

            GameObject jellyObject = _pool.Spawn(spawnPosition, Quaternion.identity);
            if (jellyObject == null) return;
            
            JellyDrop jelly = jellyObject.GetComponent<JellyDrop>();

            jelly.Play();
        }
    }
}
