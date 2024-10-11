using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Ads
{
    public sealed class MaxAdClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.SetSdkKey(adSetting.SdkKey);
            MaxSdk.InitializeSdk();
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

#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
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