using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Internal;
using VirtueSky.Core;

namespace VirtueSky.Global
{
    public struct App
    {
        private static MonoGlobal _monoGlobal;

        public static void InitMonoGlobalComponent(MonoGlobal monoGlobal)
        {
            App._monoGlobal = monoGlobal;
        }

        public static void AddPauseCallback(Action<bool> callback)
        {
            _monoGlobal.OnGamePause -= callback;
            _monoGlobal.OnGamePause += callback;
        }

        public static void RemovePauseCallback(Action<bool> callback)
        {
            _monoGlobal.OnGamePause -= callback;
        }

        public static void AddFocusCallback(Action<bool> callback)
        {
            _monoGlobal.OnGameFocus -= callback;
            _monoGlobal.OnGameFocus += callback;
        }

        public static void RemoveFocusCallback(Action<bool> callback)
        {
            _monoGlobal.OnGameFocus -= callback;
        }

        public static void AddQuitCallback(Action callback)
        {
            _monoGlobal.OnGameQuit -= callback;
            _monoGlobal.OnGameQuit += callback;
        }

        public static void RemoveQuitCallback(Action callback)
        {
            _monoGlobal.OnGameQuit -= callback;
        }

        #region Update

        public static void SubTick(IEntity tick)
        {
            _monoGlobal.AddTickProcess(tick);
        }

        public static void SubFixedTick(IEntity fixedTick)
        {
            _monoGlobal.AddFixedTickProcess(fixedTick);
        }

        public static void SubLateTick(IEntity lateTick)
        {
            _monoGlobal.AddLateTickProcess(lateTick);
        }

        public static void UnSubTick(IEntity tick)
        {
            _monoGlobal.RemoveTickProcess(tick);
        }

        public static void UnSubFixedTick(IEntity fixedTick)
        {
            _monoGlobal.RemoveFixedTickProcess(fixedTick);
        }

        public static void UnSubLateTick(IEntity lateTick)
        {
            _monoGlobal.RemoveLateTickProcess(lateTick);
        }

        #endregion

        #region Effective

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(IEnumerator routine) => _monoGlobal.StartCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value) => _monoGlobal.StartCoroutineImpl(methodName, value);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(string methodName) => _monoGlobal.StartCoroutineImpl(methodName);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(IEnumerator routine) => _monoGlobal.StopCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(Coroutine routine) => _monoGlobal.StopCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(string methodName) => _monoGlobal.StopCoroutineImpl(methodName);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopAllCoroutine() => _monoGlobal.StopAllCoroutinesImpl();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action ToMainThread(Action action) => _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T> ToMainThread<T>(Action<T> action) => _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2> ToMainThread<T1, T2>(Action<T1, T2> action) => _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3> ToMainThread<T1, T2, T3>(Action<T1, T2, T3> action) => _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void RunOnMainThread(Action action) => _monoGlobal.RunOnMainThreadImpl(action);

        #endregion
    }
}