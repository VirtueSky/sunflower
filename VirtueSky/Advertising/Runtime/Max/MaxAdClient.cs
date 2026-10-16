using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Ads
{
    public sealed class MaxAdClient : AdClient
    {
        private readonly BackupAdUnitGroup interGroup;
        private readonly BackupAdUnitGroup rewardGroup;

        public MaxAdClient(AdSetting _adSetting)
        {
            adSetting = _adSetting;
            interGroup = BackupAdUnitGroup.Setup(adSetting.MaxInterGroup, adSetting.MaxInterVariable,
                adSetting.MaxInterVariableBackup, () => adSetting.UseMaxInterBackup,
                () => adSetting.MaxBackupAdUnitExpireTime);
            rewardGroup = BackupAdUnitGroup.Setup(adSetting.MaxRewardGroup, adSetting.MaxRewardVariable,
                adSetting.MaxRewardVariableBackup, () => adSetting.UseMaxRewardBackup,
                () => adSetting.MaxBackupAdUnitExpireTime);
        }

        public override void Initialize()
        {
            SdkInitializationCompleted = false;
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.SetSdkKey(adSetting.SdkKey);
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
            MaxSdk.InitializeSdk();
            adSetting.MaxBannerVariable.Init(adSetting);
            interGroup.Init(adSetting);
            rewardGroup.Init(adSetting);
            adSetting.MaxAppOpenVariable.Init(adSetting);
            App.AddPauseCallback(OnAppStateChange);
#endif
        }


#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
        private void OnAppStateChange(bool pauseStatus)
        {
            if (!pauseStatus && adSetting.MaxAppOpenVariable.autoShow)
            {
                if (adSetting.IsApplovin()) ShowAppOpen();
            }
        }

        private void OnSdkInitialized(MaxSdkBase.SdkConfiguration configuration)
        {
            SdkInitializationCompleted = true;
            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
           // LoadBanner();
        }
#endif

        public override void LoadBanner()
        {
            if (adSetting.MaxBannerVariable == null) return;
            adSetting.MaxBannerVariable.Load();
        }

        public override void LoadInterstitial()
        {
            interGroup.Load();
        }

        public override void LoadRewarded()
        {
            rewardGroup.Load();
        }

        public override void LoadRewardedInterstitial()
        {
        }

        public override void LoadAppOpen()
        {
            if (adSetting.MaxAppOpenVariable == null) return;
            if (!adSetting.MaxAppOpenVariable.IsReady() && !adSetting.MaxAppOpenVariable.IsLoading) adSetting.MaxAppOpenVariable.Load();
        }

        public override void ShowAppOpen()
        {
            if (statusAppOpenFirstIgnore) adSetting.MaxAppOpenVariable.Show();
            statusAppOpenFirstIgnore = true;
        }

        public override void LoadNativeOverlay()
        {
        }

        public override void ShowAdMediationDebugger()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (SdkInitializationCompleted)
            {
                MaxSdk.ShowMediationDebugger();
                Debug.Log("Ad Mediation Debugger opened successfully.");
            }
            else
            {
                Debug.LogWarning("Failed to open Ad Mediation Debugger: SDK not initialized.");
            }
#endif
        }
    }
}