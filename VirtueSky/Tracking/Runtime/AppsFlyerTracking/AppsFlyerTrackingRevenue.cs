using System;
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
        public static Action OnTracked;
        public static void AppsFlyerTrackRevenueAd(double value, string network, string unitId,
            string format, string currentAdSettingNetwork)
        {
#if VIRTUESKY_APPSFLYER
            var mediationNetworks = MediationNetwork.GoogleAdMob;
            switch (currentAdSettingNetwork.ToLower())
            {
                case "admob":
                    mediationNetworks = MediationNetwork.GoogleAdMob;
                    break;
                case "max":
                    mediationNetworks = MediationNetwork.ApplovinMax;
                    break;
                case "ironsource":
                    mediationNetworks = MediationNetwork.IronSource;
                    break;
            }

            Dictionary<string, string> additionalParams = new Dictionary<string, string>();
            additionalParams.Add(AdRevenueScheme.COUNTRY, "US");
            additionalParams.Add(AdRevenueScheme.AD_UNIT, unitId);
            additionalParams.Add(AdRevenueScheme.AD_TYPE, format);
            AppsFlyer.logAdRevenue(new AFAdRevenueData(network, mediationNetworks, "USD", value), additionalParams);
            OnTracked?.Invoke();
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