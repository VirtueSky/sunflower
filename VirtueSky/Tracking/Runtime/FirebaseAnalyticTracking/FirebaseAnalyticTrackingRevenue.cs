using System;
#if VIRTUESKY_FIREBASE_ANALYTIC
using Firebase.Analytics;
#endif

namespace VirtueSky.Tracking
{
    public struct FirebaseAnalyticTrackingRevenue
    {
        public static Action OnTracked;
        public static bool autoTrackAdImpressionAdmob;

        public static void FirebaseAnalyticTrackRevenue(double value, string network, string unitId,
            string format, string currentAdSettingNetwork)
        {
#if VIRTUESKY_FIREBASE_ANALYTIC
            string ad_platform = "";
            switch (currentAdSettingNetwork.ToLower())
            {
                case "admob":
                    if (autoTrackAdImpressionAdmob) return;
                    ad_platform = "Admob";
                    break;

                case "max":
                    ad_platform = "AppLovin";
                    break;
                case "ironsource":
                    ad_platform = "IronSource";
                    break;
            }

            Parameter[] parameters =
            {
                new("value", value),
                new("ad_platform", ad_platform),
                new("ad_format", format),
                new("currency", "USD"),
                new("ad_unit_name", unitId),
                new("ad_source", network)
            };

            FirebaseAnalytics.LogEvent("ad_impression", parameters);
            OnTracked?.Invoke();
#endif
        }
    }
}