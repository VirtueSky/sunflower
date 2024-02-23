#if VIRTUESKY_IAP
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_purchase_product.asset", menuName = "Sunflower/Iap/Purchase Product Event")]
    [EditorIcon("icon_so")]
    public class EventIapProduct : BaseEvent<IapDataVariable>
    {
    }
}
#endif