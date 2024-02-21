#if VIRTUESKY_IAP
using UnityEngine;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_is_purchase_product.asset", menuName = "Sunflower/Iap/Is Purchase Product Event")]
    public class EventIsPurchaseProduct : BaseEvent<IapDataVariable, bool>
    {
    }
}
#endif