using VirtueSky.Core;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    public sealed class IronSourceAdClient : AdClient
    {
        public IronSourceAdClient(AdSetting _adSetting)
        {
            adSetting = _adSetting;
        }

        public bool SdkInitializationCompleted { get; private set; }

        public override void Initialize()
        {
            SdkInitializationCompleted = false;
            if (adSetting.UseTestAppKey)
            {
                adSetting.AndroidAppKey = "85460dcd";
                adSetting.IosAppKey = "8545d445";
            }
#if VIRTUESKY_ADS && VIRTUESKY_IRONSOURCE
            App.AddPauseCallback(OnAppStateChange);
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;
            adSetting.IronSourceBannerVariable.Init();
            adSetting.IronSourceInterVariable.Init();
            adSetting.IronSourceRewardVariable.Init();
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(adSetting.AppKey);
#endif
            LoadInterstitial();
            LoadRewarded();
            LoadBanner();
        }

#if VIRTUESKY_ADS && VIRTUESKY_IRONSOURCE
        private void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            if (impressionData.revenue != null)
            {
                AppTracking.TrackRevenue((double)impressionData.revenue, impressionData.adNetwork,
                    impressionData.adUnit,
                    impressionData.placement, AdNetwork.IronSource.ToString());
            }
        }

        private void OnAppStateChange(bool pauseStatus)
        {
            IronSource.Agent.onApplicationPause(pauseStatus);
        }
#endif
        void SdkInitializationCompletedEvent()
        {
            SdkInitializationCompleted = true;
        }

        public override void LoadBanner()
        {
            if (adSetting.IronSourceBannerVariable == null) return;
            adSetting.IronSourceBannerVariable.Load();
        }

        public override void LoadInterstitial()
        {
            if (adSetting.IronSourceInterVariable == null) return;
            if (!adSetting.IronSourceInterVariable.IsReady()) adSetting.IronSourceInterVariable.Load();
        }

        public override void LoadRewarded()
        {
            if (adSetting.IronSourceRewardVariable == null) return;
            if (!adSetting.IronSourceRewardVariable.IsReady()) adSetting.IronSourceRewardVariable.Load();
        }

        public override void LoadRewardedInterstitial()
        {
        }

        public override void LoadAppOpen()
        {
        }

        public override void ShowAppOpen()
        {
        }

        public override void LoadNativeOverlay()
        {
        }
    }
}