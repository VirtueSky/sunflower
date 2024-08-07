using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Ads
{
    public abstract class AdClient
    {
        protected AdSetting adSetting;
        private bool _statusAppOpenFirstIgnore;

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
                AdNetwork.Admob => adSetting.AdmobInterVariable,
                _ => adSetting.IronSourceInterVariable,
            };
        }

        protected virtual bool IsInterstitialReady()
        {
            return InterstitialAdUnit().IsReady();
        }

        public virtual void LoadInterstitial()
        {
            if (InterstitialAdUnit() == null) return;
            if (!IsInterstitialReady()) InterstitialAdUnit().Load();
        }

        #endregion

        #region Reward Ad

        private AdUnitVariable RewardAdUnit()
        {
            return adSetting.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSetting.MaxRewardVariable,
                AdNetwork.Admob => adSetting.AdmobRewardVariable,
                _ => adSetting.IronSourceRewardVariable,
            };
        }

        protected virtual bool IsRewardedReady()
        {
            return RewardAdUnit().IsReady();
        }

        public virtual void LoadRewarded()
        {
            if (RewardAdUnit() == null) return;
            if (!IsRewardedReady()) RewardAdUnit().Load();
        }

        #endregion

        #region Reward Inter Ad

        private AdUnitVariable RewardedInterstitialAdUnit()
        {
            return adSetting.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSetting.MaxRewardInterVariable,
                AdNetwork.Admob => adSetting.AdmobRewardInterVariable,
                _ => null,
            };
        }

        protected virtual bool IsRewardedInterstitialReady()
        {
            return RewardedInterstitialAdUnit().IsReady();
        }

        public virtual void LoadRewardedInterstitial()
        {
            if (RewardedInterstitialAdUnit() == null) return;
            if (!IsRewardedInterstitialReady()) RewardedInterstitialAdUnit().Load();
        }

        #endregion

        #region AppOpen Ad

        private AdUnitVariable AppOpenAdUnit()
        {
            return adSetting.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSetting.MaxAppOpenVariable,
                AdNetwork.Admob => adSetting.AdmobAppOpenVariable,
                _ => null
            };
        }

        protected virtual bool IsAppOpenReady()
        {
            return AppOpenAdUnit().IsReady();
        }

        public virtual void LoadAppOpen()
        {
            if (AppOpenAdUnit() == null) return;
            if (!IsAppOpenReady()) AppOpenAdUnit().Load();
        }

        public virtual void ShowAppOpen()
        {
            if (_statusAppOpenFirstIgnore) AppOpenAdUnit().Show();
            _statusAppOpenFirstIgnore = true;
        }

        #endregion
    }
}