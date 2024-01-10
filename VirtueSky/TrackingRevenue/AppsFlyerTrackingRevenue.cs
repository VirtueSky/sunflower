using System.Collections.Generic;
using AppsFlyerSDK;

namespace VirtueSky.TrackingRevenue
{
    public static class AppsFlyerTrackingRevenue
    {
        public static void AppsFlyerTrackRevenue(double value, string network, string unitId,
            string format, string adNetwork)
        {
            Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
            purchaseEvent.Add("value", value.ToString());
            switch (adNetwork.ToLower())
            {
                case "admob":
                    purchaseEvent.Add("ad_platform", "Admob");
                    break;
                case "max":
                    purchaseEvent.Add("ad_platform", "Applovin");
                    break;
            }

            purchaseEvent.Add("ad_format", format);
            purchaseEvent.Add("currency", "USD");
            purchaseEvent.Add("ad_unit_name", unitId);
            purchaseEvent.Add("ad_source", network);

            AppsFlyer.sendEvent("af_purchase", purchaseEvent);
        }
    }
}