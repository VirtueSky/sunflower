#if VIRTUESKY_IAP
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Misc;

namespace VirtueSky.Iap
{
    public class IapManager : BaseMono, IDetailedStoreListener
    {
        [SerializeField] private IapSetting iapSetting;
        [SerializeField] private EventIapProduct eventIapProduct;
        [SerializeField] private BooleanEvent changePreventDisplayAppOpenEvent;
#if UNITY_IOS
        [SerializeField] private EventNoParam restoreEvent;
#endif

        private IStoreController _controller;
        private IExtensionProvider _extensionProvider;

        public override void DoEnable()
        {
            base.DoEnable();
            eventIapProduct.AddListener(PurchaseProduct);
#if UNITY_IOS
             restoreEvent.AddListener(RestorePurchase);
#endif
        }

        public override void DoDisable()
        {
            base.DoDisable();
            eventIapProduct.RemoveListener(PurchaseProduct);
#if UNITY_IOS
             restoreEvent.RemoveListener(RestorePurchase);
#endif
        }

        private bool IsPurchasedProduct(IapDataVariable product)
        {
            if (_controller == null) return false;
            return product.productType == ProductType.NonConsumable && _controller.products.WithID(product.id).hasReceipt;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            bool validatedPurchase = true;
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
            var validator =
                new UnityEngine.Purchasing.Security.CrossPlatformValidator(UnityEngine.Purchasing.Security.GooglePlayTangle.Data(),
                    UnityEngine.Purchasing.Security.AppleTangle.Data(), Application.identifier);

            try
            {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(purchaseEvent.purchasedProduct.receipt);
                Debug.Log("Receipt is valid");
            }
            catch (UnityEngine.Purchasing.Security.IAPSecurityException)
            {
                Debug.Log("Invalid receipt, not unlocking content");
                validatedPurchase = false;
            }
#endif
            if (validatedPurchase) PurchaseVerified(purchaseEvent);

            return PurchaseProcessingResult.Complete;
        }


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensionProvider = extensions;

#if UNITY_ANDROID && !UNITY_EDITOR
            foreach (var product in _controller.products.all)
            {
                if (product != null && !string.IsNullOrEmpty(product.transactionID)) _controller.ConfirmPendingPurchase(product);
            }
#endif
        }

        private IapDataVariable PurchaseProductInternal(IapDataVariable product)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            _controller?.InitiatePurchase(product.id);
#elif UNITY_EDITOR
            InternalPurchaseSuccess(product.id);
#endif
            return product;
        }

        void PurchaseVerified(PurchaseEventArgs purchaseEvent)
        {
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(false);
            InternalPurchaseSuccess(purchaseEvent.purchasedProduct.definition.id);
        }

        void PurchaseProduct(IapDataVariable product)
        {
            // call when IAPDataVariable raise event
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(true);
            PurchaseProductInternal(product);
        }

        #region Purchase Success

        void InternalPurchaseSuccess(string id)
        {
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(false);
            foreach (var product in iapSetting.Products)
            {
                if (product.id != id) continue;
                product.OnPurchaseSuccess.Raise();
                Common.CallActionAndClean(ref product.purchaseSuccessCallback);
            }
        }

        #endregion


        #region Purchase Failed

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            switch (error)
            {
                case InitializationFailureReason.AppNotKnown:
                    Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                    break;
                case InitializationFailureReason.PurchasingUnavailable:
                    Debug.LogWarning("In App Purchases disabled in device settings!");
                    break;
                case InitializationFailureReason.NoProductsAvailable:
                    Debug.LogWarning("No products available for purchase!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            OnInitializeFailed(error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            InternalPurchaseFailed(product.definition.id);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            InternalPurchaseFailed(product.definition.id);
        }

        private void InternalPurchaseFailed(string id)
        {
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(false);
            foreach (var product in iapSetting.Products)
            {
                if (product.id != id) continue;
                product.OnPurchaseFailed.Raise();
                Common.CallActionAndClean(ref product.purchaseFailedCallback);
            }
        }

        #endregion

#if UNITY_IOS
        private void RestorePurchase()
        {
            if (!IsInitialized)
            {
                Debug.Log("Restore purchases fail. not initialized!");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                Debug.Log("Restore purchase started ...");

                var storeProvider = _extensions.GetExtension<IAppleExtensions>();
                storeProvider.RestoreTransactions(_ =>
                {
                    // no purchase are avaiable to restore
                    Debug.Log("Restore purchase continuting: " + _ + ". If no further messages, no purchase available to restore.");
                });
            }
            else
            {
                Debug.Log("Restore purchase fail. not supported on this platform. current = " + Application.platform);
            }
        }
#endif
    }
}
#endif