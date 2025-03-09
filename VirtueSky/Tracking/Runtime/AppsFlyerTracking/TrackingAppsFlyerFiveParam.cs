using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/AppsFlyer/Tracking 5 Param",
        fileName = "tracking_appsflyer_5_param")]
    [EditorIcon("scriptable_af")]
    public class TrackingAppsFlyerFiveParam : TrackingAppsFlyer
    {
        [Space, HeaderLine("Parameter Name"), SerializeField]
        private string parameterName1;

        [SerializeField] private string parameterName2;
        [SerializeField] private string parameterName3;
        [SerializeField] private string parameterName4;
        [SerializeField] private string parameterName5;

        public void TrackEvent(string parameterValue1, string parameterValue2, string parameterValue3,
            string parameterValue4, string parameterValue5)
        {
#if VIRTUESKY_APPSFLYER
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(parameterName1, parameterValue1);
            eventValues.Add(parameterName2, parameterValue2);
            eventValues.Add(parameterName3, parameterValue3);
            eventValues.Add(parameterName4, parameterValue4);
            eventValues.Add(parameterName5, parameterValue5);
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, eventValues);
            onTracked?.Invoke();
#endif
        }
    }
}