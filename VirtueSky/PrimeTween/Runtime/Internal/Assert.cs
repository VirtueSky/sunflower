using JetBrains.Annotations;
using UnityAssert = UnityEngine.Assertions;

namespace PrimeTween {
    internal static class Assert {
        [CanBeNull, PublicAPI]
        internal static string TryAddStackTrace([CanBeNull] string msg, int tweenId) {
            #if UNITY_ASSERTIONS && UNITY_2019_4_OR_NEWER
                #if PRIME_TWEEN_SAFETY_CHECKS 
                if (tweenId == 0) {
                    msg += "Tween is not created (id == 0).\n";
                } else {
                    msg += $"\nTween (id {tweenId}) creation stack trace:\n{StackTraces.Get(tweenId)}";
                }
                #else
                msg += Constants.addSafetyCheckDefineForMoreInfo;
                #endif
            #endif
            return msg;
        }
        
        #if UNITY_ASSERTIONS && !PRIME_TWEEN_DISABLE_ASSERTIONS
        internal static void IsTrue(bool condition, int? tweenId = null, string msg = null) => UnityAssert.Assert.IsTrue(condition, AddStackTrace(!condition, msg, tweenId));
        internal static void AreEqual<T>(T expected, T actual, string msg = null) => UnityAssert.Assert.AreEqual(expected, actual, msg);
        internal static void AreNotEqual<T>(T expected, T actual, string msg = null) => UnityAssert.Assert.AreNotEqual(expected, actual, msg);
        internal static void IsFalse(bool condition, string msg = null) => UnityAssert.Assert.IsFalse(condition, msg);
        [ContractAnnotation("value:null => halt")]
        internal static void IsNotNull<T>(T value, string msg = null) where T : class => UnityAssert.Assert.IsNotNull(value, msg);
        internal static void IsNull<T>(T value, string msg = null) where T : class => UnityAssert.Assert.IsNull(value, msg);
        [CanBeNull]
        static string AddStackTrace(bool add, [CanBeNull] string msg, int? tweenId) {
            if (add && tweenId.HasValue) {
                return TryAddStackTrace(msg, tweenId.Value);
            }
            return msg;
        }
        #else
        const string DUMMY = "_";
        // ReSharper disable UnusedParameter.Global
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsTrue(bool condition, int? tweenId = null, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void AreEqual<T>(T expected, T actual, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void AreNotEqual<T>(T expected, T actual, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsFalse(bool condition, string msg = null) {}
        [ContractAnnotation("value:null => halt")]
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsNotNull<T>(T value, string msg = null) where T : class {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsNull<T>(T value, string msg = null) where T : class {}
        #endif
    }
}