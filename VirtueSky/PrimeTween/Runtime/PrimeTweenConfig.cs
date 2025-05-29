using System;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    /// Global PrimeTween configuration.
    [PublicAPI]
    public static partial class PrimeTweenConfig {
        internal static PrimeTweenManager Instance => PrimeTweenManager.Instance;

        /// <summary>
        /// If <see cref="PrimeTweenManager"/> instance is already created, <see cref="SetTweensCapacity"/> will allocate garbage,
        ///     so it's recommended to use it when no active gameplay is happening. For example, on game launch or when loading a level.
        /// <para>To set initial capacity before <see cref="PrimeTweenManager"/> is created, call <see cref="SetTweensCapacity"/> from a method
        /// with <see cref="RuntimeInitializeOnLoadMethodAttribute"/> and <see cref="RuntimeInitializeLoadType.BeforeSplashScreen"/>. See example below.</para>
        /// </summary>
        /// <example>
        /// <code>
        /// [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        /// static void beforeSplashScreen() {
        ///     PrimeTweenConfig.SetTweensCapacity(42);
        /// }
        /// </code>
        /// </example>
        public static void SetTweensCapacity(int capacity) {
            Assert.IsTrue(capacity >= 0);
            if (PrimeTweenManager.HasInstance) {
                PrimeTweenManager.Instance.SetTweensCapacity(capacity);
            } else {
                PrimeTweenManager.customInitialCapacity = capacity;
            }
        }

        public static Ease defaultEase {
            get => Instance.defaultEase;
            set {
                if (value == Ease.Custom || value == Ease.Default) {
                    Debug.LogError("defaultEase can't be Ease.Custom or Ease.Default.");
                    return;
                }
                Instance.defaultEase = value;
            }
        }

        public static UpdateType defaultUpdateType {
            get => new UpdateType(Instance.defaultUpdateType);
            set {
                if (value == UpdateType.Default) {
                    Debug.LogError(nameof(defaultUpdateType) + " can't be " + nameof(_UpdateType.Default) + ".");
                    return;
                }
                Instance.defaultUpdateType = value.enumValue;
            }
        }
        
        public static bool warnTweenOnDisabledTarget {
            set => Instance.warnTweenOnDisabledTarget = value;
        }
        
        public static bool warnZeroDuration {
            internal get => Instance.warnZeroDuration;
            set => Instance.warnZeroDuration = value;
        }

        public static bool warnStructBoxingAllocationInCoroutine {
            set => Instance.warnStructBoxingAllocationInCoroutine = value;
        }

        public static bool validateCustomCurves {
            set => Instance.validateCustomCurves = value;
        }

        public static bool warnBenchmarkWithAsserts {
            set => Instance.warnBenchmarkWithAsserts = value;
        }

        internal const bool defaultUseUnscaledTimeForShakes = false;

        public static bool warnEndValueEqualsCurrent {
            set => Instance.warnEndValueEqualsCurrent = value;
        }

        #if PRIME_TWEEN_EXPERIMENTAL
        public
        #endif
        static void ManualUpdate(UpdateType updateType, float? deltaTime = null, float? unscaledDeltaTime = null) {
            Instance.enabled = false;
            Instance.UpdateTweens(updateType.enumValue, deltaTime, unscaledDeltaTime);
        }

        #if PRIME_TWEEN_EXPERIMENTAL
        public
        #endif
        static void ManualUpdateApplyStartValues(UpdateType updateType) {
            Instance.enabled = false;
            Instance.ApplyStartValues(updateType.enumValue);
        }

        #if PRIME_TWEEN_EXPERIMENTAL
        public
        #else
        internal
        #endif
        static void ManualInitialize() {
            if (!PrimeTweenManager.HasInstance) {
                PrimeTweenManager.CreateInstanceAndDontDestroy();
            } else {
                const string error = "'" + nameof(PrimeTweenManager) + "' is already created. Use this method only to create '" + nameof(PrimeTweenManager) + "' before '" + nameof(RuntimeInitializeLoadType) + "." + nameof(RuntimeInitializeLoadType.BeforeSceneLoad) + "'.";
                Debug.LogError(error);
            }
        }
    }
}
