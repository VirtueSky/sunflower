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
        [UnityEngine.Tooltip("Destroy and recreate the LevelPlay ad object when reloading ads.")]
        public bool isDestroyAdOnReload = true;
        public bool IsEarnRewarded { get; private set; }
        private const float FinalizeCloseDelay = 0.2f;
        private DelayHandle _finalizeCloseHandle;

#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        LevelPlayRewardedAd rewardedAd;
#endif
        public override bool IsShowing { get; internal set; }
        public override bool IsLoading { get; internal set; }

        public override void Init(AdSetting _adSetting)
        {
            base.Init(_adSetting);
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            paidedCallback += TrackRevenue;
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
            if (IsShowing || IsLoading || IsReady()) return;

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

                IsLoading = true;
                OnRequestAdEvent?.Invoke();
                rewardedAd.LoadAd();
            }
            catch (Exception e)
            {
                IsLoading = false;
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
            ResetFinalizeCloseHandle();
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
            IsLoading = false;
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

        private void ResetRewardedAdForReload()
        {
            if (isDestroyAdOnReload) ResetRewardedAd(true);
        }

#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY

        #region Fun Callback

        internal void OnAdPaidEvent(LevelPlayImpressionData impressionData)
        {
            if (impressionData.MediationAdUnitId.Equals(Id))
            {
                paidedCallback?.Invoke(new AdsInfo(impressionData));
            }
        }

        void OnAdLoaded(LevelPlayAdInfo adInfo)
        {
            IsLoading = false;
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref loadedCallback, info);
                OnLoadedAdEvent?.Invoke(info);
            });
        }

        private void RewardedVideoOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
        {
            IsShowing = false;
            IsLoading = false;
            var errorInfo = new AdsError(ironSourceError);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToLoadCallback, errorInfo);
                OnFailedToLoadAdEvent?.Invoke(errorInfo);
            });

            ResetRewardedAdForReload();
        }

        void RewardedVideoOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = true;
            IsShowing = true;
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref displayedCallback, info);
                OnDisplayedAdEvent?.Invoke(info);
            });
        }

        void RewardedVideoOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = false;
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref closedCallback, info);
                OnClosedAdEvent?.Invoke(info);
            });

            App.CancelDelay(_finalizeCloseHandle);
            _finalizeCloseHandle = App.Delay(FinalizeCloseDelay, FinalizeClose);
        }

        void RewardedVideoOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError ironSourceError)
        {
            var errorInfo = new AdsError(ironSourceError);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref failedToDisplayCallback, errorInfo);
                OnFailedToDisplayAdEvent?.Invoke(errorInfo);
            });

            IsShowing = false;
            ResetRewardedAdForReload();
        }

        void RewardedVideoOnAdRewardedEvent(LevelPlayAdInfo info, LevelPlayReward reward)
        {
            IsEarnRewarded = true;
            ExcuteCallbackOnMainThread(() => { Common.CallActionAndClean(ref receivedRewardCallback); });
        }

        void RewardedVideoOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            var info = new AdsInfo(adInfo);
            ExcuteCallbackOnMainThread(() =>
            {
                Common.CallActionAndClean(ref clickedCallback, info);
                OnClickedAdEvent?.Invoke(info);
            });
        }

        private void FinalizeClose()
        {
            _finalizeCloseHandle = null;
            if (IsEarnRewarded)
            {
                IsEarnRewarded = false;
                ExcuteCallbackOnMainThread(() => { Common.CallActionAndClean(ref completedCallback); });
                ResetFinalizeCloseHandle();
                IsShowing = false;
                ResetRewardedAdForReload();
                Load();
                return;
            }

            ExcuteCallbackOnMainThread(() => { Common.CallActionAndClean(ref skippedCallback); });
            ResetFinalizeCloseHandle();
            IsShowing = false;
            ResetRewardedAdForReload();
            Load();
        }

        #endregion

#endif
    }
}
