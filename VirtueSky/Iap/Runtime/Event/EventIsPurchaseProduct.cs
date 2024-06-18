#if VIRTUESKY_IAP
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_is_purchase_product_event.asset",
        menuName = "Sunflower/Iap/Is Purchase Product Event")]
    [EditorIcon("icon_scriptable")]
    public class EventIsPurchaseProduct : BaseEvent<IapDataVariable, bool>
    {
    }
}
#endif