using UnityEngine;

namespace VirtueSky.Iap
{
    public abstract class ValidatePurchase : ScriptableObject
    {
        public abstract bool IsValidate();
    }
}