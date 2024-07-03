#if VIRTUESKY_IAP
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [CreateAssetMenu(fileName = "iap_localized_price_product_event.asset",
        menuName = "Sunflower/Iap/Localized Price Product Event")]
    [EditorIcon("scriptable_event")]
    public class EventLocalizedPriceProduct : BaseEvent<IapDataVariable, string>
    {
    }
}
#endif