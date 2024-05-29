using System;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class MaxBannerVariable : AdUnitVariable
    {
        public BannerSize size;
        public BannerPosition position;

        private bool isBannerDestroyed = true;
        private bool _registerCallback = false;
        private bool _isBannerShowing;
        private bool _previousBannerShowStatus;

        public override void Init()
        {
            _registerCallback = false;
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            if (!_registerCallback)
            {
                MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnAdExpanded;
                MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnAdCollapsed;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                MaxSdkCallbacks.Banner.OnAdClickedEvent += OnAdClicked;
                if (size != BannerSize.Adaptive)
                    MaxSdk.SetBannerExtraParameter(Id, "adaptive_banner", "false");
                _registerCallback = true;
            }

            if (isBannerDestroyed)
            {
                MaxSdk.CreateBanner(Id, ConvertPosition());
                isBannerDestroyed = false;
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
            if (_isBannerShowing) Hide();
        }

        public override bool IsReady()
        {
            return !string.IsNullOrEmpty(Id);
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            _isBannerShowing = true;
            AdStatic.waitAppOpenClosedAction = OnWaitAppOpenClosed;
            AdStatic.waitAppOpenDisplayedAction = OnWaitAppOpenDisplayed;
            Load();
            MaxSdk.ShowBanner(Id);
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (string.IsNullOrEmpty(Id)) return;
            _isBannerShowing = false;
            isBannerDestroyed = true;
            AdStatic.waitAppOpenClosedAction = null;
            AdStatic.waitAppOpenDisplayedAction = null;
            MaxSdk.DestroyBanner(Id);
#endif
        }

        public void Hide()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            _isBannerShowing = false;
            if (string.IsNullOrEmpty(Id)) return;
            MaxSdk.HideBanner(Id);
#endif
        }

        #region Fun Callback

#if VIRTUESKY_ADS && ADS_APPLOVIN
        public MaxSdkBase.BannerPosition ConvertPosition()
        {
            switch (position)
            {
                case BannerPosition.Top: return MaxSdkBase.BannerPosition.TopCenter;
                case BannerPosition.Bottom: return MaxSdkBase.BannerPosition.BottomCenter;
                case BannerPosition.TopLeft: return MaxSdkBase.BannerPosition.TopLeft;
                case BannerPosition.TopRight: return MaxSdkBase.BannerPosition.TopRight;
                case BannerPosition.BottomLeft: return MaxSdkBase.BannerPosition.BottomLeft;
                case BannerPosition.BottomRight: return MaxSdkBase.BannerPosition.BottomRight;
                default:
                    return MaxSdkBase.BannerPosition.BottomCenter;
            }
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(info.Revenue,
                info.NetworkName,
                unit,
                info.AdFormat, AdNetwork.Max.ToString());
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        private void OnAdExpanded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(info.Message);
        }

        private void OnAdCollapsed(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref closedCallback);
            OnClosedAdEvent?.Invoke();
        }

        private void OnAdClicked(string arg1, MaxSdkBase.AdInfo arg2)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

#endif

        #endregion
    }
}