using System;
using PrimeTween;
using UnityEngine;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static Transform ClearTransform(this Transform transform)
        {
            var childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject, true);
            }

            return transform;
        }

        public static void Shrug(this Transform transformObj, float time, float strength = .1f, Ease easingTypes = Ease.OutQuad,
            Action completed = null)
        {
            Tween tween = default;
            Vector3 baseScale = transformObj.localScale;
            Vector3 targetBounceX = new Vector3(1 + strength, 1 - strength) * baseScale.x;
            Vector3 targetBounceY = new Vector3(1 - strength, 1 + strength) * baseScale.y;
            tween = Tween.Scale(transformObj, targetBounceX, time / 3, easingTypes).OnComplete(() =>
            {
                Tween.Scale(transformObj, targetBounceY, time / 3, easingTypes).OnComplete(() =>
                {
                    Tween.Scale(transformObj, baseScale, time / 3, easingTypes).OnComplete(() =>
                    {
                        tween.Stop();
                        completed?.Invoke();
                    });
                });
            });
        }

        public static Camera CameraShake(this Camera camera, float strengthFactor = 1.0f, float duration = 0.5f, int frequency = 10)
        {
            Tween.ShakeCamera(camera, strengthFactor, duration, frequency);
            return camera;
        }
    }
}