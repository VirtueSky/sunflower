using System;
using UnityEngine;

namespace PrimeTween {
    /// <summary>Contains 'start' and 'end' values of an animation in addition to <see cref="TweenSettings"/>. Can be serialized and tweaked from the Inspector in Unity 2020.1+.<br/>
    /// Use this struct when the 'start' and 'end' values of an animation are known in advance.</summary>
    /// <example>Tweak animation from the Inspector, then pass the settings to the Tween method:
    /// <code>
    /// [SerializeField] TweenSettings&lt;Vector3&gt; tweenSettings;
    /// public void AnimatePosition() {
    ///     Tween.Position(transform, tweenSettings);
    /// }
    /// </code></example>
    /// <example>
    /// <br/>Or create the TweenSettings in code:
    /// <code>
    /// var tweenSettings = new TweenSettings&lt;Vector3&gt;(startValue: Vector3.zero, endValue: Vector3.one, duration: 1f, Ease.OutQuad, startDelay: 0.5f);
    /// Tween.Position(transform, tweenSettings);
    /// </code></example>
    [Serializable]
    public struct TweenSettings<T> where T: struct {
        [Tooltip(Constants.startFromCurrentTooltip)] public bool startFromCurrent;
        [Tooltip(Constants.startValueTooltip)] public T startValue;
        [Tooltip(Constants.endValueTooltip)] public T endValue;
        public TweenSettings settings;

        public TweenSettings(T endValue, TweenSettings settings) {
            startFromCurrent = true;
            startValue = default;
            this.endValue = endValue;
            this.settings = settings;
        }

        public TweenSettings(T startValue, T endValue, TweenSettings settings) {
            startFromCurrent = false;
            this.startValue = startValue;
            this.endValue = endValue;
            this.settings = settings;
        }

        public TweenSettings(T endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false, UpdateType updateType = default)
            : this(endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime, updateType)) {
        }

        public TweenSettings(T startValue, T endValue, float duration, Ease ease = Ease.Default, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false, UpdateType updateType = default)
            : this(startValue, endValue, new TweenSettings(duration, ease, cycles, cycleMode, startDelay, endDelay, useUnscaledTime, updateType)) {
        }

        public TweenSettings(T endValue, float duration, Easing customEase, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false, UpdateType updateType = default)
            : this(endValue, new TweenSettings(duration, customEase, cycles, cycleMode, startDelay, endDelay, useUnscaledTime, updateType)) {
        }

        public TweenSettings(T startValue, T endValue, float duration, Easing customEase, int cycles = 1, CycleMode cycleMode = CycleMode.Restart, float startDelay = 0, float endDelay = 0, bool useUnscaledTime = false, UpdateType updateType = default)
            : this(startValue, endValue, new TweenSettings(duration, customEase, cycles, cycleMode, startDelay, endDelay, useUnscaledTime, updateType)) {
        }

        /// <summary>Use this method to choose the direction of an animation based on the '<paramref name="toEndValue"/>' parameter.</summary>
        /// <param name="toEndValue">If true, returns TweenSettings to animate towards the <see cref="endValue"/>.<br/>
        /// If false, returns TweenSettings to animate towards the <see cref="startValue"/>.</param>
        /// <param name="_startFromCurrent">If true (default), the animation will start from the current value. Useful to prevent abrupt changes in case the previous animation if still running.<br/>
        /// If false, the animation will start from the <see cref="startValue"/> or the <see cref="endValue"/> depending on the <paramref name="toEndValue"/> parameter.</param>
        /// <example>For example, to animate a window to an opened or closed position:
        /// <code>
        /// // Tweak all animation properties from Inspector
        /// [SerializeField] TweenSettings&lt;Vector3&gt; windowAnimationSettings;
        /// public void SetWindowOpened(bool isOpened) {
        ///     // Pass isOpened to the WithDirection() method to animate the window towards opened or closed position
        ///     Tween.LocalPosition(transform, windowAnimationSettings.WithDirection(toEndValue: isOpened));
        /// }
        /// </code></example>
        public
            #if UNITY_2020_2_OR_NEWER
            readonly
            #endif
            TweenSettings<T> WithDirection(bool toEndValue, bool _startFromCurrent = true) {
            if (startFromCurrent) {
                Debug.LogWarning(nameof(startFromCurrent) + " is already enabled on this TweenSettings. The " + nameof(WithDirection) + "() should be called on the TweenSettings once to choose the direction.");
            }
            var result = this;
            result.startFromCurrent = _startFromCurrent;
            if (toEndValue) {
                return result;
            }
            (result.startValue, result.endValue) = (result.endValue, result.startValue);
            return result;
        }

        /// <summary> Similar to <see cref="WithDirection(bool,bool)"/>, but intended for use with custom tweens.<br/>
        /// Custom tweens (Tween.Custom) don't know the current value of the animated property, so this method addresses this limitation.</summary>
        /// <param name="currentValue">Pass the current value of the animated property to start the tween from the current value and prevent abrupt changes.</param>
        /// <example><code> 
        /// [SerializeField] TweenSettings&lt;float&gt; tweenSettings;
        /// float currentValue; 
        /// public void AnimateCustomField(bool toEndValue) {
        ///     Tween.Custom(this, tweenSettings.WithDirection(toEndValue, currentValue), (_this, val) =&gt; _this.currentValue = val);
        /// }
        /// </code></example>
        public
            #if UNITY_2020_2_OR_NEWER
            readonly
            #endif
            TweenSettings<T> WithDirection(bool toEndValue, T currentValue) {
                var result = this;
                if (result.startFromCurrent) {
                    result.startFromCurrent = false;
                    Debug.LogWarning(Constants.customTweensDontSupportStartFromCurrentWarning);
                }
                result.startValue = currentValue;
                result.endValue = toEndValue ? endValue : startValue;
                return result;
            }
    }
}