#if VIRTUESKY_IAP
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using VirtueSky.Inspector;

namespace VirtueSky.Iap
{
    [Serializable]
    [CreateAssetMenu(menuName = "Iap/Iap Data Variable", fileName = "iap_data_variables")]
    public class IapDataVariable : ScriptableObject
    {
        [ReadOnly] public string id;
        [ReadOnly] public ProductType productType;

        [Space] [SerializeField] private IapPurchaseSuccess onPurchaseSuccess;
        [SerializeField] IapPurchaseFailed onPurchaseFailed;
        internal IapPurchaseSuccess OnPurchaseSuccess => onPurchaseSuccess;
        internal IapPurchaseFailed OnPurchaseFailed => onPurchaseFailed;

        [NonSerialized] public Action purchaseSuccessCallback;
        [NonSerialized] public Action purchaseFailedCallback;
    }
}
#endif