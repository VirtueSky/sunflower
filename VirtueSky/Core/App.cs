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

        internal static void SubTick(IEntity tick)
        {
            _monoGlobal.AddTick(tick);
        }

        public static void SubTick(Action action)
        {
            _monoGlobal.AddTick(action);
        }

        internal static void SubFixedTick(IEntity fixedTick)
        {
            _monoGlobal.AddFixedTick(fixedTick);
        }

        public static void SubFixedTick(Action action)
        {
            _monoGlobal.AddFixedTick(action);
        }

        internal static void SubLateTick(IEntity lateTick)
        {
            _monoGlobal.AddLateTick(lateTick);
        }

        public static void SubLateTick(Action action)
        {
            _monoGlobal.AddLateTick(action);
        }

        internal static void UnSubTick(IEntity tick)
        {
            _monoGlobal.RemoveTick(tick);
        }

        public static void UnSubTick(Action action)
        {
            _monoGlobal.RemoveTick(action);
        }

        internal static void UnSubFixedTick(IEntity fixedTick)
        {
            _monoGlobal.RemoveFixedTick(fixedTick);
        }

        public static void UnSubFixedTick(Action action)
        {
            _monoGlobal.RemoveFixedTick(action);
        }

        internal static void UnSubLateTick(IEntity lateTick)
        {
            _monoGlobal.RemoveLateTick(lateTick);
        }

        public static void UnSubLateTick(Action action)
        {
            _monoGlobal.RemoveLateTick(action);
        }

        #endregion

        #region DelayHandle

        /// <summary>
        /// Delay call
        /// </summary>
        /// <param name="duration">The duration to wait before the DelayHandle fires.</param>
        /// <param name="onComplete">The action to run when the DelayHandle elapses.</param>
        /// <param name="onUpdate">A function to call each tick of the DelayHandle. Takes the number of seconds elapsed since
        /// the start of the current cycle.</param>
        /// <param name="isLooped">Whether the DelayHandle should restart after executing.</param>
        /// <param name="useRealTime">Whether the DelayHandle uses real-time(not affected by slow-mo or pausing) or
        /// game-time(affected by time scale changes).</param>
        /// <returns></returns>
        public static DelayHandle Delay(float duration, Action onComplete, Action<float> onUpdate = null,
            bool isLooped = false, bool useRealTime = false)
        {
            var timer = new DelayHandle(duration,
                onComplete,
                onUpdate,
                isLooped,
                useRealTime,
                null);
            _monoGlobal.RegisterDelayHandle(timer);
            return timer;
        }


        /// <summary>
        /// Safe Delay call when it had target, progress delay will be cancel when target was destroyed
        /// </summary>
        /// <param name="duration">The duration to wait before the DelayHandle fires.</param>
        /// <param name="onComplete">The action to run when the DelayHandle elapses.</param>
        /// <param name="onUpdate">A function to call each tick of the DelayHandle. Takes the number of seconds elapsed since
        /// the start of the current cycle.</param>
        /// <param name="isLooped">Whether the DelayHandle should restart after executing.</param>
        /// <param name="useRealTime">Whether the DelayHandle uses real-time(not affected by slow-mo or pausing) or
        /// game-time(affected by time scale changes).</param>
        /// <param name="target">The target (behaviour) to attach this DelayHandle to.</param>
        public static DelayHandle Delay(
            MonoBehaviour target,
            float duration,
            Action onComplete,
            Action<float> onUpdate = null,
            bool isLooped = false,
            bool useRealTime = false)
        {
            var timer = new DelayHandle(duration,
                onComplete,
                onUpdate,
                isLooped,
                useRealTime,
                target);
            _monoGlobal.RegisterDelayHandle(timer);
            return timer;
        }

        public static void CancelDelay(DelayHandle delayHandle)
        {
            delayHandle?.Cancel();
        }

        public static void PauseDelay(DelayHandle delayHandle)
        {
            delayHandle?.Pause();
        }

        public static void ResumeDelay(DelayHandle delayHandle)
        {
            delayHandle?.Resume();
        }

        public static void CancelAllDelay()
        {
            _monoGlobal.CancelAllDelayHandle();
        }

        public static void PauseAllDelay()
        {
            _monoGlobal.PauseAllDelayHandle();
        }

        public static void ResumeAllDelay()
        {
            _monoGlobal.ResumeAllDelayHandle();
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