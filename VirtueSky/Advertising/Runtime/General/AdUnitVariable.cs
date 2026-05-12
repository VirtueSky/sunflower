using System;
using UnityEngine;


namespace VirtueSky.Ads
{
    public abstract class AdUnitVariable : ScriptableObject, IAdUnit
    {
        [NonSerialized] internal Action<AdsInfo> loadedCallback;
        [NonSerialized] internal Action<AdsError> failedToLoadCallback;
        [NonSerialized] internal Action<AdsInfo> displayedCallback;
        [NonSerialized] internal Action<AdsError> failedToDisplayCallback;
        [NonSerialized] internal Action<AdsInfo> closedCallback;
        [NonSerialized] internal Action<AdsInfo> clickedCallback;
        [NonSerialized] public Action<double, string, string, string, string> paidedCallback;

        public Action<AdsInfo> OnLoadAdEvent;
        public Action<AdsError> OnFailedToLoadAdEvent;
        public Action<AdsInfo> OnDisplayedAdEvent;
        public Action<AdsError> OnFailedToDisplayAdEvent;
        public Action<AdsInfo> OnClosedAdEvent;
        public Action<AdsInfo> OnClickedAdEvent;

        public abstract bool IsShowing { get; internal set; }
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

        public virtual void Init()
        {
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
    }
}
