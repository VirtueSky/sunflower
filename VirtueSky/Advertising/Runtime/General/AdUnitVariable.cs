using System;
using UnityEngine;


namespace VirtueSky.Ads
{
    public abstract class AdUnitVariable : ScriptableObject, IAdUnit
    {
        [NonSerialized] internal Action loadedCallback;
        [NonSerialized] internal Action failedToLoadCallback;
        [NonSerialized] internal Action displayedCallback;
        [NonSerialized] internal Action failedToDisplayCallback;
        [NonSerialized] internal Action closedCallback;
        [NonSerialized] internal Action clickedCallback;
        [NonSerialized] public Action<double, string, string, string, string> paidedCallback;

        public Action OnLoadAdEvent;
        public Action<string> OnFailedToLoadAdEvent;
        public Action OnDisplayedAdEvent;
        public Action<string> OnFailedToDisplayAdEvent;
        public Action OnClosedAdEvent;
        public Action OnClickedAdEvent;


        protected virtual void ShowImpl()
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

        public abstract AdUnitVariable Show();

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