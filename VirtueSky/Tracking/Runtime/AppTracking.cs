#if VIRTUESKY_FIREBASE_ANALYTIC
using Firebase.Analytics;
#endif


namespace VirtueSky.Tracking
{
    public struct AppTracking
    {
        public static void TrackRevenue(double value, string network, string unitId, string format,
            string adNetwork)
        {
            AdjustTrackingRevenue.AdjustTrackRevenue(value, network, unitId, format, adNetwork);
            FirebaseAnalyticTrackingRevenue.FirebaseAnalyticTrackRevenue(value, network, unitId,
                format, adNetwork);
            AppsFlyerTrackingRevenue.AppsFlyerTrackRevenueAd(value, network, unitId, format, adNetwork);
        }

        public static void FirebaseAnalyticTrackATTResult(int status)
        {
#if VIRTUESKY_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent("app_tracking_transparency", "status", status);
#endif
        }

        public static void StartTrackingAdjust()
        {
#if VIRTUESKY_ADJUST
            var adjust = new UnityEngine.GameObject("Adjust", typeof(com.adjust.sdk.Adjust));
            com.adjust.sdk.Adjust.StartTracking(AdjustSetting.AppToken, AdjustSetting.AdjustEnvironment,
                AdjustSetting.LogLevel);
#endif
        }

        public static void StartTrackingAppsFlyer()
        {
#if VIRTUESKY_APPSFLYER
            var appFlyerObject =
                new UnityEngine.GameObject("AppsFlyerObject", typeof(VirtueSky.Tracking.AppsFlyerObject));
#endif
        }
    }
}