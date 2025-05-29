#if PRIME_TWEEN_SAFETY_CHECKS && UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    internal static class StackTraces {
        static readonly List<int> idToHash = new List<int>(1000);
        static readonly Dictionary<int, List<byte[]>> hashToTraces = new Dictionary<int, List<byte[]>>(100);
        static bool didWarn;

        /// https://github.com/Unity-Technologies/UnityCsReference/blob/6230ef8f9bed142ddf6a5e338d6e0faf3368d313/Runtime/Export/Scripting/StackTrace.cs#L31-L47
        internal static unsafe void Record(long id) {
            if (!didWarn && ENABLE_IL2CPP) {
                didWarn = true;
                Debug.LogWarning("PRIME_TWEEN_SAFETY_CHECKS is used in IL2CPP build, which has negative performance impact in Development Builds. " +
                                 "Please remove the PRIME_TWEEN_SAFETY_CHECKS from 'Project Settings/Player/Scripting Define Symbols' if you no longer need deep debugging support.");
            }
            if (id == 1) {
                idToHash.Clear();
                idToHash.Add(0);
            }
            const int bufLength = 16 * 1024;
            byte* buf = stackalloc byte[bufLength];
            int len;
            #if UNITY_2019_4_OR_NEWER
            len = Debug.ExtractStackTraceNoAlloc(buf, bufLength, Application.dataPath);
            #else
            Fallback();
            #endif
            if (len <= 0) {
                // ExtractStackTraceNoAlloc() doesn't work with IL2CPP, so use the allocating version instead
                Fallback();
            }
            void Fallback() {
                string trace = StackTraceUtility.ExtractStackTrace();
                fixed (char* chars = trace) {
                    Encoding.UTF8.GetEncoder().Convert(chars, trace.Length, buf, bufLength, true, out _, out len, out _);
                }
            }
            int hash = ComputeHash(buf, len);
            if (id != idToHash.Count) {
                Debug.LogError("PRIME_TWEEN_SAFETY_CHECKS doesn't support script hot reloading (Recompile And Continue Playing). Please remove the PRIME_TWEEN_SAFETY_CHECKS define."); // todo fix?
                return;
            }
            idToHash.Add(hash);
            if (hashToTraces.TryGetValue(hash, out var traces)) {
                if (!Contains(traces, buf, len)) {
                    traces.Add(BufToArray());
                }
            } else {
                hashToTraces.Add(hash, new List<byte[]> { BufToArray() });
            }

            byte[] BufToArray() {
                var result = new byte[len];
                for (int i = 0; i < len; i++) {
                    result[i] = buf[i];
                }
                return result;
            }
        }

        static bool ENABLE_IL2CPP {
            get {
                #if ENABLE_IL2CPP
                return true;
                #else
                return false;
                #endif
            }
        }

        static unsafe bool Contains([NotNull] List<byte[]> arrays, byte* data, int length) {
            foreach (var arr in arrays) {
                if (arr.Length == length) {
                    if (SequenceEqual()) {
                        return true;
                    }

                    bool SequenceEqual() {
                        for (int i = 0; i < length; i++) {
                            if (arr[i] != data[i]) {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /// https://stackoverflow.com/a/468084
        static unsafe int ComputeHash(byte* data, int length) {
            unchecked {
                const int p = 16777619;
                int hash = (int)2166136261;
                for (int i = 0; i < length; i++) {
                    hash = (hash ^ data[i]) * p;
                }
                return hash;
            }
        }

        [NotNull]
        internal static string Get(long id) {
            Assert.IsTrue(id < idToHash.Count);
            // todo limit the max number of stack traces or wrap them around max value
            bool isSuccess = hashToTraces.TryGetValue(idToHash[(int)id], out var traces);
            Assert.IsTrue(isSuccess);
            Assert.IsNotNull(traces);
            return string.Join("\n\n", traces.Select(bytes => {
                string str = Encoding.UTF8.GetString(bytes);
                Assert.IsFalse(string.IsNullOrEmpty(str));
                int i = 0;
                while (true) {
                    var prev = i;
                    i = str.IndexOf('\n', i);
                    if (i == -1) {
                        return str;
                    }
                    i++;
                    int j = str.IndexOf("PrimeTween.", i, StringComparison.Ordinal);
                    if (j != i) {
                        return str.Substring(prev);
                    }
                }
            }));
        }

        internal static void logDebugInfo() {
            Debug.Log($"idToHash.Count: {idToHash.Count}, hashToTraces.Count: {hashToTraces.Count}");
        }
    }
}
#endif
