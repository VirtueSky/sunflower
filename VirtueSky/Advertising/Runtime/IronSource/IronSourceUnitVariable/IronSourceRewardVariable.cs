using System;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class IronSourceRewardVariable : AdUnitVariable
    {
        [NonSerialized] internal Action completedCallback;
        [NonSerialized] internal Action skippedCallback;
        public bool IsEarnRewarded { get; private set; }

        public override void Init()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            if (AdStatic.IsRemoveAd) return;
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += RewardedVideoOnAdLoadFailedEvent;
#endif
        }


        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            if (AdStatic.IsRemoveAd) return;
            IronSource.Agent.loadRewardedVideo();
            OnAdLoaded();
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            return IronSource.Agent.isRewardedVideoAvailable();
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            IronSource.Agent.showRewardedVideo();
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


#if VIRTUESKY_ADS && ADS_IRONSOURCE

        #region Fun Callback

        void OnAdLoaded()
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        private void RewardedVideoOnAdLoadFailedEvent(IronSourceError ironSourceError)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(ironSourceError.ToString());
        }

        void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            AdStatic.isShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            AdStatic.isShowingAd = false;
            Common.CallActionAndClean(ref closedCallback);
            OnClosedAdEvent?.Invoke();
            if (!IsReady()) IronSource.Agent.loadRewardedVideo();
            if (IsEarnRewarded)
            {
                Common.CallActionAndClean(ref completedCallback);
                IsEarnRewarded = false;
                return;
            }

            Common.CallActionAndClean(ref skippedCallback);
        }

        void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
        {
        }

        void RewardedVideoOnAdUnavailable()
        {
        }

        void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
            OnFailedToDisplayAdEvent?.Invoke(ironSourceError.ToString());
        }

        void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        {
            IsEarnRewarded = true;
        }

        void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        #endregion

#endif
    }
}