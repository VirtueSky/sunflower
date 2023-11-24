using UnityEngine;
using PrimeTween;

namespace VirtueSky.Component
{
    public class EffectAppearComponent : MonoBehaviour
    {
        [Range(0, 2f)] public float TimeScale = .7f;
        public Ease ease = Ease.OutBack;
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
            Tween.Scale(transform, CurrentScale, TimeScale, ease);
        }
    }
}