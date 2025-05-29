/*
// ReSharper disable PossibleNullReferenceException
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
using System;
using JetBrains.Annotations;

namespace PrimeTween {
    internal static class CodeTemplates {
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 endValue, float averageSpeed, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 endValue, float averageSpeed, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float averageSpeed, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(startValue, endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, UnityEngine.Vector3 startValue, UnityEngine.Vector3 endValue, float averageSpeed, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => PositionAtSpeed(target, new TweenSettings<UnityEngine.Vector3>(startValue, endValue, new TweenSettings(averageSpeed, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        static Tween PositionAtSpeed([NotNull] UnityEngine.Transform target, TweenSettings<UnityEngine.Vector3> settings) {
            var speed = settings.settings.duration;
            if (speed <= 0) {
                UnityEngine.Debug.LogError($"Invalid speed provided to the Tween.{nameof(PositionAtSpeed)}() method: {speed}.");
                return default;
            }
            if (settings.startFromCurrent) {
                settings.startFromCurrent = false;
                settings.startValue = target.position;
            }
            settings.settings.duration = Extensions.CalcDistance(settings.startValue, settings.endValue) / speed;
            return Tween.Position(target, settings);
        }

        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => METHOD_NAME(target, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => METHOD_NAME(target, new TweenSettings<float>(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single startValue, Single endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => METHOD_NAME(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single startValue, Single endValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => METHOD_NAME(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single endValue, TweenSettings settings) => METHOD_NAME(target, new TweenSettings<float>(endValue, settings));
        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, Single startValue, Single endValue, TweenSettings settings) => METHOD_NAME(target, new TweenSettings<float>(startValue, endValue, settings));

        public static Tween METHOD_NAME([NotNull] UnityEngine.Camera target, TweenSettings<float> settings) {
            return animate(target, ref settings, _tween => {
                var _target = _tween.target as UnityEngine.Camera;
                var val = _tween.FloatVal;
                _target.orthographicSize = val;
            }, t => (t.target as UnityEngine.Camera).orthographicSize.ToContainer(), TweenType.CameraOrthographicSize);
        }

        public static Tween Custom_TEMPLATE(Single startValue, Single endValue, float duration, [NotNull] Action<Single> onValueChange, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => Custom_TEMPLATE(new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE(Single startValue, Single endValue, float duration, [NotNull] Action<Single> onValueChange, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => Custom_TEMPLATE(new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE(Single startValue, Single endValue, TweenSettings settings, [NotNull] Action<Single> onValueChange) => Custom_TEMPLATE(new TweenSettings<float>(startValue, endValue, settings), onValueChange);
        public static Tween Custom_TEMPLATE(TweenSettings<float> settings, [NotNull] Action<Single> onValueChange) {
            Assert.IsNotNull(onValueChange);
            if (settings.startFromCurrent) {
                UnityEngine.Debug.LogWarning(Constants.customTweensDontSupportStartFromCurrentWarning);
            }
            var tween = PrimeTweenManager.fetchTween();
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.propType = PropType.Float;
            tween.customOnValueChange = onValueChange;
            tween.Setup(PrimeTweenManager.dummyTarget, ref settings.settings, _tween => {
                var _onValueChange = _tween.customOnValueChange as Action<Single>;
                var val = _tween.FloatVal;
                try {
                    _onValueChange(val);
                } catch (Exception e) {
                    UnityEngine.Debug.LogException(e);
                    Assert.LogWarning($"Tween was stopped because of exception in {nameof(onValueChange)} callback, tween: {_tween.GetDescription()}\n", _tween.id, _tween.target as UnityEngine.Object);
                    _tween.EmergencyStop();
                }
            }, null, false, TweenType.CustomFloat);
            return PrimeTweenManager.Animate(tween);
        }
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, Single startValue, Single endValue, float duration, [NotNull] Action<T, Single> onValueChange, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) where T : class
            => Custom_internal(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, Single startValue, Single endValue, float duration, [NotNull] Action<T, Single> onValueChange, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false) where T : class
            => Custom_internal(target, new TweenSettings<float>(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime)), onValueChange);
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, Single startValue, Single endValue, TweenSettings settings, [NotNull] Action<T, Single> onValueChange) where T : class
            => Custom_internal(target, new TweenSettings<float>(startValue, endValue, settings), onValueChange);
        public static Tween Custom_TEMPLATE<T>([NotNull] T target, TweenSettings<float> settings, [NotNull] Action<T, Single> onValueChange) where T : class
            => Custom_internal(target, settings, onValueChange);
        #if PRIME_TWEEN_EXPERIMENTAL
        public static Tween CustomAdditive<T>([NotNull] T target, Single deltaValue, TweenSettings settings, [NotNull] Action<T, Single> onDeltaChange) where T : class
            => Custom_internal(target, new TweenSettings<float>(default, deltaValue, settings), onDeltaChange, true);
        #endif
        static Tween Custom_internal<T>([NotNull] T target, TweenSettings<float> settings, [NotNull] Action<T, Single> onValueChange, bool isAdditive = false) where T : class {
            Assert.IsNotNull(onValueChange);
            if (settings.startFromCurrent) {
                UnityEngine.Debug.LogWarning(Constants.customTweensDontSupportStartFromCurrentWarning);
            }
            var tween = PrimeTweenManager.fetchTween();
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.propType = PropType.Float;
            tween.customOnValueChange = onValueChange;
            tween.isAdditive = isAdditive;
            tween.Setup(target, ref settings.settings, _tween => {
                var _onValueChange = _tween.customOnValueChange as Action<T, Single>;
                var _target = _tween.target as T;
                Single val;
                if (_tween.isAdditive) {
                    var newVal = _tween.FloatVal;
                    val = newVal.calcDelta(_tween.prevVal);
                    _tween.prevVal.FloatVal = newVal;
                } else {
                    val = _tween.FloatVal;
                }
                try {
                    _onValueChange(_target, val);
                } catch (Exception e) {
                    UnityEngine.Debug.LogException(e, _target as UnityEngine.Object);
                    Assert.LogWarning($"Tween was stopped because of exception in {nameof(onValueChange)} callback, tween: {_tween.GetDescription()}\n", _tween.id, _tween.target as UnityEngine.Object);
                    _tween.EmergencyStop();
                }
            }, null, false, TweenType.CustomFloat);
            return PrimeTweenManager.Animate(tween);
        }
        static Tween animate(object target, ref TweenSettings<float> settings, [NotNull] Action<ReusableTween> setter, Func<ReusableTween, ValueContainer> getter, TweenType _tweenType) {
            var tween = PrimeTweenManager.fetchTween();
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.propType = PropType.Float;
            tween.Setup(target, ref settings.settings, setter, getter, settings.startFromCurrent, _tweenType);
            return PrimeTweenManager.Animate(tween);
        }
        static Tween animateWithIntParam([NotNull] object target, int intParam, ref TweenSettings<float> settings, [NotNull] Action<ReusableTween> setter, [NotNull] Func<ReusableTween, ValueContainer> getter, TweenType _tweenType) {
            var tween = PrimeTweenManager.fetchTween();
            tween.intParam = intParam;
            tween.startValue.CopyFrom(ref settings.startValue);
            tween.endValue.CopyFrom(ref settings.endValue);
            tween.propType = PropType.Float;
            tween.Setup(target, ref settings.settings, setter, getter, settings.startFromCurrent, _tweenType);
            return PrimeTweenManager.Animate(tween);
        }

        public static Tween PositionAdditive([NotNull] UnityEngine.Transform target, Single deltaValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => PositionAdditive(target, deltaValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
        public static Tween PositionAdditive([NotNull] UnityEngine.Transform target, Single deltaValue, float duration, Easing ease, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false)
            => PositionAdditive(target, deltaValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime));
        public static Tween PositionAdditive([NotNull] UnityEngine.Transform target, Single deltaValue, TweenSettings settings)
            => CustomAdditive(target, deltaValue, settings, (_, _) => additiveTweenSetter());

        static void additiveTweenSetter() {}
    }
}
*/
