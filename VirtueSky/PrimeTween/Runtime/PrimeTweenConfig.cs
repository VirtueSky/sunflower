using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    /// Global PrimeTween configuration.
    [PublicAPI]
    public static partial class PrimeTweenConfig {
        internal static PrimeTweenManager Instance {
            get {
                #if UNITY_EDITOR
                Assert.IsFalse(Constants.noInstance, Constants.editModeWarning);
                #endif
                return PrimeTweenManager.Instance;
            }
        }

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
            var instance = PrimeTweenManager.Instance; // should use PrimeTweenManager.Instance because Instance property has a built-in null check 
            if (instance == null) {
                PrimeTweenManager.customInitialCapacity = capacity;
            } else {
                instance.SetTweensCapacity(capacity);
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
    }
}