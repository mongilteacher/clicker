using UnityEngine;

public class JellyDropFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        JellyDropSpawner.Instance.Spawn(transform.position);
    }
}
