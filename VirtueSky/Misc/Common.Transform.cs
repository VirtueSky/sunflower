using System;
using UnityEngine;
using VirtueSky.Tween;

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

        public static void Shrug(this Transform transformObj, float time, float strength = .1f, EasingTypes easingTypes = EasingTypes.QuadraticOut,
            Action completed = null)
        {
            Coroutine coroutine = null;
            Vector3 baseScale = transformObj.localScale;
            Vector3 targetBounceX = new Vector3(1 + strength, 1 - strength) * baseScale.x;
            Vector3 targetBounceY = new Vector3(1 - strength, 1 + strength) * baseScale.y;
            coroutine = transformObj.ScaleTo(targetBounceX, time / 3, easingTypes, false, TweenRepeat.Once,
                () =>
                {
                    transformObj.ScaleTo(targetBounceY, time / 3, easingTypes, false, TweenRepeat.Once,
                        () =>
                        {
                            transformObj.ScaleTo(baseScale, time / 3, easingTypes, false, TweenRepeat.Once, () =>
                            {
                                if (coroutine != null)
                                {
                                    TweenManager.StopTween(coroutine);
                                }

                                completed?.Invoke();
                            });
                        });
                });
        }
    }

    // public static Camera CameraShake(this Camera camera, float _durationPosition, float _durationRotation, Vector3 _positionStrength,
    //     Vector3 _rotationStrength)
    // {
    //     camera.DOComplete();
    //     camera.DOShakePosition(_durationPosition, _positionStrength);
    //     camera.DOShakeRotation(_durationRotation, _rotationStrength);
    //     return camera;
    // }
}