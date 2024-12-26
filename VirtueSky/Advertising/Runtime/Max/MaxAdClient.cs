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
            LoadBanner();
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

        public override void LoadBanner()
        {
            if (adSetting.MaxBannerVariable == null) return;
            adSetting.MaxBannerVariable.Load();
        }

        public override void LoadInterstitial()
        {
            if (adSetting.MaxInterVariable == null) return;
            if (!adSetting.MaxInterVariable.IsReady()) adSetting.MaxInterVariable.Load();
        }

        public override void LoadRewarded()
        {
            if (adSetting.MaxRewardVariable == null) return;
            if (!adSetting.MaxRewardVariable.IsReady()) adSetting.MaxRewardVariable.Load();
        }

        public override void LoadRewardedInterstitial()
        {
            if (adSetting.MaxRewardInterVariable == null) return;
            if (!adSetting.MaxRewardInterVariable.IsReady()) adSetting.MaxRewardInterVariable.Load();
        }

        public override void LoadAppOpen()
        {
            if (adSetting.MaxAppOpenVariable == null) return;
            if (!adSetting.MaxAppOpenVariable.IsReady()) adSetting.MaxAppOpenVariable.Load();
        }

        public override void ShowAppOpen()
        {
            if (statusAppOpenFirstIgnore) adSetting.MaxAppOpenVariable.Show();
            statusAppOpenFirstIgnore = true;
        }

        public override void LoadNativeOverlay()
        {
        }
    }
}