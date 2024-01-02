using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PrimeTween {
    [Serializable, StructLayout(LayoutKind.Explicit)]
    [SuppressMessage("ReSharper", "Unity.RedundantHideInInspectorAttribute")]
    internal struct ValueContainer {
        [FieldOffset(sizeof(float) * 0), SerializeField] internal float x;
        [FieldOffset(sizeof(float) * 1), SerializeField] internal float y;
        [FieldOffset(sizeof(float) * 2), SerializeField] internal float z;
        [FieldOffset(sizeof(float) * 3), SerializeField] internal float w;
        [FieldOffset(0), HideInInspector] internal float FloatVal;
        [FieldOffset(0), HideInInspector] internal Color ColorVal;
        [FieldOffset(0), HideInInspector] internal Vector2 Vector2Val;
        [FieldOffset(0), HideInInspector] internal Vector3 Vector3Val;
        [FieldOffset(0), HideInInspector] internal Vector4 Vector4Val;
        [FieldOffset(0), HideInInspector] internal Quaternion QuaternionVal;
        [FieldOffset(0), HideInInspector] internal Rect RectVal;

        internal void CopyFrom(ref float val) {
            x = val;
            y = 0f;
            z = 0f;
            w = 0f;
        }

        internal void CopyFrom(ref Color val) {
            x = val.r;
            y = val.g;
            z = val.b;
            w = val.a;
        }

        internal void CopyFrom(ref Vector2 val) {
            x = val.x;
            y = val.y;
            z = 0f;
            w = 0f;
        }

        internal void CopyFrom(ref Vector3 val) {
            x = val.x;
            y = val.y;
            z = val.z;
            w = 0f;
        }

        internal void CopyFrom(ref Vector4 val) {
            x = val.x;
            y = val.y;
            z = val.z;
            w = val.w;
        }

        internal void CopyFrom(ref Rect val) {
            x = val.x;
            y = val.y;
            z = val.width;
            w = val.height;
        }

        internal void CopyFrom(ref Quaternion val) {
            x = val.x;
            y = val.y;
            z = val.z;
            w = val.w;
        }

        internal void Reset() {
            x = y = z = w = 0f;
        }
    }
}