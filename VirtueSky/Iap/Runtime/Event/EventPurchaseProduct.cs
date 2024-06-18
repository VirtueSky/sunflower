#if VIRTUESKY_IAP
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_purchase_product_event.asset", menuName = "Sunflower/Iap/Purchase Product Event")]
    [EditorIcon("icon_scriptable")]
    public class EventPurchaseProduct : BaseEvent<IapDataVariable>
    {
    }
}
#endif