#if VIRTUESKY_IAP
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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

        [Tooltip("Price config used for UI"), Space]
        public float priceConfig;

        [SerializeField] private IapPurchaseSuccess onPurchaseSuccess;
        [SerializeField] private IapPurchaseFailed onPurchaseFailed;
        internal IapPurchaseSuccess OnPurchaseSuccess => onPurchaseSuccess;
        internal IapPurchaseFailed OnPurchaseFailed => onPurchaseFailed;

        [NonSerialized] public Action purchaseSuccessCallback;
        [NonSerialized] public Action<string> purchaseFailedCallback;

        private IapManager iapManager;

        internal void InitIapManager(IapManager _iapManager)
        {
            iapManager = _iapManager;
        }

        public Product GetProduct()
        {
            if (iapManager == null) return null;
            return iapManager.GetProduct(this);
        }

        public SubscriptionInfo GetSubscriptionInfo()
        {
            if (iapManager == null) return null;
            return iapManager.GetSubscriptionInfo(this);
        }

        public void Purchase()
        {
            if (iapManager == null) return;
            iapManager.PurchaseProduct(this);
        }

        public bool IsPurchased()
        {
            if (iapManager == null) return false;
            return iapManager.IsPurchasedProduct(this);
        }

        public string GetLocalizedPriceString()
        {
            if (GetProduct() == null) return String.Empty;
            return GetProduct().metadata.localizedPriceString;
        }

        public string GetIsoCurrencyCode()
        {
            if (GetProduct() == null) return String.Empty;
            return GetProduct().metadata.isoCurrencyCode;
        }

        public string GetLocalizedDescription()
        {
            if (GetProduct() == null) return String.Empty;
            return GetProduct().metadata.localizedDescription;
        }

        public string GetLocalizedTitle()
        {
            if (GetProduct() == null) return String.Empty;
            return GetProduct().metadata.localizedTitle;
        }

        public decimal GetLocalizedPrice()
        {
            if (GetProduct() == null) return 0;
            return GetProduct().metadata.localizedPrice;
        }
    }
}
#endif