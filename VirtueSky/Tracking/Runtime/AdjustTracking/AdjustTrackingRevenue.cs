using System;
#if VIRTUESKY_ADJUST
using AdjustSdk;
#endif

namespace VirtueSky.Tracking
{
    public struct AdjustTrackingRevenue
    {
        public static Action OnTracked;

        public static void AdjustTrackRevenue(double value, string network, string unitId,
            string placement, string currentAdSettingNetwork)
        {
#if VIRTUESKY_ADJUST
            var source = "";
            switch (currentAdSettingNetwork.ToLower())
            {
                case "admob":
                    source = "admob_sdk";
                    break;
                case "max":
                    source = "applovin_max_sdk";
                    break;
                case "ironsource":
                    source = "levelplay_ironsource_sdk";
                    break;
            }

            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(source);
            adjustAdRevenue.SetRevenue(value, "USD");
            adjustAdRevenue.AdRevenueNetwork = network;
            adjustAdRevenue.AdRevenueUnit = unitId;
            adjustAdRevenue.AdRevenuePlacement = placement;
            Adjust.TrackAdRevenue(adjustAdRevenue);
            OnTracked?.Invoke();
#endif
        }
    }
}