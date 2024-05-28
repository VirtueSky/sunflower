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
    public class AdmobAppOpenVariable : AdUnitVariable
    {
        [Tooltip("Automatically show AppOpenAd when app status is changed"), SerializeField]
        private bool autoShow = false;

        public bool useTestId;
#if VIRTUESKY_ADS && ADS_ADMOB
        private AppOpenAd _appOpenAd;
#endif
        private DateTime _expireTime;
        public bool AutoShow => autoShow;

        public override void Init()
        {
            if (useTestId)
            {
                GetUnitTest();
            }
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;

            Destroy();
            AppOpenAd.Load(Id, new AdRequest(), OnAdLoadCallback);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            return _appOpenAd != null && _appOpenAd.CanShowAd() && DateTime.Now < _expireTime;
#else
            return false;
#endif
        }

        public override AdUnitVariable Show()
        {
            ResetChainCallback();
            if (!Application.isMobilePlatform || string.IsNullOrEmpty(Id) || AdStatic.IsRemoveAd ||
                !IsReady()) return this;
            ShowImpl();
            return this;
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            _appOpenAd.Show();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            if (_appOpenAd == null) return;
            _appOpenAd.Destroy();
            _appOpenAd = null;
#endif
        }

#if VIRTUESKY_ADS && ADS_ADMOB
        private void OnAdLoadCallback(AppOpenAd ad, LoadAdError error)
        {
            // if error is not null, the load request failed.
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
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        private void OnAdOpening()
        {
            AdStatic.isShowingAd = true;
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
            AdStatic.isShowingAd = false;
            Common.CallActionAndClean(ref closedCallback);
            OnClosedAdEvent?.Invoke();
            Destroy();
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "AppOpenAd", AdNetwork.Admob.ToString());
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

#endif

        [ContextMenu("Get Id test")]
        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/3419835294";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/5662855259";
#endif
        }
    }
}