namespace VirtueSky.Ads
{
    public abstract class AdClient
    {
        protected AdSetting adSetting;
        protected bool statusAppOpenFirstIgnore;

        public void SetupAdSetting(AdSetting _adSetting)
        {
            this.adSetting = _adSetting;
        }

        public abstract void Initialize();
        public abstract void LoadBanner();
        public abstract void LoadInterstitial();
        public abstract void LoadRewarded();
        public abstract void LoadRewardedInterstitial();
        public abstract void LoadAppOpen();
        public abstract void ShowAppOpen();

        //Currently, native overlay ads is only available for admob.
        public abstract void LoadNativeOverlay();
    }
}