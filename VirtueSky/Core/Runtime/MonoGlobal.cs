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
        internal event Action OnTick;
        internal event Action OnFixedTick;
        internal event Action OnLateTick;

        #region Sub / UnSub For Update Procresses

        internal void AddTick(IEntity tick)
        {
            OnTick += tick.Tick;
        }

        internal void AddTick(Action action)
        {
            OnTick += action;
        }

        internal void AddFixedTick(IEntity fixedTick)
        {
            OnFixedTick += fixedTick.FixedTick;
        }

        internal void AddFixedTick(Action action)
        {
            OnFixedTick += action;
        }

        internal void AddLateTick(IEntity lateTick)
        {
            OnLateTick += lateTick.LateTick;
        }

        internal void AddLateTick(Action action)
        {
            OnLateTick += action;
        }

        internal void RemoveTick(IEntity tick)
        {
            OnTick -= tick.Tick;
        }

        internal void RemoveTick(Action action)
        {
            OnTick -= action;
        }

        internal void RemoveFixedTick(IEntity fixedTick)
        {
            OnFixedTick -= fixedTick.FixedTick;
        }

        internal void RemoveFixedTick(Action action)
        {
            OnFixedTick -= action;
        }

        internal void RemoveLateTick(IEntity lateTick)
        {
            OnLateTick -= lateTick.LateTick;
        }

        internal void RemoveLateTick(Action action)
        {
            OnLateTick -= action;
        }

        #endregion

        #region Update Handle

        private void Update()
        {
            OnTick?.Invoke();
            UpdateAllDelayHandle();

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
            OnFixedTick?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateTick?.Invoke();
        }

        #endregion

        #region App Handle

        private void OnApplicationFocus(bool hasFocus)
        {
            OnGameFocus?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnGamePause?.Invoke(pauseStatus);
            if (pauseStatus && GameData.IsAutoSave)
            {
                GameData.Save();
            }
        }

        private void OnApplicationQuit()
        {
            OnGameQuit?.Invoke();
            if (GameData.IsAutoSave)
            {
                GameData.Save();
            }
        }

        #endregion

        #region delay handle

        private List<DelayHandle> _timers = new();

        // buffer adding timers so we don't edit a collection during iteration
        private List<DelayHandle> _timersToAdd = new();
        //private int _fixedFrameCount;

        internal void RegisterDelayHandle(DelayHandle delayHandle)
        {
            _timersToAdd.Add(delayHandle);
        }

        internal void CancelAllDelayHandle()
        {
            foreach (var timer in _timers)
            {
                timer.Cancel();
            }

            _timers = new List<DelayHandle>();
            _timersToAdd = new List<DelayHandle>();
        }

        internal void PauseAllDelayHandle()
        {
            foreach (var timer in _timers)
            {
                timer.Pause();
            }
        }

        internal void ResumeAllDelayHandle()
        {
            foreach (var timer in _timers)
            {
                timer.Resume();
            }
        }

        private void UpdateAllDelayHandle()
        {
            if (_timersToAdd.Count > 0)
            {
                _timers.AddRange(_timersToAdd);
                _timersToAdd.Clear();
            }

            foreach (var timer in _timers)
            {
                timer.Update();
            }

            _timers.RemoveAll(t => t.IsDone);
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