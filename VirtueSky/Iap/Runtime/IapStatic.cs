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

        public static IapDataVariable OnPurchaseFailed(this IapDataVariable product, Action<string> onFailed)
        {
            product.purchaseFailedCallback = onFailed;
            return product;
        }
    }
}
#endif