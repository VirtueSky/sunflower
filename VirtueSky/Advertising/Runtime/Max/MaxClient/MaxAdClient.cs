using VirtueSky.Inspector;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    [EditorIcon("icon_scriptable")]
    public sealed class MaxAdClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            MaxSdk.SetSdkKey(adSetting.SdkKey);
            MaxSdk.InitializeSdk();
            MaxSdk.SetIsAgeRestrictedUser(adSetting.ApplovinEnableAgeRestrictedUser);
            adSetting.MaxBannerVariable.Init();
            adSetting.MaxInterVariable.Init();
            adSetting.MaxRewardVariable.Init();
            adSetting.MaxAppOpenVariable.Init();
            adSetting.MaxRewardInterVariable.Init();

            adSetting.MaxBannerVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxInterVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxRewardVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxRewardInterVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxAppOpenVariable.paidedCallback = AppTracking.TrackRevenue;

            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }
    }
}