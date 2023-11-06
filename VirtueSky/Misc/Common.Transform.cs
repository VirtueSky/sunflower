using System;
using DG.Tweening;
using UnityEngine;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static void ClearTransform(this Transform transform)
        {
            var childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject, true);
            }
        }

        public static void Shrug(this Transform transformObj, float time, float strength = .1f,
            Action completed = null)
        {
            Vector3 baseScale = transformObj.localScale;
            Vector3 targetBounceX = new Vector3(1 + strength, 1 - strength) * baseScale.x;
            Vector3 targetBounceY = new Vector3(1 - strength, 1 + strength) * baseScale.y;
            transformObj.DOScale(targetBounceX, time / 3).OnComplete(() =>
            {
                transformObj.DOScale(targetBounceY, time / 3).OnComplete(() => { transformObj.DOScale(baseScale, time / 3).OnComplete(() => { completed?.Invoke(); }); });
            });
        }
    }
}