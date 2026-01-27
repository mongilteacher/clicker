using System.Collections;
using Lean.Pool;
using UnityEngine;

public class Effect : MonoBehaviour, IPoolable
{
    [SerializeField] private ParticleSystem _particle;

    public void Play()
    {
        _particle.Play();
        StartCoroutine(DespawnAfterFinish());
    }

    private IEnumerator DespawnAfterFinish()
    {
        yield return new WaitWhile(() => _particle.isPlaying);
        LeanPool.Despawn(gameObject);
    }

    public void OnSpawn()
    {
        _particle.Clear();
    }

    public void OnDespawn()
    {
        _particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
