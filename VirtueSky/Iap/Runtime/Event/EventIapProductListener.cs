#if VIRTUESKY_IAP
using UnityEngine.Events;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
    public class IapProductEventListener : BaseEventListener<IapDataVariable, EventIapProduct, UnityEvent<IapDataVariable>>
    {
    }
}
#endif