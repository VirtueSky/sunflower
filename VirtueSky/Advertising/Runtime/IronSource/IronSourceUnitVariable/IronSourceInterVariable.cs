using System;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    [EditorIcon("icon_scriptable")]
    public class IronSourceInterVariable : AdUnitVariable
    {
        [NonSerialized] internal Action completedCallback;

        public override void Init()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            if (AdStatic.IsRemoveAd) return;
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
            IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
#endif
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            if (AdStatic.IsRemoveAd) return;
            IronSource.Agent.loadInterstitial();
            OnAdLoaded();
#endif
        }

        public override bool IsReady()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            return IronSource.Agent.isInterstitialReady();
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            IronSource.Agent.showInterstitial();
#endif
        }

        public override void Destroy()
        {
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
        }


#if VIRTUESKY_ADS && ADS_IRONSOURCE

        #region Fun Callback

        void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
        {
        }

        void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnFailedToLoadAdEvent?.Invoke(ironSourceError.ToString());
        }

        void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
        {
            AdStatic.isShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
            OnDisplayedAdEvent?.Invoke();
        }

        void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnClickedAdEvent?.Invoke();
        }

        void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
        {
        }

        void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
            OnFailedToDisplayAdEvent?.Invoke(ironSourceError.ToString());
        }

        void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            AdStatic.isShowingAd = false;
            Common.CallActionAndClean(ref completedCallback);
            OnClosedAdEvent?.Invoke();
            Load();
        }

        private void OnAdLoaded()
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnLoadAdEvent?.Invoke();
        }

        #endregion

#endif
    }
}