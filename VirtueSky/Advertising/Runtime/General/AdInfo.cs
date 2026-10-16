namespace VirtueSky.Ads
{
    public class AdsInfo
    {
        public string AdUnitId { get; internal set; }
        public string AdFormat { get; internal set; }
        public string Placement { get; internal set; }
        public string AdNetwork { get; internal set; }
        public double Revenue { get; internal set; }
        public string AdMediation { get; internal set; }
        public string AuctionId { get; internal set; }
        public string Precision { get; internal set; }

#if VIRTUESKY_APPLOVIN
        public AdsInfo(MaxSdkBase.AdInfo info)
        {
            AdUnitId = info.AdUnitIdentifier;
            AdFormat = info.AdFormat;
            Placement = info.Placement;
            AdNetwork = info.NetworkName;
            Revenue = info.Revenue;
            Precision = info.RevenuePrecision;
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
            Precision = info.Precision;
            AuctionId = info.AuctionId;
        }
        public AdsInfo(Unity.Services.LevelPlay.LevelPlayImpressionData info)
        {
            AdUnitId = info.MediationAdUnitId;
            AdFormat = info.AdFormat;
            Placement = info.Placement;
            AdNetwork = info.AdNetwork;
            Revenue = (double)info.Revenue;
            AdMediation = Ads.AdMediation.LevelPlay.ToString();
            Precision = info.Precision;
            AuctionId = info.AuctionId;
        }
#endif

        public AdsInfo(string adUnitId, string adFormat, string placement, string adNetwork, double revenue, string adMediation, string auctionId, string precision = "")
        {
            AdUnitId = adUnitId;
            AdFormat = adFormat;
            Placement = placement;
            AdNetwork = adNetwork;
            Revenue = revenue;
            AdMediation = adMediation;
            AuctionId = auctionId;
            Precision = precision;
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
            Precision = "";
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
            Precision = "";
        }
    }

    public class AdsError
    {
        public string AdUnitId { get; internal set; }
        public int ErrorCode { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public string AdMediation { get; internal set; }

#if VIRTUESKY_APPLOVIN
        public AdsError(MaxSdkBase.ErrorInfo info, string adUnitId)
        {
            AdUnitId = adUnitId;
            ErrorCode = (int)info.Code;
            ErrorMessage = info.Message;
            AdMediation = Ads.AdMediation.AppLovin.ToString();
        }
#endif

#if VIRTUESKY_LEVELPLAY
        public AdsError(Unity.Services.LevelPlay.LevelPlayAdError adError)
        {
            AdUnitId = adError.AdUnitId;
            ErrorCode = adError.ErrorCode;
            ErrorMessage = adError.ErrorMessage;
            AdMediation = Ads.AdMediation.LevelPlay.ToString();
        }
#endif

#if VIRTUESKY_ADMOB
        public AdsError(GoogleMobileAds.Api.AdError adError, string adUnitId)
        {
            AdUnitId = adUnitId;
            ErrorCode = adError.GetCode();
            ErrorMessage = adError.GetMessage();
            AdMediation = Ads.AdMediation.Admob.ToString();
        }
#endif
        public AdsError(string adUnitId, int errorCode, string errorMessage, string adMediation)
        {
            AdUnitId = adUnitId;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            AdMediation = adMediation;
        }

        public AdsError()
        {
            AdUnitId = "";
            ErrorCode = -1;
            ErrorMessage = "";
            AdMediation = "";
        }

        public AdsError(AdMediation adMediation)
        {
            AdUnitId = "";
            ErrorCode = -1;
            ErrorMessage = "";
            AdMediation = adMediation.ToString();
        }
    }
}
