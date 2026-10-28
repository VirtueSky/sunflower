using UnityEngine;

namespace VirtueSky.Iap
{
    public abstract class ValidatePurchase : ScriptableObject
    {
        public abstract bool IsValidate();

        /// <summary>
        /// Validate with order data. Override to validate on your own server (receipt / transaction id).
        /// Default falls back to <see cref="IsValidate()"/> for backward compatibility.
        /// </summary>
        public virtual bool IsValidate(string productId, string receipt, string transactionId) => IsValidate();
    }
}