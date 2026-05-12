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
    public class AdmobAppOpenVariable : AdmobAdUnitVariable
    {
        [Tooltip("Automatically show AppOpenAd when app status is changed")]
        public bool autoShow = false;

        [Tooltip("Time between closing the previous full-screen ad and starting to show the app open ad - in seconds")]
        public float timeBetweenFullScreenAd = 2f;

        public bool useTestId;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private AppOpenAd _appOpenAd;
#endif
        private DateTime _expireTime;

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
            AppOpenAd.Load(Id, new AdRequest(), OnAdLoadCallback);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return _appOpenAd != null && _appOpenAd.CanShowAd() && DateTime.Now < _expireTime &&
                   (DateTime.Now - AdStatic.AdClosingTime).TotalSeconds > timeBetweenFullScreenAd;
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            _appOpenAd.Show();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_appOpenAd == null) return;
            _appOpenAd.Destroy();
            _appOpenAd = null;
#endif
        }

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private void OnAdLoadCallback(AppOpenAd ad, LoadAdError error)
        {
            if (error != null || ad == null)
            {
                OnAdFailedToLoad(error);
                return;
            }

            _appOpenAd = ad;
            _appOpenAd.OnAdPaid += OnAdPaided;
            _appOpenAd.OnAdFullScreenContentClosed += OnAdClosed;
            _appOpenAd.OnAdFullScreenContentFailed += OnAdFailedToShow;
            _appOpenAd.OnAdFullScreenContentOpened += OnAdOpening;
            _appOpenAd.OnAdClicked += OnAdClicked;
            OnAdLoaded();

            // App open ads can be preloaded for up to 4 hours.
            _expireTime = DateTime.Now + TimeSpan.FromHours(4);
        }

        private void OnAdClicked()
        {
            var info = new AdsInfo(AdMediation.Admob);
            Common.CallActionAndClean(ref clickedCallback, info);
            OnClickedAdEvent?.Invoke(info);
        }

        private void OnAdOpening()
        {
            AdStatic.waitAppOpenDisplayedAction?.Invoke();
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
            AdStatic.waitAppOpenClosedAction?.Invoke();
            AdStatic.IsShowingAd = false;
            IsShowing = false;
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
                "AppOpenAd", AdMediation.Admob.ToString());
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
            androidId = "ca-app-pub-3940256099942544/9257395921";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/5575463023";
#endif
        }
    }
}
