using UnityEngine;
using PrimeTween;

namespace VirtueSky.Component
{
    public class EffectAppearComponent : MonoBehaviour
    {
        [Range(0, 2f)] public float TimeScale = .7f;
        public Ease ease = Ease.OutBack;
        public Vector3 fromScale = new Vector3(.5f, .5f, .5f);
        private Vector3 CurrentScale;
        private Tween _tween;

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
            _tween = Tween.Scale(transform, CurrentScale, TimeScale, ease).OnComplete(() => { _tween.Stop(); });
        }
    }
}