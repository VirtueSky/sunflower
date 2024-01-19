using System;
using JetBrains.Annotations;

namespace PrimeTween
{
    public static class TweenStatic
    {
        // target, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime
        // target, startValue, endValue, duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime

        #region EulerAngles

        public static Tween EulerAngles([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.EulerAngles(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalEulerAngles([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalEulerAngles(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Position

        public static Tween Position([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Position(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween Position([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.Position(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween PositionAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 endValue, float averageSpeed, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionAtSpeed(target, endValue, averageSpeed, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween PositionAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float averageSpeed,
            Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionAtSpeed(target, startValue, endValue, averageSpeed, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        // X
        public static Tween PositionX([NotNull] this UnityEngine.Transform target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionX(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween PositionX([NotNull] this UnityEngine.Transform target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionX(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        // Y
        public static Tween PositionY([NotNull] this UnityEngine.Transform target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionY(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween PositionY([NotNull] this UnityEngine.Transform target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionY(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        // Z
        public static Tween PositionZ([NotNull] this UnityEngine.Transform target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionZ(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween PositionZ([NotNull] this UnityEngine.Transform target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.PositionZ(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Local Position

        public static Tween LocalPosition([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPosition(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalPosition([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.LocalPosition(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalPositionAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 endValue, float averageSpeed, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPositionAtSpeed(target, endValue, averageSpeed, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalPositionAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float averageSpeed,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.LocalPositionAtSpeed(target, startValue, endValue, averageSpeed, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        // X
        public static Tween LocalPositionX([NotNull] this UnityEngine.Transform target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPositionX(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalPositionX([NotNull] this UnityEngine.Transform target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPositionX(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        // Y
        public static Tween LocalPositionY([NotNull] this UnityEngine.Transform target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPositionY(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalPositionY([NotNull] this UnityEngine.Transform target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPositionY(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        // Z
        public static Tween LocalPositionZ([NotNull] this UnityEngine.Transform target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPositionZ(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalPositionZ([NotNull] this UnityEngine.Transform target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalPositionZ(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Rotation

        public static Tween Rotation([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Rotation(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween Rotation([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.Rotation(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween RotationAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Quaternion endValue, float averageAngularSpeed, Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.RotationAtSpeed(target, endValue, averageAngularSpeed, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween RotationAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Quaternion startValue, UnityEngine.Quaternion endValue,
            float averageAngularSpeed,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.RotationAtSpeed(target, startValue, endValue, averageAngularSpeed, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Local Rotation

        public static Tween LocalRotation([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalRotation(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalRotation([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.LocalRotation(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalRotationAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Quaternion endValue, float averageAngularSpeed, Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.LocalRotationAtSpeed(target, endValue, averageAngularSpeed, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween LocalRotationAtSpeed([NotNull] this UnityEngine.Transform target,
            UnityEngine.Quaternion startValue, UnityEngine.Quaternion endValue,
            float averageAngularSpeed, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.LocalRotationAtSpeed(target, startValue, endValue, averageAngularSpeed,
                ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Scale

        public static Tween Scale([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.Scale(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Scale([NotNull] this UnityEngine.Transform target,
            UnityEngine.Vector3 endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Scale(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween ScaleX([NotNull] this UnityEngine.Transform target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.ScaleX(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween ScaleX([NotNull] this UnityEngine.Transform target, Single startValue,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.ScaleX(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween ScaleY([NotNull] this UnityEngine.Transform target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.ScaleY(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween ScaleY([NotNull] this UnityEngine.Transform target, Single startValue,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.ScaleY(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween ScaleZ([NotNull] this UnityEngine.Transform target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.ScaleZ(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween ScaleZ([NotNull] this UnityEngine.Transform target, Single startValue,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.ScaleZ(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Color SpriteRenderer

        public static Tween Color([NotNull] this UnityEngine.SpriteRenderer target,
            UnityEngine.Color endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Color(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween Color([NotNull] this UnityEngine.SpriteRenderer target,
            UnityEngine.Color startValue, UnityEngine.Color endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.Color(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Alpha([NotNull] this UnityEngine.SpriteRenderer target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Alpha(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween Alpha([NotNull] this UnityEngine.SpriteRenderer target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Alpha(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Camera

        public static Tween CameraOrthographicSize([NotNull] this UnityEngine.Camera target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraOrthographicSize(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraOrthographicSize([NotNull] this UnityEngine.Camera target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.CameraOrthographicSize(target, startValue, endValue, duration, ease,
                cycles, cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraBackgroundColor([NotNull] this UnityEngine.Camera target,
            UnityEngine.Color endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraBackgroundColor(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraBackgroundColor([NotNull] this UnityEngine.Camera target,
            UnityEngine.Color startValue, UnityEngine.Color endValue, float duration,
            Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraBackgroundColor(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraAspect([NotNull] this UnityEngine.Camera target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraAspect(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraAspect([NotNull] this UnityEngine.Camera target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraAspect(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraFarClipPlane([NotNull] this UnityEngine.Camera target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraFarClipPlane(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraFarClipPlane([NotNull] this UnityEngine.Camera target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraFarClipPlane(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraFieldOfView([NotNull] this UnityEngine.Camera target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraFieldOfView(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraFieldOfView([NotNull] this UnityEngine.Camera target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraFieldOfView(target, startDelay, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraNearClipPlane([NotNull] this UnityEngine.Camera target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraNearClipPlane(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween CameraNearClipPlane([NotNull] this UnityEngine.Camera target,
            Single startvalue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.CameraNearClipPlane(target, startvalue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Color UI Graphic

        public static Tween Color([NotNull] this UnityEngine.UI.Graphic target,
            UnityEngine.Color endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Color(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween Color([NotNull] this UnityEngine.UI.Graphic target,
            UnityEngine.Color startValue, UnityEngine.Color endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.Color(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Alpha([NotNull] this UnityEngine.UI.Graphic target, Single endValue,
            float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Alpha(target, endValue, duration, ease, cycles, cycleMode, startDelay,
                endDelay, useUnscaledTime);
        }

        public static Tween Alpha([NotNull] this UnityEngine.UI.Graphic target, Single startValue,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.Alpha(target, startValue, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region UI Image

        public static Tween UIFillAmount([NotNull] this UnityEngine.UI.Image target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.UIFillAmount(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween UIFillAmount([NotNull] this UnityEngine.UI.Image target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.UIFillAmount(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Rigidbody

        public static Tween RigidbodyMovePosition([NotNull] this UnityEngine.Rigidbody target,
            UnityEngine.Vector3 endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMovePosition(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween RigidbodyMovePosition([NotNull] this UnityEngine.Rigidbody target,
            UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMovePosition(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween RigidbodyMoveRotation([NotNull] this UnityEngine.Rigidbody target,
            UnityEngine.Quaternion endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMoveRotation(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween RigidbodyMoveRotation([NotNull] this UnityEngine.Rigidbody target,
            UnityEngine.Quaternion startValue, UnityEngine.Quaternion endValue, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMoveRotation(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Rigidbody2D

        public static Tween Rigidbody2DMovePosition([NotNull] this UnityEngine.Rigidbody2D target,
            UnityEngine.Vector2 endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMovePosition(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Rigidbody2DMovePosition([NotNull] this UnityEngine.Rigidbody2D target,
            UnityEngine.Vector2 startValue, UnityEngine.Vector2 endValue, float duration,
            Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMovePosition(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Rigidbody2DMoveRotation([NotNull] this UnityEngine.Rigidbody2D target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMoveRotation(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween Rigidbody2DMoveRotation([NotNull] this UnityEngine.Rigidbody2D target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.RigidbodyMoveRotation(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion

        #region Material

        public static Tween MaterialColor([NotNull] this UnityEngine.Material target,
            UnityEngine.Color endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.MaterialColor(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween MaterialColor([NotNull] this UnityEngine.Material target,
            UnityEngine.Color startValue, UnityEngine.Color endValue, float duration,
            Ease ease = Ease.Default,
            int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0,
            float endDelay = 0, bool useUnscaledTime = false)
        {
            return Tween.MaterialColor(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        public static Tween MaterialAlpha([NotNull] this UnityEngine.Material target,
            Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.MaterialAlpha(target, endValue, duration, ease, cycles, cycleMode,
                startDelay, endDelay, useUnscaledTime);
        }

        public static Tween MaterialAlpha([NotNull] this UnityEngine.Material target,
            Single startValue, Single endValue, float duration, Ease ease = Ease.Default,
            int cycles = 1,
            CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0,
            bool useUnscaledTime = false)
        {
            return Tween.MaterialAlpha(target, startValue, endValue, duration, ease, cycles,
                cycleMode, startDelay, endDelay, useUnscaledTime);
        }

        #endregion
    }
}