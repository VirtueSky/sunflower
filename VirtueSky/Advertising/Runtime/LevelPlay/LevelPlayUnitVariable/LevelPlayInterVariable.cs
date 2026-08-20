using System;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
using Unity.Services.LevelPlay;
using VirtueSky.Tracking;
#endif
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class LevelPlayInterVariable : LevelPlayAdUnitVariable
    {
        [NonSerialized] internal Action completedCallback;
        [Tooltip("Destroy and recreate the LevelPlay ad object when reloading ads.")]
        public bool isDestroyAdOnReload = true;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        private LevelPlayInterstitialAd interstitialAd;
#endif
        public override bool IsShowing { get; internal set; }
        public override bool IsLoading { get; internal set; }

        public override void Init(AdSetting _adSetting)
        {
            base.Init(_adSetting);
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            paidedCallback += TrackRevenue;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            if (string.IsNullOrEmpty(Id))
            {
                Debug.LogWarning("LevelPlay interstitial load skipped because ad unit id is empty.");
                return;
            }
            if (IsShowing || IsLoading || IsReady()) return;

            try
            {
                if (interstitialAd == null)
                {
                    var configBuilder = new LevelPlayInterstitialAd.Config.Builder();
                    var config = configBuilder.Build();
                    interstitialAd = new LevelPlayInterstitialAd(Id, config);
                    interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
                    interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailed;
                    interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayEvent;
                    interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
                    interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
                    interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
                }

                IsLoading = true;
                OnRequestAdEvent?.Invoke();
                interstitialAd.LoadAd();
            }
            catch (Exception e)
            {
                IsLoading = false;
                Debug.LogWarning($"LevelPlay interstitial load failed during SDK call, resetting ad instance. {e}");
                ResetInterstitialAd();
            }
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (interstitialAd == null) return false;

            try
            {
                return interstitialAd.IsAdReady();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"LevelPlay interstitial IsAdReady failed, resetting ad instance. {e}");
                ResetInterstitialAd();
                return false;
            }
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (interstitialAd != null)
            {
                IsShowing = true;
                interstitialAd.ShowAd(placement);
            }
#endif
        }

        public override AdUnitVariable Show(string placement = "")
        {
            ResetChainCallback();
            if (!Application.isMobilePlatform || AdStatic.IsRemoveAd || !IsReady()) return this;
            ShowImpl(placement);
            return this;
        }

        public override void Destroy()
        {
            IsShowing = false;
            ResetInterstitialAd(true);
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
        }

        private void ResetInterstitialAd(bool isDestroy = false, bool keepObject = false)
        {
            IsLoading = false;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (interstitialAd == null) return;
            if (keepObject) return;
            interstitialAd.OnAdLoaded -= InterstitialOnAdLoadedEvent;
            interstitialAd.OnAdLoadFailed -= InterstitialOnAdLoadFailed;
            interstitialAd.OnAdDisplayed -= InterstitialOnAdDisplayEvent;
            interstitialAd.OnAdClicked -= InterstitialOnAdClickedEvent;
            interstitialAd.OnAdDisplayFailed -= InterstitialOnAdDisplayFailedEvent;
            interstitialAd.OnAdClosed -= InterstitialOnAdClosedEvent;
            if (isDestroy) interstitialAd.DestroyAd();
            interstitialAd = null;
#endif
        }

        private void ResetInterstitialAdForReload()
        {
            ResetInterstitialAd(isDestroyAdOnReload, !isDestroyAdOnReload);
        }

#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY

        #region Fun Callback

        internal void OnAdPaidEvent(LevelPlayImpressionData impressionData)
        {
            if (impressionData.MediationAdUnitId.Equals(Id))
            {
                paidedCallback?.Invoke(new AdsInfo(impressionData));
            }
        }

        void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            IsLoading = false;
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref loadedCallback, info);
                OnLoadedAdEvent?.Invoke(info);
            });
        }

        void InterstitialOnAdLoadFailed(LevelPlayAdError ironSourceError)
        {
            IsLoading = false;
            var errorInfo = new AdsError(ironSourceError);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
                OnFailedToLoadAdEvent?.Invoke(errorInfo);
            });

            ResetInterstitialAdForReload();
        }

        void InterstitialOnAdDisplayEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref displayedCallback, info);
                OnDisplayedAdEvent?.Invoke(info);
            });
        }

        void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref clickedCallback, info);
                OnClickedAdEvent?.Invoke(info);
            });
        }

        void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError adError)
        {
            var errorInfo = new AdsError(adError);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
                OnFailedToDisplayAdEvent?.Invoke(errorInfo);
            });
            IsShowing = false;
            ResetInterstitialAdForReload();
        }

        void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = false;
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref completedCallback);
                Common.CallActionAndClean(ref closedCallback, info);
                OnClosedAdEvent?.Invoke(info);
            });
            IsShowing = false;
            ResetInterstitialAdForReload();
            Load();
        }

        #endregion

#endif
    }
}
