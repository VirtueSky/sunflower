#if VIRTUESKY_IAP
using System;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
    public static class IapStatic
    {
        public static IapDataVariable OnPurchaseCompleted(this IapDataVariable product, Action onComplete)
        {
            product.purchaseSuccessCallback = onComplete;
            return product;
        }

        public static IapDataVariable OnPurchaseFailed(this IapDataVariable product, Action onFailed)
        {
            product.purchaseFailedCallback = onFailed;
            return product;
        }

        public static void Purchase(this IapDataVariable product, EventPurchaseProduct @event)
        {
            @event.Raise(product);
        }

        public static void RestorePurchase(this EventNoParam @event)
        {
            @event.Raise();
        }

        public static bool IsPurchased(this IapDataVariable product, EventIsPurchaseProduct @event)
        {
            return @event.Raise(product);
        }

        public static string LocalizedPrice(this IapDataVariable product,
            EventLocalizedPriceProduct @event)
        {
            return @event.Raise(product);
        }
    }
}
#endif