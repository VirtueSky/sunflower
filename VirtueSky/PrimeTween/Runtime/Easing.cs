using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    /// <summary>
    /// A wrapper struct that encapsulates three available easing methods: standard Ease, AnimationCurve, or Parametric Easing.<br/>
    /// Use static methods to create an Easing struct, for example: Easing.Standard(Ease.OutBounce), Easing.Curve(animationCurve),
    /// Easing.Elastic(strength, period), etc.
    /// </summary>
    [PublicAPI]
    public readonly struct Easing {
        internal readonly Ease ease;
        internal readonly AnimationCurve curve;
        internal readonly ParametricEase parametricEase;
        internal readonly float parametricEaseStrength;
        internal readonly float parametricEasePeriod;

        Easing(ParametricEase type, float strength, float period = float.NaN) {
            ease = Ease.Custom;
            curve = null;
            parametricEase = type;
            parametricEaseStrength = strength;
            parametricEasePeriod = period;
        }

        Easing(Ease ease, [CanBeNull] AnimationCurve curve) {
            if (ease == Ease.Custom) {
                if (curve == null || !TweenSettings.ValidateCustomCurveKeyframes(curve)) {
                    Debug.LogError("Ease is Ease.Custom, but AnimationCurve is not configured correctly. Using Ease.Default instead.");
                    ease = Ease.Default;
                }
            }
            this.ease = ease;
            this.curve = curve;
            parametricEase = ParametricEase.None;
            parametricEaseStrength = float.NaN;
            parametricEasePeriod = float.NaN;
        }

        public static implicit operator Easing(Ease ease) => Standard(ease);

        /// <summary>Standard Robert Penner's easing method. Or simply use Ease enum instead.</summary>
        public static Easing Standard(Ease ease) {
            Assert.AreNotEqual(Ease.Custom, ease);
            if (ease == Ease.Default) {
                ease = PrimeTweenConfig.defaultEase;
            }
            return new Easing(ease, null);
        }

        public static implicit operator Easing([NotNull] AnimationCurve curve) => Curve(curve);

        /// <summary>AnimationCurve to use as an easing function. Or simply use AnimationCurve instead.</summary>
        public static Easing Curve([NotNull] AnimationCurve curve) => new Easing(Ease.Custom, curve);

        /// <summary>Customizes the bounce <see cref="strength"/> of Ease.OutBounce.</summary>
        public static Easing Bounce(float strength) => new Easing(ParametricEase.Bounce, strength);

        /// <summary>Customizes the exact <see cref="amplitude"/> of the first bounce in meters/angles.</summary>
        public static Easing BounceExact(float amplitude) => new Easing(ParametricEase.BounceExact, amplitude);

        /// <summary>Customizes the overshoot <see cref="strength"/> of Ease.OutBack.</summary>
        public static Easing Overshoot(float strength) => new Easing(ParametricEase.Overshoot, strength * StandardEasing.backEaseConst);

        /// <summary>Customizes the <see cref="strength"/> and oscillation <see cref="period"/> of Ease.OutElastic.</summary>
        public static Easing Elastic(float strength, float period = StandardEasing.defaultElasticEasePeriod) {
            if (strength < 1) {
                strength = Mathf.Lerp(0.2f, 1f, strength); // remap strength to limit decayFactor
            }
            return new Easing(ParametricEase.Elastic, strength, Mathf.Max(0.1f, period));
        }

        internal static float Evaluate(float t, ParametricEase parametricEase, float strength, float period, float duration) {
            switch (parametricEase) {
                case ParametricEase.Overshoot:
                    t -= 1.0f;
                    return t * t * ((strength + 1) * t + strength) + 1.0f;
                case ParametricEase.Elastic:
                    const float twoPi = 2 * Mathf.PI;
                    float decayFactor;
                    if (strength >= 1) {
                        decayFactor = 1f;
                    } else {
                        decayFactor = 1 / strength;
                        strength = 1;
                    }
                    float decay = Mathf.Pow(2, -10f * t * decayFactor);
                    if (duration == 0) {
                        return 1;
                    }
                    period /= duration;
                    float phase = period / twoPi * Mathf.Asin(1f / strength);
                    return t > 0.9999f ? 1 : strength * decay * Mathf.Sin((t - phase) * twoPi / period) + 1f;
                case ParametricEase.Bounce:
                    return Bounce(t, strength);
                case ParametricEase.BounceExact:
                case ParametricEase.None:
                default:
                    throw new System.Exception();
            }
        }

        internal static float Evaluate(float t, [NotNull] ReusableTween tween) {
            var settings = tween.settings;
            var parametricEase = settings.parametricEase;
            var strength = settings.parametricEaseStrength;
            if (parametricEase == ParametricEase.BounceExact) {
                var fullAmplitude = tween.propType == PropType.Quaternion ?
                    Quaternion.Angle(tween.startValue.QuaternionVal, tween.endValue.QuaternionVal) :
                    tween.diff.Vector4Val.magnitude;
                // todo account for double
                /*double calcFullAmplitude() {
                    switch (tween.propType) {
                        case PropType.Quaternion:
                            return Quaternion.Angle(tween.startValue.QuaternionVal, tween.endValue.QuaternionVal);
                        case PropType.Double:
                            return tween.startValue.DoubleVal - tween.endValue.DoubleVal;
                        default:
                            return tween.diff.Vector4Val.magnitude;
                    }
                }*/
                float strengthFactor = fullAmplitude < 0.0001f ? 1f : 1f / (fullAmplitude * (1f - firstBounceAmpl));
                return Bounce(t, strength * strengthFactor);
            }
            return Evaluate(t, parametricEase, strength, settings.parametricEasePeriod, settings.duration);
        }

        const float firstBounceAmpl = 0.75f;
        static float Bounce(float t, float strength) {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            if (t < 1 / d1) {
                return n1 * t * t;
            }
            return 1 - (1 - bounce()) * strength;
            float bounce() {
                if (t < 2 / d1) {
                    return n1 * (t -= 1.5f / d1) * t + firstBounceAmpl;
                }
                if (t < 2.5 / d1) {
                    return n1 * (t -= 2.25f / d1) * t + 0.9375f;
                }
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }

        #if PRIME_TWEEN_DOTWEEN_ADAPTER
        /// Can't be public API because ParametricEase.BounceExact can only be evaluated with these params: propType, startValue, endValue
        /// <see cref="Evaluate(float,PrimeTween.ReusableTween)"/>
        internal float Evaluate(float interpolationFactor) {
            if (ease == Ease.Custom) {
                if (parametricEase != ParametricEase.None) {
                    Assert.AreNotEqual(ParametricEase.BounceExact, parametricEase);
                    return Evaluate(interpolationFactor, parametricEase, parametricEaseStrength, parametricEasePeriod, 1f);
                }
                Assert.IsNull(curve);
                return curve.Evaluate(interpolationFactor);
            }
            return Evaluate(interpolationFactor, ease);
        }
        #endif

        public static float Evaluate(float interpolationFactor, Ease ease) {
            switch (ease) {
                case Ease.Custom:
                    Debug.LogError("Ease.Custom is an invalid type for Easing.Evaluate(). Please choose another Ease type instead.");
                    return interpolationFactor;
                case Ease.Default:
                    return StandardEasing.Evaluate(interpolationFactor, PrimeTweenManager.Instance.defaultEase);
                default:
                    return StandardEasing.Evaluate(interpolationFactor, ease);
            }
        }
    }

    internal enum ParametricEase {
        None = 0,
        Overshoot = 5,
        Bounce = 7,
        Elastic = 11,
        BounceExact
    }
}
