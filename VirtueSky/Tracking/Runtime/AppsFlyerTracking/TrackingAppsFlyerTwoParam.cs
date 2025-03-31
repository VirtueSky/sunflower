using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/AppsFlyer/Tracking 2 Param",
        fileName = "tracking_appsflyer_2_param")]
    [EditorIcon("scriptable_af")]
    public class TrackingAppsFlyerTwoParam : TrackingAppsFlyer
    {
        [Space, HeaderLine("Parameter Name"), SerializeField]
        private string parameterName1;

        [SerializeField] private string parameterName2;

        public void TrackEvent(string parameterValue1, string parameterValue2)
        {
#if VIRTUESKY_APPSFLYER
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(parameterName1, parameterValue1);
            eventValues.Add(parameterName2, parameterValue2);
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, eventValues);
            onTracked?.Invoke();
#endif
        }
    }
}