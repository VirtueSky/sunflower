#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
#endif
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
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            // On Android, Unity is paused when displaying interstitial or rewarded video.
            // This setting makes iOS behave consistently with Android.
            MobileAds.SetiOSAppPauseOnBackground(true);

            // When true all events raised by GoogleMobileAds will be raised
            // on the Unity main thread. The default value is false.
            // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            MobileAds.Initialize(initStatus =>
            {
                App.RunOnMainThread(() =>
                {
                    if (!adSetting.AdmobEnableTestMode) return;
                    var configuration = new RequestConfiguration
                        { TestDeviceIds = adSetting.AdmobDevicesTest };
                    MobileAds.SetRequestConfiguration(configuration);
                });
            });
            FirebaseAnalyticTrackingRevenue.autoTrackAdImpressionAdmob = adSetting.AutoTrackingAdImpressionAdmob;
            adSetting.AdmobBannerVariable.Init();
            adSetting.AdmobInterVariable.Init();
            adSetting.AdmobRewardVariable.Init();
            adSetting.AdmobRewardInterVariable.Init();
            adSetting.AdmobAppOpenVariable.Init();
            adSetting.AdmobNativeOverlayVariable.Init();

            RegisterAppStateChange();
            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
            LoadBanner();
            LoadNativeOverlay();
#endif
        }


#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        void RegisterAppStateChange()
        {
            GoogleMobileAds.Api.AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        void OnAppStateChanged(GoogleMobileAds.Common.AppState state)
        {
            if (state == GoogleMobileAds.Common.AppState.Foreground && adSetting.AdmobAppOpenVariable.AutoShow &&
                !AdStatic.isShowingAd)
            {
                if (adSetting.UseAdmob) ShowAppOpen();
            }
        }
#endif
        public override void LoadBanner()
        {
            if (adSetting.AdmobBannerVariable == null) return;
            adSetting.AdmobBannerVariable.Load();
        }

        public override void LoadInterstitial()
        {
            if (adSetting.AdmobInterVariable == null) return;
            if (!adSetting.AdmobInterVariable.IsReady()) adSetting.AdmobInterVariable.Load();
        }

        public override void LoadRewarded()
        {
            if (adSetting.AdmobRewardVariable == null) return;
            if (!adSetting.AdmobRewardVariable.IsReady()) adSetting.AdmobRewardVariable.Load();
        }

        public override void LoadRewardedInterstitial()
        {
            if (adSetting.AdmobRewardInterVariable == null) return;
            if (!adSetting.AdmobRewardInterVariable.IsReady()) adSetting.AdmobRewardInterVariable.Load();
        }

        public override void LoadAppOpen()
        {
            if (adSetting.AdmobAppOpenVariable == null) return;
            if (!adSetting.AdmobAppOpenVariable.IsReady()) adSetting.AdmobAppOpenVariable.Load();
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
    }
}