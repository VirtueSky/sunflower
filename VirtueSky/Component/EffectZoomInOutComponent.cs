﻿using PrimeTween;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;


namespace VirtueSky.Component
{
    [EditorIcon("icon_csharp")]
    public class EffectZoomInOutComponent : BaseMono
    {
        [Range(0, 2f)] public float timeDelay;
        [Range(.1f, 2f)] public float offsetScale = .1f;
        [Range(0, 2f)] public float timeScale = .7f;
        public Ease ease = Ease.Linear;
        private Vector3 currentScale;
        private Tween tween;

        public void Awake()
        {
            currentScale = transform.localScale;
        }

        public void OnEnable()
        {
            DoEffect(offsetScale, false);
        }

        private void OnDisable()
        {
            tween.Stop();
        }

        public void DoEffect(float offsetScale, bool delay)
        {
            if (!gameObject.activeInHierarchy) return;
            tween = Tween.Delay(timeDelay * (delay ? 1 : 0),
                () =>
                {
                    tween = transform.Scale(
                        new Vector3(currentScale.x + offsetScale, currentScale.y + offsetScale,
                            currentScale.z + offsetScale), timeScale, ease).OnComplete(() =>
                    {
                        DoEffect(-offsetScale, !delay);
                    });
                });
        }
    }
}