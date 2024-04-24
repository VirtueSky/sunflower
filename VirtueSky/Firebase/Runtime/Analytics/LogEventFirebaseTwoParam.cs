using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.FirebaseTracking
{
    [CreateAssetMenu(menuName = "Sunflower/Firebase Analytic/Log Event 2 Param",
        fileName = "log_event_firebase_2_param")]
    [EditorIcon("scriptable_firebase")]
    public class LogEventFirebaseTwoParam : ScriptableObject
    {
        [Space] [HeaderLine("Event Name")] [SerializeField]
        private string eventName;

        [Space] [HeaderLine("Parameter Name")] [SerializeField]
        private string parameterName1;

        [SerializeField] private string parameterName2;

        public void LogEvent(string parameterValue1, string parameterValue2)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            Firebase.Analytics.Parameter[] parameters =
                { new(parameterName1, parameterValue1), new(parameterName2, parameterValue2) };
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameters);
#endif
        }
    }
}