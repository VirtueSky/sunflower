using System;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
#endif
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class AdmobRewardVariable : AdmobAdUnitVariable
    {
        public bool useTestId;
        [NonSerialized] internal Action completedCallback;
        [NonSerialized] internal Action skippedCallback;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private RewardedAd _rewardedAd;
#endif
        public override void Init()
        {
            if (useTestId)
            {
                GetUnitTest();
            }
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (string.IsNullOrEmpty(Id)) return;
            paidedCallback += AppTracking.TrackRevenue;
#endif
        }

        public bool IsEarnRewarded { get; private set; }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (string.IsNullOrEmpty(Id)) return;
            Destroy();
            RewardedAd.Load(Id, new AdRequest(), AdLoadCallback);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return _rewardedAd != null && _rewardedAd.CanShowAd();
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            _rewardedAd.Show(UserRewardEarnedCallback);
#endif
        }

        public override AdUnitVariable Show()
        {
            ResetChainCallback();
            if (!UnityEngine.Application.isMobilePlatform || string.IsNullOrEmpty(Id) || !IsReady())
                return this;
            ShowImpl();
            return this;
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
            skippedCallback = null;
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_rewardedAd == null) return;
            _rewardedAd.Destroy();
            _rewardedAd = null;
            IsEarnRewarded = false;
#endif
        }

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private void AdLoadCallback(RewardedAd ad, LoadAdError error)
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                OnAdFailedToLoad(error);
                return;
            }

            _rewardedAd = ad;
            _rewardedAd.OnAdFullScreenContentClosed += OnAdClosed;
            _rewardedAd.OnAdFullScreenContentFailed += OnAdFailedToShow;
            _rewardedAd.OnAdFullScreenContentOpened += OnAdOpening;
            _rewardedAd.OnAdPaid += OnAdPaided;
            _rewardedAd.OnAdClicked += OnAdClicked;
            OnAdLoaded();
        }

        private void OnAdClicked()
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "RewardedAd", AdNetwork.Admob.ToString());
        }

        private void OnAdOpening()
        {
            AdStatic.IsShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        private void OnAdFailedToShow(AdError obj)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
            OnFailedToDisplayAdEvent?.Invoke(obj.GetMessage());
        }

        private void OnAdClosed()
        {
            AdStatic.IsShowingAd = false;
            Common.CallActionAndClean(ref closedCallback);
            OnClosedAdEvent?.Invoke();
            if (IsEarnRewarded)
            {
                Common.CallActionAndClean(ref completedCallback);
                Destroy();
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
            Destroy();
        }

        private void OnAdLoaded()
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(error.GetMessage());
        }

        private void UserRewardEarnedCallback(Reward reward)
        {
            IsEarnRewarded = true;
        }
#endif

        [ContextMenu("Get Id test")]
        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/1712485313";
#endif
        }
    }
}