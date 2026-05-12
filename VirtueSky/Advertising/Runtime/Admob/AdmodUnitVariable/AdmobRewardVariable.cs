using System;
using VirtueSky.Core;
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
        [NonSerialized] internal Action receivedRewardCallback;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private RewardedAd _rewardedAd;
#endif
        private const float FinalizeCloseDelay = 0.2f;
        private DelayHandle _finalizeCloseHandle;

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

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            _rewardedAd.Show(UserRewardEarnedCallback);
#endif
        }

        public override AdUnitVariable Show(string placement = "")
        {
            ResetChainCallback();
            if (!UnityEngine.Application.isMobilePlatform || string.IsNullOrEmpty(Id) || !IsReady())
                return this;
            ShowImpl(placement);
            return this;
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
            skippedCallback = null;
            receivedRewardCallback = null;
            IsEarnRewarded = false;
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

        private void ResetFinalizeCloseHandle()
        {
            App.CancelDelay(_finalizeCloseHandle);
            _finalizeCloseHandle = null;
        }

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private void AdLoadCallback(RewardedAd ad, LoadAdError error)
        {
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
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref clickedCallback, info);
            OnClickedAdEvent?.Invoke(info);
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "RewardedAd", AdMediation.Admob.ToString());
        }

        private void OnAdOpening()
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref displayedCallback, info);
            OnDisplayedAdEvent?.Invoke(info);
        }

        private void OnAdFailedToShow(AdError obj)
        {
            var errorInfo = new AdsError(obj);
            Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
            OnFailedToDisplayAdEvent?.Invoke(errorInfo);
        }

        private void OnAdClosed()
        {
            AdStatic.IsShowingAd = false;
            IsShowing = false;
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref closedCallback, info);
            OnClosedAdEvent?.Invoke(info);
            App.CancelDelay(_finalizeCloseHandle);
            _finalizeCloseHandle = App.Delay(FinalizeCloseDelay, FinalizeClose);
        }

        private void OnAdLoaded()
        {
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref loadedCallback, info);
            OnLoadAdEvent?.Invoke(info);
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            var errorInfo = new AdsError(error);
            Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
            OnFailedToLoadAdEvent?.Invoke(errorInfo);
        }

        private void UserRewardEarnedCallback(Reward reward)
        {
            IsEarnRewarded = true;
            Common.CallActionAndClean(ref receivedRewardCallback);
        }

        private void FinalizeClose()
        {
            _finalizeCloseHandle = null;
            if (IsEarnRewarded)
            {
                Common.CallActionAndClean(ref completedCallback);
                IsEarnRewarded = false;
                ResetFinalizeCloseHandle();
                Destroy();
                Load();
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
            ResetFinalizeCloseHandle();
            Destroy();
            Load();
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
