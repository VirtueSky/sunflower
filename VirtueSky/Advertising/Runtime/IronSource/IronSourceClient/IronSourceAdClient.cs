using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    public class IronSourceAdClient : AdClient
    {
        public bool SdkInitializationCompleted { get; private set; }
        public override void Initialize()
        {
            SdkInitializationCompleted = false;
            if (adSetting.UseTestAppKey)
            {
                adSetting.AndroidAppKey = "85460dcd";
                adSetting.IosAppKey = "8545d445";
            }
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            adSetting.IronSourceBannerVariable.Init();
            adSetting.IronSourceInterVariable.Init();
            adSetting.IronSourceRewardVariable.Init();
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(adSetting.AppKey);

            LoadInterstitial();
            LoadRewarded();
#endif
        }
#if VIRTUESKY_ADS && ADS_IRONSOURCE
        private void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            if (impressionData.revenue != null)
            {
                AppTracking.TrackRevenue((double)impressionData.revenue, impressionData.adNetwork,
                    impressionData.adUnit,
                    impressionData.placement, AdNetwork.IronSource.ToString());
            }
        }
#endif
        void SdkInitializationCompletedEvent()
        {
            SdkInitializationCompleted = true;
        }
    }
}