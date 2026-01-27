using Lean.Pool;
using UnityEngine;

public class DamageFloaterSpawner : MonoBehaviour
{
    public static DamageFloaterSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _pool;
    

    private void Awake()
    {
        Instance = this;

        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void ShowDamage(ClickInfo clickInfo)
    {
        // 1. 풀로부터 DamageFloater를 가져와서
        GameObject floaterObject = _pool.Spawn(clickInfo.Position, Quaternion.identity);
        DamageFloater floater = floaterObject.GetComponent<DamageFloater>();
        
        // 2. 클릭한 위치에 생성한다.
        floater.Show(clickInfo.Damage);
    }
}
