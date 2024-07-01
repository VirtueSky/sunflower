namespace VirtueSky.Ads
{
    public class IronSourceAdClient : AdClient
    {
        public override void Initialize()
        {
            if (adSetting.UseTestAppKey)
            {
                adSetting.AndroidAppKey = "85460dcd";
                adSetting.IosAppKey = "8545d445";
            }
#if VIRTUESKY_ADS && ADS_IRONSOURCE
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(adSetting.AppKey);
#endif
        }
    }
}