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
        private readonly BackupAdUnitGroup interGroup;
        private readonly BackupAdUnitGroup rewardGroup;

        public LevelPlayAdClient(AdSetting _adSetting)
        {
            adSetting = _adSetting;
            interGroup = BackupAdUnitGroup.Setup(adSetting.LevelPlayInterGroup, adSetting.LevelPlayInterVariable,
                adSetting.LevelPlayInterVariableBackup, () => adSetting.UseLevelPlayInterBackup,
                () => adSetting.LevelPlayBackupAdUnitExpireTime);
            rewardGroup = BackupAdUnitGroup.Setup(adSetting.LevelPlayRewardGroup, adSetting.LevelPlayRewardVariable,
                adSetting.LevelPlayRewardVariableBackup, () => adSetting.UseLevelPlayRewardBackup,
                () => adSetting.LevelPlayBackupAdUnitExpireTime);
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
            interGroup.Init(adSetting);
            rewardGroup.Init(adSetting);
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
                if (interGroup.IsBackupEnabled) adSetting.LevelPlayInterVariableBackup.OnAdPaidEvent(impressionData);
                if (rewardGroup.IsBackupEnabled) adSetting.LevelPlayRewardVariableBackup.OnAdPaidEvent(impressionData);
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
            interGroup.Load();
        }

        public override void LoadRewarded()
        {
            if (!SdkInitializationCompleted) return;
            rewardGroup.Load();
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