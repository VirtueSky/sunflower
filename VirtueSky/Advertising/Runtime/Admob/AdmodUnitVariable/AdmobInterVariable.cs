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
        public bool usePreload;
        [Min(0)] public int preloadBufferSize = 2;
        [NonSerialized] internal Action completedCallback;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private InterstitialAd _interstitialAd;
        private InterstitialAd _preloadedInterstitialAd;
        private ResponseInfo adsInfo = null;
        private bool isPreloadStarted;
#endif
        private AdsInfo cacheAdInfo;
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

            if (usePreload)
            {
                StartPreload();
                return;
            }

            DestroyLoadedAd();
            IsLoading = true;
            OnRequestAdEvent?.Invoke();
            InterstitialAd.Load(Id, new AdRequest(), AdLoadCallback);

#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (usePreload)
            {
                return InterstitialAdPreloader.IsAdAvailable(Id);
            }

            return _interstitialAd != null && _interstitialAd.CanShowAd();
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
            if (usePreload)
            {
                _preloadedInterstitialAd = InterstitialAdPreloader.DequeueAd(Id);
                if (_preloadedInterstitialAd == null)
                {
                    Debug.LogWarning($"Advertising: InterstitialAd preload dequeue failed, ad is not ready: {Id}");
                    return;
                }

                adsInfo = _preloadedInterstitialAd.GetResponseInfo();
                BindAdEvents(_preloadedInterstitialAd);
                CacheAdsInfo();
                if (cacheAdInfo != null)
                {
                    cacheAdInfo.Placement = placement;
                }

                _preloadedInterstitialAd.Show();
                return;
            }

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
            if (usePreload)
            {
                DestroyPreloadedAd();
                return;
            }

            DestroyLoadedAd();
#endif
            IsLoading = false;
        }

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private void DestroyLoadedAd()
        {
            if (_interstitialAd == null) return;
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        private void DestroyPreloadedAd()
        {
            IsLoading = false;
            IsShowing = false;
            if (_preloadedInterstitialAd == null) return;
            _preloadedInterstitialAd.Destroy();
            _preloadedInterstitialAd = null;
        }
#endif

        #region Fun Callback

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private void StartPreload()
        {
            if (isPreloadStarted) return;

            isPreloadStarted = true;
            IsLoading = true;
            Debug.Log($"Advertising: Preload InterstitialAd: {Id}");
            OnRequestAdEvent?.Invoke();

            var config = new PreloadConfiguration
            {
                AdUnitId = Id,
                Request = new AdRequest(),
                BufferSize = (uint)Math.Max(1, preloadBufferSize)
            };

            try
            {
                bool preloadStarted = InterstitialAdPreloader.Preload(
                    Id,
                    config,
                    onAdPreloaded: (adUnitId, responseInfo) =>
                    {
                        Debug.Log($"Advertising: InterstitialAd Preload callback loaded: {adUnitId}");
                        adsInfo = responseInfo;
                        CacheAdsInfo();
                        OnAdLoaded();
                    },
                    onAdFailedToPreload: (adUnitId, error) =>
                    {
                        Debug.LogWarning($"Advertising: InterstitialAd Preload callback failed: {adUnitId}");
                        OnAdFailedToLoad(error);
                    },
                    onAdsExhausted: adUnitId =>
                    {
                        Debug.LogWarning($"Advertising: InterstitialAd preload exhausted: {adUnitId}");
                    }
                );

                Debug.Log($"Advertising: InterstitialAd Preload started: {preloadStarted}, adUnitId: {Id}, bufferSize: {config.BufferSize}");
                if (!preloadStarted)
                {
                    isPreloadStarted = false;
                    IsLoading = false;
                }
            }
            catch (Exception e)
            {
                isPreloadStarted = false;
                IsLoading = false;
                Debug.LogWarning($"Advertising: InterstitialAd Preload exception: {Id}, {e}");
            }
        }

        private void AdLoadCallback(InterstitialAd ad, LoadAdError error)
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                OnAdFailedToLoad(error);
                return;
            }

            _interstitialAd = ad;
            adsInfo = ad.GetResponseInfo();
            BindAdEvents(_interstitialAd);
            CacheAdsInfo();
            OnAdLoaded();
        }

        private void BindAdEvents(InterstitialAd ad)
        {
            ad.OnAdPaid += OnAdPaided;
            ad.OnAdFullScreenContentClosed += OnAdClosed;
            ad.OnAdFullScreenContentFailed += OnAdFailedToShow;
            ad.OnAdFullScreenContentOpened += OnAdOpening;
            ad.OnAdClicked += OnAdClicked;
        }

        private void OnAdClicked()
        {
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref clickedCallback, cacheAdInfo);
                OnClickedAdEvent?.Invoke(cacheAdInfo);
            });
        }

        private void OnAdOpening()
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref displayedCallback, cacheAdInfo);
                OnDisplayedAdEvent?.Invoke(cacheAdInfo);
            });
        }

        private void OnAdFailedToShow(AdError error)
        {
            var errorInfo = new AdsError(error);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
                OnFailedToDisplayAdEvent?.Invoke(errorInfo);
            });

            IsShowing = false;
            Destroy();
            if (!usePreload) Load();
        }

        private void OnAdClosed()
        {
            AdStatic.IsShowingAd = false;
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref completedCallback);
                Common.CallActionAndClean(ref closedCallback, cacheAdInfo);
                OnClosedAdEvent?.Invoke(cacheAdInfo);
            });
            Destroy();
            IsShowing = false;
            if (!usePreload) Load();
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
            cacheAdInfo.AdFormat = "InterstitialAd";
            cacheAdInfo.AdNetwork = adsInfo?.GetLoadedAdapterResponseInfo()?.AdSourceName ?? "";
        }

        private void OnAdLoaded()
        {
            IsLoading = false;
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref loadedCallback, cacheAdInfo);
                OnLoadedAdEvent?.Invoke(cacheAdInfo);
            });
        }

        private void OnAdFailedToLoad(AdError error)
        {
            IsLoading = false;
            if (error == null)
            {
                Debug.LogWarning($"Advertising: InterstitialAd FailedToLoad: {Id}, error is null");
                return;
            }

            var errorInfo = new AdsError(error);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
                OnFailedToLoadAdEvent?.Invoke(errorInfo);
            });
        }
#endif

        #endregion

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
