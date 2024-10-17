using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.UtilsEditor
{
    /// <summary>
    /// Represents a coroutine that has been started running in the editor.
    /// <para>
    /// Also offers static methods for <see cref="Start">starting</see> and <see cref="Stop()">stopping</see> coroutines.
    /// </para>
    /// </summary>
    public sealed class EditorCoroutine : YieldInstruction
    {
        public static event Action<EditorCoroutine> Stopped;
        private static readonly List<EditorCoroutine> Running = new(1);
        private static readonly FieldInfo WaitForSecondsSecondsField;
        private static readonly MethodInfo InvokeCompletionEventMethod;

        private readonly Stack<object> _yielding = new(1);
        private double _waitUntil;

        public bool IsFinished => _yielding.Count == 0;

        static EditorCoroutine()
        {
            WaitForSecondsSecondsField = typeof(WaitForSeconds).GetField("m_Seconds",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (WaitForSecondsSecondsField is null) Debug.LogWarning("Field WaitForSeconds.m_Seconds not found.");
            InvokeCompletionEventMethod = typeof(AsyncOperation).GetMethod("InvokeCompletionEvent",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (InvokeCompletionEventMethod is null)
                Debug.LogWarning("Method AsyncOperation.InvokeCompletionEvent not found.");
        }

        private EditorCoroutine(IEnumerator routine) => _yielding.Push(routine);

        /// <summary>
        /// Starts the provided <paramref name="coroutine"/>.
        /// </summary>
        /// <param name="coroutine"> The coroutine to start. </param>
        /// <returns>
        /// A reference to the started <paramref name="coroutine"/>.
        /// <para>
        /// This reference can be passed to <see cref="Stop()"/> to stop
        /// the execution of the coroutine.
        /// </para>
        /// </returns>
        public static EditorCoroutine Start(IEnumerator coroutine)
        {
            if (Running.Count == 0) EditorApplication.update += UpdateRunningCoroutines;

            var editorCoroutine = new EditorCoroutine(coroutine);
            Running.Add(editorCoroutine);
            return editorCoroutine;
        }

        /// <summary>
        /// Stops the coroutine that is running in edit mode.
        /// </summary>
        public void Stop()
        {
            Running.Remove(this);

            if (Running.Count == 0) EditorApplication.update -= UpdateRunningCoroutines;

            Stopped?.Invoke(this);
        }

        /// <summary>
        /// Stops the <paramref name="coroutine"/> that is running.
        /// </summary>
        /// <param name="coroutine"> The <see cref="IEnumerator">coroutine</see> to stop. </param>
        public static void Stop(IEnumerator coroutine)
        {
            foreach (var editorCoroutine in Running)
            {
                int counter = editorCoroutine._yielding.Count;

                foreach (object item in editorCoroutine._yielding)
                {
                    if (counter == 1 && item == coroutine)
                    {
                        editorCoroutine.Stop();
                        return;
                    }

                    counter--;
                }
            }
        }

        /// <summary>
        /// Stops all coroutines that have been started using <see cref="Start"/> that are currently still running.
        /// </summary>
        public static void StopAll()
        {
            Running.Clear();
            EditorApplication.update -= UpdateRunningCoroutines;
        }

        /// <summary>
        /// Continuously advances all currently running coroutines to their
        /// next phases until all of them have reached the end.
        /// <para>
        /// Note that this locks the current thread until all running coroutines have fully finished.
        /// If any coroutine contains <see cref="CustomYieldInstruction">CustomYieldInstructions</see>
        /// that take a long time to finish (or never finish in edit mode) this can cause the editor
        /// to freeze for the same duration.
        /// </para>
        /// </summary>
        public static void MoveAllToEnd()
        {
            int count = Running.Count;
            while (count > 0)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    Running[i].MoveNext(true);
                }

                count = Running.Count;
            }
        }

        /// <summary>
        /// Advances all currently running coroutine to their next phase.
        /// </summary>
        /// <param name="skipWaits">
        /// (Optional) If <see langword="true"/> then yield instructions
        /// <see cref="WaitForSeconds"/> and <see cref="WaitForSecondsRealtime"/> are skipped.
        /// </param>
        /// <returns> <see langword="true"/> if any coroutines are still running, <see langword="false"/> if all have finished. </returns>
        public static bool MoveAllNext(bool skipWaits = false)
        {
            for (int i = Running.Count - 1; i >= 0; i--)
            {
                Running[i].MoveNext(skipWaits);
            }

            return Running.Count > 0;
        }

        private static void UpdateRunningCoroutines()
        {
            for (int i = Running.Count - 1; i >= 0; i--)
            {
                Running[i].MoveNext();
            }
        }

        /// <summary>
        /// Advances the coroutine to the next phase.
        /// </summary>
        /// <param name="skipWaits">
        /// (Optional) If <see langword="true"/> then yield instructions
        /// <see cref="WaitForSeconds"/> and <see cref="WaitForSecondsRealtime"/> are skipped.
        /// </param>
        /// <returns> <see langword="true"/> if coroutine is still running, <see langword="false"/> if it has finished. </returns>
        public bool MoveNext(bool skipWaits = false)
        {
            if (EditorApplication.timeSinceStartup < _waitUntil && !skipWaits) return true;

            if (_yielding.Count == 0)
            {
                Stop();
                return false;
            }

            object current = _yielding.Peek();

            if (current is IEnumerator enumerator)
            {
                bool keepWaiting;
                try
                {
                    keepWaiting = enumerator.MoveNext();
                }
                catch
                {
                    keepWaiting = true;
                }

                if (!keepWaiting)
                {
                    _yielding.Pop();
                    if (_yielding.Count > 0) return true;

                    Stop();
                    return false;
                }

                _yielding.Push(enumerator.Current);
                return true;
            }

            else if (current is CustomYieldInstruction yieldInstruction)
            {
                bool keepWaiting;
                try
                {
                    keepWaiting = yieldInstruction.keepWaiting;
                }
                catch
                {
                    keepWaiting = true;
                }

                if (!skipWaits)
                {
                    if (!keepWaiting) _yielding.Pop();

                    return true;
                }
            }

            else if (current is WaitForSeconds waitForSeconds)
            {
                _waitUntil = EditorApplication.timeSinceStartup +
                             (float)WaitForSecondsSecondsField.GetValue(waitForSeconds);
                if (!skipWaits)
                {
                    _yielding.Pop();
                    return true;
                }
            }

            else if (current is WaitForSecondsRealtime waitForSecondsRealtime)
            {
                _waitUntil = EditorApplication.timeSinceStartup + waitForSecondsRealtime.waitTime;
                if (!skipWaits)
                {
                    _yielding.Pop();
                    return true;
                }
            }

            else if (current is WaitForEndOfFrame or WaitForFixedUpdate)
            {
                if (!skipWaits)
                {
                    _yielding.Pop();
                    return true;
                }
            }

            else if (current is AsyncOperation { isDone: false } asyncOperation)
            {
                if (!skipWaits) return true;

                InvokeCompletionEventMethod?.Invoke(asyncOperation, null);
            }

            _yielding.Pop();
            if (_yielding.Count > 0)
            {
                return true;
            }

            Stop();
            return false;
        }

        /// <summary>
        /// Continuously advances the coroutine to the next phase until it has reached the end.
        /// <para>
        /// Note that this locks the current thread until the coroutine has fully finished.
        /// If the coroutine contains <see cref="CustomYieldInstruction">CustomYieldInstructions</see>
        /// that take a long time to finish (or never finish in edit mode) this can cause the editor
        /// to freeze for the same duration.
        /// </para>
        /// </summary>
        public void MoveToEnd()
        {
            while (MoveNext(true))
            {
            }
        }

        public bool Equals(IEnumerator coroutine)
        {
            int counter = _yielding.Count;
            foreach (object item in _yielding)
            {
                if (counter == 1 && item == coroutine) return true;

                counter--;
            }

            return false;
        }
    }
}