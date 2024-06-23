#if VIRTUESKY_IAP
using UnityEngine;
using UnityEngine.Purchasing;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_tracking_purchase_product_event.asset",
        menuName = "Sunflower/Iap/Tracking Purchase Product Event")]
    [EditorIcon("scriptable_event")]
    public class EventIapTrackingPurchaseProduct : BaseEvent<Product>
    {
    }
}
#endif