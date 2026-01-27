using System.Collections;
using Lean.Pool;
using UnityEngine;

public class FeverMeteorSpawner : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────
    // 풀 & 스폰 설정
    // ─────────────────────────────────────────────────────────────
    [SerializeField] private LeanGameObjectPool _pool;

    [Header("Spawn")]
    [SerializeField] private float _spawnInterval = 0.15f;
    [SerializeField] private int _spawnPerTick = 1;

    [Header("Spawn Area")]
    [SerializeField] private float _spawnY = 7f;
    [SerializeField] private float _spawnXRange = 5f;

    // ─────────────────────────────────────────────────────────────
    // 내부 변수
    // ─────────────────────────────────────────────────────────────
    private Coroutine _spawnCoroutine;

    // ═════════════════════════════════════════════════════════════
    // 라이프사이클
    // ═════════════════════════════════════════════════════════════

    private void Start()
    {
        FeverManager.OnFeverModeChanged += Refresh;
    }

    private void OnDestroy()
    {
        FeverManager.OnFeverModeChanged -= Refresh;
    }

    // ═════════════════════════════════════════════════════════════
    // 피버 모드 연동
    // ═════════════════════════════════════════════════════════════

    private void Refresh()
    {
        if (FeverManager.Instance.IsFeverMode)
        {
            if (_spawnCoroutine == null)
                _spawnCoroutine = StartCoroutine(SpawnLoop());
        }
        else
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
    }

    // ═════════════════════════════════════════════════════════════
    // 스폰 루프
    // ═════════════════════════════════════════════════════════════

    private IEnumerator SpawnLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnInterval);

        while (true)
        {
            for (int i = 0; i < _spawnPerTick; i++)
            {
                SpawnOne();
            }

            yield return wait;
        }
    }

    private void SpawnOne()
    {
        Vector2 position = new Vector2(
            Random.Range(-_spawnXRange, _spawnXRange),
            _spawnY
        );

        GameObject meteorObject = _pool.Spawn(position, Quaternion.identity);
        FeverMeteor meteor = meteorObject.GetComponent<FeverMeteor>();

        meteor.Play();
    }
}
