using System;
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
using Unity.Services.LevelPlay;
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
        public bool IsEarnRewarded { get; private set; }
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
        LevelPlayRewardedAd rewardedAd;
#endif
        public override void Init()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            rewardedAd.OnAdLoaded += OnAdLoaded;
            rewardedAd.OnAdDisplayed += RewardedVideoOnAdDisplayedEvent;
            rewardedAd.OnAdClosed += RewardedVideoOnAdClosedEvent;
            rewardedAd.OnAdDisplayFailed += RewardedVideoOnAdDisplayFailedEvent;
            rewardedAd.OnAdRewarded += RewardedVideoOnAdRewardedEvent;
            rewardedAd.OnAdClicked += RewardedVideoOnAdClickedEvent;
            rewardedAd.OnAdLoadFailed += RewardedVideoOnAdLoadFailedEvent;
#endif
        }


        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            if (AdStatic.IsRemoveAd) return;
            var configBuilder = new LevelPlayRewardedAd.Config.Builder();
            var config = configBuilder.Build();
            rewardedAd = new LevelPlayRewardedAd(Id, config);
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            return rewardedAd.IsAdReady();
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY
            rewardedAd.ShowAd();
#endif
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

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
            skippedCallback = null;
        }


#if VIRTUESKY_ADS && VIRTUESKY_LEVELPLAY

        #region Fun Callback

        void OnAdLoaded(LevelPlayAdInfo adInfo)
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        private void RewardedVideoOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(ironSourceError.ToString());
        }

        void RewardedVideoOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        void RewardedVideoOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            AdStatic.IsShowingAd = false;
            Common.CallActionAndClean(ref closedCallback);
            OnClosedAdEvent?.Invoke();
            if (!IsReady()) rewardedAd.LoadAd();
            if (IsEarnRewarded)
            {
                Common.CallActionAndClean(ref completedCallback);
                IsEarnRewarded = false;
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
        }

        void RewardedVideoOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError ironSourceError)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
            OnFailedToDisplayAdEvent?.Invoke(ironSourceError.ToString());
        }

        void RewardedVideoOnAdRewardedEvent(LevelPlayAdInfo info, LevelPlayReward reward)
        {
            IsEarnRewarded = true;
        }

        void RewardedVideoOnAdClickedEvent(LevelPlayAdInfo adInfo)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        #endregion

#endif
    }
}