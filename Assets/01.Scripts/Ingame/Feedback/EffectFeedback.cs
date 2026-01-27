using UnityEngine;

public class EffectFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        EffectSpawner.Instance.Spawn(clickInfo);
    }
}
