using UnityEngine;

namespace VirtueSky.Firebase
{
    [CreateAssetMenu(menuName = "Firebase Analytic/Log Event Has Param",
        fileName = "log_event_firebase_analytic_has_param")]
    public class LogEventFirebaseAnalyticHasParam : LogEventFirebaseAnalytic
    {
        [SerializeField] private string eventName;
        [SerializeField] private string parameterName;
        [SerializeField] private string parameterValue;

        public void LogEventHasParam()
        {
            LogEvent(eventName, parameterName, parameterValue);
        }

        public void LogEvent(string parameterValue)
        {
            LogEvent(eventName, parameterName, parameterValue);
        }
    }
}