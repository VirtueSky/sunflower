using UnityEngine;

namespace VirtueSky.Ads
{
    public class MaxAdClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            MaxSdk.SetSdkKey(adSetting.SdkKey);
            MaxSdk.InitializeSdk();
            MaxSdk.SetIsAgeRestrictedUser(adSetting.ApplovinEnableAgeRestrictedUser);
            adSetting.MaxBannerVariable.Init();
            adSetting.MaxInterVariable.Init();
            adSetting.MaxRewardVariable.Init();
            adSetting.MaxAppOpenVariable.Init();
            adSetting.MaxRewardInterVariable.Init();

            adSetting.MaxBannerVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxInterVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxRewardVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxRewardInterVariable.paidedCallback = AppTracking.TrackRevenue;
            adSetting.MaxAppOpenVariable.paidedCallback = AppTracking.TrackRevenue;

            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }

        public override void LoadInterstitial()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (!IsInterstitialReady()) adSetting.MaxInterVariable.Load();
#endif
        }

        public override bool IsInterstitialReady()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            return adSetting.MaxInterVariable.IsReady();
#else
            return false;
#endif
        }

        public override void LoadRewarded()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (!IsRewardedReady()) adSetting.MaxRewardVariable.Load();
#endif
        }

        public override bool IsRewardedReady()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            return adSetting.MaxRewardVariable.IsReady();
#else
            return false;
#endif
        }

        public override void LoadRewardedInterstitial()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (!IsRewardedInterstitialReady()) adSetting.MaxRewardInterVariable.Load();
#endif
        }

        public override bool IsRewardedInterstitialReady()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            return adSetting.MaxRewardInterVariable.IsReady();
#else
            return false;
#endif
        }

        public override void LoadAppOpen()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (!IsAppOpenReady()) adSetting.MaxAppOpenVariable.Load();
#endif
        }

        public override bool IsAppOpenReady()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            return adSetting.MaxAppOpenVariable.IsReady();
#else
            return false;
#endif
        }

        internal void ShowAppOpen()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (statusAppOpenFirstIgnore) adSetting.MaxAppOpenVariable.Show();
            statusAppOpenFirstIgnore = true;
#endif
        }
    }
}