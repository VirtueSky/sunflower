using System;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Tracking;


namespace VirtueSky.Ads
{
    public abstract class AdUnitVariable : ScriptableObject
    {
        [NonSerialized] internal Action<AdsInfo> loadedCallback;
        [NonSerialized] internal Action<AdsError> failedToLoadCallback;
        [NonSerialized] internal Action<AdsInfo> displayedCallback;
        [NonSerialized] internal Action<AdsError> failedToDisplayCallback;
        [NonSerialized] internal Action<AdsInfo> closedCallback;
        [NonSerialized] internal Action<AdsInfo> clickedCallback;
        [NonSerialized] public Action<AdsInfo> paidedCallback;

        public Action<AdsInfo> OnLoadAdEvent;
        public Action<AdsError> OnFailedToLoadAdEvent;
        public Action<AdsInfo> OnDisplayedAdEvent;
        public Action<AdsError> OnFailedToDisplayAdEvent;
        public Action<AdsInfo> OnClosedAdEvent;
        public Action<AdsInfo> OnClickedAdEvent;

        public abstract bool IsShowing { get; internal set; }
        public abstract bool IsLoading { get; internal set; }

        protected AdSetting adSetting;
        public virtual string Id
        {
            get => "";
        }

        protected virtual void ShowImpl(string placement = "")
        {
        }

        protected virtual void ResetChainCallback()
        {
            loadedCallback = null;
            failedToDisplayCallback = null;
            failedToLoadCallback = null;
            closedCallback = null;
        }

        public virtual void HideBanner()
        {
        }

        public abstract AdUnitVariable Show(string placement = "");

        public virtual void Init(AdSetting _adSetting)
        {
            adSetting = _adSetting;
        }

        public virtual void Load()
        {
        }

        public virtual bool IsReady()
        {
            return false;
        }

        public virtual void Destroy()
        {
        }
        protected void ExcuteCallbackOnMainThread(Action action)
        {
            if (action == null)
                return;
            if (adSetting == null || adSetting.ExcuteCallbackOnMainThread)
            {
                App.RunOnMainThread(action);
                return;
            }

            action.Invoke();
        }
        protected void TrackRevenue(AdsInfo info)
        {
            if (!adSetting.EnableTrackAdRevenue) return;
            FirebaseAnalyticTrackingRevenue.FirebaseAnalyticTrackRevenue(info.Revenue, info.AdNetwork, info.AdUnitId, info.AdFormat, info.AdMediation);
            AdjustTrackingRevenue.AdjustTrackRevenue(info.Revenue, info.AdNetwork, info.AdUnitId, info.AdFormat, info.AdMediation);
            AppsFlyerTrackingRevenue.AppsFlyerTrackRevenueAd(info.Revenue, info.AdNetwork, info.AdUnitId, info.AdFormat, info.AdMediation);
        }
    }
}
