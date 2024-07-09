using UnityEngine;
using VirtueSky.Core;

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
            App.AddPauseCallback(OnAppStateChange);
            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }

#if VIRTUESKY_ADS && ADS_APPLOVIN
        private void OnAppStateChange(bool pauseStatus)
        {
            if (!pauseStatus && adSetting.MaxAppOpenVariable.AutoShow && !AdStatic.isShowingAd)
            {
                ShowAppOpen();
            }
        }
#endif
    }
}