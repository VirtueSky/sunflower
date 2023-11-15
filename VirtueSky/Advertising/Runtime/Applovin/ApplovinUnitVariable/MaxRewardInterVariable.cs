using System;
using VirtueSky.Ads;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    public class MaxRewardInterVariable : AdUnitVariable
    {
        [NonSerialized] internal Action completedCallback;
        [NonSerialized] internal Action skippedCallback;

        private bool _registerCallback = false;
        public bool IsEarnRewarded { get; private set; }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            return !string.IsNullOrEmpty(Id) && MaxSdk.IsRewardedInterstitialAdReady(Id);
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            MaxSdk.ShowRewardedInterstitialAd(Id);
#endif
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
            skippedCallback = null;
        }

        public override AdUnitVariable Show()
        {
            ResetChainCallback();
            if (!UnityEngine.Application.isMobilePlatform || !IsReady()) return this;
            ShowImpl();
            return this;
        }

        public override void Destroy()
        {
        }

        public override void Init()
        {
            _registerCallback = false;
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_APPLOVIN
            if (string.IsNullOrEmpty(Id)) return;
            if (!_registerCallback)
            {
                MaxSdkCallbacks.RewardedInterstitial.OnAdDisplayedEvent += OnAdDisplayed;
                MaxSdkCallbacks.RewardedInterstitial.OnAdHiddenEvent += OnAdHidden;
                MaxSdkCallbacks.RewardedInterstitial.OnAdDisplayFailedEvent += OnAdDisplayFailed;
                MaxSdkCallbacks.RewardedInterstitial.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.RewardedInterstitial.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.RewardedInterstitial.OnAdReceivedRewardEvent += OnAdReceivedReward;
                MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                _registerCallback = true;
            }

            MaxSdk.LoadRewardedInterstitialAd(Id);
#endif
        }

        #region Func Callback

#if VIRTUESKY_ADS && ADS_APPLOVIN
        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(info.Revenue,
                info.NetworkName,
                unit,
                info.AdFormat);
        }

        private void OnAdReceivedReward(string unit, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo info)
        {
            IsEarnRewarded
                =
                true;
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo error)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref loadedCallback);
        }

        private void OnAdDisplayFailed(string unit, MaxSdkBase.ErrorInfo error, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
        }

        private void OnAdHidden(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.isShowingAd = false;
            Common.CallActionAndClean(ref closedCallback);
            if (!IsReady()) MaxSdk.LoadRewardedInterstitialAd(Id);

            if (IsEarnRewarded)
            {
                Common.CallActionAndClean(ref completedCallback);
                IsEarnRewarded = false;
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
        }

        private void OnAdDisplayed(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.isShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
        }
#endif

        #endregion
    }
}