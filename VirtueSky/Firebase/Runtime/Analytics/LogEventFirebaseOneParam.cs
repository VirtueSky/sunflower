using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.FirebaseTraking
{
    [CreateAssetMenu(menuName = "Sunflower/Firebase Analytic/Log Event 1 Param",
        fileName = "log_event_firebase_1_param")]
    [EditorIcon("scriptable_firebase")]
    public class LogEventFirebaseOneParam : ScriptableObject
    {
        [Space] [HeaderLine("Event Name")] [SerializeField]
        private string eventName;

        [Space] [HeaderLine("Parameter Name")] [SerializeField]
        private string parameterName;

        public void LogEvent(string parameterValue)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
#endif
        }
    }
}