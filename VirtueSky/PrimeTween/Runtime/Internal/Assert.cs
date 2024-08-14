using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    internal static class Assert {
        internal static void LogError(string msg, long id, [CanBeNull] Object context = null) {
            Debug.LogError(TryAddStackTrace(msg, id), context);
        }

        internal static void LogWarning(string msg, long id, [CanBeNull] Object context = null) {
            Debug.LogWarning(TryAddStackTrace(msg, id), context);
        }

        [CanBeNull, PublicAPI]
        static string TryAddStackTrace([CanBeNull] string msg, long tweenId) {
            #if UNITY_ASSERTIONS
                #if PRIME_TWEEN_SAFETY_CHECKS
                if (tweenId == 0) {
                    msg += "\nTween is not created (id == 0).\n";
                } else {
                    msg += $"\nTween (id {tweenId}) creation stack trace:\n{StackTraces.Get(tweenId)}";
                }
                #else
                msg += "\nAdd 'PRIME_TWEEN_SAFETY_CHECKS' to 'Project Settings/Player/Scripting Define Symbols' to see which tween produced this error (works only in Development Builds).\n";
                #endif
            #endif
            return msg;
        }

        #if UNITY_ASSERTIONS && !PRIME_TWEEN_DISABLE_ASSERTIONS
        [ContractAnnotation("condition:false => halt")]
        internal static void IsTrue(bool condition, long? tweenId = null, string msg = null) => UnityEngine.Assertions.Assert.IsTrue(condition, AddStackTrace(!condition, msg, tweenId));
        internal static void AreEqual<T>(T expected, T actual, string msg = null) => UnityEngine.Assertions.Assert.AreEqual(expected, actual, msg);
        internal static void AreNotEqual<T>(T expected, T actual, string msg = null) => UnityEngine.Assertions.Assert.AreNotEqual(expected, actual, msg);
        internal static void IsFalse(bool condition, string msg = null) => UnityEngine.Assertions.Assert.IsFalse(condition, msg);
        [ContractAnnotation("value:null => halt")]
        internal static void IsNotNull<T>(T value, string msg = null) where T : class => UnityEngine.Assertions.Assert.IsNotNull(value, msg);
        internal static void IsNull<T>(T value, string msg = null) where T : class => UnityEngine.Assertions.Assert.IsNull(value, msg);
        [CanBeNull]
        static string AddStackTrace(bool add, [CanBeNull] string msg, long? tweenId) {
            if (add && tweenId.HasValue) {
                return TryAddStackTrace(msg, tweenId.Value);
            }
            return msg;
        }
        #else
        const string DUMMY = "_";
        // ReSharper disable UnusedParameter.Global
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsTrue(bool condition, long? tweenId = null, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void AreEqual<T>(T expected, T actual, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void AreNotEqual<T>(T expected, T actual, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsFalse(bool condition, string msg = null) {}
        [ContractAnnotation("value:null => halt")]
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsNotNull<T>(T value, string msg = null) where T : class {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsNull<T>(T value, string msg = null) where T : class {}
        #endif
    }
}
