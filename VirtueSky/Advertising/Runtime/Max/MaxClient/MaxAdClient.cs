namespace VirtueSky.Ads
{
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
            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }
    }
}