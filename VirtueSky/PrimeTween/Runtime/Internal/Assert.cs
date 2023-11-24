using UnityAssert = UnityEngine.Assertions;

namespace PrimeTween
{
    internal static class Assert
    {
#if UNITY_ASSERTIONS && !PRIME_TWEEN_DISABLE_ASSERTIONS
        internal static void IsTrue(bool condition, string msg = null) => UnityAssert.Assert.IsTrue(condition, msg);
        internal static void AreEqual<T>(T expected, T actual, string msg = null) => UnityAssert.Assert.AreEqual(expected, actual, msg);
        internal static void AreNotEqual<T>(T expected, T actual, string msg = null) => UnityAssert.Assert.AreNotEqual(expected, actual, msg);
        internal static void IsFalse(bool condition, string msg = null) => UnityAssert.Assert.IsFalse(condition, msg);
        internal static void IsNotNull<T>(T value, string msg = null) where T : class => UnityAssert.Assert.IsNotNull(value, msg);
        internal static void IsNull<T>(T value, string msg = null) where T : class => UnityAssert.Assert.IsNull(value, msg);
#else
        const string DUMMY = "_";
        // ReSharper disable UnusedParameter.Global
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsTrue(bool condition, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void AreEqual<T>(T expected, T actual, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void AreNotEqual<T>(T expected, T actual, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsFalse(bool condition, string msg = null) {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsNotNull<T>(T value, string msg = null) where T : class {}
        [System.Diagnostics.Conditional(DUMMY)] internal static void IsNull<T>(T value, string msg = null) where T : class {}
#endif
    }
}