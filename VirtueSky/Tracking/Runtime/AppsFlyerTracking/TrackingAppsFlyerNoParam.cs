using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/AppsFlyer/Tracking No Param",
        fileName = "tracking_appsflyer_no_param")]
    [EditorIcon("scriptable_af")]
    public class TrackingAppsFlyerNoParam : TrackingAppsFlyer
    {
        public void TrackEvent()
        {
#if VIRTUESKY_APPSFLYER
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, null);
            onTracked?.Invoke();
#endif
        }
    }
}