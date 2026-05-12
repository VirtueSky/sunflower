namespace VirtueSky.Ads
{
    public class AdsInfo
    {
        public string AdUnitId { get; private set; }
        public string AdFormat { get; private set; }
        public string Placement { get; private set; }
        public string AdNetwork { get; private set; }
        public double Revenue { get; private set; }
        public string AdMediation { get; private set; }
        public string AuctionId { get; private set; }

#if VIRTUESKY_APPLOVIN
        public AdsInfo(MaxSdkBase.AdInfo info)
        {
            AdUnitId = info.AdUnitIdentifier;
            AdFormat = info.AdFormat;
            Placement = info.Placement;
            AdNetwork = info.NetworkName;
            Revenue = info.Revenue;
            AdMediation = Ads.AdMediation.AppLovin.ToString();
            AuctionId = "";
        }
#endif

#if VIRTUESKY_LEVELPLAY
        public AdsInfo(Unity.Services.LevelPlay.LevelPlayAdInfo info)
        {
            AdUnitId = info.AdUnitId;
            AdFormat = info.AdFormat;
            Placement = info.PlacementName;
            AdNetwork = info.AdNetwork;
            Revenue = (double)info.Revenue;
            AdMediation = Ads.AdMediation.LevelPlay.ToString();
            AuctionId = info.AuctionId;
        }
#endif

        public AdsInfo(string adUnitId, string adFormat, string placement, string adNetwork, double revenue, string adMediation, string auctionId)
        {
            AdUnitId = adUnitId;
            AdFormat = adFormat;
            Placement = placement;
            AdNetwork = adNetwork;
            Revenue = revenue;
            AdMediation = adMediation;
            AuctionId = auctionId;
        }

        public AdsInfo()
        {
            AdUnitId = "";
            AdFormat = "";
            Placement = "";
            AdNetwork = "";
            Revenue = 0;
            AdMediation = "";
            AuctionId = "";
        }

        public AdsInfo(AdMediation adMediation)
        {
            AdUnitId = "";
            AdFormat = "";
            Placement = "";
            AdNetwork = "";
            Revenue = 0;
            AdMediation = adMediation.ToString();
            AuctionId = "";
        }
    }

    public class AdsError
    {
        public int ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public string AdMediation { get; private set; }

#if VIRTUESKY_APPLOVIN
        public AdsError(MaxSdkBase.ErrorInfo info)
        {
            ErrorCode = (int)info.Code;
            ErrorMessage = info.Message;
            AdMediation = Ads.AdMediation.AppLovin.ToString();
        }
#endif

#if VIRTUESKY_LEVELPLAY
        public AdsError(Unity.Services.LevelPlay.LevelPlayAdError adError)
        {
            ErrorCode = adError.ErrorCode;
            ErrorMessage = adError.ErrorMessage;
            AdMediation = Ads.AdMediation.LevelPlay.ToString();
        }
#endif

#if VIRTUESKY_ADMOB
        public AdsError(GoogleMobileAds.Api.AdError adError)
        {
            ErrorCode = adError.GetCode();
            ErrorMessage = adError.GetMessage();
            AdMediation = Ads.AdMediation.Admob.ToString();
        }
#endif
        public AdsError(int errorCode, string errorMessage, string adMediation)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            AdMediation = adMediation;
        }

        public AdsError()
        {
            ErrorCode = -1;
            ErrorMessage = "";
            AdMediation = "";
        }

        public AdsError(AdMediation adMediation)
        {
            ErrorCode = -1;
            ErrorMessage = "";
            AdMediation = adMediation.ToString();
        }
    }
}
