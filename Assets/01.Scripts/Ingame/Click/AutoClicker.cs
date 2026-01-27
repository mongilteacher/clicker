using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] private float _interval = 2f;
    [SerializeField] private DashAbility _dashAbility;

    private float _timer;

    private void Update()
    {
        if (_dashAbility.IsDashing)
            return;

        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            _timer = 0f;

            GameObject[] clickables = GameObject.FindGameObjectsWithTag("Clickable");
            if (clickables.Length > 0)
            {
                GameObject target = clickables[Random.Range(0, clickables.Length)];
                Clickable clickable = target.GetComponent<Clickable>();

                _dashAbility.Execute(target.transform, () =>
                {
                    ClickInfo clickInfo = new ClickInfo
                    {
                        Type = EClickType.Auto,
                        Damage = GameManager.Instance.AutoDamage,
                        Position = transform.position,
                    };
                    clickable.OnClick(clickInfo);
                });
            }
        }
    }
}
