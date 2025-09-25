#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
using Unity.Services.LevelPlay;
#endif
using VirtueSky.Core;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    public sealed class LevelPlayAdClient : AdClient
    {
        public LevelPlayAdClient(AdSetting _adSetting)
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
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            App.AddPauseCallback(OnAppStateChange);
            LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
            LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;
            adSetting.LevelPlayBannerVariable.Init();
            adSetting.LevelPlayInterVariable.Init();
            adSetting.LevelPlayRewardVariable.Init();
            LevelPlay.ValidateIntegration();
            LevelPlay.Init(adSetting.AppKey);
#endif
            LoadInterstitial();
            LoadRewarded();
            LoadBanner();
        }

#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        private void ImpressionDataReadyEvent(LevelPlayImpressionData impressionData)
        {
            if (impressionData.Revenue != null)
            {
                AppTracking.TrackRevenue((double)impressionData.Revenue, impressionData.AdNetwork,
                    impressionData.MediationAdUnitId,
                    impressionData.AdFormat, AdNetwork.LevelPlay.ToString());
            }
        }

        private void OnAppStateChange(bool pauseStatus)
        {
            LevelPlay.SetPauseGame(pauseStatus);
        }
        void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
        {
            SdkInitializationCompleted = true;
        }
#endif
       

        public override void LoadBanner()
        {
            if (adSetting.LevelPlayBannerVariable == null) return;
            adSetting.LevelPlayBannerVariable.Load();
        }

        public override void LoadInterstitial()
        {
            if (adSetting.LevelPlayInterVariable == null) return;
            if (!adSetting.LevelPlayInterVariable.IsReady()) adSetting.LevelPlayInterVariable.Load();
        }

        public override void LoadRewarded()
        {
            if (adSetting.LevelPlayRewardVariable == null) return;
            if (!adSetting.LevelPlayRewardVariable.IsReady()) adSetting.LevelPlayRewardVariable.Load();
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