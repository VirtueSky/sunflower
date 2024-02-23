using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using VirtueSky.DataStorage;
using VirtueSky.Inspector;

namespace VirtueSky.Core
{
    [EditorIcon("icon_csharp")]
    public class MonoGlobal : MonoBehaviour
    {
        private readonly List<Action> _toMainThreads = new();
        private volatile bool _isToMainThreadQueueEmpty = true;
        private List<Action> _localToMainThreads = new();
        internal event Action<bool> OnGamePause;
        internal event Action OnGameQuit;
        internal event Action<bool> OnGameFocus;
        readonly List<IEntity> tickProcesses = new List<IEntity>(1024);
        readonly List<IEntity> fixedTickProcesses = new List<IEntity>(512);
        readonly List<IEntity> lateTickProcesses = new List<IEntity>(256);

        #region Sub / UnSub For Update Procresses

        internal void AddTickProcess(IEntity tick)
        {
            tickProcesses.Add(tick);
        }

        internal void AddFixedTickProcess(IEntity fixedTick)
        {
            fixedTickProcesses.Add(fixedTick);
        }

        internal void AddLateTickProcess(IEntity lateTick)
        {
            lateTickProcesses.Add(lateTick);
        }

        internal void RemoveTickProcess(IEntity tick)
        {
            tickProcesses.Remove(tick);
        }

        internal void RemoveFixedTickProcess(IEntity fixedTick)
        {
            fixedTickProcesses.Remove(fixedTick);
        }

        internal void RemoveLateTickProcess(IEntity lateTick)
        {
            lateTickProcesses.Remove(lateTick);
        }

        #endregion

        #region Update Handle

        private void Update()
        {
            for (int i = 0; i < tickProcesses.Count; i++)
            {
                tickProcesses[i]?.Tick();
            }

            if (_isToMainThreadQueueEmpty) return;
            _localToMainThreads.Clear();
            lock (_toMainThreads)
            {
                _localToMainThreads.AddRange(_toMainThreads);
                _toMainThreads.Clear();
                _isToMainThreadQueueEmpty = true;
            }

            for (int i = 0; i < _localToMainThreads.Count; i++)
            {
                _localToMainThreads[i].Invoke();
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < fixedTickProcesses.Count; i++)
            {
                fixedTickProcesses[i]?.FixedTick();
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < lateTickProcesses.Count; i++)
            {
                lateTickProcesses[i]?.LateTick();
            }
        }

        #endregion

        #region App Handle

        private void OnApplicationFocus(bool hasFocus)
        {
            OnGamePause?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnGamePause?.Invoke(pauseStatus);
            if (pauseStatus)
            {
                GameData.Save();
            }
        }

        private void OnApplicationQuit()
        {
            OnGameQuit?.Invoke();
            GameData.Save();
        }

        #endregion

        #region Effective

        internal Coroutine StartCoroutineImpl(IEnumerator routine)
        {
            if (routine != null)
            {
                return StartCoroutine(routine);
            }

            return null;
        }

        internal Coroutine StartCoroutineImpl(string methodName, [DefaultValue("null")] object value)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                return StartCoroutine(methodName, value);
            }

            return null;
        }

        internal Coroutine StartCoroutineImpl(string methodName)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                return StartCoroutine(methodName);
            }

            return null;
        }

        internal void StopCoroutineImpl(IEnumerator routine)
        {
            if (routine != null) StopCoroutine(routine);
        }

        internal void StopCoroutineImpl(Coroutine routine)
        {
            if (routine != null) StopCoroutine(routine);
        }

        internal void StopCoroutineImpl(string methodName)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                StopCoroutine(methodName);
            }
        }

        internal void StopAllCoroutinesImpl()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Schedules the specifies action to be run on the main thread (game thread).
        /// The action will be invoked upon the next Unity Update event.
        /// </summary>
        /// <param name="action">Action.</param>
        internal void RunOnMainThreadImpl(Action action)
        {
            lock (_toMainThreads)
            {
                _toMainThreads.Add(action);
                _isToMainThreadQueueEmpty = false;
            }
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        /// <returns>The main thread.</returns>
        /// <param name="action">Act.</param>
        internal Action ToMainThreadImpl(Action action)
        {
            if (action == null) return delegate { };
            return () => RunOnMainThreadImpl(action);
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        /// <returns>The main thread.</returns>
        /// <param name="action">Act.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        internal Action<T> ToMainThreadImpl<T>(Action<T> action)
        {
            if (action == null) return delegate { };
            return (arg) => RunOnMainThreadImpl(() => action(arg));
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        /// <returns>The main thread.</returns>
        /// <param name="action">Act.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        internal Action<T1, T2> ToMainThreadImpl<T1, T2>(Action<T1, T2> action)
        {
            if (action == null) return delegate { };
            return (arg1, arg2) => RunOnMainThreadImpl(() => action(arg1, arg2));
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        /// <returns>The main thread.</returns>
        /// <param name="action">Act.</param>
        /// <typeparam name="T1">The 1st type parameter.</typeparam>
        /// <typeparam name="T2">The 2nd type parameter.</typeparam>
        /// <typeparam name="T3">The 3rd type parameter.</typeparam>
        internal Action<T1, T2, T3> ToMainThreadImpl<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            if (action == null) return delegate { };
            return (arg1, arg2, arg3) => RunOnMainThreadImpl(() => action(arg1, arg2, arg3));
        }

        #endregion
    }
}