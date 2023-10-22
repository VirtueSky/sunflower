using System;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static bool IsInteger(float value)
        {
            return (value == (int)value);
        }

        public static int GetNumberInAString(string str)
        {
            try
            {
                var getNumb = Regex.Match(str, @"\d+").Value;
                return Int32.Parse(getNumb);
            }
            catch (Exception e)
            {
                return -1;
            }

            return -1;
        }

        public static void ClearTransform(this Transform transform)
        {
            var childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject, true);
            }
        }

        public static float GetScreenRatio()
        {
            return (1920f / 1080f) / (Screen.height / (float)Screen.width);
        }

        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
        {
            Transform t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
            }

            return null;
        }

        public static void SetLayerForAllChildObject(GameObject obj, int layerIndex)
        {
            obj.layer = layerIndex;
            obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.layer = layerIndex; });
        }

        public static void SetTagForAllChildObject(GameObject obj, string tag)
        {
            obj.tag = tag;
            obj.GetComponentsInChildren<Transform>().ToList().ForEach(x => { x.gameObject.tag = tag; });
        }

        public static void Bounce(this Transform transformObj, float time, float strength = .1f,
            Action completed = null)
        {
            Vector3 baseScale = transformObj.localScale;
            Vector3 targetBounceX = new Vector3(1 + strength, 1 - strength) * baseScale.x;
            Vector3 targetBounceY = new Vector3(1 - strength, 1 + strength) * baseScale.y;
            transformObj.DOScale(targetBounceX, time / 3).OnComplete(() =>
            {
                transformObj.DOScale(targetBounceY, time / 3).OnComplete(() =>
                {
                    transformObj.DOScale(baseScale, time / 3).OnComplete(() => { completed?.Invoke(); });
                });
            });
        }

        public static void CallActionAndClean(ref Action action)
        {
            if (action == null) return;
            var a = action;
            a();
            action = null;
        }
    }
}