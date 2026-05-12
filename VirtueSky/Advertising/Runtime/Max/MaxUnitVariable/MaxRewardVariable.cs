using System;
using VirtueSky.Core;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class MaxRewardVariable : MaxAdUnitVariable
    {
        [NonSerialized] internal Action completedCallback;
        [NonSerialized] internal Action skippedCallback;
        [NonSerialized] internal Action receivedRewardCallback;
        public bool IsEarnRewarded { get; private set; }
        private const float FinalizeCloseDelay = 0.2f;
        private DelayHandle _finalizeCloseHandle;

        public override void Init()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (string.IsNullOrEmpty(Id)) return;
            paidedCallback += AppTracking.TrackRevenue;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdHidden;
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnAdDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaid;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedReward;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnAdClicked;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            if (string.IsNullOrEmpty(Id)) return;
            MaxSdk.LoadRewardedAd(Id);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            return !string.IsNullOrEmpty(Id) && MaxSdk.IsRewardedAdReady(Id);
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.ShowRewardedAd(Id, placement: placement);
#endif
        }

        public override AdUnitVariable Show(string placement = "")
        {
            ResetChainCallback();
            if (!UnityEngine.Application.isMobilePlatform || !IsReady()) return this;
            ShowImpl(placement);
            return this;
        }

        public override void Destroy()
        {
            IsShowing = false;
        }

        private void ResetFinalizeCloseHandle()
        {
            App.CancelDelay(_finalizeCloseHandle);
            _finalizeCloseHandle = null;
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
            skippedCallback = null;
            receivedRewardCallback = null;
            IsEarnRewarded = false;
        }

        #region Func Callback

#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
        private void OnAdReceivedReward(string unit, MaxSdkBase.Reward reward,
            MaxSdkBase.AdInfo info)
        {
            IsEarnRewarded = true;
            Common.CallActionAndClean(ref receivedRewardCallback);
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(info.Revenue,
                info.NetworkName,
                unit,
                info.AdFormat, AdMediation.AppLovin.ToString());
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            var errorInfo = new AdsError(info);
            Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
            OnFailedToLoadAdEvent?.Invoke(errorInfo);
        }

        private void OnAdDisplayFailed(string unit, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo info)
        {
            var error = new AdsError(errorInfo);
            Common.CallActionAndClean(ref failedToDisplayCallback, error);
            OnFailedToDisplayAdEvent?.Invoke(error);
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            var adsInfo = new AdsInfo(info);
            Common.CallActionAndClean(ref loadedCallback, adsInfo);
            OnLoadAdEvent?.Invoke(adsInfo);
        }

        private void OnAdHidden(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.IsShowingAd = false;
            var adsInfo = new AdsInfo(info);
            Common.CallActionAndClean(ref closedCallback, adsInfo);
            OnClosedAdEvent?.Invoke(adsInfo);
            App.CancelDelay(_finalizeCloseHandle);
            _finalizeCloseHandle = App.Delay(FinalizeCloseDelay, FinalizeClose);
        }

        private void OnAdDisplayed(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var adsInfo = new AdsInfo(info);
            Common.CallActionAndClean(ref displayedCallback, adsInfo);
            OnDisplayedAdEvent?.Invoke(adsInfo);
        }

        private void OnAdClicked(string unit, MaxSdkBase.AdInfo info)
        {
            var adsInfo = new AdsInfo(info);
            Common.CallActionAndClean(ref clickedCallback, adsInfo);
            OnClickedAdEvent?.Invoke(adsInfo);
        }

        private void FinalizeClose()
        {
            _finalizeCloseHandle = null;
            if (IsEarnRewarded)
            {
                Common.CallActionAndClean(ref completedCallback);
                IsEarnRewarded = false;
                ResetFinalizeCloseHandle();
                IsShowing = false;
                if (!IsReady()) Load();
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
            ResetFinalizeCloseHandle();
            IsShowing = false;
            if (!IsReady()) Load();
        }
#endif

        #endregion
    }
}
