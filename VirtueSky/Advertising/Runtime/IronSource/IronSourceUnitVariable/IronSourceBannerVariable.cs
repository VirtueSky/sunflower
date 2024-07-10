using System;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class IronSourceBannerVariable : AdUnitVariable
    {
        public BannerSize size;
        public BannerPosition position;
        private bool _isBannerDestroyed = true;
        private bool _isBannerShowing;
        private bool _previousBannerShowStatus;

        public override void Init()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            if (AdStatic.IsRemoveAd) return;
            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            if (AdStatic.IsRemoveAd) return;
            var bannerSize = ConvertBannerSize();
            if (size == BannerSize.Adaptive) bannerSize.SetAdaptive(true);
            if (_isBannerDestroyed)
            {
                IronSource.Agent.loadBanner(bannerSize, ConvertBannerPosition());
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
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            return true;
#else
            return false;
#endif
        }

        public override AdUnitVariable Show()
        {
            ResetChainCallback();
            if (!Application.isMobilePlatform || AdStatic.IsRemoveAd || !IsReady()) return this;
            ShowImpl();
            return this;
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            _isBannerShowing = true;
            AdStatic.waitAppOpenClosedAction = OnWaitAppOpenClosed;
            AdStatic.waitAppOpenDisplayedAction = OnWaitAppOpenDisplayed;
            Load();
            IronSource.Agent.displayBanner();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            _isBannerShowing = false;
            _isBannerDestroyed = true;
            AdStatic.waitAppOpenClosedAction = null;
            AdStatic.waitAppOpenDisplayedAction = null;
            IronSource.Agent.destroyBanner();
#endif
        }

        public override void HideBanner()
        {
            base.HideBanner();
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            _isBannerShowing = false;
            IronSource.Agent.hideBanner();
#endif
        }


#if VIRTUESKY_ADS && ADS_IRONSOURCE

        private IronSourceBannerSize ConvertBannerSize()
        {
            switch (size)
            {
                case BannerSize.Banner: return IronSourceBannerSize.BANNER;
                case BannerSize.Adaptive: return IronSourceBannerSize.BANNER;
                case BannerSize.MediumRectangle: return IronSourceBannerSize.RECTANGLE;
                case BannerSize.Leaderboard: return IronSourceBannerSize.LARGE;
                default: return IronSourceBannerSize.BANNER;
            }
        }

        private IronSourceBannerPosition ConvertBannerPosition()
        {
            switch (position)
            {
                case BannerPosition.Bottom: return IronSourceBannerPosition.BOTTOM;
                case BannerPosition.Top: return IronSourceBannerPosition.TOP;
                default: return IronSourceBannerPosition.BOTTOM;
            }
        }

        #region Fun Callback

        void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(ironSourceError.ToString());
            Destroy();
        }

        void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref closedCallback);
            OnClosedAdEvent?.Invoke();
        }

        void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
        {
        }

        #endregion

#endif
    }
}