using System.Collections;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class DamageFloater : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    public void Show(int damage)
    {
        gameObject.SetActive(true);

        _text.text = damage.ToString();
        
        // Dotween을 이용한 애니메이션 효과
        
        StartCoroutine(Show_Coroutine());
    }

    private IEnumerator Show_Coroutine()
    {
        yield return new WaitForSeconds(0.8f);

        //gameObject.SetActive(false);
        LeanPool.Despawn(gameObject);
    }
}
