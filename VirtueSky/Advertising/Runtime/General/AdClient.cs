using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Ads
{
    public abstract class AdClient
    {
        [SerializeField, ReadOnly] protected AdSetting adSetting;
        protected bool statusAppOpenFirstIgnore;

        public void SetupAdSetting(AdSetting _adSetting)
        {
            this.adSetting = _adSetting;
        }

        public abstract void Initialize();

        #region Inter Ad

        private AdUnitVariable InterstitialAdUnit()
        {
            return adSetting.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSetting.MaxInterVariable,
                _ => adSetting.AdmobInterVariable,
            };
        }

        protected virtual bool IsInterstitialReady()
        {
            return InterstitialAdUnit().IsReady();
        }

        public virtual void LoadInterstitial()
        {
            if (!IsInterstitialReady()) InterstitialAdUnit().Load();
        }

        #endregion

        #region Reward Ad

        private AdUnitVariable RewardAdUnit()
        {
            return adSetting.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSetting.MaxRewardVariable,
                _ => adSetting.AdmobRewardVariable,
            };
        }

        protected virtual bool IsRewardedReady()
        {
            return RewardAdUnit().IsReady();
        }

        public virtual void LoadRewarded()
        {
            if (!IsRewardedReady()) RewardAdUnit().Load();
        }

        #endregion

        #region Reward Inter Ad

        private AdUnitVariable RewardedInterstitialAdUnit()
        {
            return adSetting.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSetting.MaxRewardInterVariable,
                _ => adSetting.AdmobRewardInterVariable,
            };
        }

        protected virtual bool IsRewardedInterstitialReady()
        {
            return RewardedInterstitialAdUnit().IsReady();
        }

        public virtual void LoadRewardedInterstitial()
        {
            if (!IsRewardedInterstitialReady()) RewardedInterstitialAdUnit().Load();
        }

        #endregion

        #region AppOpen Ad

        private AdUnitVariable AppOpenAdUnit()
        {
            return adSetting.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSetting.MaxAppOpenVariable,
                _ => adSetting.AdmobAppOpenVariable,
            };
        }

        protected virtual bool IsAppOpenReady()
        {
            return AppOpenAdUnit().IsReady();
        }

        public virtual void LoadAppOpen()
        {
            if (!IsAppOpenReady()) AppOpenAdUnit().Load();
        }

        public virtual void ShowAppOpen()
        {
            if (statusAppOpenFirstIgnore) AppOpenAdUnit().Show();
            statusAppOpenFirstIgnore = true;
        }

        #endregion
    }
}