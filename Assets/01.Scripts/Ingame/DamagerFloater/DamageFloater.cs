using System.Collections;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class DamageFloater : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    public void Show(ClickInfo clickInfo)
    {
        gameObject.SetActive(true);

        // 과제: 숫자 포맷팅(영어,한글)
        // 큰 숫자를 K, M, B, T, aa, ab... 단위로 축약해서 표기한다.
        // 1,000     -> 1k
        // 1,000,000 -> 1M
        // 1,600,000 -> 1.6M
        
        _text.text = clickInfo.Damage.ToString();
        
        
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
