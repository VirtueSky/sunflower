using DG.Tweening;
using UnityEngine;

public class EffectAppear : MonoBehaviour
{
    [Range(0, 2f)] public float TimeScale = .7f;
    public Ease EaseType;
    public Vector3 fromScale;
    private Vector3 CurrentScale;

    public void Awake()
    {
        CurrentScale = transform.localScale;
    }

    public void OnEnable()
    {
        transform.localScale = fromScale;
        DoEffect();
    }

    public void DoEffect()
    {
        if (!gameObject.activeInHierarchy) return;
        transform.DOScale(CurrentScale, TimeScale).SetEase(EaseType);
    }
}