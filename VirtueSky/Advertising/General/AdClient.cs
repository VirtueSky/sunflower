using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VirtueSky.EditorUtils;

namespace VirtueSky.Ads
{
    public abstract class AdClient : ScriptableObject
    {
        [SerializeField, ReadOnly] protected AdSetting adSetting;
        protected bool statusAppOpenFirstIgnore;

        public abstract void Initialize();
        public abstract void LoadInterstitial();
        public abstract bool IsInterstitialReady();
        public abstract void LoadRewarded();
        public abstract bool IsRewardedReady();
        public abstract void LoadRewardedInterstitial();
        public abstract bool IsRewardedInterstitialReady();
        public abstract void LoadAppOpen();
        public abstract bool IsAppOpenReady();

#if UNITY_EDITOR
        private void Reset()
        {
            adSetting = ScriptableSetting.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads");
        }
#endif
    }
}