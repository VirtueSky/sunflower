using System;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class MaxBannerVariable : MaxAdUnitVariable
    {
        public AdsSize size = AdsSize.Banner;
        public AdsPosition position = AdsPosition.Bottom;

        private bool _isBannerDestroyed = true;
        private bool _isBannerShowing;
        private bool _previousBannerShowStatus;
        private string _placement;

        public override bool IsShowing { get; internal set; }
        public override bool IsLoading { get; internal set; }

        public override void Init(AdSetting _adSetting)
        {
            base.Init(_adSetting);
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            paidedCallback += TrackRevenue;
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoaded;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnAdExpanded;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailed;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnAdCollapsed;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnAdClicked;
            // if (size != AdsSize.Adaptive)
            // {
            //     MaxSdk.SetBannerExtraParameter(Id, "adaptive_banner", "false");
            // }
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            if (_isBannerDestroyed)
            {
                if (!string.IsNullOrEmpty(_placement))
                {
                    MaxSdk.SetBannerPlacement(Id, _placement);
                }

                IsLoading = true;
                var config = new MaxSdkBase.AdViewConfiguration(ConvertPosition())
                {
                    IsAdaptive = size == AdsSize.Adaptive
                };
                OnRequestAdEvent?.Invoke();
                MaxSdk.CreateBanner(Id, config);
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
            return !string.IsNullOrEmpty(Id);
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            _isBannerShowing = true;
            IsShowing = true;
            _placement = placement;
            AdStatic.waitAppOpenClosedAction = OnWaitAppOpenClosed;
            AdStatic.waitAppOpenDisplayedAction = OnWaitAppOpenDisplayed;
            Load();
            MaxSdk.ShowBanner(Id);
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (string.IsNullOrEmpty(Id)) return;
            _isBannerShowing = false;
            _isBannerDestroyed = true;
            IsShowing = false;
            AdStatic.waitAppOpenClosedAction = null;
            AdStatic.waitAppOpenDisplayedAction = null;
            MaxSdk.DestroyBanner(Id);
#endif
        }

        public override void HideBanner()
        {
            base.HideBanner();
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            _isBannerShowing = false;
            IsShowing = false;
            if (string.IsNullOrEmpty(Id)) return;
            MaxSdk.HideBanner(Id);
#endif
        }

        #region Fun Callback

#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
        public MaxSdkBase.AdViewPosition ConvertPosition()
        {
            switch (position)
            {
                case AdsPosition.Top: return MaxSdkBase.AdViewPosition.TopCenter;
                case AdsPosition.Bottom: return MaxSdkBase.AdViewPosition.BottomCenter;
                case AdsPosition.TopLeft: return MaxSdkBase.AdViewPosition.TopLeft;
                case AdsPosition.TopRight: return MaxSdkBase.AdViewPosition.TopRight;
                case AdsPosition.BottomLeft: return MaxSdkBase.AdViewPosition.BottomLeft;
                case AdsPosition.BottomRight: return MaxSdkBase.AdViewPosition.BottomRight;
                default:
                    return MaxSdkBase.AdViewPosition.BottomCenter;
            }
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(new AdsInfo(info));
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            IsLoading = false;
            var adsInfo = new AdsInfo(info);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref loadedCallback, adsInfo);
                OnLoadedAdEvent?.Invoke(adsInfo);
            });
        }

        private void OnAdClicked(string arg1, MaxSdkBase.AdInfo arg2)
        {
            var info = new AdsInfo(arg2);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref clickedCallback, info);
                OnClickedAdEvent?.Invoke(info);
            });
        }

        private void OnAdExpanded(string unit, MaxSdkBase.AdInfo info)
        {
            var adsInfo = new AdsInfo(info);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref displayedCallback, adsInfo);
                OnDisplayedAdEvent?.Invoke(adsInfo);
            });
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            IsLoading = false;
            var errorInfo = new AdsError(info);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
                OnFailedToLoadAdEvent?.Invoke(errorInfo);
            });

            Destroy();
        }

        private void OnAdCollapsed(string unit, MaxSdkBase.AdInfo info)
        {
            var adsInfo = new AdsInfo(info);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref closedCallback, adsInfo);
                OnClosedAdEvent?.Invoke(adsInfo);
            });
        }
#endif

        #endregion
    }
}
