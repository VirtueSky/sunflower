using System;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
using VirtueSky.Tracking;
#endif
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class AdmobInterVariable : AdmobAdUnitVariable
    {
        public bool useTestId;
        [NonSerialized] internal Action completedCallback;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private InterstitialAd _interstitialAd;
#endif
        public override void Init()
        {
            if (useTestId)
            {
                GetUnitTest();
            }
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            paidedCallback += AppTracking.TrackRevenue;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;

            Destroy();
            InterstitialAd.Load(Id, new AdRequest(), AdLoadCallback);

#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return _interstitialAd != null && _interstitialAd.CanShowAd();
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            _interstitialAd.Show();
#endif
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_interstitialAd == null) return;
            _interstitialAd.Destroy();
            _interstitialAd = null;
#endif
        }

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private void AdLoadCallback(InterstitialAd ad, LoadAdError error)
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                OnAdFailedToLoad(error);
                return;
            }

            _interstitialAd = ad;
            _interstitialAd.OnAdPaid += OnAdPaided;
            _interstitialAd.OnAdFullScreenContentClosed += OnAdClosed;
            _interstitialAd.OnAdFullScreenContentFailed += OnAdFailedToShow;
            _interstitialAd.OnAdFullScreenContentOpened += OnAdOpening;
            _interstitialAd.OnAdClicked += OnAdClicked;
            OnAdLoaded();
        }

        private void OnAdClicked()
        {
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref clickedCallback, info);
            OnClickedAdEvent?.Invoke(info);
        }

        private void OnAdOpening()
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref displayedCallback, info);
            OnDisplayedAdEvent?.Invoke(info);
        }

        private void OnAdFailedToShow(AdError error)
        {
            var errorInfo = new AdsError(error);
            Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
            OnFailedToDisplayAdEvent?.Invoke(errorInfo);
        }

        private void OnAdClosed()
        {
            AdStatic.IsShowingAd = false;
            IsShowing = false;
            Common.CallActionAndClean(ref completedCallback);
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref closedCallback, info);
            OnClosedAdEvent?.Invoke(info);
            Destroy();
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "InterstitialAd", AdMediation.Admob.ToString());
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
#endif

        [ContextMenu("Get Id test")]
        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/4411468910";
#endif
        }
    }
}
