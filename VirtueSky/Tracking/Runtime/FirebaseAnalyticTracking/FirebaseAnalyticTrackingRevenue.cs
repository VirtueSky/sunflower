#if VIRTUESKY_FIREBASE_ANALYTIC
using Firebase.Analytics;
#endif

namespace VirtueSky.Tracking
{
    public struct FirebaseAnalyticTrackingRevenue
    {
        public static void FirebaseAnalyticTrackRevenue(double value, string network, string unitId,
            string format, string adNetwork)
        {
#if VIRTUESKY_FIREBASE_ANALYTIC
            string ad_platform = "";
            switch (adNetwork.ToLower())
            {
                case "admob":
                    return;
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
#endif
        }
    }
}