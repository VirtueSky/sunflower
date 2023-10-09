using System;
using System.Collections;
#if VIRTUESKY_ADS && ADS_ADMOB
using GoogleMobileAds.Api;
#endif
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Global;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    public class AdmobBannerVariable : AdUnitVariable
    {
        public BannerSize size = BannerSize.Adaptive;
        public BannerPosition position = BannerPosition.Bottom;
#if VIRTUESKY_ADS && ADS_ADMOB
        private BannerView _bannerView;
#endif

        private readonly WaitForSeconds _waitBannerReload = new WaitForSeconds(5f);
        private IEnumerator _reload;

        public override void Init()
        {
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            Destroy();
            _bannerView = new BannerView(Id, ConvertSize(), ConvertPosition());
            _bannerView.OnAdFullScreenContentClosed += OnAdClosed;
            _bannerView.OnBannerAdLoadFailed += OnAdFailedToLoad;
            _bannerView.OnBannerAdLoaded += OnAdLoaded;
            _bannerView.OnAdFullScreenContentOpened += OnAdOpening;
            _bannerView.OnAdPaid += OnAdPaided;
            _bannerView.LoadAd(new AdRequest());

#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            return _bannerView != null;
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            Load();
            _bannerView.Show();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && ADS_ADMOB
            if (_bannerView == null) return;
            _bannerView.Destroy();
            _bannerView = null;
#endif
        }


#if VIRTUESKY_ADS && ADS_ADMOB

        public AdSize ConvertSize()
        {
            switch (size)
            {
                case BannerSize.Adaptive: return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
                default: return AdSize.Banner;
            }
        }

        public AdPosition ConvertPosition()
        {
            switch (position)
            {
                case BannerPosition.Top: return AdPosition.Top;
                case BannerPosition.Bottom: return AdPosition.Bottom;
                case BannerPosition.TopLeft: return AdPosition.TopLeft;
                case BannerPosition.TopRight: return AdPosition.TopRight;
                case BannerPosition.BottomLeft: return AdPosition.BottomLeft;
                case BannerPosition.BottomRight: return AdPosition.BottomRight;
                default: return AdPosition.Bottom;
            }
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "BannerAd");
        }

        private void OnAdOpening()
        {
            Common.CallActionAndClean(ref displayedCallback);
        }

        private void OnAdLoaded()
        {
            Common.CallActionAndClean(ref loadedCallback);
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            if (_reload != null) App.StopCoroutine(_reload);
            _reload = DelayBannerReload();
            App.StartCoroutine(_reload);
        }

        private void OnAdClosed()
        {
            Common.CallActionAndClean(ref closedCallback);
        }

        private IEnumerator DelayBannerReload()
        {
            yield return _waitBannerReload;
            Load();
        }
#endif
#if UNITY_EDITOR
        [ContextMenu("Get Id test")]
        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/2934735716";
#endif
        }
#endif
    }
}