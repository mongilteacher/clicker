using System;
using System.Collections;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private float _dashSpeed = 30f;
    [SerializeField] private float _returnSpeed = 15f;
    [SerializeField] private float _hitDistanceMin = 0.2f;
    [SerializeField] private float _hitDistanceMax = 0.5f;

    private Vector3 _origin;
    private FloatingAbility _floatingAbility;

    public bool IsDashing { get; private set; }

    private void Start()
    {
        _origin = transform.position;
        _floatingAbility = GetComponent<FloatingAbility>();
    }

    public void Execute(Transform target, Action onHit)
    {
        if (IsDashing)
            return;

        StartCoroutine(DashRoutine(target, onHit));
    }

    private IEnumerator DashRoutine(Transform target, Action onHit)
    {
        IsDashing = true;
        if (_floatingAbility != null)
            _floatingAbility.enabled = false;

        float hitDistance = UnityEngine.Random.Range(_hitDistanceMin, _hitDistanceMax);

        // 타겟을 향해 돌진
        while (target != null && Vector3.Distance(transform.position, target.position) > hitDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target.position, _dashSpeed * Time.deltaTime);
            yield return null;
        }

        // 도착 → 콜백 실행
        onHit?.Invoke();

        // 원래 자리로 복귀
        while (Vector3.Distance(transform.position, _origin) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _origin, _returnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = _origin;
        if (_floatingAbility != null)
            _floatingAbility.enabled = true;
        IsDashing = false;
    }
}
