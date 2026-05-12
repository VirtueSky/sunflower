using System;
using VirtueSky.Core;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
using Unity.Services.LevelPlay;
using VirtueSky.Tracking;
#endif
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class LevelPlayRewardVariable : LevelPlayAdUnitVariable
    {
        [NonSerialized] internal Action completedCallback;
        [NonSerialized] internal Action skippedCallback;
        [NonSerialized] internal Action receivedRewardCallback;
        public bool IsEarnRewarded { get; private set; }
        private const float FinalizeCloseDelay = 0.2f;
        private DelayHandle _finalizeCloseHandle;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        LevelPlayRewardedAd rewardedAd;
#endif
        public override void Init()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            paidedCallback += AppTracking.TrackRevenue;
#endif
        }


        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            if (string.IsNullOrEmpty(Id))
            {
                UnityEngine.Debug.LogWarning("LevelPlay rewarded load skipped because ad unit id is empty.");
                return;
            }

            try
            {
                if (rewardedAd == null)
                {
                    var configBuilder = new LevelPlayRewardedAd.Config.Builder();
                    var config = configBuilder.Build();
                    rewardedAd = new LevelPlayRewardedAd(Id, config);
                    rewardedAd.OnAdLoaded += OnAdLoaded;
                    rewardedAd.OnAdDisplayed += RewardedVideoOnAdDisplayedEvent;
                    rewardedAd.OnAdClosed += RewardedVideoOnAdClosedEvent;
                    rewardedAd.OnAdDisplayFailed += RewardedVideoOnAdDisplayFailedEvent;
                    rewardedAd.OnAdRewarded += RewardedVideoOnAdRewardedEvent;
                    rewardedAd.OnAdClicked += RewardedVideoOnAdClickedEvent;
                    rewardedAd.OnAdLoadFailed += RewardedVideoOnAdLoadFailedEvent;
                }

                rewardedAd.LoadAd();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"LevelPlay rewarded load failed during SDK call, resetting ad instance. {e}");
                ResetRewardedAd();
            }
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (rewardedAd == null) return false;

            try
            {
                return rewardedAd.IsAdReady();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"LevelPlay rewarded IsAdReady failed, resetting ad instance. {e}");
                ResetRewardedAd();
                return false;
            }
#else
            return false;
#endif
        }

        protected override void ShowImpl(string placement = "")
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (rewardedAd != null)
            {
                IsShowing = true;
                rewardedAd.ShowAd(placement);
            }
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
            ResetRewardedAd(true);
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

        private void ResetRewardedAd(bool isDestroy = false)
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (rewardedAd == null) return;
            rewardedAd.OnAdLoaded -= OnAdLoaded;
            rewardedAd.OnAdDisplayed -= RewardedVideoOnAdDisplayedEvent;
            rewardedAd.OnAdClosed -= RewardedVideoOnAdClosedEvent;
            rewardedAd.OnAdDisplayFailed -= RewardedVideoOnAdDisplayFailedEvent;
            rewardedAd.OnAdRewarded -= RewardedVideoOnAdRewardedEvent;
            rewardedAd.OnAdClicked -= RewardedVideoOnAdClickedEvent;
            rewardedAd.OnAdLoadFailed -= RewardedVideoOnAdLoadFailedEvent;
            if (isDestroy) rewardedAd.DestroyAd();
            rewardedAd = null;
#endif
        }

#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY

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

        void OnAdLoaded(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref loadedCallback, info);
            OnLoadAdEvent?.Invoke(info);
        }

        private void RewardedVideoOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
        {
            IsShowing = false;
            var errorInfo = new AdsError(ironSourceError);
            Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
            OnFailedToLoadAdEvent?.Invoke(errorInfo);
            ResetRewardedAd(true);
        }

        void RewardedVideoOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref displayedCallback, info);
            OnDisplayedAdEvent?.Invoke(info);
        }

        void RewardedVideoOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = false;
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref closedCallback, info);
            OnClosedAdEvent?.Invoke(info);
            App.CancelDelay(_finalizeCloseHandle);
            _finalizeCloseHandle = App.Delay(FinalizeCloseDelay, FinalizeClose);
        }

        void RewardedVideoOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError ironSourceError)
        {
            var errorInfo = new AdsError(ironSourceError);
            Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
            OnFailedToDisplayAdEvent?.Invoke(errorInfo);
            IsShowing = false;
            ResetRewardedAd(true);
        }

        void RewardedVideoOnAdRewardedEvent(LevelPlayAdInfo info, LevelPlayReward reward)
        {
            IsEarnRewarded = true;
            Common.CallActionAndClean(ref receivedRewardCallback);
        }

        void RewardedVideoOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            Common.CallActionAndClean(ref clickedCallback, info);
            OnClickedAdEvent?.Invoke(info);
        }

        private void FinalizeClose()
        {
            _finalizeCloseHandle = null;
            if (IsEarnRewarded)
            {
                IsEarnRewarded = false;
                Common.CallActionAndClean(ref completedCallback);
                ResetFinalizeCloseHandle();
                IsShowing = false;
                ResetRewardedAd(true);
                Load();
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
            ResetFinalizeCloseHandle();
            IsShowing = false;
            ResetRewardedAd(true);
            Load();
        }

        #endregion

#endif
    }
}
