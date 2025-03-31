using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower/Tracking Event/AppsFlyer/Tracking 4 Param",
        fileName = "tracking_appsflyer_4_param")]
    [EditorIcon("scriptable_af")]
    public class TrackingAppsFlyerFourParam : TrackingAppsFlyer
    {
        [Space, HeaderLine("Parameter Name"), SerializeField]
        private string parameterName1;

        [SerializeField] private string parameterName2;
        [SerializeField] private string parameterName3;
        [SerializeField] private string parameterName4;

        public void TrackEvent(string parameterValue1, string parameterValue2, string parameterValue3,
            string parameterValue4)
        {
#if VIRTUESKY_APPSFLYER
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(parameterName1, parameterValue1);
            eventValues.Add(parameterName2, parameterValue2);
            eventValues.Add(parameterName3, parameterValue3);
            eventValues.Add(parameterName4, parameterValue4);
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, eventValues);
            onTracked?.Invoke();
#endif
        }
    }
}