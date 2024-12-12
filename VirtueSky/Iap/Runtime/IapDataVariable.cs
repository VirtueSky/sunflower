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

        private IapManager iapManager;

        internal void InitIapManager(IapManager _iapManager)
        {
            iapManager = _iapManager;
        }

        public void Purchase()
        {
            iapManager.PurchaseProduct(this);
        }

        public bool IsPurchased()
        {
            return iapManager.IsPurchasedProduct(this);
        }
    }
}
#endif