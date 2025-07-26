using VirtueSky.Core;

namespace VirtueSky.Ads
{
    public sealed class MaxAdClient : AdClient
    {
        public MaxAdClient(AdSetting _adSetting)
        {
            adSetting = _adSetting;
        }

        public override void Initialize()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.SetSdkKey(adSetting.SdkKey);
            MaxSdk.InitializeSdk();
            adSetting.MaxBannerVariable.Init();
            adSetting.MaxInterVariable.Init();
            adSetting.MaxRewardVariable.Init();
            adSetting.MaxAppOpenVariable.Init();
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
            if (!pauseStatus && adSetting.MaxAppOpenVariable.autoShow)
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