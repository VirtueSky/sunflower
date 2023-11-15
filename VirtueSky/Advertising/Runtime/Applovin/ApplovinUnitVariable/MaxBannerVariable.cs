using System;
using UnityEngine;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    public class MaxBannerVariable : AdUnitVariable
    {
        public BannerSize size;
        public BannerPosition position;

        private bool isBannerDestroyed = true;
        private bool _registerCallback = false;

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
                if (size != BannerSize.Adaptive) MaxSdk.SetBannerExtraParameter(Id, "adaptive_banner", "false");
                _registerCallback = true;
            }

            if (isBannerDestroyed)
            {
                MaxSdk.CreateBanner(Id, ConvertPosition());
            }
#endif
        }

        public override bool IsReady()
        {
            return !string.IsNullOrEmpty(Id);
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            Load();
            MaxSdk.ShowBanner(Id);
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (string.IsNullOrEmpty(Id)) return;
            isBannerDestroyed = true;
            MaxSdk.DestroyBanner(Id);
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
                info.AdFormat);
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref loadedCallback);
        }

        private void OnAdExpanded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref displayedCallback);
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
        }

        private void OnAdCollapsed(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref closedCallback);
        }


#endif

        #endregion
    }
}