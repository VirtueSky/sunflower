using System;
using System.Collections;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
using VirtueSky.Tracking;
#endif
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class AdmobBannerVariable : AdmobAdUnitVariable
    {
        public AdsSize size = AdsSize.Adaptive;
        public AdsPosition position = AdsPosition.Bottom;
        public bool useCollapsible;
        public bool useTestId;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private BannerView _bannerView;
        private ResponseInfo adsInfo = null;
#endif
        private AdsInfo cacheAdInfo;
        private const float BannerReloadInitialDelay = 5f;
        private const float BannerReloadMaxDelay = 60f;
        private IEnumerator _reload;
        private int _bannerReloadAttempt;
        private bool _isBannerShowing;
        private bool _previousBannerShowStatus;
        private string placement = "";

        public override bool IsShowing { get; internal set; }
        public override bool IsLoading { get; internal set; }

        public override void Init(AdSetting _adSetting)
        {
            base.Init(_adSetting);
            if (useTestId)
            {
                GetUnitTest();
            }
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            paidedCallback += TrackRevenue;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            CancelBannerReload();
            DestroyBannerView();
            IsLoading = true;
            _bannerView = new BannerView(Id, ConvertSize(), ConvertPosition());
            _bannerView.OnAdFullScreenContentClosed += OnAdClosed;
            _bannerView.OnBannerAdLoadFailed += OnAdFailedToLoad;
            _bannerView.OnBannerAdLoaded += OnAdLoaded;
            _bannerView.OnAdFullScreenContentOpened += OnAdOpening;
            _bannerView.OnAdPaid += OnAdPaided;
            _bannerView.OnAdClicked += OnAdClicked;
            var adRequest = new AdRequest();
            if (useCollapsible)
            {
                adRequest.Extras.Add("collapsible", ConvertPlacementCollapsible());
            }

            OnRequestAdEvent?.Invoke();
            _bannerView.LoadAd(adRequest);

#endif
        }

        public bool IsCollapsible()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_bannerView == null) return false;
            return _bannerView.IsCollapsible();
#else
            return false;
#endif
        }

        void OnWaitAppOpenClosed()
        {
            if (_previousBannerShowStatus)
            {
                _previousBannerShowStatus = false;
                Show();
            }
        }

        void OnWaitAppOpenDisplayed()
        {
            _previousBannerShowStatus = _isBannerShowing;
            if (_isBannerShowing) HideBanner();
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return _bannerView != null;
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
            this.placement = placement;
            if (cacheAdInfo != null)
            {
                cacheAdInfo.Placement = placement;
            }
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            _isBannerShowing = true;
            IsShowing = true;
            AdStatic.waitAppOpenClosedAction = OnWaitAppOpenClosed;
            AdStatic.waitAppOpenDisplayedAction = OnWaitAppOpenDisplayed;
            if (_bannerView == null)
            {
                Load();
            }

            _bannerView.Show();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            ResetBannerReload();
            DestroyBannerView();
#endif
        }

        private void DestroyBannerView()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            _isBannerShowing = false;
            IsShowing = false;
            AdStatic.waitAppOpenClosedAction = null;
            AdStatic.waitAppOpenDisplayedAction = null;
            if (_bannerView == null) return;
            _bannerView.Destroy();
            _bannerView = null;
#endif
        }

        public override void HideBanner()
        {
            base.HideBanner();
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            _isBannerShowing = false;
            IsShowing = false;
            if (_bannerView != null) _bannerView.Hide();
#endif
        }

        #region Fun Callback

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        public AdSize ConvertSize()
        {
            switch (size)
            {
                case AdsSize.Adaptive:
                    return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(
                        AdSize.FullWidth);
                case AdsSize.MediumRectangle: return AdSize.MediumRectangle;
                case AdsSize.Leaderboard: return AdSize.Leaderboard;
                case AdsSize.IABBanner: return AdSize.IABBanner;
                //case BannerSize.SmartBanner: return AdSize.SmartBanner;
                default: return AdSize.Banner;
            }
        }

        private void OnAdClicked()
        {
            Common.CallActionAndClean(ref clickedCallback, cacheAdInfo);
            OnClickedAdEvent?.Invoke(cacheAdInfo);
        }

        public AdPosition ConvertPosition()
        {
            switch (position)
            {
                case AdsPosition.Top: return AdPosition.Top;
                case AdsPosition.Bottom: return AdPosition.Bottom;
                case AdsPosition.TopLeft: return AdPosition.TopLeft;
                case AdsPosition.TopRight: return AdPosition.TopRight;
                case AdsPosition.BottomLeft: return AdPosition.BottomLeft;
                case AdsPosition.BottomRight: return AdPosition.BottomRight;
                default: return AdPosition.Bottom;
            }
        }

        public string ConvertPlacementCollapsible()
        {
            if (position == AdsPosition.Top)
            {
                return "top";
            }
            else if (position == AdsPosition.Bottom)
            {
                return "bottom";
            }

            return "bottom";
        }

        private void OnAdPaided(AdValue value)
        {
            cacheAdInfo.Revenue = value.Value / 1000000f;
            cacheAdInfo.Precision = value.Precision.ToString();
            paidedCallback?.Invoke(cacheAdInfo);
        }

        private void CacheAdsInfo()
        {
            if (cacheAdInfo != null) cacheAdInfo = null;
            cacheAdInfo = new AdsInfo(AdMediation.Admob);
            cacheAdInfo.AdUnitId = Id;
            cacheAdInfo.AdFormat = "BannerAd";
            cacheAdInfo.AdNetwork = adsInfo?.GetLoadedAdapterResponseInfo()?.AdSourceName ?? "";
        }

        private void OnAdOpening()
        {
            Common.CallActionAndClean(ref displayedCallback, cacheAdInfo);
            OnDisplayedAdEvent?.Invoke(cacheAdInfo);
        }

        private void OnAdLoaded()
        {
            IsLoading = false;
            adsInfo = _bannerView?.GetResponseInfo();
            CacheAdsInfo();
            Common.CallActionAndClean(ref loadedCallback, cacheAdInfo);
            OnLoadedAdEvent?.Invoke(cacheAdInfo);
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            IsLoading = false;
            var errorInfo = new AdsError(error);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
                OnFailedToLoadAdEvent?.Invoke(errorInfo);
            });

            ScheduleBannerReload();
        }

        private void ScheduleBannerReload()
        {
            CancelBannerReload();
            var delay = GetNextBannerReloadDelay();
            _reload = DelayBannerReload(delay);
            App.StartCoroutine(_reload);
        }

        private void OnAdClosed()
        {
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref closedCallback, cacheAdInfo);
                OnClosedAdEvent?.Invoke(cacheAdInfo);
            });
        }

        private float GetNextBannerReloadDelay()
        {
            var delay = BannerReloadInitialDelay * Mathf.Pow(2f, _bannerReloadAttempt);
            _bannerReloadAttempt++;
            return Mathf.Min(delay, BannerReloadMaxDelay);
        }

        private void ResetBannerReload()
        {
            CancelBannerReload();
            _bannerReloadAttempt = 0;
        }

        private void CancelBannerReload()
        {
            if (_reload == null) return;
            App.StopCoroutine(_reload);
            _reload = null;
        }

        private IEnumerator DelayBannerReload(float delay)
        {
            yield return new WaitForSeconds(delay);
            _reload = null;
            Load();
        }
#endif

        #endregion

        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/2934735716";
#endif
        }
    }
}
