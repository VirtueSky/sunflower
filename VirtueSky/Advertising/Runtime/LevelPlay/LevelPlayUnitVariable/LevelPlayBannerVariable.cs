using System;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
using Unity.Services.LevelPlay;
using VirtueSky.Core;
using VirtueSky.Tracking;
#endif
using System.Collections;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class LevelPlayBannerVariable : LevelPlayAdUnitVariable
    {
        public AdsSize size;
        public AdsPosition position;
        public bool isShowOnLoad;
        private bool _isBannerDestroyed = true;
        private bool _isBannerShowing;
        private bool _previousBannerShowStatus;
        private string _placement;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        private LevelPlayBannerAd bannerAd;
        private readonly WaitForSeconds _waitBannerReload = new WaitForSeconds(5f);
        private IEnumerator _reload;
#endif
        public override void Init()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            _isBannerDestroyed = true;
            paidedCallback += AppTracking.TrackRevenue;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            if (_isBannerDestroyed || bannerAd == null)
            {
                LevelPlayBannerAd.Config.Builder builder = new LevelPlayBannerAd.Config.Builder();
                builder.SetPosition(ConvertBannerPosition());
                builder.SetSize(ConvertBannerSize());
                builder.SetDisplayOnLoad(isShowOnLoad);
                builder.SetPlacementName(_placement);
                var config = builder.Build();
                bannerAd = new LevelPlayBannerAd(Id, config);
                bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
                bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
                bannerAd.OnAdClicked += BannerOnAdClickedEvent;
                bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
                bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
                bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
                _isBannerDestroyed = false;
            }

            bannerAd.LoadAd();
#endif
        }

        void OnWaitAppOpenClosed()
        {
            if (_previousBannerShowStatus)
            {
                _previousBannerShowStatus = false;
                Show(_placement);
            }
        }

        void OnWaitAppOpenDisplayed()
        {
            _previousBannerShowStatus = _isBannerShowing;
            if (_isBannerShowing) HideBanner();
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            return bannerAd != null;
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            _isBannerShowing = true;
            IsShowing = true;
            _placement = placement;
            AdStatic.waitAppOpenClosedAction = OnWaitAppOpenClosed;
            AdStatic.waitAppOpenDisplayedAction = OnWaitAppOpenDisplayed;
            Load();
            if (bannerAd != null) bannerAd.ShowAd();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            _isBannerShowing = false;
            _isBannerDestroyed = true;
            IsShowing = false;
            AdStatic.waitAppOpenClosedAction = null;
            AdStatic.waitAppOpenDisplayedAction = null;
            if (bannerAd != null)
            {
                bannerAd.DestroyAd();
                bannerAd = null;
            }
#endif
        }

        public override void HideBanner()
        {
            base.HideBanner();
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            _isBannerShowing = false;
            IsShowing = false;
            if (bannerAd != null) bannerAd.HideAd();
#endif
        }


#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY

        private LevelPlayAdSize ConvertBannerSize()
        {
            switch (size)
            {
                case AdsSize.Banner: return LevelPlayAdSize.BANNER;
                case AdsSize.Adaptive: return LevelPlayAdSize.LARGE;
                case AdsSize.MediumRectangle: return LevelPlayAdSize.MEDIUM_RECTANGLE;
                case AdsSize.Leaderboard: return LevelPlayAdSize.LEADERBOARD;
                default: return LevelPlayAdSize.BANNER;
            }
        }

        private LevelPlayBannerPosition ConvertBannerPosition()
        {
            switch (position)
            {
                case AdsPosition.Bottom: return LevelPlayBannerPosition.BottomCenter;
                case AdsPosition.Top: return LevelPlayBannerPosition.TopCenter;
                default: return LevelPlayBannerPosition.BottomCenter;
            }
        }

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

        void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref loadedCallback, info);
            OnLoadAdEvent?.Invoke(info);
        }

        void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
        {
            var errorInfo = new AdsError(ironSourceError);
            Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
            OnFailedToLoadAdEvent?.Invoke(errorInfo);
            Destroy();
        }

        void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref clickedCallback, info);
            OnClickedAdEvent?.Invoke(info);
        }

        void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            IsShowing = true;
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref displayedCallback, info);
            OnDisplayedAdEvent?.Invoke(info);
        }

        void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError adError)
        {
            var errorInfo = new AdsError(adError);
            Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
            OnFailedToDisplayAdEvent?.Invoke(errorInfo);
        }

        void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
        {
        }

        #endregion

#endif
    }
}
