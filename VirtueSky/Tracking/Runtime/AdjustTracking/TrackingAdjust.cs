using System;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/Adjust",
        fileName = "tracking_adjust")]
    [EditorIcon("scriptable_adjust2")]
    public class TrackingAdjust : ScriptableObject
    {
        [HeaderLine("Event Token"), SerializeField]
        private string eventToken;

        public static Action OnTracked;

        public void TrackEvent()
        {
#if VIRTUESKY_ADJUST
            AdjustSdk.Adjust.TrackEvent(new AdjustSdk.AdjustEvent(eventToken));
            OnTracked?.Invoke();
#endif
        }
    }
}