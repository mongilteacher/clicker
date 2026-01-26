using DG.Tweening;
using UnityEngine;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private ClickTarget _owner;


    private void Awake()
    {
        _owner = GetComponent<ClickTarget>();
    }
    
    [Header("Squash & Stretch")]
    [SerializeField] private float _squashScaleX = 1.2f;
    [SerializeField] private float _squashScaleY = 0.85f;
    [SerializeField] private float _squashDuration = 0.1f;
    [SerializeField] private float _stretchScaleX = 0.9f;
    [SerializeField] private float _stretchScaleY = 1.15f;
    [SerializeField] private float _stretchDuration = 0.12f;
    [SerializeField] private float _recoverDuration = 0.15f;
    [SerializeField] private Ease _squashEase = Ease.OutQuad;
    [SerializeField] private Ease _stretchEase = Ease.OutQuad;
    [SerializeField] private Ease _recoverEase = Ease.OutBack;

    // 역할: 스케일 트위닝 피드백에 대한 로직을 담당 (squash & stretch)
    public void Play(ClickInfo clickInfo)
    {
        Transform t = _owner.transform;
        t.DOKill();

        // 1) 눌리는 느낌: 즉시 적용 (연속 클릭 시 끊김 방지)
        t.localScale = new Vector3(_squashScaleX, _squashScaleY, 1f);

        Sequence seq = DOTween.Sequence();
        // 2) 튀어오르는 느낌: X 좁아지고 Y 늘어남
        seq.Append(t.DOScale(new Vector3(_stretchScaleX, _stretchScaleY, 1f), _stretchDuration).SetEase(_stretchEase));
        // 3) 원래 크기로 복귀
        seq.Append(t.DOScale(Vector3.one, _recoverDuration).SetEase(_recoverEase));
        seq.SetTarget(t);
    }
    
}
