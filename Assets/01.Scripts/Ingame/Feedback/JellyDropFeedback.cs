using UnityEngine;

public class JellyDropFeedback : MonoBehaviour, IFeedback
{
    public Vector3 Offset = new Vector2(0, 1.5f);
    public void Play(ClickInfo clickInfo)
    {
        JellyDropSpawner.Instance.Spawn(transform.position + Offset);
    }
}
