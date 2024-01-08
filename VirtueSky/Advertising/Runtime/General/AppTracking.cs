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
        public static void TrackRevenue(double value, string network, string unitId, string format, AdNetwork adNetwork)
        {
        }

        #region Adjust

        public static void AdjustTrackRevenue(double value, string network, string unitId, string placement, AdNetwork adNetwork)
        {
#if VIRTUESKY_ADJUST
            var source = "";
            switch (adNetwork)
            {
                case AdNetwork.Admob:
                    source = com.adjust.sdk.AdjustConfig.AdjustAdRevenueSourceAdMob;
                    break;
                case AdNetwork.Applovin:
                    source = com.adjust.sdk.AdjustConfig.AdjustAdRevenueSourceAppLovinMAX;
                    break;
            }

            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(source);
            adjustAdRevenue.setRevenue(value, "USD");
            adjustAdRevenue.setAdRevenueNetwork(network);
            adjustAdRevenue.setAdRevenueUnit(unitId);
            adjustAdRevenue.setAdRevenuePlacement(placement);
            Adjust.trackAdRevenue(adjustAdRevenue);
#endif
        }

        #endregion

        #region Firebase Analytics

        public static void FirebaseAnalyticTrackRevenue(double value, string network, string unitId, string format, AdNetwork adNetwork)
        {
#if VIRTUESKY_FIREBASE_ANALYTIC
            switch (adNetwork)
            {
                case AdNetwork.Admob:
                    return;
                case AdNetwork.Applovin:
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

        public static void FirebaseAnalyticTrackATTResult(int status)
        {
#if VIRTUESKY_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent("app_tracking_transparency", "status", status);
#endif
        }

        #endregion
    }
}