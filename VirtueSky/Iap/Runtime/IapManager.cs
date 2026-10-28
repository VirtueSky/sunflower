#if VIRTUESKY_IAP
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
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
    public class IapManager : BaseMono
    {
        public enum InitType
        {
            InitOnStart,
            InitOnAwake,
        }
        [Space] [SerializeField] private bool dontDestroyOnLoad = false;
        [SerializeField] private InitType initType = InitType.InitOnStart;
        [Tooltip("Require"), SerializeField] private IapSetting iapSetting;

        [Tooltip("Allows nulls"), SerializeField]
        private BooleanEvent changePreventDisplayAppOpenEvent;

        [Tooltip("Allows nulls. Raised after purchases are fetched from the store (on init or after Restore), use it to refresh owned state"),
         SerializeField]
        private EventNoParam purchasesFetchedEvent;


        private StoreController _store;
        private bool flag = false;

        // NonConsumable / Subscription owned by the user, filled from confirmed orders
        private readonly HashSet<string> _ownedProductIds = new HashSet<string>();
        private readonly Dictionary<string, SubscriptionInfo> _subscriptionInfos = new Dictionary<string, SubscriptionInfo>();

        // Avoid granting the same transaction twice in one session
        private readonly HashSet<string> _processedTransactions = new HashSet<string>();

#if UNITY_ANDROID && !UNITY_EDITOR
        private UnityEngine.Purchasing.Security.CrossPlatformValidator _validator;
#endif

        private static event Action RestoreEvent;
        public static bool IsInitialized { get; private set; }
        public static void Restore() => RestoreEvent?.Invoke();

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }

            if (initType == InitType.InitOnAwake)
            {
                StartCoroutine(Init());
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
            if (initType == InitType.InitOnStart)
            {
                StartCoroutine(Init());
            }
        }

        private void OnDestroy()
        {
            UnregisterStoreEvents();
            if (_store != null) IsInitialized = false;
        }

        private IEnumerator Init()
        {
            if (IsInitialized || flag)
                yield break;
            flag = true;

            while (!UnityServiceInitialization.IsUnityServiceReady)
            {
                yield return null;
            }

            ConnectAndFetchProducts();
        }

        private async void ConnectAndFetchProducts()
        {
            if (_store == null)
            {
                _store = UnityIAPServices.StoreController();
                RegisterStoreEvents();
            }

            try
            {
                await _store.Connect();
            }
            catch (Exception e)
            {
                Debug.LogError($"[IapManager] Connect to store failed: {e}");
                flag = false;
                return;
            }

            _store.FetchProducts(BuildProductDefinitions());
            Debug.Log("IapManager connected to store, fetching products...");
        }

        private void RegisterStoreEvents()
        {
            // OnPurchasePending must be registered before Connect to receive pending orders from previous sessions
            _store.OnPurchasePending += OnPurchasePending;
            _store.OnPurchaseConfirmed += OnPurchaseConfirmed;
            _store.OnPurchaseFailed += OnPurchaseFailed;
            _store.OnPurchaseDeferred += OnPurchaseDeferred;
            _store.OnPurchasesFetched += OnPurchasesFetched;
            _store.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
            _store.OnProductsFetched += OnProductsFetched;
            _store.OnProductsFetchFailed += OnProductsFetchFailed;
            _store.OnStoreDisconnected += OnStoreDisconnected;
        }

        private void UnregisterStoreEvents()
        {
            if (_store == null) return;
            _store.OnPurchasePending -= OnPurchasePending;
            _store.OnPurchaseConfirmed -= OnPurchaseConfirmed;
            _store.OnPurchaseFailed -= OnPurchaseFailed;
            _store.OnPurchaseDeferred -= OnPurchaseDeferred;
            _store.OnPurchasesFetched -= OnPurchasesFetched;
            _store.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;
            _store.OnProductsFetched -= OnProductsFetched;
            _store.OnProductsFetchFailed -= OnProductsFetchFailed;
            _store.OnStoreDisconnected -= OnStoreDisconnected;
        }

        #region Internal Api

        internal bool IsPurchasedProduct(IapDataVariable product)
        {
            if (_store == null || product == null) return false;
            return IsOwned(product.Id, product.productType);
        }

        internal void PurchaseProduct(IapDataVariable product)
        {
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(true);
            PurchaseProductInternal(product);
        }

        internal Product GetProduct(IapDataVariable product)
        {
            if (_store == null || product == null) return null;
            return _store.GetProductById(product.Id);
        }

        internal SubscriptionInfo GetSubscriptionInfo(IapDataVariable product)
        {
            if (_store == null || product == null || product.productType != ProductType.Subscription) return null;
            return _subscriptionInfos.TryGetValue(product.Id, out var info) ? info : null;
        }

        #endregion

        #region Store Callbacks

        private void OnStoreDisconnected(StoreConnectionFailureDescription description)
        {
            Debug.LogWarning($"[IapManager] Store disconnected: {description.Message} (retryable: {description.IsRetryable})");
        }

        private void OnProductsFetched(List<Product> fetchedProducts)
        {
            foreach (var iapDataVariable in iapSetting.Products)
            {
                iapDataVariable.InitIapManager(this);
            }

            IsInitialized = true;
            Debug.Log($"IapManager initialized! Fetched {fetchedProducts.Count} products.");
            _store.FetchPurchases();
        }

        private void OnProductsFetchFailed(ProductFetchFailed failed)
        {
            Debug.LogWarning($"[IapManager] Fetch products failed: {failed.FailureReason}. " +
                             $"Failed products: {failed.FailedFetchProducts.Count}. Is your App correctly uploaded on the relevant publisher console?");
            flag = false;
        }

        private void OnPurchasesFetched(Orders orders)
        {
            foreach (var order in orders.ConfirmedOrders)
            {
                MarkTransactionProcessed(order);
                CacheOwnedProducts(order);
            }

            // Pending orders are delivered through OnPurchasePending (Google: ProcessPendingOrdersOnPurchasesFetched is on by default,
            // Apple: StoreKit pushes them at startup), so they are not granted here to avoid double grants.
            if (purchasesFetchedEvent != null) purchasesFetchedEvent.Raise();
        }

        private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription description)
        {
            Debug.LogWarning($"[IapManager] Fetch purchases failed: {description.FailureReason} - {description.Message}");
        }

        private void OnPurchasePending(PendingOrder order)
        {
            if (IsTransactionProcessed(order))
            {
                _store.ConfirmPurchase(order);
                return;
            }

            var productIds = GetProductIds(order);
            if (IsOrderValid(order, productIds))
            {
                MarkTransactionProcessed(order);
                CacheOwnedProducts(order);
                foreach (var id in productIds) PurchaseVerified(id);
            }
            else
            {
                Debug.Log("Invalid receipt, not unlocking content");
                foreach (var id in productIds) InternalPurchaseFailed(id, "ValidationFailed");
            }

            // Always confirm (same as returning PurchaseProcessingResult.Complete in v4) so the order is not stuck
            _store.ConfirmPurchase(order);
        }

        private void OnPurchaseConfirmed(Order order)
        {
            switch (order)
            {
                case ConfirmedOrder confirmedOrder:
                    MarkTransactionProcessed(confirmedOrder);
                    CacheOwnedProducts(confirmedOrder);
                    break;
                case FailedOrder failedOrder:
                    // Content was already granted in OnPurchasePending, the order will be re-delivered and deduplicated next time
                    Debug.LogWarning($"[IapManager] Confirm purchase failed: {failedOrder.FailureReason} - {failedOrder.Details}");
                    break;
            }
        }

        private void OnPurchaseFailed(FailedOrder order)
        {
            foreach (var id in GetProductIds(order))
            {
                InternalPurchaseFailed(id, order.FailureReason.ToString());
            }
        }

        private void OnPurchaseDeferred(DeferredOrder order)
        {
            // Ask to Buy / slow payment: content is granted later through OnPurchasePending
            Debug.Log("[IapManager] Purchase deferred, waiting for approval.");
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(false);
        }

        #endregion

        #region Order Helpers

        private static List<string> GetProductIds(Order order)
        {
            var ids = new List<string>();
            var items = order.CartOrdered?.Items();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var id = item.Product?.definition?.id;
                    if (!string.IsNullOrEmpty(id) && !ids.Contains(id)) ids.Add(id);
                }
            }

            if (ids.Count == 0 && order.Info?.PurchasedProductInfo != null)
            {
                foreach (var info in order.Info.PurchasedProductInfo)
                {
                    if (!string.IsNullOrEmpty(info.productId) && !ids.Contains(info.productId)) ids.Add(info.productId);
                }
            }

            return ids;
        }

        private bool IsTransactionProcessed(Order order)
        {
            var transactionId = order.Info?.TransactionID;
            return !string.IsNullOrEmpty(transactionId) && _processedTransactions.Contains(transactionId);
        }

        private void MarkTransactionProcessed(Order order)
        {
            var transactionId = order.Info?.TransactionID;
            if (!string.IsNullOrEmpty(transactionId)) _processedTransactions.Add(transactionId);
        }

        private void CacheOwnedProducts(Order order)
        {
            foreach (var id in GetProductIds(order))
            {
                var iapDataVariable = GetIapDataVariable(id);
                if (iapDataVariable == null) continue;
                if (iapDataVariable.productType is ProductType.NonConsumable or ProductType.Subscription) _ownedProductIds.Add(id);
            }

            if (order.Info?.PurchasedProductInfo == null) return;
            foreach (var info in order.Info.PurchasedProductInfo)
            {
                try
                {
                    if (info.subscriptionInfo != null) _subscriptionInfos[info.productId] = info.subscriptionInfo;
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[IapManager] Can not read subscription info of '{info.productId}': {e.Message}");
                }
            }
        }

        private bool IsOrderValid(Order order, List<string> productIds)
        {
            if (!iapSetting.IsValidatePurchase) return true;

            if (iapSetting.IsCustomValidatePurchase && iapSetting.ValidatePurchase != null)
            {
                var productId = productIds.Count > 0 ? productIds[0] : string.Empty;
                return iapSetting.ValidatePurchase.IsValidate(productId, order.Info?.Receipt, order.Info?.TransactionID);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                _validator ??= new UnityEngine.Purchasing.Security.CrossPlatformValidator(
                    UnityEngine.Purchasing.Security.GooglePlayTangle.Data(), Application.identifier);
                _validator.Validate(order.Info.Receipt);
                Debug.Log("Receipt is valid");
                return true;
            }
            catch (UnityEngine.Purchasing.Security.IAPSecurityException)
            {
                return false;
            }
#else
            // iOS: StoreKit 2 already verifies the transaction. Use order.Info.Apple.jwsRepresentation for server-side validation.
            return true;
#endif
        }

        private bool IsOwned(string id, ProductType productType)
        {
            if (productType is not (ProductType.NonConsumable or ProductType.Subscription)) return false;
            if (!_ownedProductIds.Contains(id)) return false;
            if (productType == ProductType.Subscription && _subscriptionInfos.TryGetValue(id, out var info))
            {
                return info.IsSubscribed() == Result.True && info.IsExpired() != Result.True;
            }

            return true;
        }

        private IapDataVariable GetIapDataVariable(string id)
        {
            foreach (var product in iapSetting.Products)
            {
                if (product != null && product.Id == id) return product;
            }

            return null;
        }

        #endregion

        private IapDataVariable PurchaseProductInternal(IapDataVariable product)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            if (_store == null || !IsInitialized)
            {
                InternalPurchaseFailed(product.Id, "NotInitialized");
                return product;
            }

            _store.PurchaseProduct(product.Id);
#elif UNITY_EDITOR
            InternalPurchaseSuccess(product.Id);
#endif
            return product;
        }

        void PurchaseVerified(string id)
        {
            if (changePreventDisplayAppOpenEvent != null) changePreventDisplayAppOpenEvent.Raise(false);
            InternalPurchaseSuccess(id);
        }


        #region Purchase Success

        void InternalPurchaseSuccess(string id)
        {
            foreach (var product in iapSetting.Products)
            {
                if (product.Id != id) continue;
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
                if (product.Id != id) continue;
                product.OnPurchaseFailed.Raise(reason);
                Common.CallActionAndClean(ref product.purchaseFailedCallback, reason);
            }
        }

        #endregion

        private List<ProductDefinition> BuildProductDefinitions()
        {
            var definitions = new List<ProductDefinition>();
            foreach (var p in iapSetting.Products)
            {
                if (p == null || string.IsNullOrEmpty(p.Id)) continue;
                definitions.Add(new ProductDefinition(p.Id, p.productType));
            }

            return definitions;
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

                // Restored purchases are delivered through OnPurchasesFetched
                _store.RestoreTransactions((success, error) =>
                {
                    Debug.Log($"Restore purchase continuing: {success}. {error} If no further messages, no purchase available to restore.");
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
