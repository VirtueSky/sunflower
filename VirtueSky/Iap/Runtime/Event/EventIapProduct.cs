#if VIRTUESKY_IAP
using UnityEngine;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_purchase_product.asset", menuName = "Iap/Purchase Product Event")]
    public class EventIapProduct : BaseEvent<IapDataVariable>
    {
    }
}
#endif