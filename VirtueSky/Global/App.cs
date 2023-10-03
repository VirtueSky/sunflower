using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Internal;

namespace VirtueSky.Global
{
    public struct App
    {
        private static MonoGlobalComponent monoGlobalComponent;

        public static void InitMonoGlobalComponent(MonoGlobalComponent _monoGlobalComponent)
        {
            App.monoGlobalComponent = _monoGlobalComponent;
        }

        public static void AddPauseCallback(Action<bool> callback)
        {
            monoGlobalComponent.OnGamePause -= callback;
            monoGlobalComponent.OnGamePause += callback;
        }

        public static void RemovePauseCallback(Action<bool> callback)
        {
            monoGlobalComponent.OnGamePause -= callback;
        }

        public static void AddFocusCallback(Action<bool> callback)
        {
            monoGlobalComponent.OnGameFocus -= callback;
            monoGlobalComponent.OnGameFocus += callback;
        }

        public static void RemoveFocusCallback(Action<bool> callback)
        {
            monoGlobalComponent.OnGameFocus -= callback;
        }

        public static void AddQuitCallback(Action callback)
        {
            monoGlobalComponent.OnGameQuit -= callback;
            monoGlobalComponent.OnGameQuit += callback;
        }

        public static void RemoveQuitCallback(Action callback)
        {
            monoGlobalComponent.OnGameQuit -= callback;
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(IEnumerator routine) => monoGlobalComponent.StartCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value) => monoGlobalComponent.StartCoroutineImpl(methodName, value);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(string methodName) => monoGlobalComponent.StartCoroutineImpl(methodName);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(IEnumerator routine) => monoGlobalComponent.StopCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(string methodName) => monoGlobalComponent.StopCoroutineImpl(methodName);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopAllCoroutine() => monoGlobalComponent.StopAllCoroutinesImpl();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action ToMainThread(Action action) => monoGlobalComponent.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T> ToMainThread<T>(Action<T> action) => monoGlobalComponent.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2> ToMainThread<T1, T2>(Action<T1, T2> action) => monoGlobalComponent.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3> ToMainThread<T1, T2, T3>(Action<T1, T2, T3> action) => monoGlobalComponent.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void RunOnMainThread(Action action) => monoGlobalComponent.RunOnMainThreadImpl(action);
    }
}