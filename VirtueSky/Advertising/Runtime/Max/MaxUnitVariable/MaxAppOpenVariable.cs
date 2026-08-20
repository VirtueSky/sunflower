using System;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class MaxAppOpenVariable : MaxAdUnitVariable
    {
        [Tooltip("Automatically show AppOpenAd when app status is changed")]
        public bool autoShow = false;

        [Tooltip("Time between closing the previous full-screen ad and starting to show the app open ad - in seconds")]
        public float timeBetweenFullScreenAd = 2f;

        public override bool IsShowing { get; internal set; }
        public override bool IsLoading { get; internal set; }

        public override void Init(AdSetting _adSetting)
        {
            base.Init(_adSetting);
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            paidedCallback += TrackRevenue;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAdDisplayed;
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAdHidden;
            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAdLoaded;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAdDisplayFailed;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAdLoadFailed;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdkCallbacks.AppOpen.OnAdClickedEvent += OnAdClicked;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            IsLoading = true;
            OnRequestAdEvent?.Invoke();
            MaxSdk.LoadAppOpenAd(Id);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            return !string.IsNullOrEmpty(Id) && MaxSdk.IsAppOpenAdReady(Id) &&
                   (DateTime.Now - AdStatic.AdClosingTime).TotalSeconds > timeBetweenFullScreenAd;
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.ShowAppOpenAd(Id, placement: placement);
#endif
        }

        public override void Destroy()
        {
        }

        #region Func Callback

#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
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

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(new AdsInfo(info));
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
        }

        private void OnAdClicked(string arg1, MaxSdkBase.AdInfo info)
        {
            var adInfo = new AdsInfo(info);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref clickedCallback, adInfo);
                OnClickedAdEvent?.Invoke(adInfo);
            });
        }

        private void OnAdDisplayFailed(string unit, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo info)
        {
            var error = new AdsError(errorInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToDisplayCallback, error);
                OnFailedToDisplayAdEvent?.Invoke(error);
            });
        }

        private void OnAdHidden(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.waitAppOpenClosedAction?.Invoke();
            AdStatic.IsShowingAd = false;
            IsShowing = false;
            var adsInfo = new AdsInfo(info);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref closedCallback, adsInfo);
                OnClosedAdEvent?.Invoke(adsInfo);
            });

            Load();
        }

        private void OnAdDisplayed(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.waitAppOpenDisplayedAction?.Invoke();
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var adsInfo = new AdsInfo(info);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref displayedCallback, adsInfo);
                OnDisplayedAdEvent?.Invoke(adsInfo);
            });
        }
#endif

        #endregion
    }
}
