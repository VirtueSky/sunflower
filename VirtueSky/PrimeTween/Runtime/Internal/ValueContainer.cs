using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tooltip = UnityEngine.TooltipAttribute;
using SerializeField = UnityEngine.SerializeField;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;
using Quaternion = UnityEngine.Quaternion;
using Color = UnityEngine.Color;
using Rect = UnityEngine.Rect;

namespace PrimeTween {
    [Serializable]
    internal struct ValueContainerStartEnd {
        [SerializeField] internal TweenType tweenType; // todo HideInInspector?
        [SerializeField, Tooltip(Constants.startFromCurrentTooltip)] internal bool startFromCurrent;
        [SerializeField, Tooltip(Constants.startValueTooltip)] internal ValueContainer startValue;
        [SerializeField, Tooltip(Constants.endValueTooltip)] internal ValueContainer endValue;
    }

    [Serializable, StructLayout(LayoutKind.Explicit)]
    internal struct ValueContainer {
        // todo check if it was possible to modify ValueContainer in Debug Inspector before
        [FieldOffset(sizeof(float) * 0), SerializeField] internal float x;
        [FieldOffset(sizeof(float) * 1), SerializeField] internal float y;
        [FieldOffset(sizeof(float) * 2), SerializeField] internal float z;
        [FieldOffset(sizeof(float) * 3), SerializeField] internal float w;
        [FieldOffset(0), NonSerialized] internal float FloatVal;
        [FieldOffset(0), NonSerialized] internal Color ColorVal;
        [FieldOffset(0), NonSerialized] internal Vector2 Vector2Val;
        [FieldOffset(0), NonSerialized] internal Vector3 Vector3Val;
        [FieldOffset(0), NonSerialized] internal Vector4 Vector4Val;
        [FieldOffset(0), NonSerialized] internal Quaternion QuaternionVal;
        [FieldOffset(0), NonSerialized] internal Rect RectVal;
        [FieldOffset(0), NonSerialized] internal double DoubleVal;
        [FieldOffset(0), NonSerialized] internal Vector3f Vector3fVal;

        internal static ValueContainer Create(float x, float y = 0f, float z = 0f, float w = 0f) {
            return new ValueContainer {
                x = x,
                y = y,
                z = z,
                w = w
            };
        }

        internal void CopyFrom(ref float val) {
            x = val;
            y = 0f;
            z = 0f;
            w = 0f;
        }

        internal void CopyFrom(ref Color val) => ColorVal = val;

        internal void CopyFrom(ref Vector2 val) {
            Vector2Val = val;
            z = 0f;
            w = 0f;
        }

        internal void CopyFrom(ref Vector3 val) {
            Vector3Val = val;
            w = 0f;
        }

        internal void CopyFrom(ref Vector4 val) => Vector4Val = val;
        internal void CopyFrom(ref Rect val) => RectVal = val;
        internal void CopyFrom(ref Quaternion val) => QuaternionVal = val;

        internal void CopyFrom(ref double val) {
            DoubleVal = val;
            z = 0f;
            w = 0f;
        }

        internal void Reset() => x = y = z = w = 0f;

        internal float this[int i] {
            get {
                switch (i) {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (i) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float QuaternionAngle(ValueContainer a, ValueContainer b) {
            float num = Mathf.Min(Mathf.Abs(QuaternionDot(a, b)), 1f);
            return QuaternionIsEqualUsingDot(num) ? 0.0f : (float)(Mathf.Acos(num) * 2.0 * 57.295780181884766);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool QuaternionIsEqualUsingDot(float dot) => dot > 0.9999989867210388;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float QuaternionDot(ValueContainer a, ValueContainer b) {
            return (float)((double)a.x * (double)b.x + (double)a.y * (double)b.y + (double)a.z * (double)b.z + (double)a.w * (double)b.w);
        }

        internal void QuaternionNormalize() {
            if (Mathf.Approximately(w, 0f)) {
                w = 1f;
            }
            float magnitudeSquared = Vector4Dot(this, this);
            float invNorm = 1.0f / Mathf.Sqrt(magnitudeSquared);
            x *= invNorm;
            y *= invNorm;
            z *= invNorm;
            w *= invNorm;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal float Vector4Magnitude() => Mathf.Sqrt(Vector4Dot(this, this));

        static float Vector4Dot(ValueContainer a, ValueContainer b) {
            return (float) ((double) a.x * (double) b.x + (double) a.y * (double) b.y + (double) a.z * (double) b.z + (double) a.w * (double) b.w);
        }

        public override string ToString() => Vector4Val.ToString();
    }
}
