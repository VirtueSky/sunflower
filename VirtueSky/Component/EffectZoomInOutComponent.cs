using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace VirtueSky.Component
{
    public class EffectZoomInOutComponent : MonoBehaviour
    {
        public Vector3 CurrentScale;
        [Range(0, 2f)] public float TimeDelay;
        [Range(.1f, 2f)] public float SizeScale = .1f;
        [Range(0, 2f)] public float TimeScale = .7f;

        private Sequence _sequence;
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerCore;

        public void Awake()
        {
            CurrentScale = transform.localScale;
        }

        public void OnEnable()
        {
            transform.localScale = CurrentScale;
            DoEffect(SizeScale, false);
        }

        private void OnDisable()
        {
            _sequence?.Kill();
            _tweenerCore?.Kill();
        }

        public void DoEffect(float sizeScale, bool delay)
        {
            if (!gameObject.activeInHierarchy) return;
            _sequence = DOTween.Sequence().AppendInterval(TimeDelay * (delay ? 1 : 0)).AppendCallback(() =>
            {
                _tweenerCore = transform.DOScale(
                    new Vector3(transform.localScale.x + sizeScale, transform.localScale.y + sizeScale,
                        transform.localScale.z),
                    TimeScale).SetEase(Ease.Linear).OnComplete(() => { DoEffect(-sizeScale, !delay); });
            });
        }
    }
}