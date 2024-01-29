using UnityEngine;

namespace VirtueSky.Firebase
{
    [CreateAssetMenu(menuName = "Firebase Analytic/Log Event No Param",
        fileName = "log_event_firebase_analytic_no_param")]
    public class LogEventFirebaseAnalyticNoParam : LogEventFirebaseAnalytic
    {
        [SerializeField] private string eventName;

        public void LogEventNoParam()
        {
            LogEvent(eventName);
        }
    }
}