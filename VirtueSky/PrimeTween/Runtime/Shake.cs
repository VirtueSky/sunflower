// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global
using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;
using SuppressMessage = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;

namespace PrimeTween {
    public partial struct Tween {
        /// <summary>Shakes the camera.<br/>
        /// If the camera is perspective, shakes all angles.<br/>
        /// If the camera is orthographic, shakes the z angle and x/y coordinates.<br/>
        /// Reference strengthFactor values - light: 0.2, medium: 0.5, heavy: 1.0.</summary>
        public static Sequence ShakeCamera([NotNull] Camera camera, float strengthFactor, float duration = 0.5f, float frequency = ShakeSettings.defaultFrequency, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = PrimeTweenConfig.defaultUseUnscaledTimeForShakes) {
            var transform = camera.transform;
            if (camera.orthographic) {
                float orthoPosStrength = strengthFactor * camera.orthographicSize * 0.03f;
                return Sequence.Create()
                    .Group(ShakeLocalPosition(transform, new ShakeSettings(new Vector3(orthoPosStrength, orthoPosStrength), duration, frequency, startDelay: startDelay, endDelay: endDelay, useUnscaledTime: useUnscaledTime)))
                    .Group(ShakeLocalRotation(transform, new ShakeSettings(new Vector3(0, 0, strengthFactor * 0.6f), duration, frequency, startDelay: startDelay, endDelay: endDelay, useUnscaledTime: useUnscaledTime)));
            }
            return Sequence.Create()
                .Group(ShakeLocalRotation(transform, new ShakeSettings(strengthFactor * Vector3.one, duration, frequency, startDelay: startDelay, endDelay: endDelay, useUnscaledTime: useUnscaledTime)));
        }

        public static Tween ShakeLocalPosition([NotNull] Transform target, Vector3 strength, float duration, float frequency = ShakeSettings.defaultFrequency, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = PrimeTweenConfig.defaultUseUnscaledTimeForShakes)
            => ShakeLocalPosition(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static Tween ShakeLocalPosition([NotNull] Transform target, ShakeSettings settings) {
            return shake(TweenType.ShakeLocalPosition, PropType.Vector3, target, settings, (state, shakeVal) => {
                (state.target as Transform).localPosition = state.startValue.Vector3Val + shakeVal;
            }, _ => (_.target as Transform).localPosition.ToContainer());
        }
        public static Tween PunchLocalPosition([NotNull] Transform target, Vector3 strength, float duration, float frequency = ShakeSettings.defaultFrequency, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = PrimeTweenConfig.defaultUseUnscaledTimeForShakes)
            => PunchLocalPosition(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
        public static Tween PunchLocalPosition([NotNull] Transform target, ShakeSettings settings) => ShakeLocalPosition(target, settings.WithPunch());

        public static Tween ShakeLocalRotation([NotNull] Transform target, Vector3 strength, float duration, float frequency = ShakeSettings.defaultFrequency, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = PrimeTweenConfig.defaultUseUnscaledTimeForShakes)
            => ShakeLocalRotation(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static Tween ShakeLocalRotation([NotNull] Transform target, ShakeSettings settings) {
            return shake(TweenType.ShakeLocalRotation, PropType.Quaternion,  target, settings, (state, shakeVal) => {
                (state.target as Transform).localRotation = state.startValue.QuaternionVal * Quaternion.Euler(shakeVal);
            }, t => (t.target as Transform).localRotation.ToContainer());
        }
        public static Tween PunchLocalRotation([NotNull] Transform target, Vector3 strength, float duration, float frequency = ShakeSettings.defaultFrequency, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = PrimeTweenConfig.defaultUseUnscaledTimeForShakes)
            => PunchLocalRotation(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
        public static Tween PunchLocalRotation([NotNull] Transform target, ShakeSettings settings) => ShakeLocalRotation(target, settings.WithPunch());

        public static Tween ShakeScale([NotNull] Transform target, Vector3 strength, float duration, float frequency = ShakeSettings.defaultFrequency, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = PrimeTweenConfig.defaultUseUnscaledTimeForShakes)
            => ShakeScale(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static Tween ShakeScale([NotNull] Transform target, ShakeSettings settings) {
            return shake(TweenType.ShakeScale, PropType.Vector3, target, settings, (state, shakeVal) => {
                (state.target as Transform).localScale = state.startValue.Vector3Val + shakeVal;
            }, t => (t.target as Transform).localScale.ToContainer());
        }
        public static Tween PunchScale([NotNull] Transform target, Vector3 strength, float duration, float frequency = ShakeSettings.defaultFrequency, bool enableFalloff = true, Ease easeBetweenShakes = Ease.Default, float asymmetryFactor = 0f, int cycles = 1,
            float startDelay = 0, float endDelay = 0, bool useUnscaledTime = PrimeTweenConfig.defaultUseUnscaledTimeForShakes)
            => PunchScale(target, new ShakeSettings(strength, duration, frequency, enableFalloff, easeBetweenShakes, asymmetryFactor, cycles, startDelay, endDelay, useUnscaledTime));
        public static Tween PunchScale([NotNull] Transform target, ShakeSettings settings) => ShakeScale(target, settings.WithPunch());

        static Tween shake(TweenType tweenType, PropType propType, [NotNull] Transform target, ShakeSettings settings, [NotNull] Action<ReusableTween, Vector3> onValueChange, [NotNull] Func<ReusableTween, ValueContainer> getter) {
            Assert.IsNotNull(onValueChange);
            Assert.IsNotNull(getter);
            var tween = PrimeTweenManager.fetchTween();
            prepareShakeData(settings, tween);
            tween.customOnValueChange = onValueChange;
            var tweenSettings = settings.tweenSettings;
            tween.Setup(target, ref tweenSettings, state => {
                var _onValueChange = state.customOnValueChange as Action<ReusableTween, Vector3>;
                Assert.IsNotNull(_onValueChange);
                var shakeVal = getShakeVal(state);
                _onValueChange(state, shakeVal);
            }, getter, true, tweenType);
            return PrimeTweenManager.Animate(tween);
        }

        public static Tween ShakeCustom<T>([NotNull] T target, Vector3 startValue, ShakeSettings settings, [NotNull] Action<T, Vector3> onValueChange) where T : class {
            Assert.IsNotNull(onValueChange);
            var tween = PrimeTweenManager.fetchTween();
            tween.startValue.CopyFrom(ref startValue);
            prepareShakeData(settings, tween);
            tween.customOnValueChange = onValueChange;
            var tweenSettings = settings.tweenSettings;
            tween.Setup(target, ref tweenSettings, _tween => {
                var _onValueChange = _tween.customOnValueChange as Action<T, Vector3>;
                Assert.IsNotNull(_onValueChange);
                var _target = _tween.target as T;
                var val = _tween.startValue.Vector3Val + getShakeVal(_tween);
                try {
                    _onValueChange(_target, val);
                } catch (Exception e) {
                    Debug.LogError($"Tween was stopped because of exception in {nameof(onValueChange)} callback, tween: {_tween.GetDescription()}, exception:\n{e}", _tween.target as UnityEngine.Object);
                    _tween.EmergencyStop();
                }
            }, null, false, TweenType.ShakeCustom);
            return PrimeTweenManager.Animate(tween);
        }
        public static Tween PunchCustom<T>([NotNull] T target, Vector3 startValue, ShakeSettings settings, [NotNull] Action<T, Vector3> onValueChange) where T : class => ShakeCustom(target, startValue, settings.WithPunch(), onValueChange);

        static void prepareShakeData(ShakeSettings settings, [NotNull] ReusableTween tween) {
            tween.endValue.Reset(); // not used
            tween.shakeData.Setup(settings, tween);
        }

        static Vector3 getShakeVal([NotNull] ReusableTween tween) {
            return tween.shakeData.getNextVal(tween) * calcFadeInOutFactor();
            float calcFadeInOutFactor() {
                var elapsedTimeInterpolating = tween.easedInterpolationFactor * tween.settings.duration;
                Assert.IsTrue(elapsedTimeInterpolating >= 0f);
                var duration = tween.settings.duration;
                if (duration == 0f) {
                    return 0f;
                }
                Assert.IsTrue(duration > 0f);
                float halfDuration = duration * 0.5f;
                var oneShakeDuration = 1f / tween.shakeData.frequency;
                if (oneShakeDuration > halfDuration) {
                    oneShakeDuration = halfDuration;
                }
                float fadeInDuration = oneShakeDuration * 0.5f;
                if (elapsedTimeInterpolating < fadeInDuration) {
                    return Mathf.InverseLerp(0f, fadeInDuration, elapsedTimeInterpolating);
                }
                var fadeoutStartTime = duration - oneShakeDuration;
                Assert.IsTrue(fadeoutStartTime > 0f, tween.id);
                if (elapsedTimeInterpolating > fadeoutStartTime) {
                    return Mathf.InverseLerp(duration, fadeoutStartTime, elapsedTimeInterpolating);
                }
                return 1f;
            }
        }
    }

    #if PRIME_TWEEN_INSPECTOR_DEBUGGING && UNITY_EDITOR
    [Serializable]
    #endif
    internal struct ShakeData {
        float t;
        Vector3 from, to;
        float symmetryFactor;
        int falloffEaseInt;
        AnimationCurve customStrengthOverTime;
        Ease easeBetweenShakes;
        const int disabledFalloff = -42;
        internal bool isAlive => frequency != 0f;
        internal Vector3 strengthPerAxis { get; private set; }
        internal float frequency { get; private set; }
        float prevInterpolationFactor;
        int prevCyclesDone;

        internal void Setup(ShakeSettings settings, ReusableTween tween) {
            tween.isPunch = settings.isPunch;
            symmetryFactor = Mathf.Clamp01(1 - settings.asymmetry);
            {
                var _strength = settings.strength;
                if (_strength == Vector3.zero) {
                    Debug.LogError("Shake's strength is (0, 0, 0).");
                }
                strengthPerAxis = _strength;
            }
            {
                var _frequency = settings.frequency;
                if (_frequency <= 0) {
                    Debug.LogError($"Shake's frequency should be > 0f, but was {_frequency}.");
                    _frequency = ShakeSettings.defaultFrequency;
                }
                frequency = _frequency;
            }
            {
                if (settings.enableFalloff) {
                    var _falloffEase = settings.falloffEase;
                    var _customStrengthOverTime = settings.strengthOverTime;
                    if (_falloffEase == Ease.Default) {
                        _falloffEase = Ease.Linear;
                    }
                    if (_falloffEase == Ease.Custom) {
                        if (_customStrengthOverTime == null || !TweenSettings.ValidateCustomCurve(_customStrengthOverTime)) {
                            Debug.LogError($"Shake falloff is Ease.Custom, but {nameof(ShakeSettings.strengthOverTime)} is not configured correctly. Using Ease.Linear instead.");
                            _falloffEase = Ease.Linear;
                        }
                    }
                    falloffEaseInt = (int)_falloffEase;
                    customStrengthOverTime = _customStrengthOverTime;
                } else {
                    falloffEaseInt = disabledFalloff;
                }
            }
            {
                var _easeBetweenShakes = settings.easeBetweenShakes;
                if (_easeBetweenShakes == Ease.Custom) {
                    Debug.LogError($"{nameof(ShakeSettings.easeBetweenShakes)} doesn't support Ease.Custom.");
                    _easeBetweenShakes = Ease.OutQuad;
                }
                if (_easeBetweenShakes == Ease.Default) {
                    _easeBetweenShakes = PrimeTweenManager.defaultShakeEase;
                }
                easeBetweenShakes = _easeBetweenShakes;
            }
            onCycleComplete(tween);
        }

        internal void onCycleComplete(ReusableTween tween) {
            Assert.IsTrue(isAlive);
            resetAfterCycle();
            tween.shakeSign = tween.isPunch || Random.value < 0.5f;
            to = generateShakePoint(tween);
        }

        static int getMainAxisIndex(Vector3 strengthByAxis) {
            int mainAxisIndex = -1;
            float maxStrength = float.NegativeInfinity;
            for (int i = 0; i < 3; i++) {
                var strength = Mathf.Abs(strengthByAxis[i]);
                if (strength > maxStrength) {
                    maxStrength = strength;
                    mainAxisIndex = i;
                }
            }
            Assert.IsTrue(mainAxisIndex >= 0);
            return mainAxisIndex;
        }

        internal Vector3 getNextVal([NotNull] ReusableTween tween) {
            var interpolationFactor = tween.easedInterpolationFactor;
            Assert.IsTrue(interpolationFactor <= 1);

            int cyclesDiff = tween.getCyclesDone() - prevCyclesDone;
            prevCyclesDone = tween.getCyclesDone();
            if (interpolationFactor == 0f || (cyclesDiff > 0 && tween.getCyclesDone() != tween.settings.cycles)) {
                onCycleComplete(tween);
                prevInterpolationFactor = interpolationFactor;
            }

            var dt = (interpolationFactor - prevInterpolationFactor) * tween.settings.duration;
            prevInterpolationFactor = interpolationFactor;

            var strengthOverTime = calcStrengthOverTime(interpolationFactor);
            var frequencyFactor = Mathf.Clamp01(strengthOverTime * 3f); // handpicked formula that describes the relationship between strength and frequency
            float getIniVelFactor() {
                // The initial velocity should twice as big because the first shake starts from zero (twice as short as total range).
                var elapsedTimeInterpolating = tween.easedInterpolationFactor * tween.settings.duration;
                var halfShakeDuration = 0.5f / tween.shakeData.frequency;
                return elapsedTimeInterpolating < halfShakeDuration ? 2f : 1f;
            }
            t += frequency * dt * frequencyFactor * getIniVelFactor();
            if (t < 0f || t >= 1f) {
                tween.shakeSign = !tween.shakeSign;
                if (t < 0f) {
                    t = 1f;
                    to = from;
                    from = generateShakePoint(tween);
                } else {
                    t = 0f;
                    from = to;
                    to = generateShakePoint(tween);
                }
            }

            Vector3 result = default;
            for (int i = 0; i < 3; i++) {
                result[i] = Mathf.Lerp(from[i], to[i], StandardEasing.Evaluate(t, easeBetweenShakes)) * strengthOverTime;
            }
            return result;
        }

        Vector3 generateShakePoint(ReusableTween tween) {
            var mainAxisIndex = getMainAxisIndex(strengthPerAxis);
            Vector3 result = default;
            float signFloat = tween.shakeSign ? 1f : -1f;
            for (int i = 0; i < 3; i++) {
                var strength = strengthPerAxis[i];
                if (tween.isPunch) {
                    result[i] = clampBySymmetryFactor(strength * signFloat, strength, symmetryFactor);
                } else {
                    result[i] = i == mainAxisIndex ? calcMainAxisEndVal(signFloat, strength, symmetryFactor) : calcNonMainAxisEndVal(strength, symmetryFactor);
                }
            }
            return result;
        }

        float calcStrengthOverTime(float interpolationFactor) {
            if (falloffEaseInt == disabledFalloff) {
                return 1;
            }
            var falloffEase = (Ease)falloffEaseInt;
            if (falloffEase != Ease.Custom) {
                return 1 - StandardEasing.Evaluate(interpolationFactor, falloffEase);
            }
            Assert.IsNotNull(customStrengthOverTime);
            return customStrengthOverTime.Evaluate(interpolationFactor);
        }

        static float calcMainAxisEndVal(float velocity, float strength, float symmetryFactor) {
            var result = Mathf.Sign(velocity) * strength * Random.Range(0.6f, 1f); // doesn't matter if we're using strength or its abs because velocity alternates
            return clampBySymmetryFactor(result, strength, symmetryFactor);
        }

        static float clampBySymmetryFactor(float val, float strength, float symmetryFactor) {
            if (strength > 0) {
                return Mathf.Clamp(val, -strength * symmetryFactor, strength);
            }
            return Mathf.Clamp(val, strength, -strength * symmetryFactor);
        }

        static float calcNonMainAxisEndVal(float strength, float symmetryFactor) {
            if (strength > 0) {
                return Random.Range(-strength * symmetryFactor, strength);
            }
            return Random.Range(strength, -strength * symmetryFactor);
        }

        internal static bool TryTakeStartValueFromOtherShake([NotNull] ReusableTween newTween) {
            if (!newTween.shakeData.isAlive) {
                return false;
            }
            var shakeTransform = newTween.target as Transform;
            if (shakeTransform == null) {
                return false;
            }
            var shakes = PrimeTweenManager.Instance.shakes;
            var key = (shakeTransform, newTween.tweenType);
            if (!shakes.TryGetValue(key, out var data)) {
                shakes.Add(key, (newTween.getter(newTween), 1));
                return false;
            }
            Assert.IsTrue(data.count >= 1);
            newTween.startValue = data.startValue;
            // Debug.Log($"tryTakeStartValueFromOtherShake {data.startValue.Vector4Val}");
            data.count++;
            shakes[key] = data;
            return true;
        }

        internal void Reset([NotNull] ReusableTween tween) {
            Assert.IsTrue(isAlive);
            var shakeTransform = tween.target as Transform;
            if (shakeTransform != null) {
                var key = (shakeTransform, tween.tweenType);
                var shakes = PrimeTweenManager.Instance.shakes;
                if (shakes.TryGetValue(key, out var data)) {
                    // no key present if it's a ShakeCustom() with Transform target because custom shakes have startFromCurrent == false and aren't added to shakes dict
                    Assert.IsTrue(data.count >= 1);
                    data.count--;
                    if (data.count == 0) {
                        bool isRemoved = shakes.Remove(key);
                        Assert.IsTrue(isRemoved);
                    } else {
                        shakes[key] = data;
                    }
                }
            }

            resetAfterCycle();
            customStrengthOverTime = null;
            frequency = 0f;
            prevInterpolationFactor = 0f;
            prevCyclesDone = 0;
            Assert.IsFalse(isAlive);
        }

        void resetAfterCycle() {
            t = 0f;
            from = Vector3.zero;
        }
    }
}
