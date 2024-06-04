using System.Collections.Generic;

#if VIRTUESKY_APPSFLYER
using AppsFlyerSDK;
#endif

#if VIRTUESKY_IAP
using UnityEngine.Purchasing;
#endif


namespace VirtueSky.Tracking
{
    public struct AppsFlyerTrackingRevenue
    {
        public static void AppsFlyerTrackRevenueAd(double value, string network, string unitId,
            string format, string adNetwork)
        {
#if VIRTUESKY_APPSFLYER
            var mediationNetworks = AppsFlyerAdRevenueMediationNetworkType
                .AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob;
            switch (adNetwork.ToLower())
            {
                case "admob":
                    mediationNetworks = AppsFlyerAdRevenueMediationNetworkType
                        .AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob;
                    break;
                case "max":
                    mediationNetworks = AppsFlyerAdRevenueMediationNetworkType
                        .AppsFlyerAdRevenueMediationNetworkTypeApplovinMax;
                    break;
            }

            Dictionary<string, string> additionalParams = new Dictionary<string, string>();
            additionalParams.Add(AFAdRevenueEvent.COUNTRY, "US");
            additionalParams.Add(AFAdRevenueEvent.AD_UNIT, unitId);
            additionalParams.Add(AFAdRevenueEvent.AD_TYPE, format);
            AppsFlyerAdRevenue.logAdRevenue(network,
                mediationNetworks,
                value,
                "USD",
                additionalParams);
#endif
        }
#if VIRTUESKY_APPSFLYER && VIRTUESKY_IAP
        public static void AppFlyerTrackingRevenueInAppPurchase(Product product)
        {
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_revenue", GetAppsflyerRevenue(product.metadata.localizedPrice));
            eventValue.Add("af_content_id", product.definition.id);
            eventValue.Add("af_currency", product.metadata.isoCurrencyCode);
            AppsFlyer.sendEvent("af_purchase", eventValue);
        }

        public static string GetAppsflyerRevenue(decimal amount)
        {
            decimal val = decimal.Multiply(amount, 0.63m);
            return val.ToString();
        }

#endif
    }
}