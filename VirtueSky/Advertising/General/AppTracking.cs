#if VIRTUESKY_FIREBASE_ANALYTIC
using Firebase.Analytics;
#endif

#if VIRTUESKY_ADJUST
using com.adjust.sdk;
#endif
namespace VirtueSky.Ads
{
    public static class AppTracking
    {
        public static void TrackRevenue(double value, string network, string unitId, string format)
        {
#if VIRTUESKY_ADJUST
            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            adjustAdRevenue.setRevenue(value, "USD");
            adjustAdRevenue.setAdRevenueNetwork(network);
            adjustAdRevenue.setAdRevenueUnit(unitId);
            adjustAdRevenue.setAdRevenuePlacement(format);
            Adjust.trackAdRevenue(adjustAdRevenue);
#endif
#if VIRTUESKY_FIREBASE_ANALYTIC
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
#endif
        }
    }
}