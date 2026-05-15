using System;
using UnityEngine;

namespace VirtueSky.Ads
{
    public class MaxAdUnitVariable : AdUnitVariable
    {
        [SerializeField] protected string androidId;
        [SerializeField] protected string iOSId;
        [NonSerialized] private string idRuntime = string.Empty;

        public override bool IsShowing { get; internal set; }
        public override bool IsLoading { get; internal set; }

        public override string Id
        {
            get
            {
                if (idRuntime == String.Empty)
                {
#if UNITY_ANDROID
                    return androidId;
#elif UNITY_IOS
                    return iOSId;
#else
                    return string.Empty;
#endif
                }

                return idRuntime;
            }
        }

        public override AdUnitVariable Show(string placement = null)
        {
            ResetChainCallback();
            if (!Application.isMobilePlatform || string.IsNullOrEmpty(Id) || AdStatic.IsRemoveAd ||
                !IsReady()) return this;
            ShowImpl(placement);
            return this;
        }

        public void SetIdRuntime(string unitId)
        {
            idRuntime = unitId;
        }

        
    }
}