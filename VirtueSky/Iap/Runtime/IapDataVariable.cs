#if VIRTUESKY_IAP
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [Serializable]
    [EditorIcon("scriptable_iap")]
    public class IapDataVariable : ScriptableObject
    {
        [ReadOnly] public string id;
        [ReadOnly] public ProductType productType;
        [ReadOnly] public EventPurchaseProduct eventPurchaseProduct;
        [ReadOnly] public EventIsPurchaseProduct eventIsPurchaseProduct;

        [Space] public float price;
        [SerializeField] private IapPurchaseSuccess onPurchaseSuccess;
        [SerializeField] private IapPurchaseFailed onPurchaseFailed;

        [ReadOnly] internal Product product;
        [ReadOnly] internal SubscriptionInfo subscriptionInfo;
        internal IapPurchaseSuccess OnPurchaseSuccess => onPurchaseSuccess;
        internal IapPurchaseFailed OnPurchaseFailed => onPurchaseFailed;

        [NonSerialized] public Action purchaseSuccessCallback;
        [NonSerialized] public Action<string> purchaseFailedCallback;

        public Product Product => product;
        public SubscriptionInfo SubscriptionInfo => subscriptionInfo;

        public void Purchase()
        {
            eventPurchaseProduct.Raise(this);
        }

        public bool IsPurchased()
        {
            return eventIsPurchaseProduct.Raise(this);
        }
    }
}
#endif