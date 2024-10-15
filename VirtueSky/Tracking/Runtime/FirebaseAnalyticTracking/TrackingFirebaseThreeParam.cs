using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/Firebase Analytic/Tracking 3 Param",
        fileName = "tracking_firebase_3_param")]
    [EditorIcon("scriptable_firebase")]
    public class TrackingFirebaseThreeParam : TrackingFirebase
    {
        [Space] [HeaderLine("Event Name")] [SerializeField]
        private string eventName;

        [Space] [HeaderLine("Parameter Name")] [SerializeField]
        private string parameterName1;

        [SerializeField] private string parameterName2;
        [SerializeField] private string parameterName3;

        public void TrackEvent(string parameterValue1, string parameterValue2, string parameterValue3)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            Firebase.Analytics.Parameter[] parameters =
            {
                new(parameterName1, parameterValue1), new(parameterName2, parameterValue2),
                new(parameterName3, parameterValue3)
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameters);
            onTracked?.Invoke();
#endif
        }
    }
}