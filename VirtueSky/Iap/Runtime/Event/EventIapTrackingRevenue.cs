#if VIRTUESKY_IAP
using UnityEngine;
using UnityEngine.Purchasing;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_tracking_revenue_event.asset", menuName = "Sunflower/Iap/Tracking Revenue Event")]
    public class EventIapTrackingRevenue : BaseEvent<Product>
    {
    }
}
#endif