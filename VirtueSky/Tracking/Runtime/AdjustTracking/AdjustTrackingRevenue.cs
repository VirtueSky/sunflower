#if VIRTUESKY_ADJUST
using com.adjust.sdk;
#endif

namespace VirtueSky.Tracking
{
    public struct AdjustTrackingRevenue
    {
        public static void AdjustTrackRevenue(double value, string network, string unitId,
            string placement, string adNetwork)
        {
#if VIRTUESKY_ADJUST
            var source = "";
            switch (adNetwork.ToLower())
            {
                case "admob":
                    source = com.adjust.sdk.AdjustConfig.AdjustAdRevenueSourceAdMob;
                    break;
                case "max":
                    source = com.adjust.sdk.AdjustConfig.AdjustAdRevenueSourceAppLovinMAX;
                    break;
            }

            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(source);
            adjustAdRevenue.setRevenue(value, "USD");
            adjustAdRevenue.setAdRevenueNetwork(network);
            adjustAdRevenue.setAdRevenueUnit(unitId);
            adjustAdRevenue.setAdRevenuePlacement(placement);
            Adjust.trackAdRevenue(adjustAdRevenue);
#endif
        }
    }
}