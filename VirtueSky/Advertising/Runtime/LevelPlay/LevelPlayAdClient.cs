#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
using Unity.Services.LevelPlay;
#endif
using UnityEngine;
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

        public override void Initialize()
        {
            SdkInitializationCompleted = false;
            if (adSetting.UseTestAppKey)
            {
                adSetting.AndroidAppKey = "85460dcd";
                adSetting.IosAppKey = "8545d445";
            }
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (adSetting.EnableTestSuite)
            {
                LevelPlay.SetMetaData("is_test_suite", "enable");
            }

            App.AddPauseCallback(OnAppStateChange);
            LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
            LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;
            adSetting.LevelPlayBannerVariable.Init(adSetting);
            adSetting.LevelPlayInterVariable.Init(adSetting);
            if (adSetting.UseLevelPlayInterBackup) adSetting.LevelPlayInterVariableBackup.Init(adSetting);
            adSetting.LevelPlayRewardVariable.Init(adSetting);
            if (adSetting.UseLevelPlayRewardBackup) adSetting.LevelPlayRewardVariableBackup.Init(adSetting);
            LevelPlay.ValidateIntegration();
            LevelPlay.Init(adSetting.AppKey);
#endif
        }

#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        private void ImpressionDataReadyEvent(LevelPlayImpressionData impressionData)
        {
            if (impressionData.Revenue != null)
            {
                adSetting.LevelPlayBannerVariable.OnAdPaidEvent(impressionData);
                adSetting.LevelPlayInterVariable.OnAdPaidEvent(impressionData);
                adSetting.LevelPlayRewardVariable.OnAdPaidEvent(impressionData);
                // Each ad unit filters by its own MediationAdUnitId, so the backups can safely listen too.
                if (adSetting.UseLevelPlayInterBackup) adSetting.LevelPlayInterVariableBackup.OnAdPaidEvent(impressionData);
                if (adSetting.UseLevelPlayRewardBackup) adSetting.LevelPlayRewardVariableBackup.OnAdPaidEvent(impressionData);
            }
        }

        private void OnAppStateChange(bool pauseStatus)
        {
            if (SdkInitializationCompleted)
            {
                LevelPlay.SetPauseGame(pauseStatus);
            }
        }

        void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
        {
            SdkInitializationCompleted = true;
            LoadInterstitial();
            LoadRewarded();
            //LoadBanner();
        }
#endif


        public override void LoadBanner()
        {
            if (!SdkInitializationCompleted) return;
            if (adSetting.LevelPlayBannerVariable == null) return;
            adSetting.LevelPlayBannerVariable.Load();
        }

        public override void LoadInterstitial()
        {
            if (!SdkInitializationCompleted) return;
            LoadAdUnit(adSetting.LevelPlayInterVariable);
            if (adSetting.UseLevelPlayInterBackup) LoadAdUnit(adSetting.LevelPlayInterVariableBackup);
        }

        public override void LoadRewarded()
        {
            if (!SdkInitializationCompleted) return;
            LoadAdUnit(adSetting.LevelPlayRewardVariable);
            if (adSetting.UseLevelPlayRewardBackup) LoadAdUnit(adSetting.LevelPlayRewardVariableBackup);
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

        public override void ShowAdMediationDebugger()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (SdkInitializationCompleted)
            {
                LevelPlay.LaunchTestSuite();
                Debug.Log("LevelPlay Test Suite Launched");
            }
            else
            {
                Debug.LogWarning("Failed to launch LevelPlay Test Suite: SDK initialization not completed.");
            }
#endif
        }
    }
}