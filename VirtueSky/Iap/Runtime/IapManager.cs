#if VIRTUESKY_IAP
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.Inspector;
using VirtueSky.Misc;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif

namespace VirtueSky.Iap
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class IapManager : BaseMono, IDetailedStoreListener
    {
        [Space] [SerializeField] private bool dontDestroyOnLoad = false;
        [Tooltip("Require"), SerializeField] private IapSetting iapSetting;

        [Tooltip("Allows nulls"), SerializeField]
        private BooleanEvent changePreventDisplayAppOpenEvent;


        private IStoreController _controller;
        private IExtensionProvider _extensionProvider;
        private static event Action RestoreEvent;
        public static event Func<bool> CustomValidatePurchaseEvent;
        public static bool IsInitialized { get; private set; }
        public static void Restore() => RestoreEvent?.Invoke();

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
#if UNITY_IOS
              RestoreEvent += RestorePurchase;
#endif
        }

        public override void OnDisable()
        {
            base.OnDisable();
#if UNITY_IOS
             RestoreEvent -= RestorePurchase;
#endif
        }

        private void Start()
        {
            Init();
        }

        private async void Init()
        {
            if (IsInitialized) return;
            await UniTask.WaitUntil(() => UnityServiceInitialization.IsUnityServiceReady);
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            RequestProductData(builder);
            builder.Configure<IGooglePlayConfiguration>();
            UnityPurchasing.Initialize(this, builder);
        }

        #region Internal Api

        internal bool IsPurchasedProduct(IapDataVariable product)
        {
            if (_controller == null) return false;
            return product.productType is ProductType.NonConsumable or ProductType.Subscription &&
                   _controller.products.WithID(product.id).hasReceipt;
        }

        internal void PurchaseProduct(IapDataVariable product)
        {
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(true);
            PurchaseProductInternal(product);
        }

        internal Product GetProduct(IapDataVariable product)
        {
            if (_controller == null) return null;
            return _controller.products.WithID(product.id);
        }

        internal SubscriptionInfo GetSubscriptionInfo(IapDataVariable product)
        {
            if (_controller == null || product.productType != ProductType.Subscription ||
                !_controller.products.WithID(product.id).hasReceipt) return null;
            var subscriptionManager = new SubscriptionManager(GetProduct(product), null);
            var subscriptionInfo = subscriptionManager.getSubscriptionInfo();
            return subscriptionInfo;
        }

        #endregion

        #region Implement

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if (iapSetting.IsValidatePurchase)
            {
                if (iapSetting.IsCustomValidatePurchase)
                {
                    if ((bool)CustomValidatePurchaseEvent?.Invoke()) PurchaseVerified(purchaseEvent);
                }
                else
                {
                    bool validatedPurchase = true;
#if (UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
                    var validator =
                        new UnityEngine.Purchasing.Security.CrossPlatformValidator(
                            UnityEngine.Purchasing.Security.GooglePlayTangle.Data(),
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
                }
            }
            else
            {
                PurchaseVerified(purchaseEvent);
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensionProvider = extensions;

            foreach (var iapDataVariable in iapSetting.Products)
            {
                iapDataVariable.InitIapManager(this);
            }

            IsInitialized = true;
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            OnInitializeFailed(error);
        }

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

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            InternalPurchaseFailed(product.definition.id, failureReason.ToString());
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            InternalPurchaseFailed(product.definition.id, failureDescription.reason.ToString());
        }

        #endregion

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


        #region Purchase Success

        void InternalPurchaseSuccess(string id)
        {
            foreach (var product in iapSetting.Products)
            {
                if (product.id != id) continue;
                product.OnPurchaseSuccess.Raise();
                Common.CallActionAndClean(ref product.purchaseSuccessCallback);
            }
        }

        #endregion


        #region Purchase Failed

        private void InternalPurchaseFailed(string id, string reason)
        {
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(false);
            foreach (var product in iapSetting.Products)
            {
                if (product.id != id) continue;
                product.OnPurchaseFailed.Raise(reason);
                Common.CallActionAndClean(ref product.purchaseFailedCallback, reason);
            }
        }

        #endregion

        private void RequestProductData(ConfigurationBuilder builder)
        {
            foreach (var p in iapSetting.Products)
            {
                builder.AddProduct(p.id, p.productType);
            }
        }

#if UNITY_IOS
        internal void RestorePurchase()
        {
            if (!IsInitialized)
            {
                Debug.Log("Restore purchases fail. not initialized!");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                Debug.Log("Restore purchase started ...");

                var storeProvider = _extensionProvider.GetExtension<IAppleExtensions>();
                storeProvider.RestoreTransactions((b, s) =>
                {
                    // no purchase are avaiable to restore
                    Debug.Log($"Restore purchase continuing: {b}. If no further messages, no purchase available to restore.");
                });
            }
            else
            {
                Debug.Log("Restore purchase fail. not supported on this platform. current = " + Application.platform);
            }
        }
#endif
#if UNITY_EDITOR
        private void Reset()
        {
            iapSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap/Setting");
        }
#endif
    }
}
#endif