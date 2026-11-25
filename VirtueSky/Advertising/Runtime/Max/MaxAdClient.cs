using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Ads
{
    public sealed class MaxAdClient : AdClient, IBannerClient, IInterstitialClient, IRewardClient, IAppOpenClient
    {
        private bool statusAppOpenFirstIgnore;

        public MaxAdClient(AdSetting _adSetting)
        {
            adSetting = _adSetting;
        }

        public override void Initialize()
        {
            SdkInitializationCompleted = false;
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.SetSdkKey(adSetting.SdkKey);
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;
            MaxSdk.InitializeSdk();
            adSetting.MaxBannerVariable.Init(adSetting);
            adSetting.MaxInterVariable.Init(adSetting);
            if (adSetting.UseMaxInterBackup) adSetting.MaxInterVariableBackup.Init(adSetting);
            adSetting.MaxRewardVariable.Init(adSetting);
            if (adSetting.UseMaxRewardBackup) adSetting.MaxRewardVariableBackup.Init(adSetting);
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
            LoadAppOpen();
           // LoadBanner();
        }
#endif

        public void LoadBanner()
        {
            if (adSetting.MaxBannerVariable == null) return;
            adSetting.MaxBannerVariable.Load();
        }

        public void LoadInterstitial()
        {
            LoadAdUnit(adSetting.MaxInterVariable);
            if (adSetting.UseMaxInterBackup) LoadAdUnit(adSetting.MaxInterVariableBackup);
        }

        public void LoadRewarded()
        {
            LoadAdUnit(adSetting.MaxRewardVariable);
            if (adSetting.UseMaxRewardBackup) LoadAdUnit(adSetting.MaxRewardVariableBackup);
        }

        public void LoadAppOpen()
        {
            if (adSetting.MaxAppOpenVariable == null) return;
            if (!adSetting.MaxAppOpenVariable.IsReady() && !adSetting.MaxAppOpenVariable.IsLoading) adSetting.MaxAppOpenVariable.Load();
        }

        public void ShowAppOpen()
        {
            if (statusAppOpenFirstIgnore) adSetting.MaxAppOpenVariable.Show();
            statusAppOpenFirstIgnore = true;
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