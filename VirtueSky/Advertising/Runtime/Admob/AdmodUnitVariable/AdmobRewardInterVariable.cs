using System;
#if VIRTUESKY_ADS && ADS_ADMOB
using GoogleMobileAds.Api;
#endif
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class AdmobRewardInterVariable : AdUnitVariable
    {
        public bool useTestId;
        [NonSerialized] internal Action completedCallback;
        [NonSerialized] internal Action skippedCallback;
#if VIRTUESKY_ADS && ADS_ADMOB
        private RewardedInterstitialAd _rewardedInterstitialAd;
#endif
        public override void Init()
        {
            if (useTestId)
            {
                GetUnitTest();
            }
        }

        public bool IsEarnRewarded { get; private set; }

        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            if (string.IsNullOrEmpty(Id)) return;
            Destroy();
            RewardedInterstitialAd.Load(Id, new AdRequest(), OnAdLoadCallback);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            return _rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd();
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            _rewardedInterstitialAd.Show(UserEarnedRewardCallback);
#endif
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
            skippedCallback = null;
        }

        public override AdUnitVariable Show()
        {
            ResetChainCallback();
            if (!UnityEngine.Application.isMobilePlatform || string.IsNullOrEmpty(Id) || !IsReady())
                return this;
            ShowImpl();
            return this;
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            if (_rewardedInterstitialAd == null) return;
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
            IsEarnRewarded = false;
#endif
        }

#if VIRTUESKY_ADS && ADS_ADMOB
        private void OnAdLoadCallback(RewardedInterstitialAd ad, LoadAdError error)
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                OnAdFailedToLoad(error);
                return;
            }

            _rewardedInterstitialAd = ad;
            _rewardedInterstitialAd.OnAdFullScreenContentClosed += OnAdClosed;
            _rewardedInterstitialAd.OnAdFullScreenContentOpened += OnAdOpening;
            _rewardedInterstitialAd.OnAdFullScreenContentFailed += OnAdFailedToShow;
            _rewardedInterstitialAd.OnAdPaid += OnAdPaided;
            _rewardedInterstitialAd.OnAdClicked += OnAdClicked;
            OnAdLoaded();
        }

        private void OnAdClicked()
        {
            Common.CallActionAndClean(ref clickedCallback);
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
        }

        private void OnAdLoaded()
        {
            Common.CallActionAndClean(ref loadedCallback);
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "RewardedInterstitialAd", AdNetwork.Admob.ToString());
        }

        private void OnAdFailedToShow(AdError error)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
        }

        private void OnAdOpening()
        {
            AdStatic.isShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
        }

        private void OnAdClosed()
        {
            AdStatic.isShowingAd = false;
            Common.CallActionAndClean(ref closedCallback);
            if (IsEarnRewarded)
            {
                Common.CallActionAndClean(ref completedCallback);
                _rewardedInterstitialAd.Destroy();
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
            _rewardedInterstitialAd.Destroy();
        }

        private void UserEarnedRewardCallback(Reward reward)
        {
            IsEarnRewarded = true;
        }
#endif

        [ContextMenu("Get Id test")]
        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/6978759866";
#endif
        }
    }
}