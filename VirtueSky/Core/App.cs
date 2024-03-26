using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Internal;

namespace VirtueSky.Core
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
            _monoGlobal.AddTick(tick);
        }

        public static void SubTick(Action action)
        {
            _monoGlobal.AddTick(action);
        }

        public static void SubFixedTick(IEntity fixedTick)
        {
            _monoGlobal.AddFixedTick(fixedTick);
        }

        public static void SubFixedTick(Action action)
        {
            _monoGlobal.AddFixedTick(action);
        }

        public static void SubLateTick(IEntity lateTick)
        {
            _monoGlobal.AddLateTick(lateTick);
        }

        public static void SubLateTick(Action action)
        {
            _monoGlobal.AddLateTick(action);
        }

        public static void UnSubTick(IEntity tick)
        {
            _monoGlobal.RemoveTick(tick);
        }

        public static void UnSubTick(Action action)
        {
            _monoGlobal.RemoveTick(action);
        }

        public static void UnSubFixedTick(IEntity fixedTick)
        {
            _monoGlobal.RemoveFixedTick(fixedTick);
        }

        public static void UnSubFixedTick(Action action)
        {
            _monoGlobal.RemoveFixedTick(action);
        }

        public static void UnSubLateTick(IEntity lateTick)
        {
            _monoGlobal.RemoveLateTick(lateTick);
        }

        public static void UnSubLateTick(Action action)
        {
            _monoGlobal.RemoveLateTick(action);
        }

        #endregion

        #region Effective

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(IEnumerator routine) => _monoGlobal.StartCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value) =>
            _monoGlobal.StartCoroutineImpl(methodName, value);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(string methodName) => _monoGlobal.StartCoroutineImpl(methodName);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(IEnumerator routine) => _monoGlobal.StopCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(Coroutine routine) => _monoGlobal.StopCoroutineImpl(routine);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(string methodName) => _monoGlobal.StopCoroutineImpl(methodName);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StopAllCoroutine() => _monoGlobal.StopAllCoroutinesImpl();

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action ToMainThread(Action action) => _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T> ToMainThread<T>(Action<T> action) => _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2> ToMainThread<T1, T2>(Action<T1, T2> action) =>
            _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3> ToMainThread<T1, T2, T3>(Action<T1, T2, T3> action) =>
            _monoGlobal.ToMainThreadImpl(action);

        [System.Runtime.CompilerServices.MethodImpl(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void RunOnMainThread(Action action) => _monoGlobal.RunOnMainThreadImpl(action);

        #endregion
    }
}