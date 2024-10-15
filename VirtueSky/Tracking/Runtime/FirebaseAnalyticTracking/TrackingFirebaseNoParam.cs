using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/Firebase Analytic/Tracking No Param",
        fileName = "tracking_firebase_no_param")]
    [EditorIcon("scriptable_firebase")]
    public class TrackingFirebaseNoParam : TrackingFirebase
    {
        [Space] [HeaderLine("Event Name")] [SerializeField]
        private string eventName;

        public void TrackEvent()
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
            onTracked?.Invoke();
#endif
        }
    }
}