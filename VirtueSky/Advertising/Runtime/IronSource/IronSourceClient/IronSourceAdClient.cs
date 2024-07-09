using UnityEngine;
using VirtueSky.Core;
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

        private void OnAppStateChange(bool pauseStatus)
        {
            IronSource.Agent.onApplicationPause(pauseStatus);
        }
#endif
        void SdkInitializationCompletedEvent()
        {
            SdkInitializationCompleted = true;
        }
    }
}