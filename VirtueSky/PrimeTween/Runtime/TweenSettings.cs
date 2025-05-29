using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    /// <summary>TweenSettings contains animation properties (duration, ease, delay, etc.). Can be serialized and tweaked from the Inspector.<br/>
    /// Use this struct when the 'start' and 'end' values of an animation are NOT known in advance and determined at runtime.<br/>
    /// When the 'start' and 'end' values ARE known, consider using <see cref="TweenSettings{T}"/> instead.</summary>
    /// <example>
    /// Tweak an animation settings from the Inspector, then pass the settings to the Tween method:
    /// <code>
    /// [SerializeField] TweenSettings animationSettings;
    /// public void AnimatePositionX(float targetPosX) {
    ///     Tween.PositionX(transform, targetPosX, animationSettings);
    /// }
    /// </code>
    /// </example>
    [Serializable]
    public struct TweenSettings {
        public float duration;
        [Tooltip(Constants.easeTooltip)]
        public Ease ease;
        [Tooltip("A custom Animation Curve that will work as an easing curve.")]
        [CanBeNull] public AnimationCurve customEase;
        [Tooltip(Constants.cyclesTooltip)]
        public int cycles;
        [Tooltip(Constants.cycleModeTooltip)]
        public CycleMode cycleMode;
        [Tooltip(Constants.startDelayTooltip)]
        public float startDelay;
        [Tooltip(Constants.endDelayTooltip)]
        public float endDelay;
        [Tooltip(Constants.unscaledTimeTooltip)]
        public bool useUnscaledTime;

        [Obsolete("use '" + nameof(updateType) + "' instead.")]
        public bool useFixedUpdate {
            get => updateType == UpdateType.FixedUpdate || _useFixedUpdate;
            set {
                _updateType = value ? _UpdateType.FixedUpdate : _UpdateType.Update;
                _useFixedUpdate = value;
            }
        }
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("useFixedUpdate")]
        [HideInInspector]
        bool _useFixedUpdate;

        public UpdateType updateType {
            get => _useFixedUpdate ? UpdateType.FixedUpdate : new UpdateType(_updateType);
            set {
                _updateType = value.enumValue;
                _useFixedUpdate = value == UpdateType.FixedUpdate;
            }
        }
        [SerializeField, Tooltip(Constants.updateTypeTooltip)]
        internal _UpdateType _updateType;

        [NonSerialized] internal ParametricEase parametricEase;
        [NonSerialized] internal float parametricEaseStrength;
        [NonSerialized] internal float parametricEasePeriod;

        internal TweenSettings(float duration, Ease ease, Easing? customEasing, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false, UpdateType updateType = default) {
            this.duration = duration;
            var curve = customEasing?.curve;
            if (ease == Ease.Custom && customEasing?.parametricEase == ParametricEase.None) {
                if (curve == null || !ValidateCustomCurveKeyframes(curve)) {
                    Debug.LogError("Ease is Ease.Custom, but AnimationCurve is not configured correctly. Using Ease.Default instead.");
                    ease = Ease.Default;
                }
            }
            this.ease = ease;
            customEase = ease == Ease.Custom ? curve : null;
            this.cycles = cycles;
            this.cycleMode = cycleMode;
            this.startDelay = startDelay;
            this.endDelay = endDelay;
            this.useUnscaledTime = useUnscaledTime;
            parametricEase = customEasing?.parametricEase ?? ParametricEase.None;
            parametricEaseStrength = customEasing?.parametricEaseStrength ?? float.NaN;
            parametricEasePeriod = customEasing?.parametricEasePeriod ?? float.NaN;
            _useFixedUpdate = updateType == UpdateType.FixedUpdate;
            _updateType = updateType.enumValue;
        }

        #if PRIME_TWEEN_DOTWEEN_ADAPTER
        internal void SetEasing(Easing easing) {
            ease = easing.ease;
            parametricEase = easing.parametricEase;
            parametricEaseStrength = easing.parametricEaseStrength;
            parametricEasePeriod = easing.parametricEasePeriod;
        }
        #endif

        public TweenSettings(float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false, UpdateType updateType = default)
            : this(duration, ease, null, cycles, cycleMode, startDelay, endDelay, useUnscaledTime, updateType) {
        }

        public TweenSettings(float duration, Easing easing, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false, UpdateType updateType = default)
            : this(duration, easing.ease, easing, cycles, cycleMode, startDelay, endDelay, useUnscaledTime, updateType) {
        }

        internal static void setCyclesTo1If0(ref int cycles) {
            if (cycles == 0) {
                cycles = 1;
            }
        }

        internal void CopyFrom(ref TweenSettings other) {
            duration = other.duration;
            ease = other.ease;
            customEase = other.customEase;
            cycles = other.cycles;
            cycleMode = other.cycleMode;
            startDelay = other.startDelay;
            endDelay = other.endDelay;
            useUnscaledTime = other.useUnscaledTime;
            parametricEase = other.parametricEase;
            parametricEaseStrength = other.parametricEaseStrength;
            parametricEasePeriod = other.parametricEasePeriod;
            updateType = other.updateType;
        }

        internal const float minDuration = 0.0001f;

        internal void SetValidValues() {
            validateFiniteDuration(duration);
            validateFiniteDuration(startDelay);
            validateFiniteDuration(endDelay);
            setCyclesTo1If0(ref cycles);
            if (duration != 0f) {
                #if UNITY_EDITOR && PRIME_TWEEN_SAFETY_CHECKS
                if (duration < minDuration) {
                    Debug.LogError("duration = Mathf.Max(minDuration, duration);");
                }
                #endif
                duration = Mathf.Max(minDuration, duration);
            }
            startDelay = Mathf.Max(0f, startDelay);
            endDelay = Mathf.Max(0f, endDelay);
            if (cycles == 1) {
                cycleMode = CycleMode.Restart;
            }
        }

        internal static void validateFiniteDuration(float f) {
            Assert.IsFalse(float.IsNaN(f), Constants.durationInvalidError);
            Assert.IsFalse(float.IsInfinity(f), Constants.durationInvalidError);
        }

        internal static bool ValidateCustomCurve([NotNull] AnimationCurve curve) {
            #if UNITY_ASSERTIONS && !PRIME_TWEEN_DISABLE_ASSERTIONS
            if (curve.length < 2) {
                Debug.LogError("Custom animation curve should have at least 2 keyframes, please edit the curve in Inspector.");
                return false;
            }
            return true;
            #else
            return true;
            #endif
        }

        internal static bool ValidateCustomCurveKeyframes([NotNull] AnimationCurve curve) {
            #if UNITY_ASSERTIONS && !PRIME_TWEEN_DISABLE_ASSERTIONS
            if (!ValidateCustomCurve(curve)) {
                return false;
            }
            var instance = PrimeTweenManager.Instance;
            if (instance == null || instance.validateCustomCurves) {
                var error = getError();
                if (error != null) {
                    Debug.LogWarning($"Custom animation curve is not configured correctly which may have unexpected results: {error}. " +
                                     Constants.buildWarningCanBeDisabledMessage(nameof(PrimeTweenConfig.validateCustomCurves)));
                }
                string getError() {
                    var start = curve[0];
                    if (!Mathf.Approximately(start.time, 0)) {
                        return "start time is not 0";
                    }
                    if (!Mathf.Approximately(start.value, 0) && !Mathf.Approximately(start.value, 1)) {
                        return "start value is not 0 or 1";
                    }
                    var end = curve[curve.length - 1];
                    if (!Mathf.Approximately(end.time, 1)) {
                        return "end time is not 1";
                    }
                    if (!Mathf.Approximately(end.value, 0) && !Mathf.Approximately(end.value, 1)) {
                        return "end value is not 0 or 1";
                    }
                    return null;
                }
            }
            return true;
            #else
            return true;
            #endif
        }
    }

    [Serializable]
    public struct UpdateType : IEquatable<UpdateType> {
        /// Uses <see cref="PrimeTweenConfig.defaultUpdateType"/> to control the default Unity's event function, which updates the animation.
        public static readonly UpdateType Default = new UpdateType(_UpdateType.Default);
        /// Updates the animation in MonoBehaviour.Update().<br/>
        /// If the animation has 'startValue' and doesn't have a start delay, the 'startValue' is applied in <see cref="PrimeTweenManager.LateUpdate"/>.
        /// This ensures the animation is rendered at the 'startValue' in the same frame it's created.
        public static readonly UpdateType Update = new UpdateType(_UpdateType.Update);
        /// Updates the animation in MonoBehaviour.LateUpdate().<br/>
        /// If the animation has 'startValue' and doesn't have a start delay, the 'startValue' is applied in <see cref="PrimeTweenManager.LateUpdate"/>.
        /// This ensures the animation is rendered at the 'startValue' in the same frame it's created.
        public static readonly UpdateType LateUpdate = new UpdateType(_UpdateType.LateUpdate);
        /// Updates the animation in MonoBehaviour.FixedUpdate().<br/>
        /// Unlike Update and LateUpdate animations, FixedUpdate animations don't apply the 'startValue' before the first frame is rendered.
        /// They receive their first update in the first FixedUpdate() after creation.
        public static readonly UpdateType FixedUpdate = new UpdateType(_UpdateType.FixedUpdate);

        [SerializeField]
        internal _UpdateType enumValue;
        internal UpdateType(_UpdateType enumValue) { this.enumValue = enumValue; }
        [Obsolete("use 'UpdateType.FixedUpdate' instead.")]
        public static implicit operator UpdateType(bool isFixedUpdate) => isFixedUpdate ? FixedUpdate : Update;
        public static bool operator==(UpdateType lhs, UpdateType rhs) => lhs.enumValue == rhs.enumValue;
        public static bool operator !=(UpdateType lhs, UpdateType rhs) => lhs.enumValue != rhs.enumValue;
        public bool Equals(UpdateType other) => enumValue == other.enumValue;
        public override bool Equals(object obj) => obj is UpdateType other && Equals(other);
        public override int GetHashCode() => ((int)enumValue).GetHashCode();
    }

    internal enum _UpdateType : byte {
        [Tooltip("Uses 'PrimeTweenConfig.defaultUpdateType' to control the default Unity's event function, which updates the animation.")]
        Default,
        [Tooltip("Updates the animation in MonoBehaviour.Update().\n\n" +
                 "If the animation has 'startValue' and doesn't have a start delay, the 'startValue' is applied in 'PrimeTweenManager.LateUpdate'. This ensures the animation is rendered at the 'startValue' in the same frame it's created.")]
        Update,
        [Tooltip("Updates the animation in MonoBehaviour.LateUpdate().\n\n" +
                 "If the animation has 'startValue' and doesn't have a start delay, the 'startValue' is applied in 'PrimeTweenManager.LateUpdate'. This ensures the animation is rendered at the 'startValue' in the same frame it's created.")]
        LateUpdate,
        [Tooltip("Updates the animation in 'MonoBehaviour.FixedUpdate()'.\n\n" +
                 "Unlike Update and LateUpdate animations, FixedUpdate animations don't apply the 'startValue' before the first frame is rendered. They receive their first update in the first FixedUpdate() after creation.")]
        FixedUpdate
    }

    /// <summary>The standard animation easing types. Different easing curves produce a different animation 'feeling'.<br/>
    /// Play around with different ease types to choose one that suites you the best.
    /// You can also provide a custom AnimationCurve as an ease function or parametrize eases with the Easing.Overshoot/Elastic/BounceExact(...) methods.</summary>
    public enum Ease { Custom = -1, Default = 0, Linear = 1,
        InSine, OutSine, InOutSine,
        InQuad, OutQuad, InOutQuad,
        InCubic, OutCubic, InOutCubic,
        InQuart, OutQuart, InOutQuart,
        InQuint, OutQuint, InOutQuint,
        InExpo, OutExpo, InOutExpo,
        InCirc, OutCirc, InOutCirc,
        InElastic, OutElastic, InOutElastic,
        InBack, OutBack, InOutBack,
        InBounce, OutBounce, InOutBounce
    }

    /// <summary>Controls the behavior of subsequent cycles when a tween has more than one cycle.</summary>
    public enum CycleMode {
        [Tooltip("Restarts the tween from the beginning.")]
        Restart,
        [Tooltip("Animates forth and back, like a yoyo. Easing is the same on the backward cycle.")]
        Yoyo,
        [Tooltip("At the end of a cycle increments the `endValue` by the difference between `startValue` and `endValue`.\n\n" +
                 "For example, if a tween moves position.x from 0 to 1, then after the first cycle, the tween will move the position.x from 1 to 2, and so on.")]
        Incremental,
        [Tooltip("Rewinds the tween as if time was reversed. Easing is reversed on the backward cycle.")]
        Rewind
    }
}
