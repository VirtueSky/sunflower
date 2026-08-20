#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
#endif
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    public sealed class AdmobAdClient : AdClient
    {
        public AdmobAdClient(AdSetting _adSetting)
        {
            adSetting = _adSetting;
        }

        public override void Initialize()
        {
            SdkInitializationCompleted = false;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
#if UNITY_IOS
            // On Android, Unity is paused when displaying interstitial or rewarded video.
            // This setting makes iOS behave consistently with Android.
            MobileAds.SetiOSAppPauseOnBackground(true);
#endif

            TestMode();
            MobileAds.Initialize(OnInitializeComplete);
            FirebaseAnalyticTrackingRevenue.autoTrackAdImpressionAdmob = adSetting.AutoTrackingAdImpressionAdmob;
            adSetting.AdmobBannerVariable.Init(adSetting);
            adSetting.AdmobInterVariable.Init(adSetting);
            adSetting.AdmobRewardVariable.Init(adSetting);
            adSetting.AdmobRewardInterVariable.Init(adSetting);
            adSetting.AdmobAppOpenVariable.Init(adSetting);
            adSetting.AdmobNativeOverlayVariable.Init(adSetting);

            RegisterAppStateChange();
#endif
        }


#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        void RegisterAppStateChange()
        {
            GoogleMobileAds.Api.AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        void OnAppStateChanged(GoogleMobileAds.Common.AppState state)
        {
            if (state == GoogleMobileAds.Common.AppState.Foreground && adSetting.AdmobAppOpenVariable.autoShow)
            {
                if (adSetting.IsAdmob()) ShowAppOpen();
            }
        }

        private void OnInitializeComplete(InitializationStatus initStatus)
        {
            App.RunOnMainThread(() =>
            {
                SdkInitializationCompleted = true;
                if (adSetting.AdmobEnableTestMode)
                {
                    Debug.Log("AdMob SDK Initialization Complete");
                    foreach (var keyValuePair in initStatus.getAdapterStatusMap())
                    {
                        Debug.Log(
                            $"Adapter: {keyValuePair.Key}, Description: {keyValuePair.Value.Description}, Latency: {keyValuePair.Value.Latency}, State: {keyValuePair.Value.InitializationState}");
                    }
                }

                LoadInterstitial();
                LoadRewarded();
                LoadRewardedInterstitial();
                LoadAppOpen();
                LoadNativeOverlay();
                //LoadBanner();
            });
        }

        private void TestMode()
        {
            if (!adSetting.AdmobEnableTestMode) return;
            var configuration = new RequestConfiguration
                { TestDeviceIds = adSetting.AdmobDevicesTest };
            MobileAds.SetRequestConfiguration(configuration);
        }
#endif
        public override void LoadBanner()
        {
            if (adSetting.AdmobBannerVariable == null) return;
            adSetting.AdmobBannerVariable.Load();
        }

        public override void LoadInterstitial()
        {
            if (adSetting.AdmobInterVariable == null || adSetting.AdmobInterVariable.IsShowing) return;
            if (!adSetting.AdmobInterVariable.IsReady() && !adSetting.AdmobInterVariable.IsLoading) adSetting.AdmobInterVariable.Load();
        }

        public override void LoadRewarded()
        {
            if (adSetting.AdmobRewardVariable == null || adSetting.AdmobRewardVariable.IsShowing) return;
            if (!adSetting.AdmobRewardVariable.IsReady() && !adSetting.AdmobRewardVariable.IsLoading) adSetting.AdmobRewardVariable.Load();
        }

        public override void LoadRewardedInterstitial()
        {
            if (adSetting.AdmobRewardInterVariable == null || adSetting.AdmobRewardInterVariable.IsShowing) return;
            if (!adSetting.AdmobRewardInterVariable.IsReady() && !adSetting.AdmobRewardInterVariable.IsLoading)
                adSetting.AdmobRewardInterVariable.Load();
        }

        public override void LoadAppOpen()
        {
            if (adSetting.AdmobAppOpenVariable == null) return;
            if (!adSetting.AdmobAppOpenVariable.IsReady() && !adSetting.AdmobAppOpenVariable.IsLoading) adSetting.AdmobAppOpenVariable.Load();
        }

        public override void ShowAppOpen()
        {
            if (statusAppOpenFirstIgnore) adSetting.AdmobAppOpenVariable.Show();
            statusAppOpenFirstIgnore = true;
        }

        public override void LoadNativeOverlay()
        {
            if (adSetting.AdmobNativeOverlayVariable == null) return;
            if (!adSetting.AdmobNativeOverlayVariable.IsReady()) adSetting.AdmobNativeOverlayVariable.Load();
        }

        public override void ShowAdMediationDebugger()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (SdkInitializationCompleted)
            {
                MobileAds.OpenAdInspector((result) =>
                {
                    if (result != null)
                    {
                        Debug.LogError($"Failed to open Ad Inspector: {result.GetCode()} / {result.GetMessage()}");
                    }
                    else
                    {
                        Debug.Log("Ad Inspector opened successfully.");
                    }
                });
            }
            else
            {
                Debug.LogWarning("Failed to open Ad Inspector: SDK not initialized.");
            }
#endif
        }
    }
}