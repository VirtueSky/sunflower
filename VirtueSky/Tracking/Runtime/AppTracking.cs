namespace VirtueSky.Tracking
{
    public struct AppTracking
    {
        public static bool enableTrackRevenue;

        public static void TrackRevenue(double value, string network, string unitId, string format,
            string currentAdSettingNetwork)
        {
            if (!enableTrackRevenue) return;
            AdjustTrackingRevenue.AdjustTrackRevenue(value, network, unitId, format, currentAdSettingNetwork);
            FirebaseAnalyticTrackingRevenue.FirebaseAnalyticTrackRevenue(value, network, unitId,
                format, currentAdSettingNetwork);
            AppsFlyerTrackingRevenue.AppsFlyerTrackRevenueAd(value, network, unitId, format, currentAdSettingNetwork);
        }

        public static void FirebaseAnalyticTrackATTResult(int status)
        {
#if VIRTUESKY_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent("app_tracking_transparency", "status", status);
#endif
        }

        public static void StartTrackingAdjust()
        {
#if VIRTUESKY_ADJUST
            var adjust = new UnityEngine.GameObject("Adjust", typeof(AdjustSdk.Adjust));
            var adjustConfig = new AdjustSdk.AdjustConfig(AdjustConfig.AppToken, AdjustConfig.AdjustEnvironment,
                AdjustConfig.LogLevel == AdjustSdk.AdjustLogLevel.Suppress)
            {
                LogLevel = AdjustConfig.LogLevel,
                IsAdServicesEnabled = true,
                IsIdfaReadingEnabled = true
            };
            AdjustSdk.Adjust.InitSdk(adjustConfig);
            UnityEngine.Debug.Log($"Start Tracking {adjust.name}");
#endif
        }

        public static void StartTrackingAppsFlyer()
        {
#if VIRTUESKY_APPSFLYER
            var appFlyerObject =
                new UnityEngine.GameObject("AppsFlyerObject", typeof(VirtueSky.Tracking.AppsFlyerObject));
            UnityEngine.Debug.Log($"Start Tracking {appFlyerObject.name}");
#endif
        }
    }
}