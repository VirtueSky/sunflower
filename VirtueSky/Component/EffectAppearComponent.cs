using UnityEngine;
using VirtueSky.Tween;

namespace VirtueSky.Component
{
    public class EffectAppearComponent : MonoBehaviour
    {
        [Range(0, 2f)] public float TimeScale = .7f;
        public EasingTypes easingTypes;
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
            transform.ScaleTo(CurrentScale, TimeScale, easingTypes);
        }
    }
}