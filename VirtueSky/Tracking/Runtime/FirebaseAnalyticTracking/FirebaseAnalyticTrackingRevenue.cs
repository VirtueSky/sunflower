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
            switch (adNetwork.ToLower())
            {
                case "admob":
                    return;
                case "max":
                    Parameter[] parameters =
                    {
                        new("value", value),
                        new("ad_platform", "AppLovin"),
                        new("ad_format", format),
                        new("currency", "USD"),
                        new("ad_unit_name", unitId),
                        new("ad_source", network)
                    };

                    FirebaseAnalytics.LogEvent("ad_impression", parameters);
                    break;
            }
#endif
        }
    }
}