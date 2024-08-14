// ReSharper disable AnnotateNotNullParameter
using UnityEngine;

namespace PrimeTween {
    internal static class Extensions {
        internal static float CalcDistance(Vector3 v1, Vector3 v2) => Vector3.Distance(v1, v2);
        internal static float CalcDistance(Quaternion q1, Quaternion q2) => Quaternion.Angle(q1, q2);

        internal static float calcDelta(this float val, ValueContainer prevVal) => val - prevVal.FloatVal;
        internal static double calcDelta(this double val, ValueContainer prevVal) => val - prevVal.DoubleVal;
        internal static Color calcDelta(this Color val, ValueContainer prevVal) => val - prevVal.ColorVal;
        internal static Vector2 calcDelta(this Vector2 val, ValueContainer prevVal) => val - prevVal.Vector2Val;
        internal static Vector3 calcDelta(this Vector3 val, ValueContainer prevVal) => val - prevVal.Vector3Val;
        internal static Vector4 calcDelta(this Vector4 val, ValueContainer prevVal) => val - prevVal.Vector4Val;
        internal static Quaternion calcDelta(this Quaternion val, ValueContainer prevVal) => Quaternion.Inverse(prevVal.QuaternionVal) * val;
        internal static Rect calcDelta(this Rect val, ValueContainer prevVal) => new Rect(
            val.x - prevVal.x,
            val.y - prevVal.y,
            val.width - prevVal.z,
            val.height - prevVal.w);

        internal static Color WithAlpha(this Color c, float alpha) {
            c.a = alpha;
            return c;
        }

        internal static ValueContainer ToContainer(this float f) => new ValueContainer { FloatVal = f };
        internal static ValueContainer ToContainer(this Vector2 v) => new ValueContainer { Vector2Val = v };
        internal static ValueContainer ToContainer(this Vector3 v) => new ValueContainer { Vector3Val = v };
        internal static ValueContainer ToContainer(this Vector4 v) => new ValueContainer { Vector4Val = v };
        internal static ValueContainer ToContainer(this Color c) => new ValueContainer { ColorVal = c };
        internal static ValueContainer ToContainer(this Quaternion q) => new ValueContainer { QuaternionVal = q };
        internal static ValueContainer ToContainer(this Rect r) => new ValueContainer { RectVal = r };
        internal static ValueContainer ToContainer(this double d) => new ValueContainer { DoubleVal = d };

        internal static Vector2 WithComponent(this Vector2 v, int index, float val) {
            v[index] = val;
            return v;
        }

        internal static Vector3 WithComponent(this Vector3 v, int index, float val) {
            v[index] = val;
            return v;
        }

        #if !UNITY_2019_1_OR_NEWER || UNITY_UGUI_INSTALLED
        internal static Vector2 GetFlexibleSize(this UnityEngine.UI.LayoutElement target) => new Vector2(target.flexibleWidth, target.flexibleHeight);
        internal static void SetFlexibleSize(this UnityEngine.UI.LayoutElement target, Vector2 vector2) {
            target.flexibleWidth = vector2.x;
            target.flexibleHeight = vector2.y;
        }

        internal static Vector2 GetMinSize(this UnityEngine.UI.LayoutElement target) => new Vector2(target.minWidth, target.minHeight);
        internal static void SetMinSize(this UnityEngine.UI.LayoutElement target, Vector2 vector2) {
            target.minWidth = vector2.x;
            target.minHeight = vector2.y;
        }

        internal static Vector2 GetPreferredSize(this UnityEngine.UI.LayoutElement target) => new Vector2(target.preferredWidth, target.preferredHeight);
        internal static void SetPreferredSize(this UnityEngine.UI.LayoutElement target, Vector2 vector2) {
            target.preferredWidth = vector2.x;
            target.preferredHeight = vector2.y;
        }

        internal static Vector2 GetNormalizedPosition(this UnityEngine.UI.ScrollRect target) => new Vector2(target.horizontalNormalizedPosition, target.verticalNormalizedPosition);
        internal static void SetNormalizedPosition(this UnityEngine.UI.ScrollRect target, Vector2 vector2) {
            target.horizontalNormalizedPosition = vector2.x;
            target.verticalNormalizedPosition = vector2.y;
        }
        #endif

        #if UI_ELEMENTS_MODULE_INSTALLED
        internal static Vector2 GetTopLeft(this UnityEngine.UIElements.VisualElement e) {
            var resolvedStyle = e.resolvedStyle;
            return new Vector2(resolvedStyle.left, resolvedStyle.top);
        }
        internal static void SetTopLeft(this UnityEngine.UIElements.VisualElement e, Vector2 c) {
            var style = e.style;
            style.left = c.x;
            style.top = c.y;
        }
        internal static Rect GetResolvedStyleRect(this UnityEngine.UIElements.VisualElement e) {
            var resolvedStyle = e.resolvedStyle;
            return new Rect(
                resolvedStyle.left,
                resolvedStyle.top,
                resolvedStyle.width,
                resolvedStyle.height
            );
        }
        internal static void SetStyleRect(this UnityEngine.UIElements.VisualElement e, Rect c) {
            var style = e.style;
            style.left = c.x;
            style.top = c.y;
            style.width = c.width;
            style.height = c.height;
        }
        #endif
    }
}
