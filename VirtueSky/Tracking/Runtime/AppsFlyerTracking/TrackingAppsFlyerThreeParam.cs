using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/AppsFlyer/Tracking 3 Param",
        fileName = "tracking_appsflyer_3_param")]
    [EditorIcon("scriptable_af")]
    public class TrackingAppsFlyerThreeParam : TrackingAppsFlyer
    {
        [Space, HeaderLine("Event Name"), SerializeField]
        private string eventName;

        [Space, HeaderLine("Parameter Name"), SerializeField]
        private string parameterName1;

        [SerializeField] private string parameterName2;
        [SerializeField] private string parameterName3;

        public void TrackEvent(string parameterValue1, string parameterValue2, string parameterValue3)
        {
#if VIRTUESKY_APPSFLYER
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(parameterName1, parameterValue1);
            eventValues.Add(parameterName2, parameterValue2);
            eventValues.Add(parameterName3, parameterValue3);
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, eventValues);
            onTracked?.Invoke();
#endif
        }
    }
}