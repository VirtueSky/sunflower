using System;
using UnityEngine;
using UnityEngine.Serialization;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class MaxAppOpenVariable : AdUnitVariable
    {
        [Tooltip("Automatically show AppOpenAd when app status is changed"), SerializeField]
        private bool autoShow = false;

        private bool _registerCallback = false;
        public bool AutoShow => autoShow;

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
                MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAdDisplayed;
                MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAdHidden;
                MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAdDisplayFailed;
                MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                MaxSdkCallbacks.AppOpen.OnAdClickedEvent += OnAdClicked;
                _registerCallback = true;
            }

            MaxSdk.LoadAppOpenAd(Id);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            return !string.IsNullOrEmpty(Id) && MaxSdk.IsAppOpenAdReady(Id);
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            MaxSdk.ShowAppOpenAd(Id);
#endif
        }

        public override void Destroy()
        {
        }

        #region Func Callback

#if VIRTUESKY_ADS && ADS_APPLOVIN
        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref loadedCallback);
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(info.Revenue,
                info.NetworkName,
                unit,
                info.AdFormat, AdNetwork.Max.ToString());
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
        }

        private void OnAdDisplayFailed(string unit, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
        }

        private void OnAdHidden(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.isShowingAd = false;
            Common.CallActionAndClean(ref closedCallback);

            if (!string.IsNullOrEmpty(Id)) MaxSdk.LoadAppOpenAd(Id);
        }

        private void OnAdDisplayed(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.isShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
        }

        private void OnAdClicked(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref clickedCallback);
        }
#endif

        #endregion
    }
}