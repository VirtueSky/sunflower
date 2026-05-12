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
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        private LevelPlayInterstitialAd interstitialAd;
#endif
        public override void Init()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            paidedCallback += AppTracking.TrackRevenue;
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

                interstitialAd.LoadAd();
            }
            catch (Exception e)
            {
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

        private void ResetInterstitialAd(bool isDestroy = false)
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (interstitialAd == null) return;
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

#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY

        #region Fun Callback

        internal void OnAdPaidEvent(LevelPlayImpressionData impressionData)
        {
            if (impressionData.MediationAdUnitId.Equals(Id))
            {
                paidedCallback?.Invoke((double)impressionData.Revenue, impressionData.AdNetwork,
                    impressionData.MediationAdUnitId,
                    impressionData.AdFormat, AdMediation.LevelPlay.ToString());
            }
        }

        void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref loadedCallback, info);
            OnLoadAdEvent?.Invoke(info);
        }

        void InterstitialOnAdLoadFailed(LevelPlayAdError ironSourceError)
        {
            var errorInfo = new AdsError(ironSourceError);
            Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
            OnFailedToLoadAdEvent?.Invoke(errorInfo);
            ResetInterstitialAd(true);
        }

        void InterstitialOnAdDisplayEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref displayedCallback, info);
            OnDisplayedAdEvent?.Invoke(info);
        }

        void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref clickedCallback, info);
            OnClickedAdEvent?.Invoke(info);
        }

        void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError adError)
        {
            var errorInfo = new AdsError(adError);
            Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
            OnFailedToDisplayAdEvent?.Invoke(errorInfo);
            IsShowing = false;
            ResetInterstitialAd(true);
        }

        void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = false;
            Common.CallActionAndClean(ref completedCallback);
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref closedCallback, info);
            OnClosedAdEvent?.Invoke(info);
            IsShowing = false;
            ResetInterstitialAd(true);
            Load();
        }

        #endregion

#endif
    }
}
