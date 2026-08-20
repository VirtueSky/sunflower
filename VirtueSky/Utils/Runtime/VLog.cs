using Object = UnityEngine.Object;

namespace VirtueSky.Utils
{
    public struct VLog
    {
        public static bool EnableLog = true;

        public static void Log(object message)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.Log(message);
#else
            if (EnableLog)
            {
                UnityEngine.Debug.Log(message);
            }
#endif
        }

        public static void Log(object message, Object context)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.Log(message, context);
#else
            if (EnableLog) 
            {
                UnityEngine.Debug.Log(message, context);
            }
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.LogWarning(message);
#else
            if (EnableLog)
            {
                UnityEngine.Debug.LogWarning(message);
            }
#endif
        }

        public static void LogWarning(object message, Object context)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.LogWarning(message, context);
#else
            if (EnableLog)
            {
                UnityEngine.Debug.LogWarning(message, context);
            }
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.LogError(message);
#else
            if (EnableLog)            
            {
                UnityEngine.Debug.LogError(message);
            }
#endif
        }

        public static void LogError(object message, Object context)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.LogError(message, context);
#else
            if (EnableLog)
            {
                UnityEngine.Debug.LogError(message, context);
            }
#endif
        }

        public static void LogException(System.Exception exception)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.LogException(exception);
#else
            if (EnableLog)
            {
                UnityEngine.Debug.LogException(exception);
            }
#endif
        }

        public static void LogException(System.Exception exception, Object context)
        {
#if UNITY_EDITOR || VIRTUESKY_DEBUG_LOG
            UnityEngine.Debug.LogException(exception, context);
#else
            if (EnableLog)            
            {
                UnityEngine.Debug.LogException(exception, context);
            }
#endif
        }
    }
}