using System;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
using Unity.Services.LevelPlay;
#endif
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
        private bool _isBannerDestroyed = true;
        private bool _isBannerShowing;
        private bool _previousBannerShowStatus;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        private LevelPlayBannerAd bannerAd;  
#endif
        public override void Init()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
            bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
            bannerAd.OnAdClicked += BannerOnAdClickedEvent;
            bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
            bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
            bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            if (_isBannerDestroyed)
            {
                LevelPlayBannerAd.Config.Builder builder = new LevelPlayBannerAd.Config.Builder();
                builder.SetPosition(ConvertBannerPosition());
                builder.SetSize(ConvertBannerSize());
                var config = builder.Build();
                bannerAd = new LevelPlayBannerAd(Id, config);
                bannerAd.LoadAd();
                _isBannerDestroyed = false;
            }
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
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            return true;
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            _isBannerShowing = true;
            AdStatic.waitAppOpenClosedAction = OnWaitAppOpenClosed;
            AdStatic.waitAppOpenDisplayedAction = OnWaitAppOpenDisplayed;
            Load();
            bannerAd.ShowAd();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            _isBannerShowing = false;
            _isBannerDestroyed = true;
            AdStatic.waitAppOpenClosedAction = null;
            AdStatic.waitAppOpenDisplayedAction = null;
            bannerAd.DestroyAd();
#endif
        }

        public override void HideBanner()
        {
            base.HideBanner();
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            _isBannerShowing = false;
            bannerAd.HideAd();
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

        void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(ironSourceError.ErrorMessage);
            Destroy();
        }

        void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError adError)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
            OnFailedToDisplayAdEvent?.Invoke(adError.ErrorMessage);
        }

        void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
        {
        }

        #endregion

#endif
    }
}