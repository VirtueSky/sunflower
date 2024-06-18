#if VIRTUESKY_IAP
using UnityEngine.Events;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
    public class
        IapPurchaseProductEventListener : BaseEventListener<IapDataVariable, EventPurchaseProduct, UnityEvent<IapDataVariable>>
    {
    }
}
#endif