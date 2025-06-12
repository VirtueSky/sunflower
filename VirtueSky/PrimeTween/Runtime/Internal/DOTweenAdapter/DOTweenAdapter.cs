// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
#if PRIME_TWEEN_DOTWEEN_ADAPTER
using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    public static partial class DOTweenAdapter {
        static int remapFrequency(float frequency) {
            return (int) (frequency * 1.35f);
        }

        public static Tween DOShakePosition([NotNull] this Component target, float duration, float strength, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true)
            => DOShakePosition(target, duration, Vector3.one * strength, vibrato, randomness, snapping, fadeOut);
        public static Tween DOShakePosition([NotNull] this Component target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true) {
            if (Math.Abs(randomness - 90f) > 0.001f) {
                Debug.LogWarning("PrimeTween doesn't support " + nameof(randomness));
            }
            if (snapping) {
                Debug.LogWarning("PrimeTween doesn't support " + nameof(snapping));
            }
            var settings = new ShakeSettings(strength, duration, vibrato);
            if (fadeOut) {
                settings.enableFalloff = true;
                settings.frequency = remapFrequency(settings.frequency);
            }
            return Tween.ShakeLocalPosition(target.transform, settings);
        }
        public static Tween DOPunchPosition([NotNull] this Component target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1, bool snapping = false) {
            if (snapping) {
                Debug.LogWarning("PrimeTween doesn't support " + nameof(snapping));
            }
            var shakeSettings = new ShakeSettings(punch, duration, remapFrequency(vibrato), asymmetryFactor: 1 - elasticity);
            return Tween.PunchLocalPosition(target.transform, shakeSettings);
        }

        public static Tween DOShakeRotation([NotNull] this Component target, float duration, float strength, int vibrato = 10, float randomness = 90, bool fadeOut = true)
            => DOShakeRotation(target, duration, Vector3.one * strength, vibrato, randomness, fadeOut);
        public static Tween DOShakeRotation([NotNull] this Component target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90, bool fadeOut = true) {
            if (Math.Abs(randomness - 90f) > 0.001f) {
                Debug.LogWarning("PrimeTween doesn't support " + nameof(randomness));
            }
            var settings = new ShakeSettings(strength, duration, vibrato);
            if (fadeOut) {
                settings.enableFalloff = true;
                settings.frequency = remapFrequency(settings.frequency);
            }
            return Tween.ShakeLocalRotation(target.transform, settings);
        }
        public static Tween DOPunchRotation([NotNull] this Component target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1) {
            var shakeSettings = new ShakeSettings(punch, duration, remapFrequency(vibrato), asymmetryFactor: 1 - elasticity);
            return Tween.PunchLocalRotation(target.transform, shakeSettings);
        }

        public static Tween DOShakeScale([NotNull] this Component target, float duration, float strength, int vibrato = 10, float randomness = 90, bool fadeOut = true)
            => DOShakeScale(target, duration, Vector3.one * strength, vibrato, randomness, fadeOut);
        public static Tween DOShakeScale([NotNull] this Component target, float duration, Vector3 strength, int vibrato = 10, float randomness = 90, bool fadeOut = true) {
            if (Math.Abs(randomness - 90f) > 0.001f) {
                Debug.LogWarning("PrimeTween doesn't support " + nameof(randomness));
            }
            var settings = new ShakeSettings(strength, duration, vibrato);
            if (fadeOut) {
                settings.enableFalloff = true;
                settings.frequency = remapFrequency(settings.frequency);
            }
            return Tween.ShakeScale(target.transform, settings);
        }
        public static Tween DOPunchScale([NotNull] this Component target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1) {
            var shakeSettings = new ShakeSettings(punch, duration, remapFrequency(vibrato), asymmetryFactor: 1 - elasticity);
            return Tween.PunchScale(target.transform, shakeSettings);
        }

        public static Tween DORotate([NotNull] this Transform target, Vector3 endValue, float duration) {
            return Tween.Rotation(target, Quaternion.Euler(endValue), duration);
        }

        public static Tween DOLocalRotate([NotNull] this Transform target, Vector3 endValue, float duration) {
            return Tween.LocalRotation(target, Quaternion.Euler(endValue), duration);
        }

        public static Tween DOScale([NotNull] this Transform target, Single endValue, float duration) {
            return Tween.Scale(target, endValue, duration);
        }

        public static int DOKill([NotNull] this Component target, bool complete = false) => doKill_internal(target, complete);
        public static int DOKill([NotNull] this Material target, bool complete = false) => doKill_internal(target, complete);

        internal static int doKill_internal([CanBeNull] object target, bool complete = false) {
            bool prevLogCantManipulateError = PrimeTweenManager.logCantManipulateError;
            PrimeTweenManager.logCantManipulateError = false;
            var result = complete ? Tween.CompleteAll(target) : Tween.StopAll(target);
            PrimeTweenManager.logCantManipulateError = prevLogCantManipulateError;
            return result;
        }

        internal static Easing GetEasing(Ease ease, float? maybeStrength, float? maybePeriod) {
            var strength = maybeStrength ?? 1;
            switch (ease) {
                case Ease.OutBack:
                    if (maybePeriod.HasValue) {
                        Debug.LogWarning("Ease.OutBack doesn't support custom period.");
                    }
                    return Easing.Overshoot(strength / StandardEasing.backEaseConst);
                case Ease.OutBounce:
                    return Easing.Bounce(strength);
                case Ease.OutElastic:
                    return Easing.Elastic(strength, maybePeriod ?? StandardEasing.defaultElasticEasePeriod);
            }
            return Easing.Standard(ease);
        }

        // public static Tween DOTWEEN_METHOD_NAME([NotNull] this UnityEngine.Camera target, Single endValue, float duration) => Tween.METHOD_NAME(target, endValue, duration);
    }

    public static class DOTween {
        public static Ease defaultEaseType {
            get => PrimeTweenConfig.defaultEase;
            set => PrimeTweenConfig.defaultEase = value;
        }

        public static Sequence Sequence() => PrimeTween.Sequence.Create();

        public static void Kill([NotNull] object target, bool complete = false) => DOTweenAdapter.doKill_internal(target, complete);

        public static void KillAll(bool complete = false) => DOTweenAdapter.doKill_internal(null, complete);

        public static Tween To([NotNull] Func<float> getter, [NotNull] Action<float> setter, float endValue, float duration) => Tween.Custom(getter(), endValue, duration, val => setter(val));
        public static Tween To([NotNull] Func<Vector2> getter, [NotNull] Action<Vector2> setter, Vector2 endValue, float duration) => Tween.Custom(getter(), endValue, duration, val => setter(val));
        public static Tween To([NotNull] Func<Vector3> getter, [NotNull] Action<Vector3> setter, Vector3 endValue, float duration) => Tween.Custom(getter(), endValue, duration, val => setter(val));
        public static Tween To([NotNull] Func<Vector4> getter, [NotNull] Action<Vector4> setter, Vector4 endValue, float duration) => Tween.Custom(getter(), endValue, duration, val => setter(val));
        public static Tween To([NotNull] Func<Quaternion> getter, [NotNull] Action<Quaternion> setter, Quaternion endValue, float duration) => Tween.Custom(getter(), endValue, duration, val => setter(val));
        public static Tween To([NotNull] Func<Color> getter, [NotNull] Action<Color> setter, Color endValue, float duration) => Tween.Custom(getter(), endValue, duration, val => setter(val));
        public static Tween To([NotNull] Func<Rect> getter, [NotNull] Action<Rect> setter, Rect endValue, float duration) => Tween.Custom(getter(), endValue, duration, val => setter(val));
    }

    public static class DOVirtual {
        public static Tween DelayedCall(float delay, Action callback, bool ignoreTimeScale = true)
            => Tween.Delay(delay, callback, ignoreTimeScale);

        public static Tween Float(float startValue, float endValue, float duration, Action<float> onValueChange) => Tween.Custom(startValue, endValue, duration, onValueChange);
        public static Tween Vector3(Vector3 startValue, Vector3 endValue, float duration, Action<Vector3> onValueChange) => Tween.Custom(startValue, endValue, duration, onValueChange);
        public static Tween Color(Color startValue, Color endValue, float duration, Action<Color> onValueChange) => Tween.Custom(startValue, endValue, duration, onValueChange);

        public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType, float? amplitude = null, float? period = null)
            => Mathf.LerpUnclamped(from, to, DOTweenAdapter.GetEasing(easeType, amplitude, period).Evaluate(lifetimePercentage));
        public static float EasedValue(float from, float to, float lifetimePercentage, [NotNull] AnimationCurve easeCurve)
            => Mathf.LerpUnclamped(from, to, Easing.Curve(easeCurve).Evaluate(lifetimePercentage));
        public static Vector3 EasedValue(Vector3 from, Vector3 to, float lifetimePercentage, Ease easeType, float? amplitude = null, float? period = null)
            => UnityEngine.Vector3.LerpUnclamped(from, to, DOTweenAdapter.GetEasing(easeType, amplitude, period).Evaluate(lifetimePercentage));
        public static Vector3 EasedValue(Vector3 from, Vector3 to, float lifetimePercentage, [NotNull] AnimationCurve easeCurve)
            => UnityEngine.Vector3.LerpUnclamped(from, to, Easing.Curve(easeCurve).Evaluate(lifetimePercentage));
    }

    public partial struct Sequence {
        public Sequence AppendCallback([NotNull] Action callback) {
            return ChainCallback(callback);
        }

        public Sequence SetLoops(int loops, LoopType? loopType = null) {
            root.SetLoops(loops, loopType);
            return this;
        }

        public Sequence Join(Sequence other) => Group(other);

        public Sequence Join(Tween other) {
            var tween = other.tween;
            var startDelay = tween.settings.startDelay;
            if (startDelay > 0) {
                // For some weird reason, DG.Tweening.Sequence.DoInsert shifts the lastTweenInsertTime by a tween's delay.
                tween.settings.startDelay = 0;
                tween.recalculateTotalDuration();
                Group(Tween.Delay(startDelay));
                ChainLast(other);
                return this;
            }
            return Group(other);
        }

        /// <summary>Schedules <see cref="other"/> after the last added tween.
        /// Internal because this API is hard to understand, but needed for adapter.</summary>
        internal Sequence ChainLast(Tween other) {
            if (tryManipulate()) {
                Insert(getLastInSelfOrRoot().durationWithWaitDelay, other);
            }
            return this;
        }

        public Sequence Append(Sequence other) => Chain(other);

        public Sequence Append(Tween other) {
            var tween = other.tween;
            var startDelay = tween.settings.startDelay;
            if (startDelay > 0) {
                // For some weird reason, DG.Tweening.Sequence.DoInsert shifts the lastTweenInsertTime by a tween's delay.
                tween.settings.startDelay = 0;
                tween.recalculateTotalDuration();
                Chain(Tween.Delay(startDelay));
            }
            return Chain(other);
        }

        public Sequence AppendInterval(float delay) {
            return Chain(Tween.Delay(delay));
        }

        public void Kill(bool complete = false) {
            if (complete) {
                Complete();
            } else {
                Stop();
            }
        }

        public void Complete(bool withCallbacks) {
            if (withCallbacks) {
                Debug.LogWarning("PrimeTween doesn't support " + nameof(Sequence) + "." + nameof(Complete) + "() " + nameof(withCallbacks) + " == true");
            }
            Complete();
        }

        public Sequence SetEase(Ease ease, float? amplitude = null, float? period = null) {
            root.SetEase(ease, amplitude, period);
            return this;
        }

        public Sequence SetEase([NotNull] AnimationCurve animCurve) {
            root.SetEase(animCurve);
            return this;
        }

        public Sequence SetDelay(float delay) {
            return PrependInterval(delay);
        }

        public Sequence OnStepComplete([NotNull] Action action) {
            Debug.LogWarning("Please use sequence.ChainCallback() as the last operation instead of sequence.OnStepComplete()");
            return ChainCallback(action);
        }

        public Sequence PrependInterval(float interval) {
            if (!ValidateCanManipulateSequence()) {
                return this;
            }
            foreach (var t in getSelfChildren()) {
                t.tween.waitDelay += interval;
            }
            duration += interval;
            return this;
        }

        public Sequence SetUpdate(bool isIndependentUpdate) {
            Assert.IsTrue(isAlive);
            Assert.IsTrue(root.tween.isMainSequenceRoot());
            root.tween.settings.useUnscaledTime = isIndependentUpdate;
            return this;
        }

        public Sequence AsyncWaitForCompletion() => this;

        public IEnumerator WaitForCompletion() => ToYieldInstruction();

        /// <summary>It's safe to destroy objects with running animations in PrimeTween, so this adapter method does nothing. More info: https://github.com/KyryloKuzyk/PrimeTween/discussions/4</summary>
        [PublicAPI]
        public Sequence SetLink(GameObject gameObject) => this;

        public Sequence Pause() {
            isPaused = true;
            return this;
        }

        public Sequence Play() {
            isPaused = false;
            return this;
        }
    }

    public partial struct Tween {
        public Tween SetEase(Ease ease, float? amplitude = null, float? period = null) {
            Assert.IsTrue(isAlive);
            var parametricEasing = DOTweenAdapter.GetEasing(ease, amplitude, period);
            tween.settings.SetEasing(parametricEasing);
            return this;
        }

        public Tween SetDelay(float delay) {
            Assert.IsTrue(isAlive);
            Assert.IsFalse(tween.IsInSequence());
            tween.settings.startDelay = delay;
            tween.recalculateTotalDuration();
            return this;
        }

        public Tween SetRelative(bool isRelative = true) {
            Assert.IsTrue(isAlive);
            if (!isRelative) {
                return this;
            }
            var getter = tween.getter;
            if (getter == null) {
                return this;
            }
            tween.endValue = CalculateRelative(tween, getter(tween), tween.endValue);
            /*var getter = tween.getter;
                // todo this doesn't account for double val
                if (tween.propType == PropType.Quaternion) {
                    if (getter != null) {
                        tween.endValue.QuaternionVal *= getter(tween).QuaternionVal;
                    } else {
                        tween.endValue.QuaternionVal *= tween.startValue.QuaternionVal;
                    }
                } else {
                    if (getter != null) {
                        tween.endValue.Vector4Val += getter(tween).Vector4Val;
                    } else {
                        tween.endValue.Vector4Val += tween.startValue.Vector4Val;
                    }
                }*/
            return this;
        }

        public Tween SetLoops(int loops, LoopType? loopType = null) {
            SetRemainingCycles(loops);
            if (isAlive && loopType.HasValue) {
                tween.settings.cycleMode = toCycleMode(loopType.Value);
            }
            return this;
        }

        static CycleMode toCycleMode(LoopType t) {
            switch (t) {
                case LoopType.Restart:
                    return CycleMode.Restart;
                case LoopType.Yoyo:
                    // yoyo in dotween behaves like rewind. But yoyo in other tween libraries (like tween.js) preserves the normal ease
                    return CycleMode.Rewind;
                case LoopType.Incremental:
                    return CycleMode.Incremental;
                default:
                    throw new Exception();
            }
        }

        public void Kill(bool complete = false) {
            if (complete) {
                Complete();
            } else {
                Stop();
            }
        }

        public bool IsActive() => isAlive;
        public bool active => isAlive;
        public bool IsPlaying() => isAlive && !isPaused;

        public Tween Pause() {
            isPaused = true;
            return this;
        }

        public Tween Play() {
            isPaused = false;
            return this;
        }

        public float Elapsed(bool includeLoops = true) => includeLoops ? elapsedTimeTotal : elapsedTime;
        public float Duration(bool includeLoops = true) => includeLoops ? durationTotal : duration;
        public int Loops() => cyclesTotal;
        public int CompletedLoops() => cyclesDone;
        public float ElapsedDelay() => isAlive ? Mathf.Clamp(elapsedTime, 0f, tween.settings.startDelay) : 0;
        public float ElapsedPercentage(bool includeLoops = true) => includeLoops ? progressTotal : progress;
        public void TogglePause() => isPaused = !isPaused;

        public Tween SetEase([NotNull] AnimationCurve animCurve) {
            Assert.IsTrue(isAlive);
            Assert.IsNotNull(animCurve);
            tween.settings.SetEasing(Easing.Curve(animCurve));
            return this;
        }

        public Tween SetTarget([NotNull] object target) {
            Assert.IsNotNull(target);
            Assert.IsTrue(isAlive);
            tween.target = target;
            tween.setUnityTarget(target);
            return this;
        }

        public IEnumerator WaitForCompletion() => ToYieldInstruction();

        public Tween AsyncWaitForCompletion() => this;

        public Tween SetUpdate(bool isIndependentUpdate) {
            Assert.IsTrue(isAlive);
            tween.settings.useUnscaledTime = isIndependentUpdate;
            return this;
        }

        public Tween From() => setFrom(true, false);
        public Tween From(bool isRelative) => setFrom(true, isRelative);
        public Tween From(bool setImmediately, bool isRelative) => setFrom(setImmediately, isRelative);

        public Tween From(float fromValue, bool setImmediately = true, bool isRelative = false) => setFrom(setImmediately, isRelative, fromValue.ToContainer(), PropType.Float);
        public Tween From(Color fromValue, bool setImmediately = true, bool isRelative = false) => setFrom(setImmediately, isRelative, fromValue.ToContainer(), PropType.Color);
        public Tween From(Vector2 fromValue, bool setImmediately = true, bool isRelative = false) => setFrom(setImmediately, isRelative, fromValue.ToContainer(), PropType.Vector2);
        public Tween From(Vector3 fromValue, bool setImmediately = true, bool isRelative = false) => setFrom(setImmediately, isRelative, fromValue.ToContainer(), PropType.Vector3);
        public Tween From(Vector4 fromValue, bool setImmediately = true, bool isRelative = false) => setFrom(setImmediately, isRelative, fromValue.ToContainer(), PropType.Vector4);
        public Tween From(Quaternion fromValue, bool setImmediately = true, bool isRelative = false) => setFrom(setImmediately, isRelative, fromValue.ToContainer(), PropType.Quaternion);
        public Tween From(Rect fromValue, bool setImmediately = true, bool isRelative = false) => setFrom(setImmediately, isRelative, fromValue.ToContainer(), PropType.Rect);

        static ValueContainer CalculateRelative(ReusableTween tween, ValueContainer current, ValueContainer diff) {
            switch (tween.propType) {
                case PropType.Quaternion:
                    return (current.QuaternionVal * diff.QuaternionVal).ToContainer();
                case PropType.Double:
                    return (current.DoubleVal + diff.DoubleVal).ToContainer();
                default:
                    return (current.Vector4Val + diff.Vector4Val).ToContainer();
            }
        }

        Tween setFrom(bool setImmediately, bool isRelative, ValueContainer? fromValue = null, PropType propType = PropType.None) {
            if (!tryManipulate()) {
                return this;
            }
            if (elapsedTimeTotal != 0f) {
                Debug.LogError(Constants.animationAlreadyStarted);
                return this;
            }
            if (tween.isUnityTargetDestroyed()) {
                Debug.LogError("Tween's target has been destroyed.");
                return this;
            }
            var getter = tween.getter;
            if (getter == null) {
                Debug.LogError("Custom tweens don't support 'From()'.");
                return this;
            }
            var current = getter(tween);
            if (isRelative) {
                tween.endValue = CalculateRelative(tween, current, tween.endValue);
            }
            if (fromValue.HasValue) {
                if (tween.propType != propType) {
                    Debug.LogError($"Animated value is {tween.propType}, but '{nameof(From)}()' was called with {propType}. Please provide a correct type.");
                    return this;
                }
                tween.startFromCurrent = false;
                tween.startValue = isRelative ? CalculateRelative(tween, current, fromValue.Value) : fromValue.Value;
            } else {
                tween.startFromCurrent = false;
                tween.startValue = tween.endValue;
                tween.endValue = current;
            }
            tween.cacheDiff();
            if (setImmediately) {
                tween.ReportOnValueChange(0f);
            }
            return this;
        }

        [PublicAPI]
        public Tween SetLink(GameObject gameObject) {
            return this;
        }
    }

    public enum LoopType {
        Restart,
        Yoyo,
        Incremental
    }
}
#endif
