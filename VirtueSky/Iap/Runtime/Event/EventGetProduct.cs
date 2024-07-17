#if VIRTUESKY_IAP
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_get_product_event.asset",
        menuName = "Sunflower/Iap/Get Product Event")]
    [EditorIcon("scriptable_event")]
    public class EventGetProduct : BaseEvent<IapDataVariable, UnityEngine.Purchasing.Product>
    {
    }
}
#endif