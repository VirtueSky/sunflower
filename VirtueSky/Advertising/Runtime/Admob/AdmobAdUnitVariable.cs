using System;
using UnityEngine;

namespace VirtueSky.Ads
{
    public class AdmobAdUnitVariable : AdUnitVariable
    {
        [SerializeField] protected string androidId;
        [SerializeField] protected string iOSId;
        [NonSerialized] private string idRuntime = string.Empty;

        public virtual string Id
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

        public void SetIdRuntime(string unitId)
        {
            idRuntime = unitId;
        }

        public override AdUnitVariable Show()
        {
            ResetChainCallback();
            if (!Application.isMobilePlatform || string.IsNullOrEmpty(Id) || AdStatic.IsRemoveAd) return this;
            ShowImpl();
            return this;
        }
    }
}