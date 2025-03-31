using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using VirtueSky.Core;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static string Format(this string fmt, params object[] args) =>
            string.Format(System.Globalization.CultureInfo.InvariantCulture.NumberFormat, fmt, args);

        public static bool IsInteger(this float value)
        {
            return (value == (int)value);
        }

        public static int GetNumberInAString(this string str)
        {
            try
            {
                var getNumb = Regex.Match(str, @"\d+").Value;
                return Int32.Parse(getNumb);
            }
            catch (Exception e)
            {
                return -1;
            }

            return -1;
        }

        public static float GetScreenRatio()
        {
            return (1920f / 1080f) / (Screen.height / (float)Screen.width);
        }

        public static void CallActionAndClean(ref Action action)
        {
            if (action == null) return;
            var a = action;
            a();
            action = null;
        }

        public static void CallActionAndClean<T>(ref Action<T> action, T _value)
        {
            if (action == null) return;
            var a = action;
            a(_value);
            action = null;
        }


        #region Internet Connection

        private static IEnumerator internetConnectionCoroutine;

        public static void StopCheckInternetConnection()
        {
            App.StopCoroutine(internetConnectionCoroutine);
        }

        public static void CheckInternetConnection(Action actionConnected, Action actionDisconnected)
        {
            if (internetConnectionCoroutine != null) App.StopCoroutine(internetConnectionCoroutine);
            internetConnectionCoroutine = InternetConnection((isConnected) =>
            {
                if (isConnected)
                {
                    actionConnected?.Invoke();
                }
                else
                {
                    actionDisconnected?.Invoke();
                }
            });
            App.StartCoroutine(internetConnectionCoroutine);
        }

        public static IEnumerator InternetConnection(Action<bool> action)
        {
            bool result;
            string url = "https://google.com";
#if UNITY_ANDROID
            url = "https://google.com";
#elif UNITY_IOS
            url = "https://captive.apple.com/hotspot-detect.html";
#endif

            using (UnityWebRequest request = UnityWebRequest.Head(url))
            {
                yield return request.SendWebRequest();
                result = !request.isNetworkError && !request.isHttpError && request.responseCode == 200 &&
                         request.error == null;
            }

            action(result);
            internetConnectionCoroutine = null;
        }

        #endregion

        /// <summary>
        /// Attach a DelayHandle on to the behaviour. If the behaviour is destroyed before the DelayHandle is completed,
        /// e.g. through a scene change, the DelayHandle callback will not execute.
        /// </summary>
        /// <param name="target">The behaviour to attach this DelayHandle to.</param>
        /// <param name="duration">The duration to wait before the DelayHandle fires.</param>
        /// <param name="onComplete">The action to run when the DelayHandle elapses.</param>
        /// <param name="onUpdate">A function to call each tick of the DelayHandle. Takes the number of seconds elapsed since
        /// the start of the current cycle.</param>
        /// <param name="isLooped">Whether the DelayHandle should restart after executing.</param>
        /// <param name="useRealTime">Whether the DelayHandle uses real-time(not affected by slow-mo or pausing) or
        /// game-time(affected by time scale changes).</param>
        public static DelayHandle Delay(
            this MonoBehaviour target,
            float duration,
            Action onComplete,
            Action<float> onUpdate = null,
            bool isLooped = false,
            bool useRealTime = false)
        {
            return App.Delay(
                target,
                duration,
                onComplete,
                onUpdate,
                isLooped,
                useRealTime);
        }
    }
}